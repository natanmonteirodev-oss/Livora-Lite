/**
 * Storage Manager - Acesso seguro a localStorage e sessionStorage
 * Verifica disponibilidade antes de tentar acessar
 * Evita erros com Tracking Prevention/Privacy Mode
 */

window.StorageManager = (function() {
    'use strict';

    // Testa disponibilidade de storage
    function isStorageAvailable(type) {
        try {
            const storage = window[type];
            const test = '__storage_test__';
            storage.setItem(test, test);
            storage.removeItem(test);
            return true;
        } catch (error) {
            // QuotaExceededError, SecurityError, ou PrivateMode
            if (error instanceof DOMException && (
                error.code === 22 ||
                error.code === 1016 ||
                error.name === 'QuotaExceededError' ||
                error.name === 'NS_ERROR_DOM_QUOTA_REACHED'
            )) {
                console.warn(`[StorageManager] ${type} está cheio ou indisponível`);
            } else if (error instanceof SecurityError || error.name === 'SecurityError') {
                console.warn(`[StorageManager] ${type} bloqueado por política de segurança (Tracking Prevention?)`);
            } else {
                console.warn(`[StorageManager] ${type} indisponível:`, error.message);
            }
            return false;
        }
    }

    const isLocalStorageAvailable = isStorageAvailable('localStorage');
    const isSessionStorageAvailable = isStorageAvailable('sessionStorage');

    return {
        // Estado de disponibilidade
        hasLocalStorage: isLocalStorageAvailable,
        hasSessionStorage: isSessionStorageAvailable,

        /**
         * Salva um item no localStorage de forma segura
         */
        setLocal: function(key, value) {
            if (!isLocalStorageAvailable) {
                console.warn(`[StorageManager] localStorage indisponível, ignorando setLocal("${key}")`);
                return false;
            }

            try {
                localStorage.setItem(key, value);
                return true;
            } catch (error) {
                console.error('[StorageManager] Erro ao salvar no localStorage:', error);
                return false;
            }
        },

        /**
         * Obtém um item do localStorage de forma segura
         */
        getLocal: function(key) {
            if (!isLocalStorageAvailable) {
                return null;
            }

            try {
                return localStorage.getItem(key);
            } catch (error) {
                console.error('[StorageManager] Erro ao ler do localStorage:', error);
                return null;
            }
        },

        /**
         * Remove um item do localStorage de forma segura
         */
        removeLocal: function(key) {
            if (!isLocalStorageAvailable) {
                return false;
            }

            try {
                localStorage.removeItem(key);
                return true;
            } catch (error) {
                console.error('[StorageManager] Erro ao remover do localStorage:', error);
                return false;
            }
        },

        /**
         * Limpa todo o localStorage de forma segura
         */
        clearLocal: function() {
            if (!isLocalStorageAvailable) {
                return false;
            }

            try {
                localStorage.clear();
                return true;
            } catch (error) {
                console.error('[StorageManager] Erro ao limpar localStorage:', error);
                return false;
            }
        },

        /**
         * Salva um item no sessionStorage de forma segura
         */
        setSession: function(key, value) {
            if (!isSessionStorageAvailable) {
                console.warn(`[StorageManager] sessionStorage indisponível, ignorando setSession("${key}")`);
                return false;
            }

            try {
                sessionStorage.setItem(key, value);
                return true;
            } catch (error) {
                console.error('[StorageManager] Erro ao salvar no sessionStorage:', error);
                return false;
            }
        },

        /**
         * Obtém um item do sessionStorage de forma segura
         */
        getSession: function(key) {
            if (!isSessionStorageAvailable) {
                return null;
            }

            try {
                return sessionStorage.getItem(key);
            } catch (error) {
                console.error('[StorageManager] Erro ao ler do sessionStorage:', error);
                return null;
            }
        },

        /**
         * Remove um item do sessionStorage de forma segura
         */
        removeSession: function(key) {
            if (!isSessionStorageAvailable) {
                return false;
            }

            try {
                sessionStorage.removeItem(key);
                return true;
            } catch (error) {
                console.error('[StorageManager] Erro ao remover do sessionStorage:', error);
                return false;
            }
        },

        /**
         * Limpa todo o sessionStorage de forma segura
         */
        clearSession: function() {
            if (!isSessionStorageAvailable) {
                return false;
            }

            try {
                sessionStorage.clear();
                return true;
            } catch (error) {
                console.error('[StorageManager] Erro ao limpar sessionStorage:', error);
                return false;
            }
        },

        /**
         * Log do status de storage
         */
        logStatus: function() {
            console.log('[StorageManager Status]');
            console.log('  localStorage:', isLocalStorageAvailable ? '✓ Disponível' : '✗ Bloqueado');
            console.log('  sessionStorage:', isSessionStorageAvailable ? '✓ Disponível' : '✗ Bloqueado');

            if (!isLocalStorageAvailable && !isSessionStorageAvailable) {
                console.warn('[StorageManager] ⚠️ Uma política de segurança está bloqueando o acesso a storage. Verifique:');
                console.warn('  - Tracking Prevention (Firefox)');
                console.warn('  - Private/Incognito mode');
                console.warn('  - Cookies de terceiros bloqueados');
            }
        }
    };
})();

// Log inicial do status
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', function() {
        window.StorageManager.logStatus();
    });
} else {
    window.StorageManager.logStatus();
}
