import '../styles/PricingCard.css'

interface PricingCardProps {
  name: string
  price: string | number
  description?: string
  features: string[]
  cta?: string
  featured?: boolean
  onCTA?: () => void
}

export default function PricingCard({
  name,
  price,
  description,
  features,
  cta = 'Escolher Plano',
  featured = false,
  onCTA
}: PricingCardProps) {
  return (
    <div className={`pricing-card ${featured ? 'pricing-card-featured' : ''}`}>
      {featured && <div className="pricing-badge">Mais Popular</div>}
      
      <h3 className="pricing-name">{name}</h3>
      
      {description && <p className="pricing-description">{description}</p>}
      
      <div className="pricing-price">
        <span className="pricing-currency">R$</span>
        <span className="pricing-value">{price}</span>
        <span className="pricing-period">/mês</span>
      </div>
      
      <button
        className={`btn pricing-cta ${featured ? 'btn-primary' : 'btn-outline-primary'}`}
        onClick={onCTA}
      >
        {cta}
      </button>
      
      <div className="pricing-features">
        <p className="pricing-features-label">Incluso:</p>
        <ul className="pricing-features-list">
          {features.map((feature, index) => (
            <li key={index} className="pricing-feature-item">
              <span className="feature-check">✓</span>
              {feature}
            </li>
          ))}
        </ul>
      </div>
    </div>
  )
}
