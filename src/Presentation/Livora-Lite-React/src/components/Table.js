import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import './Table.css';
export function Table({ columns, data, loading, onRowClick, actions }) {
    if (loading) {
        return (_jsx("div", { className: "table-loading", children: _jsx("div", { className: "loading" }) }));
    }
    if (data.length === 0) {
        return (_jsx("div", { className: "table-empty", children: _jsx("p", { children: "Nenhum dados encontrado" }) }));
    }
    return (_jsx("div", { className: "table-wrapper", children: _jsxs("table", { className: "table", children: [_jsx("thead", { children: _jsxs("tr", { children: [columns.map((col) => (_jsx("th", { style: { width: col.width }, children: col.label }, String(col.key)))), actions && _jsx("th", { style: { width: '120px' }, children: "A\u00E7\u00F5es" })] }) }), _jsx("tbody", { children: data.map((row, idx) => (_jsxs("tr", { onClick: () => onRowClick?.(row), className: onRowClick ? 'clickable' : '', children: [columns.map((col) => (_jsx("td", { children: col.render ? col.render(row[col.key], row) : String(row[col.key] || '-') }, String(col.key)))), actions && (_jsx("td", { className: "actions-cell", children: actions(row) }))] }, idx))) })] }) }));
}
