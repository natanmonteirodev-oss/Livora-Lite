import React, { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table, TableColumn } from '../components/Table';
import apiService from '../services/api';
import { ContractDTO } from '../types/api';
import './CRUD.css';

export const Contracts: React.FC = () => {
  const [contracts, setContracts] = useState<ContractDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<ContractDTO>>({
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
    } catch (err: any) {
      setError('Erro ao carregar contratos');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (contract?: ContractDTO) => {
    if (contract) {
      setEditingId(contract.id);
      setFormData(contract);
    } else {
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

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: name === 'rentalAmount' ? parseFloat(value) : value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await apiService.updateContract(editingId, formData);
      } else {
        await apiService.createContract(formData);
      }
      await fetchContracts();
      handleCloseModal();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao salvar contrato');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Tem certeza que deseja deletar este contrato?')) {
      try {
        await apiService.deleteContract(id);
        await fetchContracts();
      } catch (err: any) {
        setError('Erro ao deletar contrato');
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

  const columns: TableColumn<ContractDTO>[] = [
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
          <h1>📋 Contratos</h1>
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            + Novo Contrato
          </button>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <Table<ContractDTO>
          columns={columns}
          data={contracts}
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
          title={editingId ? 'Editar Contrato' : 'Novo Contrato'}
          onClose={handleCloseModal}
          size="medium"
        >
          <form onSubmit={handleSubmit} className="form">
            <div className="form-group">
              <label htmlFor="startDate">Data de Início *</label>
              <input
                id="startDate"
                type="date"
                name="startDate"
                value={formData.startDate || ''}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="endDate">Data de Término *</label>
              <input
                id="endDate"
                type="date"
                name="endDate"
                value={formData.endDate || ''}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="rentalAmount">Valor do Aluguel (R$) *</label>
              <input
                id="rentalAmount"
                type="number"
                name="rentalAmount"
                value={formData.rentalAmount || 0}
                onChange={handleChange}
                placeholder="1000.00"
                step="0.01"
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="status">Status *</label>
              <select
                id="status"
                name="status"
                value={formData.status || 'Active'}
                onChange={handleChange}
              >
                <option value="Active">Ativo</option>
                <option value="Expired">Expirado</option>
                <option value="Terminated">Encerrado</option>
              </select>
            </div>

            <div className="form-actions">
              <button type="button" className="btn btn-outline" onClick={handleCloseModal}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Atualizar' : 'Criar'} Contrato
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </div>
  );
};
