import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useState } from 'react';
import './Reports.css';
export const Reports = () => {
    const [reportType, setReportType] = useState('financial');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [loading, setLoading] = useState(false);
    const handleGenerateReport = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            // Implementar chamada à API para gerar relatório
            console.log('Gerando relatório:', { reportType, startDate, endDate });
            // const data = await apiService.getReports();
        }
        catch (err) {
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsx("h1", { children: "\uD83D\uDCCA Relat\u00F3rios" }), _jsxs("div", { className: "reports-grid", children: [_jsxs("div", { className: "report-card", children: [_jsx("div", { className: "report-icon", children: "\uD83D\uDCB0" }), _jsx("h3", { children: "Relat\u00F3rio Financeiro" }), _jsx("p", { children: "Resumo de renda, despesas e fluxo de caixa" }), _jsx("button", { className: "btn btn-sm btn-primary", children: "Gerar" })] }), _jsxs("div", { className: "report-card", children: [_jsx("div", { className: "report-icon", children: "\uD83C\uDFE0" }), _jsx("h3", { children: "Relat\u00F3rio de Propriedades" }), _jsx("p", { children: "Status e ocupa\u00E7\u00E3o de todas as propriedades" }), _jsx("button", { className: "btn btn-sm btn-primary", children: "Gerar" })] }), _jsxs("div", { className: "report-card", children: [_jsx("div", { className: "report-icon", children: "\uD83D\uDC65" }), _jsx("h3", { children: "Relat\u00F3rio de Inquilinos" }), _jsx("p", { children: "Informa\u00E7\u00F5es e hist\u00F3rico dos inquilinos" }), _jsx("button", { className: "btn btn-sm btn-primary", children: "Gerar" })] }), _jsxs("div", { className: "report-card", children: [_jsx("div", { className: "report-icon", children: "\uD83D\uDCCB" }), _jsx("h3", { children: "Relat\u00F3rio de Contratos" }), _jsx("p", { children: "Resumo dos contratos ativos e expirados" }), _jsx("button", { className: "btn btn-sm btn-primary", children: "Gerar" })] }), _jsxs("div", { className: "report-card", children: [_jsx("div", { className: "report-icon", children: "\uD83D\uDCB3" }), _jsx("h3", { children: "Relat\u00F3rio de Cobran\u00E7as" }), _jsx("p", { children: "Cobran\u00E7as pendentes e recebidas" }), _jsx("button", { className: "btn btn-sm btn-primary", children: "Gerar" })] }), _jsxs("div", { className: "report-card", children: [_jsx("div", { className: "report-icon", children: "\uD83D\uDD27" }), _jsx("h3", { children: "Relat\u00F3rio de Manuten\u00E7\u00E3o" }), _jsx("p", { children: "Solicita\u00E7\u00F5es de manuten\u00E7\u00E3o e custos" }), _jsx("button", { className: "btn btn-sm btn-primary", children: "Gerar" })] })] }), _jsxs("div", { className: "report-filter", children: [_jsx("h2", { children: "Filtrar por Per\u00EDodo" }), _jsxs("form", { onSubmit: handleGenerateReport, className: "form filter-form", children: [_jsxs("div", { className: "form-row", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "startDate", children: "Data Inicial" }), _jsx("input", { id: "startDate", type: "date", value: startDate, onChange: (e) => setStartDate(e.target.value) })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "endDate", children: "Data Final" }), _jsx("input", { id: "endDate", type: "date", value: endDate, onChange: (e) => setEndDate(e.target.value) })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "reportType", children: "Tipo de Relat\u00F3rio" }), _jsxs("select", { id: "reportType", value: reportType, onChange: (e) => setReportType(e.target.value), children: [_jsx("option", { value: "financial", children: "Financeiro" }), _jsx("option", { value: "properties", children: "Propriedades" }), _jsx("option", { value: "tenants", children: "Inquilinos" }), _jsx("option", { value: "contracts", children: "Contratos" }), _jsx("option", { value: "billings", children: "Cobran\u00E7as" }), _jsx("option", { value: "maintenance", children: "Manuten\u00E7\u00E3o" })] })] })] }), _jsx("button", { type: "submit", className: "btn btn-primary", disabled: loading, children: loading ? 'Gerando...' : 'Gerar Relatório' })] })] })] }) }));
};
