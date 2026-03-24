import '../styles/TestimonialCard.css'

interface TestimonialCardProps {
  quote: string
  author: string
  title?: string
  avatar?: string
  rating?: number
}

export default function TestimonialCard({
  quote,
  author,
  title,
  avatar,
  rating = 5
}: TestimonialCardProps) {
  return (
    <div className="testimonial-card">
      {rating > 0 && (
        <div className="testimonial-rating">
          {[...Array(rating)].map((_, i) => (
            <span key={i} className="testimonial-star">★</span>
          ))}
        </div>
      )}
      
      <p className="testimonial-quote">"{quote}"</p>
      
      <div className="testimonial-author">
        {avatar && <img src={avatar} alt={author} className="testimonial-avatar" />}
        <div className="testimonial-info">
          <p className="testimonial-name">{author}</p>
          {title && <p className="testimonial-title">{title}</p>}
        </div>
      </div>
    </div>
  )
}
