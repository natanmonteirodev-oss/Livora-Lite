// Listener handler para evitar erros de extensões do navegador
// Trata mensagens que podem vir de extensões ou content scripts

if (typeof chrome !== 'undefined' && chrome.runtime && chrome.runtime.onMessage) {
    // Para extensões Chrome
    chrome.runtime.onMessage.addListener((request, sender, sendResponse) => {
        // Sempre responder, mesmo que não processarmos a mensagem
        console.log('[MessageHandler] Mensagem recebida de extensão:', request);
        try {
            sendResponse({ received: true });
        } catch (error) {
            console.warn('[MessageHandler] Erro ao responder à extensão:', error);
        }
        // Retornar false ou true baseado na resposta
        return true;
    });
}

// Listener para window.postMessage
window.addEventListener('message', (event) => {
    // Aceitar mensagens apenas de sources confiáveis
    if (event.source === window || event.source === parent) {
        console.log('[MessageHandler] Window message recebida:', event.data);
    }
});

// Prevenir erros não capturados de listeners
window.addEventListener('unhandledrejection', (event) => {
    if (event.reason && 
        (event.reason.message?.includes('A listener indicated an asynchronous response') ||
         event.reason.message?.includes('message channel closed'))) {
        console.warn('[MessageHandler] Listener error suprimido:', event.reason);
        event.preventDefault();
    }
});

console.log('[MessageHandler] Handlers de mensagem ativados');
