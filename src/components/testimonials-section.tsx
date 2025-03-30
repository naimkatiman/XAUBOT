"use client";

import React from 'react';
import { Card, CardContent } from '@/components/ui/card';
import { Avatar, AvatarFallback, AvatarImage } from '@/components/ui/avatar';

interface TestimonialProps {
  name: string;
  role: string;
  content: string;
  avatar?: string;
  rating: number;
}

const Testimonial: React.FC<TestimonialProps> = ({
  name,
  role,
  content,
  avatar,
  rating
}) => (
  <Card className="border-gray-800 bg-gray-900/50 backdrop-blur-sm hover:bg-gray-900/70 transition-all duration-300 h-full">
    <CardContent className="p-6">
      <div className="flex justify-between items-start mb-4">
        <div className="flex items-center">
          <Avatar className="h-10 w-10 mr-4">
            <AvatarImage src={avatar} />
            <AvatarFallback className="bg-gold/20 text-gold">
              {name.charAt(0)}
            </AvatarFallback>
          </Avatar>
          <div>
            <h4 className="font-medium text-white">{name}</h4>
            <p className="text-gray-400 text-sm">{role}</p>
          </div>
        </div>
        <div className="flex">
          {[...Array(5)].map((_, i) => (
            <svg
              key={i}
              className={`w-4 h-4 ${i < rating ? 'text-gold' : 'text-gray-600'}`}
              fill="currentColor"
              viewBox="0 0 20 20"
            >
              <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
            </svg>
          ))}
        </div>
      </div>
      <p className="text-gray-300">{content}</p>
    </CardContent>
  </Card>
);

export default function TestimonialsSection() {
  const testimonials = [
    {
      name: "Michael Thompson",
      role: "Prop Firm Trader",
      content: "XAUBOT helped me pass my prop firm challenge in just 2 weeks. The automated trading is incredibly accurate and the risk management is top-notch.",
      rating: 5
    },
    {
      name: "Sarah Johnson",
      role: "Day Trader",
      content: "I was skeptical about trading bots, but XAUBOT changed my mind. The gold trading algorithms are exceptional and I've seen consistent returns.",
      rating: 4
    },
    {
      name: "David Chen",
      role: "Financial Analyst",
      content: "As someone who analyzes trading systems professionally, I can say that XAUBOT's AI approach to gold trading is cutting-edge. The results speak for themselves.",
      rating: 5
    },
    {
      name: "Emma Rodriguez",
      role: "Retail Investor",
      content: "XAUBOT made forex trading accessible to me. I don't have time to watch charts all day, but now I can still participate in the markets with confidence.",
      rating: 4
    }
  ];

  return (
    <section className="py-20 bg-darkBlue">
      <div className="container mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="text-3xl md:text-4xl font-bold mb-4">
            <span className="text-white">What Our </span>
            <span className="text-gold">Users Say</span>
          </h2>
          <p className="text-gray-300 max-w-2xl mx-auto">
            Join thousands of satisfied traders who have transformed their trading with XAUBOT.
          </p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
          {testimonials.map((testimonial, index) => (
            <Testimonial
              key={index}
              name={testimonial.name}
              role={testimonial.role}
              content={testimonial.content}
              rating={testimonial.rating}
            />
          ))}
        </div>

        <div className="mt-12 text-center">
          <div className="inline-flex items-center justify-center bg-gray-900/50 backdrop-blur-sm px-6 py-3 rounded-lg border border-gray-800">
            <div className="flex items-center mr-3">
              {[...Array(5)].map((_, i) => (
                <svg
                  key={i}
                  className="w-5 h-5 text-gold"
                  fill="currentColor"
                  viewBox="0 0 20 20"
                >
                  <path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z" />
                </svg>
              ))}
            </div>
            <span className="text-white font-medium">4.7 out of 5 stars</span>
            <span className="mx-2 text-gray-500">|</span>
            <span className="text-gray-300">Based on 75+ reviews</span>
          </div>
        </div>
      </div>
    </section>
  );
}
