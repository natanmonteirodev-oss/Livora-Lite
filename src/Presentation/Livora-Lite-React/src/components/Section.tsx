import React from 'react'
import '../styles/Section.css'

interface SectionProps {
  children: React.ReactNode
  className?: string
  title?: string
  subtitle?: string
  darker?: boolean
  centered?: boolean
  paddingSize?: 'sm' | 'md' | 'lg' | 'xl'
}

export default function Section({
  children,
  className = '',
  title,
  subtitle,
  darker = false,
  centered = false,
  paddingSize = 'lg'
}: SectionProps) {
  return (
    <section className={`section section-padding-${paddingSize} ${darker ? 'section-darker' : ''} ${className}`}>
      <div className="container">
        {(title || subtitle) && (
          <div className={`section-header ${centered ? 'section-header-centered' : ''}`}>
            {title && <h2 className="section-title">{title}</h2>}
            {subtitle && <p className="section-subtitle">{subtitle}</p>}
          </div>
        )}
        {children}
      </div>
    </section>
  )
}
