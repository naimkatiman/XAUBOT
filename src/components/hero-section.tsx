"use client";

import React from 'react';
import { Button } from '@/components/ui/button';
import Link from 'next/link';

export default function HeroSection() {
  return (
    <section className="relative bg-darkBlue py-20 overflow-hidden min-h-[90vh] flex items-center">
      {/* Enhanced background decoration with more modern effects */}
      <div className="absolute inset-0 hero-gradient"></div>
      <div className="absolute inset-0 opacity-30">
        <div className="absolute top-20 left-10 w-16 h-16 rounded-full bg-gold blur-xl animate-pulse-gold"></div>
        <div className="absolute bottom-40 right-20 w-20 h-20 rounded-full bg-cyan blur-xl animate-pulse"></div>
        <div className="absolute top-1/2 left-1/4 w-24 h-24 rounded-full bg-cyan/40 blur-xl animate-float"></div>

        {/* Enhanced chart lines animation */}
        <div className="absolute inset-0 flex items-center justify-center opacity-10">
          {Array.from({ length: 15 }).map((_, i) => (
            <div
              key={i}
              className="absolute h-px bg-white"
              style={{
                width: `${Math.random() * 500 + 200}px`,
                transform: `rotate(${Math.random() * 180}deg) translateY(${Math.random() * 400 - 200}px)`,
                opacity: Math.random() * 0.7 + 0.3,
                animation: `float ${Math.random() * 8 + 4}s ease-in-out infinite`
              }}
            ></div>
          ))}
        </div>
      </div>

      <div className="container mx-auto px-4 relative z-10">
        <div className="flex flex-col lg:flex-row items-center justify-between gap-12">
          <div className="lg:w-1/2 text-center lg:text-left">
            <h1 className="text-4xl md:text-5xl lg:text-6xl xl:text-7xl font-bold mb-6 leading-tight">
              <span className="text-white">AI Trading Robot for </span>
              <span className="text-gold relative inline-block animate-float">Gold
                <span className="absolute -bottom-1 left-0 w-full h-1 bg-gold opacity-50 rounded"></span>
              </span>
              <span className="text-gray-200"> Trading</span>
            </h1>

            <p className="text-gray-300 text-lg md:text-xl mb-8 max-w-xl mx-auto lg:mx-0 leading-relaxed">
              XAUBOT is an Expert Advisor powered by machine learning and artificial intelligence, compatible with ALL forex trading pairs.
            </p>

            <div className="flex flex-col sm:flex-row gap-4 justify-center lg:justify-start">
              <Button className="btn-gold text-lg py-6 px-8 rounded-lg shadow-lg">
                Get Started
              </Button>
              <Button variant="outline" className="border-white text-white hover:bg-white/10 text-lg py-6 px-8 rounded-lg transition-all duration-300 hover:border-gold">
                Learn More
              </Button>
            </div>

            <div className="mt-12 grid grid-cols-3 lg:flex items-center justify-center lg:justify-start gap-6 lg:gap-8">
              <div className="text-center glass-effect p-4 rounded-lg card-hover">
                <div className="text-2xl font-bold text-gold">95%</div>
                <div className="text-gray-400 text-sm">Success Rate</div>
              </div>
              <div className="text-center glass-effect p-4 rounded-lg card-hover">
                <div className="text-2xl font-bold text-gold">24/7</div>
                <div className="text-gray-400 text-sm">Automated Trading</div>
              </div>
              <div className="text-center glass-effect p-4 rounded-lg card-hover">
                <div className="text-2xl font-bold text-gold">5,000+</div>
                <div className="text-gray-400 text-sm">Active Users</div>
              </div>
            </div>
          </div>

          <div className="lg:w-1/2 relative">
            <div className="relative w-full h-[400px] md:h-[500px] glass-effect rounded-xl overflow-hidden animate-float shadow-2xl border border-gray-800">
              <div className="absolute inset-0 flex items-center justify-center">
                <div className="relative w-[80%] h-[80%]">
                  {/* Enhanced chart with more modern visualization */}
                  <div className="w-full h-full bg-gradient-to-r from-indigo-900/30 to-purple-900/30 rounded-xl flex items-center justify-center">
                    <svg width="100%" height="100%" viewBox="0 0 800 500" className="rounded-lg">
                      <defs>
                        <linearGradient id="chartBg" x1="0%" y1="0%" x2="0%" y2="100%">
                          <stop offset="0%" style={{stopColor:'#0f172a', stopOpacity:0.7}} />
                          <stop offset="100%" style={{stopColor:'#0f172a', stopOpacity:0.1}} />
                        </linearGradient>
                        <linearGradient id="goldLine" x1="0%" y1="0%" x2="100%" y2="0%">
                          <stop offset="0%" style={{stopColor:'#eab308', stopOpacity:1}} />
                          <stop offset="100%" style={{stopColor:'#d97706', stopOpacity:1}} />
                        </linearGradient>
                        <linearGradient id="cyanLine" x1="0%" y1="0%" x2="100%" y2="0%">
                          <stop offset="0%" style={{stopColor:'#0ea5e9', stopOpacity:1}} />
                          <stop offset="100%" style={{stopColor:'#06b6d4', stopOpacity:1}} />
                        </linearGradient>
                      </defs>
                      <rect x="0" y="0" width="800" height="500" fill="#0f172a" />
                      <g stroke="#1e293b" strokeWidth="1">
                        {Array.from({ length: 5 }).map((_, i) => (
                          <line key={`h-${i}`} x1="0" y1={100 * (i + 1)} x2="800" y2={100 * (i + 1)} />
                        ))}
                        {Array.from({ length: 8 }).map((_, i) => (
                          <line key={`v-${i}`} x1={100 * (i + 1)} y1="0" x2={100 * (i + 1)} y2="500" />
                        ))}
                      </g>
                      <path d="M0,400 L50,380 L100,390 L150,350 L200,360 L250,330 L300,320 L350,280 L400,300 L450,260 L500,240 L550,200 L600,220 L650,180 L700,160 L750,140 L800,120" fill="none" stroke="url(#cyanLine)" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" />
                      <path d="M0,400 L50,380 L100,390 L150,350 L200,360 L250,330 L300,320 L350,280 L400,300 L450,260 L500,240 L550,200 L600,220 L650,180 L700,160 L750,140 L800,120 L800,500 L0,500 Z" fill="url(#chartBg)" />
                      <path d="M0,350 L50,330 L100,340 L150,320 L200,330 L250,310 L300,300 L350,270 L400,290 L450,250 L500,230 L550,190 L600,210 L650,170 L700,150 L750,130 L800,110" fill="none" stroke="url(#goldLine)" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" />
                      
                      {/* Add interactive points */}
                      {[400, 600, 700].map((x, i) => (
                        <g key={`point-${i}`} className="animate-pulse">
                          <circle cx={x} cy={i === 0 ? 290 : i === 1 ? 210 : 150} r="6" fill="#eab308" />
                          <circle cx={x} cy={i === 0 ? 290 : i === 1 ? 210 : 150} r="9" fill="none" stroke="#eab308" strokeWidth="2" opacity="0.5" />
                        </g>
                      ))}
                    </svg>
                  </div>
                </div>
              </div>

              {/* Enhanced robot with modern design */}
              <div className="absolute -bottom-6 right-6 w-32 h-32 animate-float">
                <div className="relative w-full h-full">
                  <svg width="100%" height="100%" viewBox="0 0 200 200" className="w-full h-full">
                    <defs>
                      <linearGradient id="robotHead" x1="0%" y1="0%" x2="100%" y2="100%">
                        <stop offset="0%" style={{stopColor:'#ffffff', stopOpacity:1}} />
                        <stop offset="100%" style={{stopColor:'#cccccc', stopOpacity:1}} />
                      </linearGradient>
                      <linearGradient id="robotGold" x1="0%" y1="0%" x2="100%" y2="100%">
                        <stop offset="0%" style={{stopColor:'#fbbf24', stopOpacity:1}} />
                        <stop offset="100%" style={{stopColor:'#d97706', stopOpacity:1}} />
                      </linearGradient>
                      <filter id="glow" x="-20%" y="-20%" width="140%" height="140%">
                        <feGaussianBlur stdDeviation="4" result="blur" />
                        <feComposite in="SourceGraphic" in2="blur" operator="over" />
                      </filter>
                    </defs>
                    <circle cx="100" cy="80" r="50" fill="url(#robotHead)" stroke="#ffffff" strokeWidth="2" filter="url(#glow)" />
                    <g transform="translate(100, 80) scale(0.4)">
                      <path d="M-30,-40 L30,-40 L30,-35 L0,0 L30,35 L30,40 L-30,40 L-30,35 L0,0 L-30,-35 Z" fill="#4338ca" />
                      <rect x="-5" y="-40" width="10" height="80" fill="#4338ca" />
                    </g>
                    <path d="M70,130 Q100,120 130,130 L120,160 Q100,150 80,160 Z" fill="url(#robotHead)" stroke="#ffffff" strokeWidth="2" />
                    <path d="M70,135 Q60,140 55,155 Q50,165 60,170 Q70,165 72,155 Z" fill="url(#robotHead)" stroke="#ffffff" strokeWidth="2" />
                    <path d="M130,135 Q140,140 145,155 Q150,165 140,170 Q130,165 128,155 Z" fill="url(#robotHead)" stroke="#ffffff" strokeWidth="2" />
                    <path d="M75,160 Q100,170 125,160 Q115,180 85,180 Z" fill="url(#robotHead)" stroke="#ffffff" strokeWidth="2" />
                    <rect x="142" y="40" width="3" height="40" fill="#ffffff" />
                    <path d="M145,40 L145,60 L160,50 Z" fill="url(#robotGold)" />
                  </svg>
                </div>
              </div>

              {/* Enhanced trading metrics */}
              <div className="absolute top-6 left-6 glass-effect rounded-lg p-4 border border-gray-800 animate-float shadow-lg">
                <div className="text-xs text-gray-400">Trading Performance</div>
                <div className="text-lg text-gold font-bold">+34.18%</div>
                <div className="mt-1 text-xs text-green-400">↑ 2.4%</div>
              </div>
              
              {/* New live indicator */}
              <div className="absolute top-6 right-6 glass-effect rounded-lg p-3 border border-gray-800 flex items-center">
                <span className="inline-block w-3 h-3 bg-green-500 rounded-full mr-2 animate-pulse"></span>
                <span className="text-xs text-gray-300">Live Trading</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
