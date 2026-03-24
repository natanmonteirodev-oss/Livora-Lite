import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table } from '../components/Table';
import apiService from '../services/api';
import './CRUD.css';
export const Tenants = () => {
    const [tenants, setTenants] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [formData, setFormData] = useState({
        name: '',
        email: '',
        phone: '',
    });
    useEffect(() => {
        fetchTenants();
    }, []);
    const fetchTenants = async () => {
        try {
            setLoading(true);
            const data = await apiService.getAllTenants();
            setTenants(data);
            setError('');
        }
        catch (err) {
            setError('Erro ao carregar inquilinos');
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    const handleOpenModal = (tenant) => {
        if (tenant) {
            setEditingId(tenant.id);
            setFormData(tenant);
        }
        else {
            setEditingId(null);
            setFormData({
                name: '',
                email: '',
                phone: '',
            });
        }
        setModalOpen(true);
    };
    const handleCloseModal = () => {
        setModalOpen(false);
        setEditingId(null);
    };
    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (editingId) {
                await apiService.updateTenant(editingId, formData);
            }
            else {
                await apiService.createTenant(formData);
            }
            await fetchTenants();
            handleCloseModal();
        }
        catch (err) {
            setError(err.response?.data?.message || 'Erro ao salvar inquilino');
        }
    };
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja deletar este inquilino?')) {
            try {
                await apiService.deleteTenant(id);
                await fetchTenants();
            }
            catch (err) {
                setError('Erro ao deletar inquilino');
            }
        }
    };
    const columns = [
        { key: 'name', label: 'Nome', width: '35%' },
        {
            key: 'email',
            label: 'Email',
            width: '35%',
            render: (email) => _jsx("a", { href: `mailto:${email}`, children: email }),
        },
        { key: 'phone', label: 'Telefone', width: '30%' },
    ];
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "page-header", children: [_jsx("h1", { children: "\uD83D\uDC65 Inquilinos" }), _jsx("button", { className: "btn btn-primary", onClick: () => handleOpenModal(), children: "+ Novo Inquilino" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsx(Table, { columns: columns, data: tenants, loading: loading, actions: (row) => (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-sm btn-secondary", onClick: () => handleOpenModal(row), children: "Editar" }), _jsx("button", { className: "btn btn-sm btn-danger", onClick: () => handleDelete(row.id), children: "Deletar" })] })) }), _jsx(Modal, { isOpen: modalOpen, title: editingId ? 'Editar Inquilino' : 'Novo Inquilino', onClose: handleCloseModal, size: "medium", children: _jsxs("form", { onSubmit: handleSubmit, className: "form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "name", children: "Nome Completo *" }), _jsx("input", { id: "name", type: "text", name: "name", value: formData.name || '', onChange: handleChange, placeholder: "Nome do inquilino", required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "email", children: "Email *" }), _jsx("input", { id: "email", type: "email", name: "email", value: formData.email || '', onChange: handleChange, placeholder: "email@example.com", required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "phone", children: "Telefone *" }), _jsx("input", { id: "phone", type: "tel", name: "phone", value: formData.phone || '', onChange: handleChange, placeholder: "(11) 99999-9999", required: true })] }), _jsxs("div", { className: "form-actions", children: [_jsx("button", { type: "button", className: "btn btn-outline", onClick: handleCloseModal, children: "Cancelar" }), _jsxs("button", { type: "submit", className: "btn btn-primary", children: [editingId ? 'Atualizar' : 'Criar', " Inquilino"] })] })] }) })] }) }));
};
