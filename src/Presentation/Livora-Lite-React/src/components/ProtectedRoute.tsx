import React from 'react';

interface ProtectedRouteProps {
  children: React.ReactNode;
  isAuthenticated: boolean;
}

export const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children, isAuthenticated }) => {
  if (!isAuthenticated) {
    return (
      <div className="protected-route-message">
        <div className="alert alert-info">
          <h3>Acesso Restrito</h3>
          <p>Você precisa fazer login para acessar esta página.</p>
          <a href="/login" className="btn btn-primary">
            Ir para Login
          </a>
        </div>
      </div>
    );
  }

  return <>{children}</>;
};
