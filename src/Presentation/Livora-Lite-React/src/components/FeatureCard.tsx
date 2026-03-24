import React from 'react'
import '../styles/FeatureCard.css'

interface FeatureCardProps {
  icon?: React.ReactNode
  title: string
  description: string
  iconColor?: 'primary' | 'secondary' | 'success' | 'warning' | 'error' | 'info'
  iconSize?: 'sm' | 'md' | 'lg'
}

export default function FeatureCard({
  icon,
  title,
  description,
  iconColor = 'primary',
  iconSize = 'md'
}: FeatureCardProps) {
  return (
    <div className="feature-card">
      {icon && (
        <div className={`feature-card-icon feature-card-icon-${iconColor} icon-size-${iconSize}`}>
          {icon}
        </div>
      )}
      <h3 className="feature-card-title">{title}</h3>
      <p className="feature-card-description">{description}</p>
    </div>
  )
}
