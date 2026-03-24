import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table } from '../components/Table';
import apiService from '../services/api';
import './CRUD.css';
export const Maintenance = () => {
    const [requests, setRequests] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [formData, setFormData] = useState({
        description: '',
        priority: 'Medium',
        status: 'Open',
    });
    useEffect(() => {
        fetchRequests();
    }, []);
    const fetchRequests = async () => {
        try {
            setLoading(true);
            const data = await apiService.getAllMaintenanceRequests();
            setRequests(data);
            setError('');
        }
        catch (err) {
            setError('Erro ao carregar solicitações');
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    const handleOpenModal = (request) => {
        if (request) {
            setEditingId(request.id);
            setFormData(request);
        }
        else {
            setEditingId(null);
            setFormData({
                description: '',
                priority: 'Medium',
                status: 'Open',
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
                await apiService.updateMaintenanceRequest(editingId, formData);
            }
            else {
                await apiService.createMaintenanceRequest(formData);
            }
            await fetchRequests();
            handleCloseModal();
        }
        catch (err) {
            setError(err.response?.data?.message || 'Erro ao salvar solicitação');
        }
    };
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja deletar esta solicitação?')) {
            try {
                await apiService.deleteMaintenanceRequest(id);
                await fetchRequests();
            }
            catch (err) {
                setError('Erro ao deletar solicitação');
            }
        }
    };
    const formatDate = (date) => {
        return new Date(date).toLocaleDateString('pt-BR');
    };
    const getPriorityBadgeClass = (priority) => {
        switch (priority?.toLowerCase()) {
            case 'high':
                return 'badge-danger';
            case 'medium':
                return 'badge-warning';
            case 'low':
                return 'badge-info';
            default:
                return 'badge-info';
        }
    };
    const columns = [
        { key: 'description', label: 'Descrição', width: '35%' },
        {
            key: 'priority',
            label: 'Prioridade',
            width: '20%',
            render: (priority) => (_jsx("span", { className: `badge ${getPriorityBadgeClass(priority)}`, children: priority })),
        },
        {
            key: 'status',
            label: 'Status',
            width: '20%',
            render: (status) => (_jsx("span", { className: `badge badge-${status?.toLowerCase()}`, children: status })),
        },
        {
            key: 'createdDate',
            label: 'Data de Criação',
            width: '25%',
            render: (date) => formatDate(date),
        },
    ];
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "page-header", children: [_jsx("h1", { children: "\uD83D\uDD27 Manuten\u00E7\u00E3o" }), _jsx("button", { className: "btn btn-primary", onClick: () => handleOpenModal(), children: "+ Nova Solicita\u00E7\u00E3o" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsx(Table, { columns: columns, data: requests, loading: loading, actions: (row) => (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-sm btn-secondary", onClick: () => handleOpenModal(row), children: "Editar" }), _jsx("button", { className: "btn btn-sm btn-danger", onClick: () => handleDelete(row.id), children: "Deletar" })] })) }), _jsx(Modal, { isOpen: modalOpen, title: editingId ? 'Editar Solicitação' : 'Nova Solicitação', onClose: handleCloseModal, size: "medium", children: _jsxs("form", { onSubmit: handleSubmit, className: "form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "description", children: "Descri\u00E7\u00E3o *" }), _jsx("textarea", { id: "description", name: "description", value: formData.description || '', onChange: handleChange, placeholder: "Descreva a solicita\u00E7\u00E3o de manuten\u00E7\u00E3o...", rows: 4, required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "priority", children: "Prioridade *" }), _jsxs("select", { id: "priority", name: "priority", value: formData.priority || 'Medium', onChange: handleChange, children: [_jsx("option", { value: "Low", children: "Baixa" }), _jsx("option", { value: "Medium", children: "M\u00E9dia" }), _jsx("option", { value: "High", children: "Alta" })] })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "status", children: "Status *" }), _jsxs("select", { id: "status", name: "status", value: formData.status || 'Open', onChange: handleChange, children: [_jsx("option", { value: "Open", children: "Aberta" }), _jsx("option", { value: "In Progress", children: "Em Andamento" }), _jsx("option", { value: "Completed", children: "Conclu\u00EDda" }), _jsx("option", { value: "Cancelled", children: "Cancelada" })] })] }), _jsxs("div", { className: "form-actions", children: [_jsx("button", { type: "button", className: "btn btn-outline", onClick: handleCloseModal, children: "Cancelar" }), _jsxs("button", { type: "submit", className: "btn btn-primary", children: [editingId ? 'Atualizar' : 'Criar', " Solicita\u00E7\u00E3o"] })] })] }) })] }) }));
};
