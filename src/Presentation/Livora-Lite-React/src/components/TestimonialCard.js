import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import '../styles/TestimonialCard.css';
export default function TestimonialCard({ quote, author, title, avatar, rating = 5 }) {
    return (_jsxs("div", { className: "testimonial-card", children: [rating > 0 && (_jsx("div", { className: "testimonial-rating", children: [...Array(rating)].map((_, i) => (_jsx("span", { className: "testimonial-star", children: "\u2605" }, i))) })), _jsxs("p", { className: "testimonial-quote", children: ["\"", quote, "\""] }), _jsxs("div", { className: "testimonial-author", children: [avatar && _jsx("img", { src: avatar, alt: author, className: "testimonial-avatar" }), _jsxs("div", { className: "testimonial-info", children: [_jsx("p", { className: "testimonial-name", children: author }), title && _jsx("p", { className: "testimonial-title", children: title })] })] })] }));
}
