import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Header.css';
export const Header = () => {
    const { isAuthenticated, user, logout } = useAuth();
    const navigate = useNavigate();
    const handleLogout = () => {
        logout();
        navigate('/');
    };
    return (_jsx("header", { className: "header", children: _jsx("div", { className: "container", children: _jsxs("div", { className: "header-content", children: [_jsxs(Link, { to: "/", className: "logo", children: [_jsx("span", { className: "logo-icon", children: "\uD83C\uDFE0" }), _jsx("span", { className: "logo-text", children: "Livora" })] }), _jsx("nav", { className: "nav", children: isAuthenticated ? (_jsxs(_Fragment, { children: [_jsx(Link, { to: "/dashboard", className: "nav-link", children: "Dashboard" }), _jsxs("div", { className: "nav-dropdown", children: [_jsx("button", { className: "nav-link dropdown-toggle", children: "Gest\u00E3o" }), _jsxs("div", { className: "dropdown-menu", children: [_jsx(Link, { to: "/properties", className: "dropdown-item", children: "Propriedades" }), _jsx(Link, { to: "/tenants", className: "dropdown-item", children: "Inquilinos" }), _jsx(Link, { to: "/contracts", className: "dropdown-item", children: "Contratos" })] })] }), _jsxs("div", { className: "nav-dropdown", children: [_jsx("button", { className: "nav-link dropdown-toggle", children: "Financeiro" }), _jsxs("div", { className: "dropdown-menu", children: [_jsx(Link, { to: "/billings", className: "dropdown-item", children: "Cobran\u00E7as" }), _jsx(Link, { to: "/payments", className: "dropdown-item", children: "Pagamentos" }), _jsx(Link, { to: "/reports", className: "dropdown-item", children: "Relat\u00F3rios" })] })] }), _jsx(Link, { to: "/maintenance", className: "nav-link", children: "Manuten\u00E7\u00E3o" }), _jsx(Link, { to: "/profile", className: "nav-link", children: user?.firstName || 'Perfil' }), _jsx("button", { className: "btn btn-sm btn-outline", onClick: handleLogout, children: "Sair" })] })) : (_jsxs(_Fragment, { children: [_jsx(Link, { to: "/login", className: "nav-link", children: "Login" }), _jsx(Link, { to: "/register", className: "btn btn-sm btn-primary", children: "Registrar" })] })) })] }) }) }));
};
