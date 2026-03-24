import { jsx as _jsx, jsxs as _jsxs, Fragment as _Fragment } from "react/jsx-runtime";
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import Section from '../components/Section';
import FeatureCard from '../components/FeatureCard';
import TestimonialCard from '../components/TestimonialCard';
import PricingCard from '../components/PricingCard';
import MainCard from '../components/MainCard';
import { IconHome, IconFileText, IconChartBar, IconLock, IconPhone, IconHeadphones, IconDashboard, IconUsers, IconCash, IconCalendar } from '@tabler/icons-react';
import '../styles/Landing.css';
export default function Landing() {
    const navigate = useNavigate();
    const { isAuthenticated } = useAuth();
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
    ]);
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
    ]);
    return (_jsxs("div", { className: "landing-page", children: [_jsx("section", { className: "hero-section", children: _jsx("div", { className: "container", children: _jsxs("div", { className: "hero-content", children: [_jsxs("div", { className: "hero-text", children: [_jsxs("h1", { className: "hero-title", children: ["Gest\u00E3o de Im\u00F3veis ", _jsx("span", { className: "hero-highlight", children: "Simplificada" })] }), _jsx("p", { className: "hero-subtitle", children: "Simplifique a rela\u00E7\u00E3o entre pessoas e im\u00F3veis. Clareza, controle e confian\u00E7a para aluguel de longa dura\u00E7\u00E3o no Brasil." }), _jsx("div", { className: "hero-actions", children: isAuthenticated ? (_jsx("button", { className: "btn btn-primary btn-lg", onClick: () => navigate('/dashboard'), children: "Acessar Dashboard" })) : (_jsxs(_Fragment, { children: [_jsx("button", { className: "btn btn-primary btn-lg", onClick: () => navigate('/register'), children: "Come\u00E7ar Agora" }), _jsx("button", { className: "btn btn-outline-primary btn-lg", onClick: () => document.getElementById('features')?.scrollIntoView({ behavior: 'smooth' }), children: "Saiba Mais \u2192" })] })) }), _jsx("p", { className: "hero-badge", children: "\u2728 Junte-se a mais de 5 mil usu\u00E1rios que confiam em Livora" })] }), _jsx("div", { className: "hero-visual", children: _jsx("div", { className: "hero-illustration", children: _jsx(IconDashboard, { size: 200, className: "hero-icon" }) }) })] }) }) }), _jsx(Section, { title: "Por que escolher Livora?", subtitle: "Tudo que voc\u00EA precisa para gerenciar im\u00F3veis, inquilinos e cobran\u00E7as em um \u00FAnico lugar", centered: true, darker: true, paddingSize: "xl", children: _jsxs("div", { className: "grid grid-3", id: "features", children: [_jsx(FeatureCard, { icon: _jsx(IconHome, { size: 32 }), title: "Gerenciamento de Im\u00F3veis", description: "Cadastre e acompanhe todos os seus im\u00F3veis em um s\u00F3 lugar de forma organizada e eficiente", iconColor: "primary", iconSize: "lg" }), _jsx(FeatureCard, { icon: _jsx(IconFileText, { size: 32 }), title: "Contratos Digitais", description: "Crie, gerencie e acompanhe contratos de aluguel com seguran\u00E7a e transpar\u00EAncia", iconColor: "secondary", iconSize: "lg" }), _jsx(FeatureCard, { icon: _jsx(IconChartBar, { size: 32 }), title: "Relat\u00F3rios Detalhados", description: "Acompanhe cobran\u00E7as, pagamentos e tudo mais com relat\u00F3rios em tempo real", iconColor: "success", iconSize: "lg" }), _jsx(FeatureCard, { icon: _jsx(IconLock, { size: 32 }), title: "Seguran\u00E7a", description: "Seus dados est\u00E3o seguros com criptografia e autentica\u00E7\u00E3o moderna", iconColor: "info", iconSize: "lg" }), _jsx(FeatureCard, { icon: _jsx(IconPhone, { size: 32 }), title: "Acesso Mobile", description: "Gerencie seus im\u00F3veis de qualquer lugar, a qualquer hora", iconColor: "warning", iconSize: "lg" }), _jsx(FeatureCard, { icon: _jsx(IconHeadphones, { size: 32 }), title: "Suporte 24/7", description: "Estamos aqui para ajudar voc\u00EA sempre que precisar", iconColor: "error", iconSize: "lg" })] }) }), _jsx(Section, { title: "Funcionalidades Principais", subtitle: "Tudo que voc\u00EA precisa em uma \u00FAnica plataforma", centered: true, paddingSize: "lg", children: _jsxs("div", { className: "grid grid-3", children: [_jsx(MainCard, { hover: true, shadow: "md", children: _jsxs("div", { style: { textAlign: 'center' }, children: [_jsx(IconUsers, { size: 48, style: { color: 'var(--primary-main)', marginBottom: '16px' } }), _jsx("h3", { style: { fontSize: 'var(--text-lg)', marginBottom: '8px' }, children: "Ger\u00EAncia de Tenants" }), _jsx("p", { style: { fontSize: 'var(--text-sm)', color: 'var(--text-secondary)' }, children: "Mantenha todos os dados de inquilinos organizados e atualizados" })] }) }), _jsx(MainCard, { hover: true, shadow: "md", children: _jsxs("div", { style: { textAlign: 'center' }, children: [_jsx(IconCash, { size: 48, style: { color: 'var(--success-main)', marginBottom: '16px' } }), _jsx("h3", { style: { fontSize: 'var(--text-lg)', marginBottom: '8px' }, children: "Cobran\u00E7as Autom\u00E1ticas" }), _jsx("p", { style: { fontSize: 'var(--text-sm)', color: 'var(--text-secondary)' }, children: "Automatize o processo de cobran\u00E7as e acompanhe pagamentos" })] }) }), _jsx(MainCard, { hover: true, shadow: "md", children: _jsxs("div", { style: { textAlign: 'center' }, children: [_jsx(IconCalendar, { size: 48, style: { color: 'var(--warning-main)', marginBottom: '16px' } }), _jsx("h3", { style: { fontSize: 'var(--text-lg)', marginBottom: '8px' }, children: "Cronograma" }), _jsx("p", { style: { fontSize: 'var(--text-sm)', color: 'var(--text-secondary)' }, children: "Gerencie renova\u00E7\u00F5es de contratos e lembretes importantes" })] }) })] }) }), _jsx(Section, { title: "O que nossos usu\u00E1rios dizem", subtitle: "Confira as experi\u00EAncias reais de quem usa Livora", centered: true, darker: true, paddingSize: "lg", children: _jsx("div", { className: "grid grid-3", children: testimonials.map((testimonial, index) => (_jsx(TestimonialCard, { quote: testimonial.quote, author: testimonial.author, title: testimonial.title, avatar: testimonial.avatar, rating: 5 }, index))) }) }), _jsx(Section, { title: "Planos e Pre\u00E7os", subtitle: "Escolha o plano perfeito para suas necessidades", centered: true, paddingSize: "xl", children: _jsx("div", { className: "pricing-grid", children: pricingPlans.map((plan, index) => (_jsx(PricingCard, { name: plan.name, price: plan.price, description: plan.description, features: plan.features, featured: plan.featured || false, onCTA: () => navigate('/register') }, index))) }) }), _jsx(Section, { className: "cta-section", paddingSize: "xl", children: _jsx("div", { className: "cta-container", children: _jsxs("div", { className: "cta-content", children: [_jsx("h2", { children: "Pronto para simplificar a gest\u00E3o de seus im\u00F3veis?" }), _jsx("p", { children: "Comece hoje mesmo e tenha 30 dias de trial gr\u00E1tis, sem necessidade de cart\u00E3o de cr\u00E9dito." }), _jsxs("div", { className: "cta-actions", children: [_jsx("button", { className: "btn btn-primary btn-lg", onClick: () => navigate('/register'), children: "Criar Conta Gratuitamente" }), _jsx("button", { className: "btn btn-ghost btn-lg", onClick: () => navigate('/login'), children: "J\u00E1 tenho conta \u2192" })] })] }) }) }), _jsx(Section, { className: "stats-section", darker: true, paddingSize: "lg", children: _jsxs("div", { className: "stats-grid", children: [_jsxs("div", { className: "stat-item", children: [_jsx("div", { className: "stat-number", children: "5K+" }), _jsx("p", { className: "stat-label", children: "Usu\u00E1rios Ativos" })] }), _jsxs("div", { className: "stat-item", children: [_jsx("div", { className: "stat-number", children: "50K+" }), _jsx("p", { className: "stat-label", children: "Im\u00F3veis Gerenciados" })] }), _jsxs("div", { className: "stat-item", children: [_jsx("div", { className: "stat-number", children: "R$ 2B+" }), _jsx("p", { className: "stat-label", children: "Cobran\u00E7as Processadas" })] }), _jsxs("div", { className: "stat-item", children: [_jsx("div", { className: "stat-number", children: "99.9%" }), _jsx("p", { className: "stat-label", children: "Uptime" })] })] }) })] }));
}
