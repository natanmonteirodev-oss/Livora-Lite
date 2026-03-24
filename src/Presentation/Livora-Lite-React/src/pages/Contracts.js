import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table } from '../components/Table';
import apiService from '../services/api';
import './CRUD.css';
export const Contracts = () => {
    const [contracts, setContracts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [formData, setFormData] = useState({
        startDate: '',
        endDate: '',
        rentalAmount: 0,
        status: 'Active',
    });
    useEffect(() => {
        fetchContracts();
    }, []);
    const fetchContracts = async () => {
        try {
            setLoading(true);
            const data = await apiService.getAllContracts();
            setContracts(data);
            setError('');
        }
        catch (err) {
            setError('Erro ao carregar contratos');
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    const handleOpenModal = (contract) => {
        if (contract) {
            setEditingId(contract.id);
            setFormData(contract);
        }
        else {
            setEditingId(null);
            setFormData({
                startDate: '',
                endDate: '',
                rentalAmount: 0,
                status: 'Active',
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
            [name]: name === 'rentalAmount' ? parseFloat(value) : value,
        });
    };
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (editingId) {
                await apiService.updateContract(editingId, formData);
            }
            else {
                await apiService.createContract(formData);
            }
            await fetchContracts();
            handleCloseModal();
        }
        catch (err) {
            setError(err.response?.data?.message || 'Erro ao salvar contrato');
        }
    };
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja deletar este contrato?')) {
            try {
                await apiService.deleteContract(id);
                await fetchContracts();
            }
            catch (err) {
                setError('Erro ao deletar contrato');
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
            key: 'startDate',
            label: 'Data Início',
            width: '20%',
            render: (date) => formatDate(date),
        },
        {
            key: 'endDate',
            label: 'Data Fim',
            width: '20%',
            render: (date) => formatDate(date),
        },
        {
            key: 'rentalAmount',
            label: 'Valor',
            width: '20%',
            render: (amount) => formatCurrency(amount),
        },
        {
            key: 'status',
            label: 'Status',
            width: '20%',
            render: (status) => (_jsx("span", { className: `badge badge-${status?.toLowerCase()}`, children: status })),
        },
    ];
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "page-header", children: [_jsx("h1", { children: "\uD83D\uDCCB Contratos" }), _jsx("button", { className: "btn btn-primary", onClick: () => handleOpenModal(), children: "+ Novo Contrato" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsx(Table, { columns: columns, data: contracts, loading: loading, actions: (row) => (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-sm btn-secondary", onClick: () => handleOpenModal(row), children: "Editar" }), _jsx("button", { className: "btn btn-sm btn-danger", onClick: () => handleDelete(row.id), children: "Deletar" })] })) }), _jsx(Modal, { isOpen: modalOpen, title: editingId ? 'Editar Contrato' : 'Novo Contrato', onClose: handleCloseModal, size: "medium", children: _jsxs("form", { onSubmit: handleSubmit, className: "form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "startDate", children: "Data de In\u00EDcio *" }), _jsx("input", { id: "startDate", type: "date", name: "startDate", value: formData.startDate || '', onChange: handleChange, required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "endDate", children: "Data de T\u00E9rmino *" }), _jsx("input", { id: "endDate", type: "date", name: "endDate", value: formData.endDate || '', onChange: handleChange, required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "rentalAmount", children: "Valor do Aluguel (R$) *" }), _jsx("input", { id: "rentalAmount", type: "number", name: "rentalAmount", value: formData.rentalAmount || 0, onChange: handleChange, placeholder: "1000.00", step: "0.01", required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "status", children: "Status *" }), _jsxs("select", { id: "status", name: "status", value: formData.status || 'Active', onChange: handleChange, children: [_jsx("option", { value: "Active", children: "Ativo" }), _jsx("option", { value: "Expired", children: "Expirado" }), _jsx("option", { value: "Terminated", children: "Encerrado" })] })] }), _jsxs("div", { className: "form-actions", children: [_jsx("button", { type: "button", className: "btn btn-outline", onClick: handleCloseModal, children: "Cancelar" }), _jsxs("button", { type: "submit", className: "btn btn-primary", children: [editingId ? 'Atualizar' : 'Criar', " Contrato"] })] })] }) })] }) }));
};
