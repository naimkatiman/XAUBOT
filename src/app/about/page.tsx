import React from 'react';
import { Button } from '@/components/ui/button';

export default function AboutPage() {
  const teamMembers = [
    {
      name: "Michael Roberts",
      role: "Founder & CEO",
      bio: "Former hedge fund manager with 15+ years of experience in algorithmic trading. Michael founded XAUBOT with a mission to make AI-powered trading accessible to all.",
      image: "michael"
    },
    {
      name: "Sarah Chen",
      role: "Lead AI Researcher",
      bio: "PhD in Machine Learning with specialization in financial markets. Sarah is responsible for developing and optimizing XAUBOT's core trading algorithms.",
      image: "sarah"
    },
    {
      name: "David Wilson",
      role: "Head of Trading",
      bio: "Certified Financial Analyst with extensive experience in forex trading. David leads our trading strategy development and backtesting processes.",
      image: "david"
    },
    {
      name: "Emma Johnson",
      role: "Customer Success",
      bio: "With a background in both finance and customer service, Emma ensures our users get the most out of their XAUBOT experience.",
      image: "emma"
    }
  ];

  return (
    <div className="pt-20 pb-32">
      {/* Hero section */}
      <section className="relative overflow-hidden mb-24">
        <div className="absolute inset-0 bg-gradient-to-b from-indigo-900/20 to-transparent opacity-50"></div>
        <div className="absolute inset-0 opacity-10">
          <div className="absolute top-10 right-1/4 w-64 h-64 rounded-full bg-cyan blur-[100px]"></div>
          <div className="absolute bottom-10 left-1/4 w-64 h-64 rounded-full bg-gold blur-[100px]"></div>
        </div>

        <div className="container mx-auto px-4 relative z-10">
          <div className="max-w-3xl mx-auto text-center">
            <h1 className="text-4xl md:text-5xl font-bold mb-6 text-white">
              About <span className="text-gold">XAUBOT</span>
            </h1>
            <p className="text-gray-300 text-lg md:text-xl mb-8 leading-relaxed">
              We're on a mission to transform forex trading with cutting-edge artificial intelligence, making consistent profitability accessible to traders of all experience levels.
            </p>
          </div>
        </div>
      </section>

      {/* Our story section */}
      <section className="mb-24">
        <div className="container mx-auto px-4">
          <div className="max-w-4xl mx-auto">
            <div className="flex flex-col md:flex-row gap-10 items-center">
              <div className="md:w-1/2">
                <h2 className="text-3xl font-bold mb-4 text-white">Our Story</h2>
                <div className="space-y-4 text-gray-300">
                  <p>
                    Founded in 2018, XAUBOT began as a personal project by our founder Michael Roberts, who was frustrated with the limitations of traditional trading indicators and systems.
                  </p>
                  <p>
                    After years of research and development in machine learning and algorithmic trading, we launched our first version of XAUBOT in 2020, specifically designed for gold (XAU/USD) trading.
                  </p>
                  <p>
                    Today, XAUBOT has evolved into a sophisticated trading solution used by thousands of traders worldwide, from individual retail traders to professional prop firms.
                  </p>
                  <p>
                    Our team of financial experts and AI specialists continuously refine our algorithms to adapt to changing market conditions, ensuring our users always have access to cutting-edge trading technology.
                  </p>
                </div>
              </div>
              <div className="md:w-1/2">
                <div className="relative rounded-xl overflow-hidden h-80 bg-gradient-to-br from-gray-900 to-indigo-900/50 border border-gray-800 flex items-center justify-center">
                  <div className="absolute inset-0 opacity-20 bg-[radial-gradient(#4338ca_1px,transparent_1px)] [background-size:16px_16px]"></div>
                  <div className="relative z-10 p-6 text-center">
                    <div className="w-20 h-20 mx-auto mb-4 rounded-full bg-indigo-900 flex items-center justify-center">
                      <svg className="w-12 h-12 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M13 10V3L4 14h7v7l9-11h-7z" />
                      </svg>
                    </div>
                    <div className="text-4xl font-bold text-gold mb-1">5,000+</div>
                    <div className="text-gray-400">Active Traders</div>

                    <div className="mt-8 flex justify-center gap-8">
                      <div>
                        <div className="text-2xl font-bold text-gold mb-1">98%</div>
                        <div className="text-gray-400 text-sm">Customer Satisfaction</div>
                      </div>
                      <div>
                        <div className="text-2xl font-bold text-gold mb-1">24/7</div>
                        <div className="text-gray-400 text-sm">Support</div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Our values section */}
      <section className="mb-24 bg-gradient-to-b from-transparent to-gray-900/30 py-20">
        <div className="container mx-auto px-4">
          <div className="max-w-3xl mx-auto text-center mb-12">
            <h2 className="text-3xl font-bold mb-4 text-white">Our Values</h2>
            <p className="text-gray-300">
              The core principles that guide everything we do at XAUBOT.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            <div className="bg-gray-900/40 border border-gray-800 rounded-lg p-6 hover:border-gold/50 transition-colors">
              <div className="w-12 h-12 bg-gold/20 rounded-lg flex items-center justify-center mb-5">
                <svg className="w-6 h-6 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-white mb-2">Integrity</h3>
              <p className="text-gray-300">
                We operate with complete transparency. We're honest about the potential and limitations of our technology and never make unrealistic promises about trading results.
              </p>
            </div>

            <div className="bg-gray-900/40 border border-gray-800 rounded-lg p-6 hover:border-gold/50 transition-colors">
              <div className="w-12 h-12 bg-gold/20 rounded-lg flex items-center justify-center mb-5">
                <svg className="w-6 h-6 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 10V3L4 14h7v7l9-11h-7z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-white mb-2">Innovation</h3>
              <p className="text-gray-300">
                We continuously push the boundaries of what's possible with AI in trading. Our team is dedicated to research and development to keep our algorithms at the cutting edge.
              </p>
            </div>

            <div className="bg-gray-900/40 border border-gray-800 rounded-lg p-6 hover:border-gold/50 transition-colors">
              <div className="w-12 h-12 bg-gold/20 rounded-lg flex items-center justify-center mb-5">
                <svg className="w-6 h-6 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-white mb-2">Community</h3>
              <p className="text-gray-300">
                We believe in building a supportive community of traders. We're dedicated to educating and empowering our users to make informed trading decisions.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Team section */}
      <section className="mb-24">
        <div className="container mx-auto px-4">
          <div className="max-w-3xl mx-auto text-center mb-12">
            <h2 className="text-3xl font-bold mb-4 text-white">Meet Our Team</h2>
            <p className="text-gray-300">
              The talented individuals behind XAUBOT's success.
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-8">
            {teamMembers.map((member, index) => (
              <div key={index} className="bg-gray-900/40 border border-gray-800 rounded-lg overflow-hidden hover:border-gold/50 transition-colors">
                <div className="h-48 bg-gradient-to-br from-indigo-900/50 to-gray-900 flex items-center justify-center">
                  <div className="w-24 h-24 rounded-full bg-gray-800 flex items-center justify-center">
                    {getInitials(member.name, "text-gold text-xl font-bold")}
                  </div>
                </div>
                <div className="p-6">
                  <h3 className="text-lg font-bold text-white mb-1">{member.name}</h3>
                  <p className="text-gold text-sm mb-3">{member.role}</p>
                  <p className="text-gray-300 text-sm">{member.bio}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA section */}
      <section>
        <div className="container mx-auto px-4">
          <div className="bg-gradient-to-r from-indigo-900/40 to-purple-900/40 rounded-xl p-8 md:p-12 border border-gray-800 text-center">
            <h2 className="text-2xl md:text-3xl font-bold mb-6 text-white">
              Join Our Growing Community
            </h2>
            <p className="text-gray-300 mb-8 max-w-2xl mx-auto">
              Experience the power of AI-driven trading with XAUBOT. Start your journey to more consistent trading results today.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <Button className="bg-gold text-black hover:bg-gold/90">
                Get Started Now
              </Button>
              <Button variant="outline" className="border-white text-white hover:bg-white/10">
                Contact Our Team
              </Button>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}

// Helper function to get initials
function getInitials(name: string, className: string): JSX.Element {
  const initials = name
    .split(' ')
    .map(part => part[0])
    .join('')
    .toUpperCase();

  return <span className={className}>{initials}</span>;
}
