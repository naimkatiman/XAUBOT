"use client";

import React from 'react';
import { Button } from '@/components/ui/button';

export default function CTASection() {
  return (
    <section className="py-20 relative overflow-hidden">
      {/* Background */}
      <div className="absolute inset-0 bg-gradient-to-r from-darkBlue via-gray-900 to-black"></div>

      {/* Gold decoration */}
      <div className="absolute top-0 left-0 w-full h-px bg-gradient-to-r from-transparent via-gold to-transparent opacity-30"></div>
      <div className="absolute bottom-0 left-0 w-full h-px bg-gradient-to-r from-transparent via-gold to-transparent opacity-30"></div>

      {/* Decorative elements */}
      <div className="absolute inset-0 opacity-10">
        <div className="absolute top-10 right-10 w-64 h-64 rounded-full bg-gradient-to-r from-gold to-amber-400 blur-3xl"></div>
        <div className="absolute bottom-10 left-10 w-64 h-64 rounded-full bg-gradient-to-r from-cyan to-blue-400 blur-3xl"></div>
      </div>

      <div className="container mx-auto px-4 relative z-10">
        <div className="max-w-4xl mx-auto text-center">
          <h2 className="text-3xl md:text-5xl font-bold mb-6">
            <span className="text-white">Ready to Transform Your </span>
            <span className="text-gold">Trading?</span>
          </h2>

          <p className="text-gray-300 text-lg md:text-xl mb-10 max-w-2xl mx-auto">
            Join thousands of traders around the world who are using XAUBOT's AI-powered technology to achieve consistent results in the forex market.
          </p>

          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <Button className="bg-gold text-black hover:bg-gold/90 text-lg py-6 px-10">
              Get Started Now
            </Button>
            <Button variant="outline" className="border-white text-white hover:bg-white/10 text-lg py-6 px-10">
              View Pricing
            </Button>
          </div>

          <div className="mt-12 flex flex-col sm:flex-row items-center justify-center gap-8 text-sm text-gray-400">
            <div className="flex items-center">
              <svg className="w-5 h-5 mr-2 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
              <span>30-Day Money Back Guarantee</span>
            </div>
            <div className="flex items-center">
              <svg className="w-5 h-5 mr-2 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
              <span>24/7 Technical Support</span>
            </div>
            <div className="flex items-center">
              <svg className="w-5 h-5 mr-2 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
              <span>Lifetime Updates</span>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
