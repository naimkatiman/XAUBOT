import React from 'react';
import HeroSection from '@/components/hero-section';
import FeaturesSection from '@/components/features-section';
import TestimonialsSection from '@/components/testimonials-section';
import CTASection from '@/components/cta-section';
import MarketDashboard from '@/components/market-dashboard';

export default function Home() {
  return (
    <React.Fragment>
      <HeroSection />
      <MarketDashboard />
      <FeaturesSection />
      <TestimonialsSection />
      <CTASection />
    </React.Fragment>
  );
}
