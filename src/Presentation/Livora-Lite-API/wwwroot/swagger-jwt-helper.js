/**
 * Swagger JWT Auto-Token Helper
 * 
 * Captura o token JWT retornado por /api/auth/login
 * e automaticamente configura o Authorization header
 */

(function() {
    console.log("[Swagger JWT Helper] ✓ Script carregado");

    // Esperar o Swagger UI inicializar
    window.addEventListener("load", function() {
        console.log("[Swagger JWT Helper] ✓ Window carregado, iniciando Swagger setup");
        
        // Encontrar o objeto swaggerUi (NSwag expõe como window.swagger)
        if (!window.swagger) {
            console.log("[Swagger JWT Helper] ⚠️ Swagger UI não encontrado, tentando wait...");
            setTimeout(arguments.callee, 500);
            return;
        }

        console.log("[Swagger JWT Helper] ✓ Swagger UI encontrado!");
        
        // Armazenar o token em sessionStorage
        const token = sessionStorage.getItem("jwtToken");
        if (token) {
            console.log("[Swagger JWT Helper] ✓ Token encontrado em sessionStorage, aplicando...");
            applyTokenToSwagger(token);
        }

        // Interceptar chamadas HTTP para capturar token de login
        interceptLoginResponse();
    });

    /**
     * Intercepta a resposta de /api/auth/login para capturar o token
     */
    function interceptLoginResponse() {
        console.log("[Swagger JWT Helper] ✓ Interceptador de login ativo");
        
        // Esperar um pouco para o Swagger carregar completamente
        setTimeout(() => {
            const originalFetch = window.fetch;
            
            window.fetch = function(...args) {
                const [resource, config] = args;
                
                // Se a requisição é para login, interceptar a resposta
                if (typeof resource === "string" && resource.includes("/api/auth/login")) {
                    console.log("[Swagger JWT Helper] 📍 Interceptada requisição de login");
                    
                    return originalFetch.apply(this, args).then(response => {
                        // Clonar a resposta para poder ler o body
                        const clonedResponse = response.clone();
                        
                        if (response.ok) {
                            clonedResponse.json().then(data => {
                                if (data.token) {
                                    console.log("[Swagger JWT Helper] ✓ Token capturado da resposta de login!");
                                    console.log("[Swagger JWT Helper] 📝 Token: " + data.token.substring(0, 50) + "...");
                                    
                                    // Armazenar em sessionStorage
                                    sessionStorage.setItem("jwtToken", data.token);
                                    
                                    // Aplicar ao Swagger imediatamente
                                    applyTokenToSwagger(data.token);
                                    
                                    // Mostrar notificação ao usuário
                                    showNotification("✅ Token JWT capturado! Autorização ativa para próximas requisições.");
                                }
                            }).catch(err => {
                                console.log("[Swagger JWT Helper] ⚠️ Erro ao processar resposta de login: " + err);
                            });
                        }
                        
                        return response;
                    });
                }
                
                return originalFetch.apply(this, args);
            };
        }, 500);
    }

    /**
     * Aplica o token JWT ao Swagger UI
     */
    function applyTokenToSwagger(token) {
        try {
            console.log("[Swagger JWT Helper] 🔐 Aplicando token ao Swagger...");
            
            // Método 1: Via window.swagger (NSwag)
            if (window.swagger && window.swagger.presets) {
                console.log("[Swagger JWT Helper] ✓ Aplicando via window.swagger.presets");
                
                // Encontrar ou criar a store de autorização
                const authorizationValue = {
                    type: "apiKey",
                    name: "Authorization",
                    in: "header",
                    value: `Bearer ${token}`
                };
                
                // Tentar aplicar para cada security scheme
                if (window.swagger.authorizationValue) {
                    window.swagger.authorizationValue("Bearer", token);
                    console.log("[Swagger JWT Helper] ✓ Token aplicado via authorizationValue");
                }
            }
            
            // Método 2: Via localStorage e reload (mais confiável)
            localStorage.setItem("swaggerToken", token);
            
            // Método 3: Injetar diretamente no DOM do Swagger (manipular inputs)
            injectTokenViaDom(token);
            
            console.log("[Swagger JWT Helper] ✓ Token aplicado!");
            
        } catch (err) {
            console.error("[Swagger JWT Helper] ✗ Erro ao aplicar token: " + err);
        }
    }

    /**
     * Tenta injetar token via manipulação do DOM
     */
    function injectTokenViaDom(token) {
        try {
            // Procurar por elementos de input de autorização
            const authInputs = document.querySelectorAll(
                'input[placeholder*="Bearer"], input[placeholder*="authorization"], input[aria-label*="token"]'
            );
            
            console.log("[Swagger JWT Helper] 🔍 Procurando inputs de auth no DOM...");
            console.log("[Swagger JWT Helper] Encontrados: " + authInputs.length + " inputs");
            
            authInputs.forEach((input, index) => {
                console.log(`[Swagger JWT Helper] 📝 Preenchendo input ${index + 1}...`);
                input.value = token;
                input.dispatchEvent(new Event("input", { bubbles: true }));
                input.dispatchEvent(new Event("change", { bubbles: true }));
            });
            
            // Se não encontrou inputs, tentar via buttons
            if (authInputs.length === 0) {
                const authorizeBtn = document.querySelector(
                    'button[aria-label="authorize"], button:contains("Authorize"), [title*="Authorize"]'
                );
                
                if (authorizeBtn) {
                    console.log("[Swagger JWT Helper] 🔘 Encontrado botão Authorize");
                    // Click no botão para abrir dialog
                    // authorizeBtn.click(); // Comentado para não interferir
                }
            }
        } catch (err) {
            console.error("[Swagger JWT Helper] ✗ Erro ao injetar via DOM: " + err);
        }
    }

    /**
     * Mostra notificação visual ao usuário
     */
    function showNotification(message) {
        try {
            // Criar elemento de notificação
            const notification = document.createElement("div");
            notification.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: #4caf50;
                color: white;
                padding: 16px 24px;
                border-radius: 4px;
                box-shadow: 0 2px 8px rgba(0,0,0,0.15);
                font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
                font-size: 14px;
                z-index: 10000;
                animation: slideIn 0.3s ease-out;
            `;
            notification.textContent = message;
            document.body.appendChild(notification);
            
            console.log("[Swagger JWT Helper] 📢 Notificação: " + message);
            
            // Remover após 5 segundos
            setTimeout(() => {
                notification.style.animation = "slideOut 0.3s ease-out";
                setTimeout(() => notification.remove(), 300);
            }, 5000);
            
        } catch (err) {
            console.warn("[Swagger JWT Helper] Erro ao mostrar notificação: " + err);
        }
    }

    // Adicionar CSS para animações
    const style = document.createElement("style");
    style.textContent = `
        @keyframes slideIn {
            from {
                transform: translateX(400px);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }
        
        @keyframes slideOut {
            from {
                transform: translateX(0);
                opacity: 1;
            }
            to {
                transform: translateX(400px);
                opacity: 0;
            }
        }
    `;
    document.head.appendChild(style);

    console.log("[Swagger JWT Helper] ✅ Inicialização concluída");
})();
