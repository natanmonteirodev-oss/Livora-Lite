import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import Section from '../components/Section'
import FeatureCard from '../components/FeatureCard'
import TestimonialCard from '../components/TestimonialCard'
import PricingCard from '../components/PricingCard'
import MainCard from '../components/MainCard'
import {
  IconHome,
  IconFileText,
  IconChartBar,
  IconLock,
  IconPhone,
  IconHeadphones,
  IconDashboard,
  IconUsers,
  IconCash,
  IconCalendar
} from '@tabler/icons-react'
import '../styles/Landing.css'

export default function Landing() {
  const navigate = useNavigate()
  const { isAuthenticated } = useAuth()
  const [pricingPlans] = useState([
    {
      name: 'Básico',
      price: '49',
      description: 'Perfeito para começar',
      features: [
        'Até 5 imóveis',
        'Gerenciamento de inquilinos',
        'Contratos digitais',
        'Relatórios básicos',
        'Suporte por email'
      ]
    },
    {
      name: 'Profissional',
      price: '99',
      description: 'Para profissionais',
      featured: true,
      features: [
        'Imóveis ilimitados',
        'Gerenciamento completo',
        'Contratos avançados',
        'Relatórios detalhados',
        'Cobranças automáticas',
        'Suporte prioritário',
        'API integração'
      ]
    },
    {
      name: 'Corporativo',
      price: '199',
      description: 'Para grandes operações',
      features: [
        'Todas as features',
        'Múltiplos usuários',
        'Dashboard avançado',
        'Análises de dados',
        'Automações customizadas',
        'Suporte 24/7',
        'Integração completa'
      ]
    }
  ])

  const [testimonials] = useState([
    {
      quote: 'Livora transformou a forma como gerencio meus imóveis. A plataforma é intuitiva e eficiente!',
      author: 'Maria Silva',
      title: 'Proprietária, São Paulo',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Maria'
    },
    {
      quote: 'Melhor investimento que já fiz para meu negócio. Economizei tempo e dinheiro.',
      author: 'João Santos',
      title: 'Gerenciador de Imóveis, Rio de Janeiro',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Joao'
    },
    {
      quote: 'Super recomendo! Contatos e suporte incríveis, além de uma plataforma robusta.',
      author: 'Ana Costa',
      title: 'Investidora Imobiliária, Belo Horizonte',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Ana'
    }
  ])

  return (
    <div className="landing-page">
      {/* Hero Section */}
      <section className="hero-section">
        <div className="container">
          <div className="hero-content">
            <div className="hero-text">
              <h1 className="hero-title">
                Gestão de Imóveis <span className="hero-highlight">Simplificada</span>
              </h1>
              <p className="hero-subtitle">
                Simplifique a relação entre pessoas e imóveis. Clareza, controle e confiança para aluguel de longa duração no Brasil.
              </p>
              <div className="hero-actions">
                {isAuthenticated ? (
                  <button
                    className="btn btn-primary btn-lg"
                    onClick={() => navigate('/dashboard')}
                  >
                    Acessar Dashboard
                  </button>
                ) : (
                  <>
                    <button
                      className="btn btn-primary btn-lg"
                      onClick={() => navigate('/register')}
                    >
                      Começar Agora
                    </button>
                    <button
                      className="btn btn-outline-primary btn-lg"
                      onClick={() => document.getElementById('features')?.scrollIntoView({ behavior: 'smooth' })}
                    >
                      Saiba Mais →
                    </button>
                  </>
                )}
              </div>
              <p className="hero-badge">✨ Junte-se a mais de 5 mil usuários que confiam em Livora</p>
            </div>
            <div className="hero-visual">
              <div className="hero-illustration">
                <IconDashboard size={200} className="hero-icon" />
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <Section
        title="Por que escolher Livora?"
        subtitle="Tudo que você precisa para gerenciar imóveis, inquilinos e cobranças em um único lugar"
        centered
        darker
        paddingSize="xl"
      >
        <div className="grid grid-3" id="features">
          <FeatureCard
            icon={<IconHome size={32} />}
            title="Gerenciamento de Imóveis"
            description="Cadastre e acompanhe todos os seus imóveis em um só lugar de forma organizada e eficiente"
            iconColor="primary"
            iconSize="lg"
          />
          <FeatureCard
            icon={<IconFileText size={32} />}
            title="Contratos Digitais"
            description="Crie, gerencie e acompanhe contratos de aluguel com segurança e transparência"
            iconColor="secondary"
            iconSize="lg"
          />
          <FeatureCard
            icon={<IconChartBar size={32} />}
            title="Relatórios Detalhados"
            description="Acompanhe cobranças, pagamentos e tudo mais com relatórios em tempo real"
            iconColor="success"
            iconSize="lg"
          />
          <FeatureCard
            icon={<IconLock size={32} />}
            title="Segurança"
            description="Seus dados estão seguros com criptografia e autenticação moderna"
            iconColor="info"
            iconSize="lg"
          />
          <FeatureCard
            icon={<IconPhone size={32} />}
            title="Acesso Mobile"
            description="Gerencie seus imóveis de qualquer lugar, a qualquer hora"
            iconColor="warning"
            iconSize="lg"
          />
          <FeatureCard
            icon={<IconHeadphones size={32} />}
            title="Suporte 24/7"
            description="Estamos aqui para ajudar você sempre que precisar"
            iconColor="error"
            iconSize="lg"
          />
        </div>
      </Section>

      {/* Key Features Section */}
      <Section
        title="Funcionalidades Principais"
        subtitle="Tudo que você precisa em uma única plataforma"
        centered
        paddingSize="lg"
      >
        <div className="grid grid-3">
          <MainCard hover shadow="md">
            <div style={{ textAlign: 'center' }}>
              <IconUsers size={48} style={{ color: 'var(--primary-main)', marginBottom: '16px' }} />
              <h3 style={{ fontSize: 'var(--text-lg)', marginBottom: '8px' }}>Gerência de Tenants</h3>
              <p style={{ fontSize: 'var(--text-sm)', color: 'var(--text-secondary)' }}>
                Mantenha todos os dados de inquilinos organizados e atualizados
              </p>
            </div>
          </MainCard>
          <MainCard hover shadow="md">
            <div style={{ textAlign: 'center' }}>
              <IconCash size={48} style={{ color: 'var(--success-main)', marginBottom: '16px' }} />
              <h3 style={{ fontSize: 'var(--text-lg)', marginBottom: '8px' }}>Cobranças Automáticas</h3>
              <p style={{ fontSize: 'var(--text-sm)', color: 'var(--text-secondary)' }}>
                Automatize o processo de cobranças e acompanhe pagamentos
              </p>
            </div>
          </MainCard>
          <MainCard hover shadow="md">
            <div style={{ textAlign: 'center' }}>
              <IconCalendar size={48} style={{ color: 'var(--warning-main)', marginBottom: '16px' }} />
              <h3 style={{ fontSize: 'var(--text-lg)', marginBottom: '8px' }}>Cronograma</h3>
              <p style={{ fontSize: 'var(--text-sm)', color: 'var(--text-secondary)' }}>
                Gerencie renovações de contratos e lembretes importantes
              </p>
            </div>
          </MainCard>
        </div>
      </Section>

      {/* Testimonials Section */}
      <Section
        title="O que nossos usuários dizem"
        subtitle="Confira as experiências reais de quem usa Livora"
        centered
        darker
        paddingSize="lg"
      >
        <div className="grid grid-3">
          {testimonials.map((testimonial, index) => (
            <TestimonialCard
              key={index}
              quote={testimonial.quote}
              author={testimonial.author}
              title={testimonial.title}
              avatar={testimonial.avatar}
              rating={5}
            />
          ))}
        </div>
      </Section>

      {/* Pricing Section */}
      <Section
        title="Planos e Preços"
        subtitle="Escolha o plano perfeito para suas necessidades"
        centered
        paddingSize="xl"
      >
        <div className="pricing-grid">
          {pricingPlans.map((plan, index) => (
            <PricingCard
              key={index}
              name={plan.name}
              price={plan.price}
              description={plan.description}
              features={plan.features}
              featured={plan.featured || false}
              onCTA={() => navigate('/register')}
            />
          ))}
        </div>
      </Section>

      {/* CTA Section */}
      <Section className="cta-section" paddingSize="xl">
        <div className="cta-container">
          <div className="cta-content">
            <h2>Pronto para simplificar a gestão de seus imóveis?</h2>
            <p>Comece hoje mesmo e tenha 30 dias de trial grátis, sem necessidade de cartão de crédito.</p>
            <div className="cta-actions">
              <button
                className="btn btn-primary btn-lg"
                onClick={() => navigate('/register')}
              >
                Criar Conta Gratuitamente
              </button>
              <button className="btn btn-ghost btn-lg" onClick={() => navigate('/login')}>
                Já tenho conta →
              </button>
            </div>
          </div>
        </div>
      </Section>

      {/* Stats Section */}
      <Section className="stats-section" darker paddingSize="lg">
        <div className="stats-grid">
          <div className="stat-item">
            <div className="stat-number">5K+</div>
            <p className="stat-label">Usuários Ativos</p>
          </div>
          <div className="stat-item">
            <div className="stat-number">50K+</div>
            <p className="stat-label">Imóveis Gerenciados</p>
          </div>
          <div className="stat-item">
            <div className="stat-number">R$ 2B+</div>
            <p className="stat-label">Cobranças Processadas</p>
          </div>
          <div className="stat-item">
            <div className="stat-number">99.9%</div>
            <p className="stat-label">Uptime</p>
          </div>
        </div>
      </Section>
    </div>
  )
}
