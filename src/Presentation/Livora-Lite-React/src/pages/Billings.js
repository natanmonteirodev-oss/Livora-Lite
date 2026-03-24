import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table } from '../components/Table';
import apiService from '../services/api';
import './CRUD.css';
export const Billings = () => {
    const [billings, setBillings] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [formData, setFormData] = useState({
        amount: 0,
        dueDate: '',
        status: 'Pending',
    });
    useEffect(() => {
        fetchBillings();
    }, []);
    const fetchBillings = async () => {
        try {
            setLoading(true);
            const data = await apiService.getAllBillings();
            setBillings(data);
            setError('');
        }
        catch (err) {
            setError('Erro ao carregar cobranças');
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    const handleOpenModal = (billing) => {
        if (billing) {
            setEditingId(billing.id);
            setFormData(billing);
        }
        else {
            setEditingId(null);
            setFormData({
                amount: 0,
                dueDate: '',
                status: 'Pending',
            });
        }
        setModalOpen(true);
    };
    const handleCloseModal = () => {
        setModalOpen(false);
        setEditingId(null);
    };
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: name === 'amount' ? parseFloat(value) : value,
        });
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (editingId) {
                await apiService.updateBilling(editingId, formData);
            }
            else {
                await apiService.createBilling(formData);
            }
            await fetchBillings();
            handleCloseModal();
        }
        catch (err) {
            setError(err.response?.data?.message || 'Erro ao salvar cobrança');
        }
    };
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja deletar esta cobrança?')) {
            try {
                await apiService.deleteBilling(id);
                await fetchBillings();
            }
            catch (err) {
                setError('Erro ao deletar cobrança');
            }
        }
    };
    const formatDate = (date) => {
        return new Date(date).toLocaleDateString('pt-BR');
    };
    const formatCurrency = (value) => {
        return new Intl.NumberFormat('pt-BR', {
            style: 'currency',
            currency: 'BRL',
        }).format(value);
    };
    const columns = [
        {
            key: 'amount',
            label: 'Valor',
            width: '25%',
            render: (amount) => formatCurrency(amount),
        },
        {
            key: 'dueDate',
            label: 'Vencimento',
            width: '25%',
            render: (date) => formatDate(date),
        },
        {
            key: 'status',
            label: 'Status',
            width: '25%',
            render: (status) => (_jsx("span", { className: `badge badge-${status?.toLowerCase()}`, children: status })),
        },
    ];
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "page-header", children: [_jsx("h1", { children: "\uD83D\uDCB3 Cobran\u00E7as" }), _jsx("button", { className: "btn btn-primary", onClick: () => handleOpenModal(), children: "+ Nova Cobran\u00E7a" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsx(Table, { columns: columns, data: billings, loading: loading, actions: (row) => (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-sm btn-secondary", onClick: () => handleOpenModal(row), children: "Editar" }), _jsx("button", { className: "btn btn-sm btn-danger", onClick: () => handleDelete(row.id), children: "Deletar" })] })) }), _jsx(Modal, { isOpen: modalOpen, title: editingId ? 'Editar Cobrança' : 'Nova Cobrança', onClose: handleCloseModal, size: "medium", children: _jsxs("form", { onSubmit: handleSubmit, className: "form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "amount", children: "Valor (R$) *" }), _jsx("input", { id: "amount", type: "number", name: "amount", value: formData.amount || 0, onChange: handleChange, placeholder: "0.00", step: "0.01", required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "dueDate", children: "Data de Vencimento *" }), _jsx("input", { id: "dueDate", type: "date", name: "dueDate", value: formData.dueDate || '', onChange: handleChange, required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "status", children: "Status *" }), _jsxs("select", { id: "status", name: "status", value: formData.status || 'Pending', onChange: handleChange, children: [_jsx("option", { value: "Pending", children: "Pendente" }), _jsx("option", { value: "Paid", children: "Pago" }), _jsx("option", { value: "Overdue", children: "Vencido" })] })] }), _jsxs("div", { className: "form-actions", children: [_jsx("button", { type: "button", className: "btn btn-outline", onClick: handleCloseModal, children: "Cancelar" }), _jsxs("button", { type: "submit", className: "btn btn-primary", children: [editingId ? 'Atualizar' : 'Criar', " Cobran\u00E7a"] })] })] }) })] }) }));
};
