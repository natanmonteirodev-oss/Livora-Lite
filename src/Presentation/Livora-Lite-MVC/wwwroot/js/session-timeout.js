/**
 * Session Timeout Manager
 * Exibe um alerta quando a sessão está prestes a expirar
 * Permitindo que o usuário renove a sessão ou fazer logout
 */

(function() {
    'use strict';

    const SessionTimeout = {
        // Configurações
        config: {
            // Tempo total da sessão em minutos (padrão: 30 minutos)
            sessionTimeout: 30,
            // Tempo para avisar antes de expirar em minutos (padrão: 5 minutos)
            warningTimeout: 5,
            // URL para renovar a sessão
            refreshUrl: '/Auth/RefreshSession',
            // URL para fazer logout
            logoutUrl: '/Auth/Logout'
        },

        // Controles
        sessionTimer: null,
        warningTimer: null,
        countdownInterval: null,
        isWarningShown: false,

        /**
         * Inicializa o gerenciador de sessão
         */
        init: function(options) {
            // Mescla configurações customizadas
            if (options) {
                Object.assign(this.config, options);
            }

            // Verifica se o usuário está autenticado
            if (this.isUserAuthenticated()) {
                // Inicia o timer de sessão
                this.startSessionTimer();

                // Reinicia o timer ao detectar atividade do usuário
                this.setupActivityListeners();
            }
        },

        /**
         * Verifica se o usuário está autenticado
         * (assumindo que existe um elemento ou classe que indica isso)
         */
        isUserAuthenticated: function() {
            // Verifica se há um elemento que indica que o usuário está autenticado
            // Por exemplo, um elemento com a classe 'user-authenticated' ou um menu do usuário visível
            return document.querySelector('.custom-navbar-cta') !== null &&
                   document.querySelector('.custom-navbar-cta a[href*="Logout"]') !== null;
        },

        /**
         * Inicia o timer de sessão
         */
        startSessionTimer: function() {
            // Limpa timers anteriores
            this.clearTimers();

            const sessionTimeMs = this.config.sessionTimeout * 60 * 1000;
            const warningTimeMs = (this.config.sessionTimeout - this.config.warningTimeout) * 60 * 1000;

            // Define quando mostrar o aviso
            this.warningTimer = setTimeout(() => {
                this.showSessionWarning();
            }, warningTimeMs);

            // Define quando fazer logout automático
            this.sessionTimer = setTimeout(() => {
                this.expireSession();
            }, sessionTimeMs);

            console.log(`[SessionTimeout] Sessão iniciada. Aviso em ${this.config.sessionTimeout - this.config.warningTimeout}min, expiração em ${this.config.sessionTimeout}min`);
        },

        /**
         * Configura listeners de atividade do usuário
         */
        setupActivityListeners: function() {
            const events = ['mousedown', 'keydown', 'scroll', 'touchstart', 'click'];
            const self = this;

            const activityHandler = function(e) {
                // Evita reiniciar o timer durante o warning
                if (!self.isWarningShown) {
                    self.startSessionTimer();
                }
                // NÃO retorna nada para evitar conflito com extensões
            };

            events.forEach(event => {
                document.addEventListener(event, activityHandler, { 
                    passive: true,
                    capture: false 
                });
            });
        },

        /**
         * Mostra o alerta de aviso de sessão expirando
         */
        showSessionWarning: function() {
            this.isWarningShown = true;
            let remainingSeconds = this.config.warningTimeout * 60;
            const self = this;

            // Verifica se SweetAlert2 está carregado
            if (typeof Swal === 'undefined') {
                console.error('[SessionTimeout] SweetAlert2 não foi carregado. Usando confirm padrão.');
                
                if (confirm('Sua sessão está prestes a expirar. Deseja continuar?')) {
                    this.refreshSession();
                } else {
                    this.logout();
                }
                return;
            }

            // Cria o HTML customizado para o alerta
            const warningHtml = `
                <div style="text-align: center;">
                    <p style="font-size: 16px; margin-bottom: 20px;">
                        Sua sessão está prestes a expirar!
                    </p>
                    <p style="font-size: 14px; color: #666; margin-bottom: 20px;">
                        Você será desconectado em:
                    </p>
                    <div style="font-size: 48px; font-weight: bold; color: #dc3545; margin-bottom: 20px;">
                        <span id="countdown-timer">5:00</span>
                    </div>
                    <p style="font-size: 12px; color: #999; margin-bottom: 10px;">
                        Clique em "Continuar" para renovar sua sessão e continuar trabalhando.
                    </p>
                </div>
            `;

            Swal.fire({
                title: '⏱️ Aviso de Sessão',
                html: warningHtml,
                icon: 'warning',
                allowOutsideClick: false,
                allowEscapeKey: false,
                didOpen: (modal) => {
                    try {
                        // Inicia o countdown
                        self.startCountdown(remainingSeconds);
                    } catch (error) {
                        console.error('[SessionTimeout] Erro ao iniciar countdown:', error);
                    }
                },
                willClose: () => {
                    // Para o countdown quando o modal fecha
                    if (this.countdownInterval) {
                        clearInterval(this.countdownInterval);
                    }
                },
                showCancelButton: true,
                confirmButtonText: '✓ Continuar',
                cancelButtonText: '✗ Fazer Logout',
                confirmButtonColor: '#0081a7',
                cancelButtonColor: '#dc3545',
                reverseButtons: true
            }).then((result) => {
                try {
                    if (result.isConfirmed) {
                        // Usuário escolheu renovar a sessão
                        self.refreshSession();
                    } else if (result.dismiss === Swal.DismissReason.cancel) {
                        // Usuário escolheu fazer logout
                        self.logout();
                    }
                } catch (error) {
                    console.error('[SessionTimeout] Erro ao processar resposta do alerta:', error);
                }
            }).catch(error => {
                console.error('[SessionTimeout] Erro no SweetAlert:', error);
            });
        },

        /**
         * Inicia o countdown regressivo
         */
        startCountdown: function(seconds) {
            const self = this;
            let remaining = seconds;

            // Limpa qualquer countdown anterior
            if (this.countdownInterval) {
                clearInterval(this.countdownInterval);
            }

            this.countdownInterval = setInterval(() => {
                try {
                    remaining--;

                    const minutes = Math.floor(remaining / 60);
                    const secs = remaining % 60;
                    const timeString = `${minutes}:${secs.toString().padStart(2, '0')}`;

                    const timerElement = document.getElementById('countdown-timer');
                    if (timerElement) {
                        timerElement.textContent = timeString;

                        // Muda a cor para vermelho mais escuro quando falta menos de 1 minuto
                        if (remaining < 60) {
                            timerElement.style.color = '#8B0000';
                        }
                    }

                    if (remaining <= 0) {
                        clearInterval(self.countdownInterval);
                    }
                } catch (error) {
                    console.error('[SessionTimeout] Erro no countdown:', error);
                    clearInterval(self.countdownInterval);
                }
            }, 1000);
        },

        /**
         * Renova a sessão do usuário
         */
        refreshSession: function() {
            const self = this;

            // Faz uma requisição para renovar a sessão no servidor
            fetch(this.config.refreshUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
                signal: AbortSignal.timeout(10000) // Timeout de 10 segundos
            })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Falha ao renovar sessão. Status: ' + response.status);
                }
            })
            .then(data => {
                // Sessão renovada com sucesso
                self.isWarningShown = false;
                self.startSessionTimer();

                Swal.fire({
                    title: '✓ Sucesso',
                    text: 'Sua sessão foi renovada. Continuando...',
                    icon: 'success',
                    timer: 2000,
                    showConfirmButton: false
                });

                console.log('[SessionTimeout] Sessão renovada com sucesso:', data);
            })
            .catch(error => {
                // Verifica se é um erro de timeout
                if (error.name === 'AbortError') {
                    console.error('[SessionTimeout] Timeout ao renovar sessão');
                } else {
                    console.error('[SessionTimeout] Erro ao renovar sessão:', error);
                }

                Swal.fire({
                    title: '✗ Erro',
                    text: 'Erro ao renovar sessão. Por favor, faça login novamente.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                }).then(() => {
                    self.logout();
                });
            });
        },

        /**
         * Faz logout do usuário
         */
        logout: function() {
            // Redireciona para a página de logout
            window.location.href = this.config.logoutUrl;
        },

        /**
         * Expira a sessão (chamado automaticamente após timeout)
         */
        expireSession: function() {
            Swal.fire({
                title: '⏰ Sessão Expirada',
                text: 'Sua sessão expirou. Você será desconectado automaticamente.',
                icon: 'error',
                allowOutsideClick: false,
                allowEscapeKey: false,
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            }).then(() => {
                this.logout();
            });
        },

        /**
         * Limpa os timers
         */
        clearTimers: function() {
            if (this.sessionTimer) {
                clearTimeout(this.sessionTimer);
                this.sessionTimer = null;
            }

            if (this.warningTimer) {
                clearTimeout(this.warningTimer);
                this.warningTimer = null;
            }

            if (this.countdownInterval) {
                clearInterval(this.countdownInterval);
                this.countdownInterval = null;
            }
        }
    };

    // Expõe o objeto globalmente
    window.SessionTimeout = SessionTimeout;

    // Inicializa quando o DOM está pronto
    const initSessionTimeout = function() {
        try {
            // Configurações padrão (pode ser customizado passando opções)
            SessionTimeout.init({
                sessionTimeout: 30,      // 30 minutos
                warningTimeout: 5        // Avisar 5 minutos antes
            });
        } catch (error) {
            console.error('[SessionTimeout] Erro ao inicializar gerenciador de sessão:', error);
        }
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initSessionTimeout);
    } else {
        // DOM já está pronto
        initSessionTimeout();
    }
})();
