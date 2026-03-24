import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import apiService from '../services/api';
import './Auth.css';
export const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setLoading(true);
        try {
            const result = await apiService.login({ email, password });
            if (result.success && result.user && result.token) {
                login(result.user, result.token);
                navigate('/dashboard');
            }
            else {
                setError(result.message || 'Erro ao fazer login. Verifique suas credenciais.');
            }
        }
        catch (err) {
            setError(err.response?.data?.message ||
                err.message ||
                'Erro ao conectar com o servidor');
        }
        finally {
            setLoading(false);
        }
    };
    return (_jsx("div", { className: "auth-container", children: _jsxs("div", { className: "auth-card", children: [_jsx("h1", { className: "auth-title", children: "Login" }), _jsx("p", { className: "auth-subtitle", children: "Bem-vindo de volta ao Livora" }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsxs("form", { onSubmit: handleSubmit, className: "auth-form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "email", children: "Email" }), _jsx("input", { id: "email", type: "email", value: email, onChange: (e) => setEmail(e.target.value), placeholder: "seu@email.com", required: true, disabled: loading })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "password", children: "Senha" }), _jsx("input", { id: "password", type: "password", value: password, onChange: (e) => setPassword(e.target.value), placeholder: "Sua senha", required: true, disabled: loading })] }), _jsx("button", { type: "submit", className: "btn btn-primary btn-lg", disabled: loading, children: loading ? 'Entrando...' : 'Entrar' })] }), _jsx("div", { className: "auth-footer", children: _jsxs("p", { children: ["N\u00E3o tem conta?", ' ', _jsx(Link, { to: "/register", className: "auth-link", children: "Registre-se aqui" })] }) })] }) }));
};
