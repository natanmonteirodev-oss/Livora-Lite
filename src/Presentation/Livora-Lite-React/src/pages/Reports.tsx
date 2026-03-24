import React, { useState } from 'react';
import './Reports.css';

export const Reports: React.FC = () => {
  const [reportType, setReportType] = useState('financial');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [loading, setLoading] = useState(false);

  const handleGenerateReport = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      // Implementar chamada à API para gerar relatório
      console.log('Gerando relatório:', { reportType, startDate, endDate });
      // const data = await apiService.getReports();
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="crud-page">
      <div className="container">
        <h1>📊 Relatórios</h1>

        <div className="reports-grid">
          <div className="report-card">
            <div className="report-icon">💰</div>
            <h3>Relatório Financeiro</h3>
            <p>Resumo de renda, despesas e fluxo de caixa</p>
            <button className="btn btn-sm btn-primary">Gerar</button>
          </div>

          <div className="report-card">
            <div className="report-icon">🏠</div>
            <h3>Relatório de Propriedades</h3>
            <p>Status e ocupação de todas as propriedades</p>
            <button className="btn btn-sm btn-primary">Gerar</button>
          </div>

          <div className="report-card">
            <div className="report-icon">👥</div>
            <h3>Relatório de Inquilinos</h3>
            <p>Informações e histórico dos inquilinos</p>
            <button className="btn btn-sm btn-primary">Gerar</button>
          </div>

          <div className="report-card">
            <div className="report-icon">📋</div>
            <h3>Relatório de Contratos</h3>
            <p>Resumo dos contratos ativos e expirados</p>
            <button className="btn btn-sm btn-primary">Gerar</button>
          </div>

          <div className="report-card">
            <div className="report-icon">💳</div>
            <h3>Relatório de Cobranças</h3>
            <p>Cobranças pendentes e recebidas</p>
            <button className="btn btn-sm btn-primary">Gerar</button>
          </div>

          <div className="report-card">
            <div className="report-icon">🔧</div>
            <h3>Relatório de Manutenção</h3>
            <p>Solicitações de manutenção e custos</p>
            <button className="btn btn-sm btn-primary">Gerar</button>
          </div>
        </div>

        <div className="report-filter">
          <h2>Filtrar por Período</h2>
          <form onSubmit={handleGenerateReport} className="form filter-form">
            <div className="form-row">
              <div className="form-group">
                <label htmlFor="startDate">Data Inicial</label>
                <input
                  id="startDate"
                  type="date"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                />
              </div>

              <div className="form-group">
                <label htmlFor="endDate">Data Final</label>
                <input
                  id="endDate"
                  type="date"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                />
              </div>

              <div className="form-group">
                <label htmlFor="reportType">Tipo de Relatório</label>
                <select
                  id="reportType"
                  value={reportType}
                  onChange={(e) => setReportType(e.target.value)}
                >
                  <option value="financial">Financeiro</option>
                  <option value="properties">Propriedades</option>
                  <option value="tenants">Inquilinos</option>
                  <option value="contracts">Contratos</option>
                  <option value="billings">Cobranças</option>
                  <option value="maintenance">Manutenção</option>
                </select>
              </div>
            </div>

            <button 
              type="submit" 
              className="btn btn-primary"
              disabled={loading}
            >
              {loading ? 'Gerando...' : 'Gerar Relatório'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};
