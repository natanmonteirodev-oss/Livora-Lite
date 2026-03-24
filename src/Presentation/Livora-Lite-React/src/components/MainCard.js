import { jsx as _jsx } from "react/jsx-runtime";
import '../styles/MainCard.css';
export default function MainCard({ children, className = '', onClick, hover = true, shadow = 'md' }) {
    return (_jsx("div", { className: `main-card main-card-shadow-${shadow} ${hover ? 'main-card-hover' : ''} ${className}`, onClick: onClick, children: children }));
}
