import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table } from '../components/Table';
import apiService from '../services/api';
import './CRUD.css';
export const Payments = () => {
    const [payments, setPayments] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [formData, setFormData] = useState({
        amount: 0,
        paymentDate: '',
        method: 'Credit Card',
    });
    useEffect(() => {
        fetchPayments();
    }, []);
    const fetchPayments = async () => {
        try {
            setLoading(true);
            const data = await apiService.getAllPayments();
            setPayments(data);
            setError('');
        }
        catch (err) {
            setError('Erro ao carregar pagamentos');
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    const handleOpenModal = (payment) => {
        if (payment) {
            setEditingId(payment.id);
            setFormData(payment);
        }
        else {
            setEditingId(null);
            setFormData({
                amount: 0,
                paymentDate: new Date().toISOString().split('T')[0],
                method: 'Credit Card',
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
                await apiService.updatePayment(editingId, formData);
            }
            else {
                await apiService.createPayment(formData);
            }
            await fetchPayments();
            handleCloseModal();
        }
        catch (err) {
            setError(err.response?.data?.message || 'Erro ao salvar pagamento');
        }
    };
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja deletar este pagamento?')) {
            try {
                await apiService.deletePayment(id);
                await fetchPayments();
            }
            catch (err) {
                setError('Erro ao deletar pagamento');
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
            key: 'paymentDate',
            label: 'Data do Pagamento',
            width: '25%',
            render: (date) => formatDate(date),
        },
        { key: 'method', label: 'Método', width: '25%' },
    ];
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "page-header", children: [_jsx("h1", { children: "\uD83D\uDCB0 Pagamentos" }), _jsx("button", { className: "btn btn-primary", onClick: () => handleOpenModal(), children: "+ Novo Pagamento" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsx(Table, { columns: columns, data: payments, loading: loading, actions: (row) => (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-sm btn-secondary", onClick: () => handleOpenModal(row), children: "Editar" }), _jsx("button", { className: "btn btn-sm btn-danger", onClick: () => handleDelete(row.id), children: "Deletar" })] })) }), _jsx(Modal, { isOpen: modalOpen, title: editingId ? 'Editar Pagamento' : 'Novo Pagamento', onClose: handleCloseModal, size: "medium", children: _jsxs("form", { onSubmit: handleSubmit, className: "form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "amount", children: "Valor (R$) *" }), _jsx("input", { id: "amount", type: "number", name: "amount", value: formData.amount || 0, onChange: handleChange, placeholder: "0.00", step: "0.01", required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "paymentDate", children: "Data do Pagamento *" }), _jsx("input", { id: "paymentDate", type: "date", name: "paymentDate", value: formData.paymentDate || '', onChange: handleChange, required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "method", children: "M\u00E9todo de Pagamento *" }), _jsxs("select", { id: "method", name: "method", value: formData.method || 'Credit Card', onChange: handleChange, children: [_jsx("option", { value: "Credit Card", children: "Cart\u00E3o de Cr\u00E9dito" }), _jsx("option", { value: "Debit Card", children: "Cart\u00E3o de D\u00E9bito" }), _jsx("option", { value: "Bank Transfer", children: "Transfer\u00EAncia Banc\u00E1ria" }), _jsx("option", { value: "Cash", children: "Dinheiro" }), _jsx("option", { value: "Check", children: "Cheque" })] })] }), _jsxs("div", { className: "form-actions", children: [_jsx("button", { type: "button", className: "btn btn-outline", onClick: handleCloseModal, children: "Cancelar" }), _jsxs("button", { type: "submit", className: "btn btn-primary", children: [editingId ? 'Atualizar' : 'Criar', " Pagamento"] })] })] }) })] }) }));
};
