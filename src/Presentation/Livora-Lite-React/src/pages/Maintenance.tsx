import React, { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table, TableColumn } from '../components/Table';
import apiService from '../services/api';
import { MaintenanceRequestDTO } from '../types/api';
import './CRUD.css';

export const Maintenance: React.FC = () => {
  const [requests, setRequests] = useState<MaintenanceRequestDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<MaintenanceRequestDTO>>({
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
    } catch (err: any) {
      setError('Erro ao carregar solicitações');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (request?: MaintenanceRequestDTO) => {
    if (request) {
      setEditingId(request.id);
      setFormData(request);
    } else {
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
        await apiService.updateMaintenanceRequest(editingId, formData);
      } else {
        await apiService.createMaintenanceRequest(formData);
      }
      await fetchRequests();
      handleCloseModal();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao salvar solicitação');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Tem certeza que deseja deletar esta solicitação?')) {
      try {
        await apiService.deleteMaintenanceRequest(id);
        await fetchRequests();
      } catch (err: any) {
        setError('Erro ao deletar solicitação');
      }
    }
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('pt-BR');
  };

  const getPriorityBadgeClass = (priority: string) => {
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

  const columns: TableColumn<MaintenanceRequestDTO>[] = [
    { key: 'description', label: 'Descrição', width: '35%' },
    {
      key: 'priority',
      label: 'Prioridade',
      width: '20%',
      render: (priority) => (
        <span className={`badge ${getPriorityBadgeClass(priority)}`}>
          {priority}
        </span>
      ),
    },
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
    {
      key: 'createdDate',
      label: 'Data de Criação',
      width: '25%',
      render: (date) => formatDate(date),
    },
  ];

  return (
    <div className="crud-page">
      <div className="container">
        <div className="page-header">
          <h1>🔧 Manutenção</h1>
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            + Nova Solicitação
          </button>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <Table<MaintenanceRequestDTO>
          columns={columns}
          data={requests}
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
          title={editingId ? 'Editar Solicitação' : 'Nova Solicitação'}
          onClose={handleCloseModal}
          size="medium"
        >
          <form onSubmit={handleSubmit} className="form">
            <div className="form-group">
              <label htmlFor="description">Descrição *</label>
              <textarea
                id="description"
                name="description"
                value={formData.description || ''}
                onChange={handleChange}
                placeholder="Descreva a solicitação de manutenção..."
                rows={4}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="priority">Prioridade *</label>
              <select
                id="priority"
                name="priority"
                value={formData.priority || 'Medium'}
                onChange={handleChange}
              >
                <option value="Low">Baixa</option>
                <option value="Medium">Média</option>
                <option value="High">Alta</option>
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="status">Status *</label>
              <select
                id="status"
                name="status"
                value={formData.status || 'Open'}
                onChange={handleChange}
              >
                <option value="Open">Aberta</option>
                <option value="In Progress">Em Andamento</option>
                <option value="Completed">Concluída</option>
                <option value="Cancelled">Cancelada</option>
              </select>
            </div>

            <div className="form-actions">
              <button type="button" className="btn btn-outline" onClick={handleCloseModal}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Atualizar' : 'Criar'} Solicitação
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </div>
  );
};
