import React, { useEffect, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import apiService from '../services/api';
import { DashboardDTO } from '../types/api';
import './Dashboard.css';

export const Dashboard: React.FC = () => {
  const { user } = useAuth();
  const [dashboard, setDashboard] = useState<DashboardDTO | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchDashboard = async () => {
      try {
        const data = await apiService.getDashboard();
        setDashboard(data);
      } catch (err: any) {
        setError('Erro ao carregar dashboard');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchDashboard();
  }, []);

  if (loading) {
    return (
      <div className="dashboard">
        <div className="container">
          <div className="flex-center" style={{ minHeight: '60vh' }}>
            <div className="loading"></div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="dashboard">
      <div className="container">
        <div className="dashboard-header">
          <h1>Bem-vindo, {user?.firstName || 'Usuário'}!</h1>
          <p>Acompanhe um resumo de suas atividades abaixo</p>
        </div>

        {error && <div className="alert alert-error">{error}</div>}

        {dashboard && (
          <div className="dashboard-grid">
            <div className="dashboard-card">
              <div className="card-icon">🏠</div>
              <div className="card-content">
                <h3>Propriedades</h3>
                <p className="card-value">{dashboard.totalProperties || 0}</p>
              </div>
            </div>

            <div className="dashboard-card">
              <div className="card-icon">👥</div>
              <div className="card-content">
                <h3>Inquilinos</h3>
                <p className="card-value">{dashboard.totalTenants || 0}</p>
              </div>
            </div>

            <div className="dashboard-card">
              <div className="card-icon">💰</div>
              <div className="card-content">
                <h3>Renda Total</h3>
                <p className="card-value">
                  R$ {(dashboard.totalIncome || 0).toLocaleString('pt-BR', {
                    minimumFractionDigits: 2,
                  })}
                </p>
              </div>
            </div>

            <div className="dashboard-card warning">
              <div className="card-icon">⏰</div>
              <div className="card-content">
                <h3>Pagamentos Pendentes</h3>
                <p className="card-value">{dashboard.pendingPayments || 0}</p>
              </div>
            </div>
          </div>
        )}

        <div className="dashboard-sections">
          <section className="dashboard-section">
            <h2>Próximas Ações</h2>
            <div className="action-list">
              <a href="/properties" className="action-item">
                <span className="action-icon">📊</span>
                <span className="action-text">Gerenciar Propriedades</span>
                <span className="action-arrow">→</span>
              </a>
              <a href="/tenants" className="action-item">
                <span className="action-icon">👥</span>
                <span className="action-text">Gerenciar Inquilinos</span>
                <span className="action-arrow">→</span>
              </a>
              <a href="/contracts" className="action-item">
                <span className="action-icon">📋</span>
                <span className="action-text">Visualizar Contratos</span>
                <span className="action-arrow">→</span>
              </a>
              <a href="/billings" className="action-item">
                <span className="action-icon">💳</span>
                <span className="action-text">Gerenciar Cobranças</span>
                <span className="action-arrow">→</span>
              </a>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
};
