import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import apiService from '../services/api';
import './Dashboard.css';
export const Dashboard = () => {
    const { user } = useAuth();
    const [dashboard, setDashboard] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    useEffect(() => {
        const fetchDashboard = async () => {
            try {
                const data = await apiService.getDashboard();
                setDashboard(data);
            }
            catch (err) {
                setError('Erro ao carregar dashboard');
                console.error(err);
            }
            finally {
                setLoading(false);
            }
        };
        fetchDashboard();
    }, []);
    if (loading) {
        return (_jsx("div", { className: "dashboard", children: _jsx("div", { className: "container", children: _jsx("div", { className: "flex-center", style: { minHeight: '60vh' }, children: _jsx("div", { className: "loading" }) }) }) }));
    }
    return (_jsx("div", { className: "dashboard", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "dashboard-header", children: [_jsxs("h1", { children: ["Bem-vindo, ", user?.firstName || 'Usuário', "!"] }), _jsx("p", { children: "Acompanhe um resumo de suas atividades abaixo" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), dashboard && (_jsxs("div", { className: "dashboard-grid", children: [_jsxs("div", { className: "dashboard-card", children: [_jsx("div", { className: "card-icon", children: "\uD83C\uDFE0" }), _jsxs("div", { className: "card-content", children: [_jsx("h3", { children: "Propriedades" }), _jsx("p", { className: "card-value", children: dashboard.totalProperties || 0 })] })] }), _jsxs("div", { className: "dashboard-card", children: [_jsx("div", { className: "card-icon", children: "\uD83D\uDC65" }), _jsxs("div", { className: "card-content", children: [_jsx("h3", { children: "Inquilinos" }), _jsx("p", { className: "card-value", children: dashboard.totalTenants || 0 })] })] }), _jsxs("div", { className: "dashboard-card", children: [_jsx("div", { className: "card-icon", children: "\uD83D\uDCB0" }), _jsxs("div", { className: "card-content", children: [_jsx("h3", { children: "Renda Total" }), _jsxs("p", { className: "card-value", children: ["R$ ", (dashboard.totalIncome || 0).toLocaleString('pt-BR', {
                                                    minimumFractionDigits: 2,
                                                })] })] })] }), _jsxs("div", { className: "dashboard-card warning", children: [_jsx("div", { className: "card-icon", children: "\u23F0" }), _jsxs("div", { className: "card-content", children: [_jsx("h3", { children: "Pagamentos Pendentes" }), _jsx("p", { className: "card-value", children: dashboard.pendingPayments || 0 })] })] })] })), _jsx("div", { className: "dashboard-sections", children: _jsxs("section", { className: "dashboard-section", children: [_jsx("h2", { children: "Pr\u00F3ximas A\u00E7\u00F5es" }), _jsxs("div", { className: "action-list", children: [_jsxs("a", { href: "/properties", className: "action-item", children: [_jsx("span", { className: "action-icon", children: "\uD83D\uDCCA" }), _jsx("span", { className: "action-text", children: "Gerenciar Propriedades" }), _jsx("span", { className: "action-arrow", children: "\u2192" })] }), _jsxs("a", { href: "/tenants", className: "action-item", children: [_jsx("span", { className: "action-icon", children: "\uD83D\uDC65" }), _jsx("span", { className: "action-text", children: "Gerenciar Inquilinos" }), _jsx("span", { className: "action-arrow", children: "\u2192" })] }), _jsxs("a", { href: "/contracts", className: "action-item", children: [_jsx("span", { className: "action-icon", children: "\uD83D\uDCCB" }), _jsx("span", { className: "action-text", children: "Visualizar Contratos" }), _jsx("span", { className: "action-arrow", children: "\u2192" })] }), _jsxs("a", { href: "/billings", className: "action-item", children: [_jsx("span", { className: "action-icon", children: "\uD83D\uDCB3" }), _jsx("span", { className: "action-text", children: "Gerenciar Cobran\u00E7as" }), _jsx("span", { className: "action-arrow", children: "\u2192" })] })] })] }) })] }) }));
};
