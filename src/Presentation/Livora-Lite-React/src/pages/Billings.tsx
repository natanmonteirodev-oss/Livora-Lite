import React, { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table, TableColumn } from '../components/Table';
import apiService from '../services/api';
import { BillingDTO } from '../types/api';
import './CRUD.css';

export const Billings: React.FC = () => {
  const [billings, setBillings] = useState<BillingDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<BillingDTO>>({
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
    } catch (err: any) {
      setError('Erro ao carregar cobranças');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (billing?: BillingDTO) => {
    if (billing) {
      setEditingId(billing.id);
      setFormData(billing);
    } else {
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === 'amount' ? parseFloat(value) : value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await apiService.updateBilling(editingId, formData);
      } else {
        await apiService.createBilling(formData);
      }
      await fetchBillings();
      handleCloseModal();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao salvar cobrança');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Tem certeza que deseja deletar esta cobrança?')) {
      try {
        await apiService.deleteBilling(id);
        await fetchBillings();
      } catch (err: any) {
        setError('Erro ao deletar cobrança');
      }
    }
  };

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('pt-BR');
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const columns: TableColumn<BillingDTO>[] = [
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
      render: (status) => (
        <span className={`badge badge-${status?.toLowerCase()}`}>
          {status}
        </span>
      ),
    },
  ];

  return (
    <div className="crud-page">
      <div className="container">
        <div className="page-header">
          <h1>💳 Cobranças</h1>
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            + Nova Cobrança
          </button>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <Table<BillingDTO>
          columns={columns}
          data={billings}
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
          title={editingId ? 'Editar Cobrança' : 'Nova Cobrança'}
          onClose={handleCloseModal}
          size="medium"
        >
          <form onSubmit={handleSubmit} className="form">
            <div className="form-group">
              <label htmlFor="amount">Valor (R$) *</label>
              <input
                id="amount"
                type="number"
                name="amount"
                value={formData.amount || 0}
                onChange={handleChange}
                placeholder="0.00"
                step="0.01"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="dueDate">Data de Vencimento *</label>
              <input
                id="dueDate"
                type="date"
                name="dueDate"
                value={formData.dueDate || ''}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="status">Status *</label>
              <select
                id="status"
                name="status"
                value={formData.status || 'Pending'}
                onChange={handleChange}
              >
                <option value="Pending">Pendente</option>
                <option value="Paid">Pago</option>
                <option value="Overdue">Vencido</option>
              </select>
            </div>

            <div className="form-actions">
              <button type="button" className="btn btn-outline" onClick={handleCloseModal}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Atualizar' : 'Criar'} Cobrança
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </div>
  );
};
