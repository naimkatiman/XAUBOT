import React from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';

export default function ProductsPage() {
  const products = [
    {
      title: "XAUBOT Pro",
      description: "Our flagship AI-powered Expert Advisor for gold trading with advanced algorithms and machine learning capabilities.",
      features: [
        "Advanced AI algorithms specifically optimized for XAU/USD",
        "Automated 24/7 trading execution",
        "Real-time market analysis and decision making",
        "Multi-timeframe analysis for better entry and exit points",
        "Compatible with all MT4/MT5 brokers",
        "Prop firm challenge ready with risk management presets"
      ],
      price: "$499",
      link: "/pricing",
      image: "pro"
    },
    {
      title: "XAUBOT Premium",
      description: "Enhanced trading capabilities with our premium offering designed for serious gold traders seeking consistent results.",
      features: [
        "All features of XAUBOT Pro",
        "Additional trading pairs support",
        "Priority technical support",
        "Access to private trading community",
        "Monthly strategy updates",
        "Performance analytics dashboard"
      ],
      price: "$799",
      link: "/pricing",
      image: "premium"
    },
    {
      title: "XAUBOT Indicators",
      description: "Standalone trading indicators to enhance your manual trading strategy with AI-driven insights.",
      features: [
        "Support/Resistance levels detection",
        "Trend strength analyzer",
        "Volatility prediction indicator",
        "Custom alerts system",
        "Market sentiment analysis",
        "Correlation matrix for multiple pairs"
      ],
      price: "$299",
      link: "/pricing",
      image: "indicators"
    }
  ];

  return (
    <div className="pt-20 pb-32">
      {/* Page header */}
      <div className="container mx-auto px-4 mb-20">
        <div className="text-center max-w-3xl mx-auto">
          <h1 className="text-4xl md:text-5xl font-bold mb-6 text-white">
            Our <span className="text-gold">Products</span>
          </h1>
          <p className="text-gray-300 text-lg md:text-xl mb-8">
            Discover our range of AI-powered trading solutions designed to help you achieve consistent results in the forex market.
          </p>
        </div>
      </div>

      {/* Products section */}
      <div className="container mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-10">
          {products.map((product, index) => (
            <Card key={index} className="border-gray-800 bg-gray-900/50 hover:bg-gray-900/70 transition-all duration-300 overflow-hidden h-full flex flex-col">
              <div className={`h-48 bg-gradient-to-r ${getProductBackground(product.image)} relative overflow-hidden`}>
                <div className="absolute inset-0 flex items-center justify-center opacity-20">
                  {getProductIcon(product.image)}
                </div>
                <div className="absolute bottom-0 left-0 w-full h-24 bg-gradient-to-t from-gray-900/90 to-transparent"></div>
                <div className="absolute bottom-4 left-4">
                  <span className="bg-gold/90 text-black text-sm font-medium py-1 px-3 rounded-full">
                    {product.price}
                  </span>
                </div>
              </div>

              <CardHeader>
                <CardTitle className="text-2xl font-bold text-white">{product.title}</CardTitle>
                <CardDescription className="text-gray-300">
                  {product.description}
                </CardDescription>
              </CardHeader>

              <CardContent className="flex-grow">
                <h3 className="text-sm font-medium text-gray-400 mb-3">Key Features:</h3>
                <ul className="space-y-2">
                  {product.features.map((feature, i) => (
                    <li key={i} className="flex items-start">
                      <svg className="w-5 h-5 text-gold mr-2 mt-0.5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                      </svg>
                      <span className="text-sm text-gray-300">{feature}</span>
                    </li>
                  ))}
                </ul>
              </CardContent>

              <CardFooter className="border-t border-gray-800 pt-4">
                <Button className="w-full bg-gold text-black hover:bg-gold/90">
                  See Plans & Pricing
                </Button>
              </CardFooter>
            </Card>
          ))}
        </div>
      </div>

      {/* CTA section */}
      <div className="container mx-auto px-4 mt-24">
        <div className="bg-gradient-to-r from-indigo-900/40 to-purple-900/40 rounded-xl p-8 md:p-12 border border-gray-800">
          <div className="max-w-3xl mx-auto text-center">
            <h2 className="text-2xl md:text-3xl font-bold mb-6 text-white">
              Ready to transform your trading with AI?
            </h2>
            <p className="text-gray-300 mb-8">
              Our products are designed to give you an edge in the market. Try XAUBOT risk-free with our 30-day money-back guarantee.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <Button className="bg-gold text-black hover:bg-gold/90">
                View Pricing Options
              </Button>
              <Button variant="outline" className="border-white text-white hover:bg-white/10">
                Schedule a Demo
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

// Helper functions for product styling
function getProductBackground(product: string): string {
  switch (product) {
    case 'pro':
      return 'from-cyan-900/50 to-emerald-900/50';
    case 'premium':
      return 'from-amber-900/50 to-red-900/50';
    case 'indicators':
      return 'from-indigo-900/50 to-violet-900/50';
    default:
      return 'from-gray-800/50 to-gray-900/50';
  }
}

function getProductIcon(product: string): JSX.Element {
  const baseClassName = "w-32 h-32 text-white/20";

  switch (product) {
    case 'pro':
      return (
        <svg className={baseClassName} fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path d="M5 12a1 1 0 1 0 0 2h2.034a1 1 0 0 0 0-2H5zm14 0a1 1 0 1 0 0 2h-2.034a1 1 0 0 0 0-2H19zM5 16a1 1 0 1 0 0 2h2.034a1 1 0 0 0 0-2H5zm14 0a1 1 0 1 0 0 2h-2.034a1 1 0 0 0 0-2H19zM9 10h6l3 4-9 4.5V22h4v-2h2v1.5a1.5 1.5 0 0 1-1.5 1.5h-5a1.5 1.5 0 0 1-1.5-1.5V18l-2.5 1.5L2 16.5V14l6-6h1zm-2 2.5L7.5 13l-4 4 3.5-1.5 5.5-3h-5.5zM5 8a1 1 0 1 0 0 2h2.034a1 1 0 1 0 0-2H5zm14 0a1 1 0 1 0 0 2h-2.034a1 1 0 1 0 0-2H19zM12 2c.5 0 1 .15 1.6.47l7 3.8a3 3 0 0 1 0 5.46l-7 3.8a3 3 0 0 1-3.2 0l-7-3.8a3 3 0 0 1 0-5.46l7-3.8A3 3 0 0 1 12 2zm0 2c-.22 0-.44.05-.65.18l-7 3.8a1 1 0 0 0 0 1.04l7 3.8a1 1 0 0 0 1.3 0l7-3.8a1 1 0 0 0 0-1.04l-7-3.8A1.3 1.3 0 0 0 12 4zm0 2.5a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3z" />
        </svg>
      );
    case 'premium':
      return (
        <svg className={baseClassName} fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path d="M12 2c.5 0 1 .15 1.6.47l7 3.8a3 3 0 0 1 0 5.46l-7 3.8a3 3 0 0 1-3.2 0l-7-3.8a3 3 0 0 1 0-5.46l7-3.8A3 3 0 0 1 12 2zm0 2c-.22 0-.44.05-.65.18l-7 3.8a1 1 0 0 0 0 1.04l7 3.8a1 1 0 0 0 1.3 0l7-3.8a1 1 0 0 0 0-1.04l-7-3.8A1.3 1.3 0 0 0 12 4zm0 2.5a1.5 1.5 0 1 1 0 3 1.5 1.5 0 0 1 0-3zm-8 8.06v2.58c0 2.04 2.41 3.82 5.2 4.49 3.95-2.3 9.2-1.52 12.8.36V14.5l-3 1.15v.85c-1.93-1.12-4.24-1.5-6.5-1.05-3.53.7-6.5 3.8-6.5 3.8l-4.1-4.7h2.1zm8 .44a2 2 0 1 1 0 4 2 2 0 0 1 0-4z" />
        </svg>
      );
    case 'indicators':
      return (
        <svg className={baseClassName} fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path d="M3 2h18c.55 0 1 .45 1 1v18c0 .55-.45 1-1 1H3c-.55 0-1-.45-1-1V3c0-.55.45-1 1-1zm1 2v16h16V4H4zm3 8h2v6H7v-6zm4-4h2v10h-2V8zm4 2h2v8h-2v-8z" />
        </svg>
      );
    default:
      return (
        <svg className={baseClassName} fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
          <path d="M12 2c.5 0 1 .15 1.6.47l7 3.8a3 3 0 0 1 0 5.46l-7 3.8a3 3 0 0 1-3.2 0l-7-3.8a3 3 0 0 1 0-5.46l7-3.8A3 3 0 0 1 12 2z" />
        </svg>
      );
  }
}
