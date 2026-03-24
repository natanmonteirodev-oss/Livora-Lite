import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import '../styles/FeatureCard.css';
export default function FeatureCard({ icon, title, description, iconColor = 'primary', iconSize = 'md' }) {
    return (_jsxs("div", { className: "feature-card", children: [icon && (_jsx("div", { className: `feature-card-icon feature-card-icon-${iconColor} icon-size-${iconSize}`, children: icon })), _jsx("h3", { className: "feature-card-title", children: title }), _jsx("p", { className: "feature-card-description", children: description })] }));
}
