// Funções para gerenciar localStorage de autenticação

export function setToken(token) {
    if (token) {
        localStorage.setItem('auth_token', token);
        console.log('[TokenStorage] Token salvo em localStorage');
    } else {
        localStorage.removeItem('auth_token');
        console.log('[TokenStorage] Token removido de localStorage');
    }
}

export function getToken() {
    const token = localStorage.getItem('auth_token');
    if (token) {
        console.log('[TokenStorage] Token recuperado de localStorage');
    }
    return token || '';
}

export function clearToken() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('current_user');
    console.log('[TokenStorage] Tokens limpos de localStorage');
}

export function setCurrentUser(user) {
    if (user) {
        localStorage.setItem('current_user', JSON.stringify(user));
        console.log('[TokenStorage] Usuário salvo em localStorage');
    } else {
        localStorage.removeItem('current_user');
        console.log('[TokenStorage] Usuário removido de localStorage');
    }
}

export function getCurrentUser() {
    const user = localStorage.getItem('current_user');
    if (user) {
        try {
            const parsed = JSON.parse(user);
            console.log('[TokenStorage] Usuário recuperado de localStorage');
            return parsed;
        } catch (e) {
            console.warn('[TokenStorage] Erro ao parsear usuário:', e);
            return null;
        }
    }
    return null;
}

console.log('[TokenStorage] Módulo de armazenamento de token carregado');
