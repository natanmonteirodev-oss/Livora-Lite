import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table } from '../components/Table';
import apiService from '../services/api';
import './CRUD.css';
export const Properties = () => {
    const [properties, setProperties] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);
    const [editingId, setEditingId] = useState(null);
    const [formData, setFormData] = useState({
        address: '',
        propertyType: '',
        status: 'Available',
        details: '',
    });
    useEffect(() => {
        fetchProperties();
    }, []);
    const fetchProperties = async () => {
        try {
            setLoading(true);
            const data = await apiService.getAllProperties();
            setProperties(data);
            setError('');
        }
        catch (err) {
            setError('Erro ao carregar propriedades');
            console.error(err);
        }
        finally {
            setLoading(false);
        }
    };
    const handleOpenModal = (property) => {
        if (property) {
            setEditingId(property.id);
            setFormData(property);
        }
        else {
            setEditingId(null);
            setFormData({
                address: '',
                propertyType: '',
                status: 'Available',
                details: '',
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
                await apiService.updateProperty(editingId, formData);
            }
            else {
                await apiService.createProperty(formData);
            }
            await fetchProperties();
            handleCloseModal();
        }
        catch (err) {
            setError(err.response?.data?.message || 'Erro ao salvar propriedade');
        }
    };
    const handleDelete = async (id) => {
        if (window.confirm('Tem certeza que deseja deletar esta propriedade?')) {
            try {
                await apiService.deleteProperty(id);
                await fetchProperties();
            }
            catch (err) {
                setError('Erro ao deletar propriedade');
            }
        }
    };
    const columns = [
        { key: 'address', label: 'Endereço', width: '40%' },
        { key: 'propertyType', label: 'Tipo', width: '20%' },
        {
            key: 'status',
            label: 'Status',
            width: '20%',
            render: (status) => (_jsx("span", { className: `badge badge-${status?.toLowerCase()}`, children: status })),
        },
        { key: 'details', label: 'Detalhes', width: '20%' },
    ];
    return (_jsx("div", { className: "crud-page", children: _jsxs("div", { className: "container", children: [_jsxs("div", { className: "page-header", children: [_jsx("h1", { children: "\uD83C\uDFE0 Propriedades" }), _jsx("button", { className: "btn btn-primary", onClick: () => handleOpenModal(), children: "+ Nova Propriedade" })] }), error && _jsx("div", { className: "alert alert-error", children: error }), _jsx(Table, { columns: columns, data: properties, loading: loading, actions: (row) => (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-sm btn-secondary", onClick: () => handleOpenModal(row), children: "Editar" }), _jsx("button", { className: "btn btn-sm btn-danger", onClick: () => handleDelete(row.id), children: "Deletar" })] })) }), _jsx(Modal, { isOpen: modalOpen, title: editingId ? 'Editar Propriedade' : 'Nova Propriedade', onClose: handleCloseModal, size: "medium", children: _jsxs("form", { onSubmit: handleSubmit, className: "form", children: [_jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "address", children: "Endere\u00E7o *" }), _jsx("input", { id: "address", type: "text", name: "address", value: formData.address || '', onChange: handleChange, placeholder: "Rua, n\u00FAmero, bairro", required: true })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "propertyType", children: "Tipo de Propriedade *" }), _jsxs("select", { id: "propertyType", name: "propertyType", value: formData.propertyType || '', onChange: handleChange, required: true, children: [_jsx("option", { value: "", children: "Selecione..." }), _jsx("option", { value: "Apartment", children: "Apartamento" }), _jsx("option", { value: "House", children: "Casa" }), _jsx("option", { value: "Commercial", children: "Comercial" }), _jsx("option", { value: "Land", children: "Terreno" })] })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "status", children: "Status *" }), _jsxs("select", { id: "status", name: "status", value: formData.status || 'Available', onChange: handleChange, children: [_jsx("option", { value: "Available", children: "Dispon\u00EDvel" }), _jsx("option", { value: "Rented", children: "Alugado" }), _jsx("option", { value: "Maintenance", children: "Manuten\u00E7\u00E3o" }), _jsx("option", { value: "Unavailable", children: "Indispon\u00EDvel" })] })] }), _jsxs("div", { className: "form-group", children: [_jsx("label", { htmlFor: "details", children: "Detalhes" }), _jsx("textarea", { id: "details", name: "details", value: formData.details || '', onChange: handleChange, placeholder: "Descri\u00E7\u00E3o adicional...", rows: 4 })] }), _jsxs("div", { className: "form-actions", children: [_jsx("button", { type: "button", className: "btn btn-outline", onClick: handleCloseModal, children: "Cancelar" }), _jsxs("button", { type: "submit", className: "btn btn-primary", children: [editingId ? 'Atualizar' : 'Criar', " Propriedade"] })] })] }) })] }) }));
};
