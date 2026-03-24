import React from 'react'
import '../styles/MainCard.css'

interface MainCardProps {
  children: React.ReactNode
  className?: string
  onClick?: () => void
  hover?: boolean
  shadow?: 'sm' | 'md' | 'lg' | 'xl'
}

export default function MainCard({
  children,
  className = '',
  onClick,
  hover = true,
  shadow = 'md'
}: MainCardProps) {
  return (
    <div
      className={`main-card main-card-shadow-${shadow} ${hover ? 'main-card-hover' : ''} ${className}`}
      onClick={onClick}
    >
      {children}
    </div>
  )
}
