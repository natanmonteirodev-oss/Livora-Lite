import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import './Header.css';

export const Header: React.FC = () => {
  const { isAuthenticated, user, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <header className="header">
      <div className="container">
        <div className="header-content">
          <Link to="/" className="logo">
            <span className="logo-icon">🏠</span>
            <span className="logo-text">Livora</span>
          </Link>

          <nav className="nav">
            {isAuthenticated ? (
              <>
                <Link to="/dashboard" className="nav-link">Dashboard</Link>
                <div className="nav-dropdown">
                  <button className="nav-link dropdown-toggle">Gestão</button>
                  <div className="dropdown-menu">
                    <Link to="/properties" className="dropdown-item">Propriedades</Link>
                    <Link to="/tenants" className="dropdown-item">Inquilinos</Link>
                    <Link to="/contracts" className="dropdown-item">Contratos</Link>
                  </div>
                </div>
                <div className="nav-dropdown">
                  <button className="nav-link dropdown-toggle">Financeiro</button>
                  <div className="dropdown-menu">
                    <Link to="/billings" className="dropdown-item">Cobranças</Link>
                    <Link to="/payments" className="dropdown-item">Pagamentos</Link>
                    <Link to="/reports" className="dropdown-item">Relatórios</Link>
                  </div>
                </div>
                <Link to="/maintenance" className="nav-link">Manutenção</Link>
                <Link to="/profile" className="nav-link">{user?.firstName || 'Perfil'}</Link>
                <button className="btn btn-sm btn-outline" onClick={handleLogout}>
                  Sair
                </button>
              </>
            ) : (
              <>
                <Link to="/login" className="nav-link">Login</Link>
                <Link to="/register" className="btn btn-sm btn-primary">
                  Registrar
                </Link>
              </>
            )}
          </nav>
        </div>
      </div>
    </header>
  );
};
