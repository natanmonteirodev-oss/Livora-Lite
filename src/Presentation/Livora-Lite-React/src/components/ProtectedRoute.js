import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
export const ProtectedRoute = ({ children, isAuthenticated }) => {
    if (!isAuthenticated) {
        return (_jsx("div", { className: "protected-route-message", children: _jsxs("div", { className: "alert alert-info", children: [_jsx("h3", { children: "Acesso Restrito" }), _jsx("p", { children: "Voc\u00EA precisa fazer login para acessar esta p\u00E1gina." }), _jsx("a", { href: "/login", className: "btn btn-primary", children: "Ir para Login" })] }) }));
    }
    return _jsx(_Fragment, { children: children });
};
