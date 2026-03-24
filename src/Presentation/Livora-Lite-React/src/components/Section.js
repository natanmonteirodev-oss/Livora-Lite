import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import '../styles/Section.css';
export default function Section({ children, className = '', title, subtitle, darker = false, centered = false, paddingSize = 'lg' }) {
    return (_jsx("section", { className: `section section-padding-${paddingSize} ${darker ? 'section-darker' : ''} ${className}`, children: _jsxs("div", { className: "container", children: [(title || subtitle) && (_jsxs("div", { className: `section-header ${centered ? 'section-header-centered' : ''}`, children: [title && _jsx("h2", { className: "section-title", children: title }), subtitle && _jsx("p", { className: "section-subtitle", children: subtitle })] })), children] }) }));
}
