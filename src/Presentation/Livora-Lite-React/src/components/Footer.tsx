import React from 'react';
import './Footer.css';

export const Footer: React.FC = () => {
  const currentYear = new Date().getFullYear();

  return (
    <footer className="footer">
      <div className="container">
        <div className="footer-content">
          <div className="footer-section">
            <h3>Livora</h3>
            <p>Simplificando a gestão de aluguéis no Brasil</p>
          </div>
          <div className="footer-section">
            <h4>Links Úteis</h4>
            <ul>
              <li><a href="#about">Sobre</a></li>
              <li><a href="#features">Recursos</a></li>
              <li><a href="#contact">Contato</a></li>
            </ul>
          </div>
          <div className="footer-section">
            <h4>Suporte</h4>
            <ul>
              <li><a href="#support">Ajuda</a></li>
              <li><a href="#privacy">Privacidade</a></li>
              <li><a href="#terms">Termos</a></li>
            </ul>
          </div>
        </div>
        <div className="footer-bottom">
          <p>&copy; {currentYear} Livora. Todos os direitos reservados.</p>
        </div>
      </div>
    </footer>
  );
};
