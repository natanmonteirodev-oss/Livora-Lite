import { jsx as _jsx, jsxs as _jsxs } from "react/jsx-runtime";
import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import apiService from '../services/api';
import './Auth.css';
export const Register = () => {
    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        confirmPassword: '',
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();
    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        if (formData.password !== formData.confirmPassword) {
            setError('As senhas não correspondem');
            return;
        }
        if (formData.password.length < 6) {
            setError('A senha deve ter no mínimo 6 caracteres');
            return;
        }
        setLoading(true);
        try {
            const result = await apiService.register({
                firstName: formData.firstName,
                lastName: formData.lastName,
                email: formData.email,
                password: formData.password,
            });
            if (result.success && result.user && result.token) {
                login(result.user, result.token);
                navigate('/dashboard');
            }
            else {
                setError(result.message || 'Erro ao registrar. Tente novamente.');
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
    return (_jsx("div", { className: "auth-container", children: _jsxs("div", { className: "auth-card", children: [_jsx("h1", { className: "auth-title", children: "Criar Conta" }), _jsx("p", { className: "auth-subtitle", children: "Junte-se ao Livora e simplifique sua gest\u00E3o de alugu\u00E9is" }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsxs("form", { onSubmit: handleSubmit, className: "auth-form", children: [_jsxs("div", { className: "form-row", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "firstName", children: "Primeiro Nome" }), _jsx("input", { id: "firstName", type: "text", name: "firstName", value: formData.firstName, onChange: handleChange, placeholder: "Seu nome", required: true, disabled: loading })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "lastName", children: "Sobrenome" }), _jsx("input", { id: "lastName", type: "text", name: "lastName", value: formData.lastName, onChange: handleChange, placeholder: "Seu sobrenome", required: true, disabled: loading })] })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "email", children: "Email" }), _jsx("input", { id: "email", type: "email", name: "email", value: formData.email, onChange: handleChange, placeholder: "seu@email.com", required: true, disabled: loading })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "password", children: "Senha" }), _jsx("input", { id: "password", type: "password", name: "password", value: formData.password, onChange: handleChange, placeholder: "M\u00EDnimo 6 caracteres", required: true, disabled: loading })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "confirmPassword", children: "Confirmar Senha" }), _jsx("input", { id: "confirmPassword", type: "password", name: "confirmPassword", value: formData.confirmPassword, onChange: handleChange, placeholder: "Confirme sua senha", required: true, disabled: loading })] }), _jsx("button", { type: "submit", className: "btn btn-primary btn-lg", disabled: loading, children: loading ? 'Criando conta...' : 'Registrar' })] }), _jsx("div", { className: "auth-footer", children: _jsxs("p", { children: ["J\u00E1 tem conta?", ' ', _jsx(Link, { to: "/login", className: "auth-link", children: "Fa\u00E7a login aqui" })] }) })] }) }));
};
