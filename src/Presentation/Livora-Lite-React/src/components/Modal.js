import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import './Modal.css';
export const Modal = ({ isOpen, title, onClose, children, size = 'medium' }) => {
    if (!isOpen)
        return null;
    return (_jsx("div", { className: "modal-overlay", onClick: onClose, children: _jsxs("div", { className: `modal-content modal-${size}`, onClick: (e) => e.stopPropagation(), children: [_jsxs("div", { className: "modal-header", children: [_jsx("h2", { children: title }), _jsx("button", { className: "modal-close", onClick: onClose, children: "\u2715" })] }), _jsx("div", { className: "modal-body", children: children })] }) }));
};
