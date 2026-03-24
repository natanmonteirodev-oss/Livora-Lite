import React, { useState, useEffect } from 'react';
import { Modal } from '../components/Modal';
import { Table, TableColumn } from '../components/Table';
import apiService from '../services/api';
import { PaymentDTO } from '../types/api';
import './CRUD.css';

export const Payments: React.FC = () => {
  const [payments, setPayments] = useState<PaymentDTO[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<PaymentDTO>>({
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
    } catch (err: any) {
      setError('Erro ao carregar pagamentos');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenModal = (payment?: PaymentDTO) => {
    if (payment) {
      setEditingId(payment.id);
      setFormData(payment);
    } else {
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
        await apiService.updatePayment(editingId, formData);
      } else {
        await apiService.createPayment(formData);
      }
      await fetchPayments();
      handleCloseModal();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao salvar pagamento');
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Tem certeza que deseja deletar este pagamento?')) {
      try {
        await apiService.deletePayment(id);
        await fetchPayments();
      } catch (err: any) {
        setError('Erro ao deletar pagamento');
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

  const columns: TableColumn<PaymentDTO>[] = [
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

  return (
    <div className="crud-page">
      <div className="container">
        <div className="page-header">
          <h1>💰 Pagamentos</h1>
          <button className="btn btn-primary" onClick={() => handleOpenModal()}>
            + Novo Pagamento
          </button>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        <Table<PaymentDTO>
          columns={columns}
          data={payments}
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
          title={editingId ? 'Editar Pagamento' : 'Novo Pagamento'}
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
              <label htmlFor="paymentDate">Data do Pagamento *</label>
              <input
                id="paymentDate"
                type="date"
                name="paymentDate"
                value={formData.paymentDate || ''}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="method">Método de Pagamento *</label>
              <select
                id="method"
                name="method"
                value={formData.method || 'Credit Card'}
                onChange={handleChange}
              >
                <option value="Credit Card">Cartão de Crédito</option>
                <option value="Debit Card">Cartão de Débito</option>
                <option value="Bank Transfer">Transferência Bancária</option>
                <option value="Cash">Dinheiro</option>
                <option value="Check">Cheque</option>
              </select>
            </div>

            <div className="form-actions">
              <button type="button" className="btn btn-outline" onClick={handleCloseModal}>
                Cancelar
              </button>
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Atualizar' : 'Criar'} Pagamento
              </button>
            </div>
          </form>
        </Modal>
      </div>
    </div>
  );
};
