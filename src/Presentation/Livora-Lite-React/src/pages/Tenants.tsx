import React, { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table, TableColumn } from '../components/Table';
import apiService from '../services/api';
import { TenantDTO } from '../types/api';
import './CRUD.css';

export const Tenants: React.FC = () => {
  const [tenants, setTenants] = useState<TenantDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<TenantDTO>>({
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
    } catch (err: any) {
      setError('Erro ao carregar inquilinos');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (tenant?: TenantDTO) => {
    if (tenant) {
      setEditingId(tenant.id);
      setFormData(tenant);
    } else {
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await apiService.updateTenant(editingId, formData);
      } else {
        await apiService.createTenant(formData);
      }
      await fetchTenants();
      handleCloseModal();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao salvar inquilino');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Tem certeza que deseja deletar este inquilino?')) {
      try {
        await apiService.deleteTenant(id);
        await fetchTenants();
      } catch (err: any) {
        setError('Erro ao deletar inquilino');
      }
    }
  };

  const columns: TableColumn<TenantDTO>[] = [
    { key: 'name', label: 'Nome', width: '35%' },
    {
      key: 'email',
      label: 'Email',
      width: '35%',
      render: (email) => <a href={`mailto:${email}`}>{email}</a>,
    },
    { key: 'phone', label: 'Telefone', width: '30%' },
  ];

  return (
    <div className="crud-page">
      <div className="container">
        <div className="page-header">
          <h1>👥 Inquilinos</h1>
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            + Novo Inquilino
          </button>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <Table<TenantDTO>
          columns={columns}
          data={tenants}
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
          title={editingId ? 'Editar Inquilino' : 'Novo Inquilino'}
          onClose={handleCloseModal}
          size="medium"
        >
          <form onSubmit={handleSubmit} className="form">
            <div className="form-group">
              <label htmlFor="name">Nome Completo *</label>
              <input
                id="name"
                type="text"
                name="name"
                value={formData.name || ''}
                onChange={handleChange}
                placeholder="Nome do inquilino"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="email">Email *</label>
              <input
                id="email"
                type="email"
                name="email"
                value={formData.email || ''}
                onChange={handleChange}
                placeholder="email@example.com"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="phone">Telefone *</label>
              <input
                id="phone"
                type="tel"
                name="phone"
                value={formData.phone || ''}
                onChange={handleChange}
                placeholder="(11) 99999-9999"
                required
              />
            </div>

            <div className="form-actions">
              <button type="button" className="btn btn-outline" onClick={handleCloseModal}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Atualizar' : 'Criar'} Inquilino
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </div>
  );
};
