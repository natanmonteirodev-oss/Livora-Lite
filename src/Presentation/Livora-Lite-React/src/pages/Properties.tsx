import React, { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table, TableColumn } from '../components/Table';
import apiService from '../services/api';
import { PropertyDTO } from '../types/api';
import './CRUD.css';

export const Properties: React.FC = () => {
  const [properties, setProperties] = useState<PropertyDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<PropertyDTO>>({
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
    } catch (err: any) {
      setError('Erro ao carregar propriedades');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (property?: PropertyDTO) => {
    if (property) {
      setEditingId(property.id);
      setFormData(property);
    } else {
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await apiService.updateProperty(editingId, formData);
      } else {
        await apiService.createProperty(formData);
      }
      await fetchProperties();
      handleCloseModal();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao salvar propriedade');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Tem certeza que deseja deletar esta propriedade?')) {
      try {
        await apiService.deleteProperty(id);
        await fetchProperties();
      } catch (err: any) {
        setError('Erro ao deletar propriedade');
      }
    }
  };

  const columns: TableColumn<PropertyDTO>[] = [
    { key: 'address', label: 'Endereço', width: '40%' },
    { key: 'propertyType', label: 'Tipo', width: '20%' },
    {
      key: 'status',
      label: 'Status',
      width: '20%',
      render: (status) => (
        <span className={`badge badge-${status?.toLowerCase()}`}>
          {status}
        </span>
      ),
    },
    { key: 'details', label: 'Detalhes', width: '20%' },
  ];

  return (
    <div className="crud-page">
      <div className="container">
        <div className="page-header">
          <h1>🏠 Propriedades</h1>
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            + Nova Propriedade
          </button>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <Table<PropertyDTO>
          columns={columns}
          data={properties}
          loading={loading}
          actions={(row) => (
            <>
              <button
                className="btn btn-sm btn-secondary"
                onClick={() => handleOpenModal(row)}
              >
                Editar
              </button>
              <button
                className="btn btn-sm btn-danger"
                onClick={() => handleDelete(row.id)}
              >
                Deletar
              </button>
            </>
          )}
        />

        <Modal
          isOpen={modalOpen}
          title={editingId ? 'Editar Propriedade' : 'Nova Propriedade'}
          onClose={handleCloseModal}
          size="medium"
        >
          <form onSubmit={handleSubmit} className="form">
            <div className="form-group">
              <label htmlFor="address">Endereço *</label>
              <input
                id="address"
                type="text"
                name="address"
                value={formData.address || ''}
                onChange={handleChange}
                placeholder="Rua, número, bairro"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="propertyType">Tipo de Propriedade *</label>
              <select
                id="propertyType"
                name="propertyType"
                value={formData.propertyType || ''}
                onChange={handleChange}
                required
              >
                <option value="">Selecione...</option>
                <option value="Apartment">Apartamento</option>
                <option value="House">Casa</option>
                <option value="Commercial">Comercial</option>
                <option value="Land">Terreno</option>
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="status">Status *</label>
              <select
                id="status"
                name="status"
                value={formData.status || 'Available'}
                onChange={handleChange}
              >
                <option value="Available">Disponível</option>
                <option value="Rented">Alugado</option>
                <option value="Maintenance">Manutenção</option>
                <option value="Unavailable">Indisponível</option>
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="details">Detalhes</label>
              <textarea
                id="details"
                name="details"
                value={formData.details || ''}
                onChange={handleChange}
                placeholder="Descrição adicional..."
                rows={4}
              />
            </div>

            <div className="form-actions">
              <button type="button" className="btn btn-outline" onClick={handleCloseModal}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Atualizar' : 'Criar'} Propriedade
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </div>
  );
};
