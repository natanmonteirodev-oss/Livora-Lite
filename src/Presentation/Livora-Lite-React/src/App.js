import { Fragment as _Fragment, jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './context/AuthContext';
import { Header } from './components/Header';
import { Footer } from './components/Footer';
import Landing from './pages/Landing';
import { Login } from './pages/Login';
import { Register } from './pages/Register';
import { Dashboard } from './pages/Dashboard';
import { Properties } from './pages/Properties';
import { Tenants } from './pages/Tenants';
import { Contracts } from './pages/Contracts';
import { Billings } from './pages/Billings';
import { Payments } from './pages/Payments';
import { Maintenance } from './pages/Maintenance';
import { Reports } from './pages/Reports';
import './styles/global.css';
import './styles/components.css';
// Componente para proteger rotas
const ProtectedRoute = ({ children }) => {
    const { isAuthenticated } = useAuth();
    return isAuthenticated ? _jsx(_Fragment, { children: children }) : _jsx(Navigate, { to: "/login" });
};
// Componente para preparar layouts
const Layout = ({ children }) => {
    return (_jsxs("div", { style: { display: 'flex', flexDirection: 'column', minHeight: '100vh' }, children: [_jsx(Header, {}), _jsx("main", { style: { flex: 1 }, children: children }), _jsx(Footer, {})] }));
};
const AppRoutes = () => {
    return (_jsx(Router, { children: _jsxs(Routes, { children: [_jsx(Route, { path: "/", element: _jsx(Layout, { children: _jsx(Landing, {}) }) }), _jsx(Route, { path: "/login", element: _jsx(Layout, { children: _jsx(Login, {}) }) }), _jsx(Route, { path: "/register", element: _jsx(Layout, { children: _jsx(Register, {}) }) }), _jsx(Route, { path: "/dashboard", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Dashboard, {}) }) }) }), _jsx(Route, { path: "/properties", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Properties, {}) }) }) }), _jsx(Route, { path: "/tenants", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Tenants, {}) }) }) }), _jsx(Route, { path: "/contracts", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Contracts, {}) }) }) }), _jsx(Route, { path: "/billings", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Billings, {}) }) }) }), _jsx(Route, { path: "/payments", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Payments, {}) }) }) }), _jsx(Route, { path: "/maintenance", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Maintenance, {}) }) }) }), _jsx(Route, { path: "/reports", element: _jsx(ProtectedRoute, { children: _jsx(Layout, { children: _jsx(Reports, {}) }) }) })] }) }));
};
function App() {
    return (_jsx(AuthProvider, { children: _jsx(AppRoutes, {}) }));
}
export default App;
