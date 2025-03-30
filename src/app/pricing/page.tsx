import React from 'react';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardFooter, CardHeader } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';

export default function PricingPage() {
  return (
    <div className="pt-20 pb-32">
      {/* Page header */}
      <div className="container mx-auto px-4 mb-16">
        <div className="text-center max-w-3xl mx-auto">
          <h1 className="text-4xl md:text-5xl font-bold mb-6 text-white">
            Transparent <span className="text-gold">Pricing</span>
          </h1>
          <p className="text-gray-300 text-lg md:text-xl mb-8">
            Choose the plan that suits your trading needs. All plans include our 30-day money-back guarantee.
          </p>
        </div>
      </div>

      {/* Pricing tabs */}
      <div className="container mx-auto px-4">
        <Tabs defaultValue="lifetime" className="max-w-5xl mx-auto">
          <div className="flex justify-center mb-10">
            <TabsList className="bg-gray-900/70 border border-gray-800">
              <TabsTrigger value="monthly" className="data-[state=active]:bg-gold data-[state=active]:text-black">
                Monthly
              </TabsTrigger>
              <TabsTrigger value="yearly" className="data-[state=active]:bg-gold data-[state=active]:text-black">
                Yearly (20% off)
              </TabsTrigger>
              <TabsTrigger value="lifetime" className="data-[state=active]:bg-gold data-[state=active]:text-black">
                Lifetime (Best Value)
              </TabsTrigger>
            </TabsList>
          </div>

          {/* Monthly pricing */}
          <TabsContent value="monthly" className="mt-0">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
              <PricingCard
                title="XAUBOT Standard"
                price="$49"
                period="per month"
                description="Perfect for beginners starting their forex trading journey."
                features={[
                  "AI-powered trading algorithms",
                  "XAU/USD trading pair",
                  "MT4/MT5 compatibility",
                  "Basic risk management",
                  "Standard support",
                  "Monthly updates"
                ]}
                recommended={false}
                buttonText="Get Started"
              />

              <PricingCard
                title="XAUBOT Pro"
                price="$99"
                period="per month"
                description="Our most popular plan for serious traders."
                features={[
                  "Everything in Standard",
                  "Multi-pair trading (5 pairs)",
                  "Advanced risk management",
                  "Prop firm challenge ready",
                  "Priority email support",
                  "Trading community access"
                ]}
                recommended={true}
                buttonText="Get Started"
              />

              <PricingCard
                title="XAUBOT Enterprise"
                price="$199"
                period="per month"
                description="For professional traders and investment firms."
                features={[
                  "Everything in Pro",
                  "Unlimited trading pairs",
                  "Custom risk parameters",
                  "VIP support channel",
                  "Strategy customization",
                  "Performance dashboard",
                  "API access"
                ]}
                recommended={false}
                buttonText="Get Started"
              />
            </div>
          </TabsContent>

          {/* Yearly pricing */}
          <TabsContent value="yearly" className="mt-0">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
              <PricingCard
                title="XAUBOT Standard"
                price="$470"
                period="per year"
                description="Perfect for beginners starting their forex trading journey."
                features={[
                  "AI-powered trading algorithms",
                  "XAU/USD trading pair",
                  "MT4/MT5 compatibility",
                  "Basic risk management",
                  "Standard support",
                  "Monthly updates"
                ]}
                recommended={false}
                buttonText="Get Started"
                discountBadge="Save $118"
              />

              <PricingCard
                title="XAUBOT Pro"
                price="$950"
                period="per year"
                description="Our most popular plan for serious traders."
                features={[
                  "Everything in Standard",
                  "Multi-pair trading (5 pairs)",
                  "Advanced risk management",
                  "Prop firm challenge ready",
                  "Priority email support",
                  "Trading community access"
                ]}
                recommended={true}
                buttonText="Get Started"
                discountBadge="Save $238"
              />

              <PricingCard
                title="XAUBOT Enterprise"
                price="$1,910"
                period="per year"
                description="For professional traders and investment firms."
                features={[
                  "Everything in Pro",
                  "Unlimited trading pairs",
                  "Custom risk parameters",
                  "VIP support channel",
                  "Strategy customization",
                  "Performance dashboard",
                  "API access"
                ]}
                recommended={false}
                buttonText="Get Started"
                discountBadge="Save $478"
              />
            </div>
          </TabsContent>

          {/* Lifetime pricing */}
          <TabsContent value="lifetime" className="mt-0">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
              <PricingCard
                title="XAUBOT Standard"
                price="$499"
                period="one-time payment"
                description="Perfect for beginners starting their forex trading journey."
                features={[
                  "AI-powered trading algorithms",
                  "XAU/USD trading pair",
                  "MT4/MT5 compatibility",
                  "Basic risk management",
                  "Lifetime standard support",
                  "Lifetime updates"
                ]}
                recommended={false}
                buttonText="Get Started"
                discountBadge="Best Value"
              />

              <PricingCard
                title="XAUBOT Pro"
                price="$799"
                period="one-time payment"
                description="Our most popular plan for serious traders."
                features={[
                  "Everything in Standard",
                  "Multi-pair trading (5 pairs)",
                  "Advanced risk management",
                  "Prop firm challenge ready",
                  "Lifetime priority support",
                  "Lifetime trading community access"
                ]}
                recommended={true}
                buttonText="Get Started"
                discountBadge="Best Value"
              />

              <PricingCard
                title="XAUBOT Enterprise"
                price="$1,499"
                period="one-time payment"
                description="For professional traders and investment firms."
                features={[
                  "Everything in Pro",
                  "Unlimited trading pairs",
                  "Custom risk parameters",
                  "Lifetime VIP support",
                  "Strategy customization",
                  "Performance dashboard",
                  "API access"
                ]}
                recommended={false}
                buttonText="Get Started"
                discountBadge="Best Value"
              />
            </div>
          </TabsContent>
        </Tabs>
      </div>

      {/* FAQ section */}
      <div className="container mx-auto px-4 mt-24">
        <div className="max-w-3xl mx-auto">
          <h2 className="text-3xl font-bold mb-10 text-center text-white">
            Frequently Asked <span className="text-gold">Questions</span>
          </h2>

          <div className="space-y-6">
            <FaqItem
              question="Do you offer a free trial?"
              answer="We don't offer a free trial, but we do provide a 30-day money-back guarantee, allowing you to try XAUBOT risk-free for a full month. If you're not satisfied with the results, we'll refund your purchase."
            />

            <FaqItem
              question="How does the lifetime license work?"
              answer="Our lifetime license grants you perpetual access to the software, including all updates within the same major version. You'll also receive technical support and access to our trading community for as long as the product exists."
            />

            <FaqItem
              question="Can I use XAUBOT with my existing broker?"
              answer="Yes, XAUBOT is compatible with most MT4/MT5 brokers. We recommend using a reputable broker with tight spreads for optimal performance. Our support team can help you verify compatibility with your specific broker."
            />

            <FaqItem
              question="What kind of results can I expect?"
              answer="While trading always involves risk, XAUBOT's AI algorithms are designed to identify profitable opportunities. Results will vary based on market conditions, account size, and risk settings. Many of our users report monthly returns between 5-15% with conservative risk settings."
            />

            <FaqItem
              question="Can I upgrade my plan later?"
              answer="Yes, you can upgrade your plan at any time. We'll apply the remaining value of your current plan as a credit toward the new plan, ensuring you don't lose any value from your initial purchase."
            />
          </div>
        </div>
      </div>

      {/* CTA section */}
      <div className="container mx-auto px-4 mt-24">
        <div className="bg-gradient-to-r from-indigo-900/40 to-purple-900/40 rounded-xl p-8 md:p-12 border border-gray-800">
          <div className="max-w-3xl mx-auto text-center">
            <h2 className="text-2xl md:text-3xl font-bold mb-6 text-white">
              Still have questions?
            </h2>
            <p className="text-gray-300 mb-8">
              Our team is here to help you choose the right plan for your trading goals. Get in touch with us today.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center">
              <Button className="bg-gold text-black hover:bg-gold/90">
                Contact Support
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

// Pricing card component
function PricingCard({
  title,
  price,
  period,
  description,
  features,
  recommended,
  buttonText,
  discountBadge
}: {
  title: string;
  price: string;
  period: string;
  description: string;
  features: string[];
  recommended: boolean;
  buttonText: string;
  discountBadge?: string;
}) {
  return (
    <Card className={`border-gray-800 ${recommended ? 'bg-gradient-to-b from-gray-900/70 to-indigo-900/40 shadow-lg shadow-indigo-800/10 relative border-gold/50' : 'bg-gray-900/50'} overflow-hidden flex flex-col`}>
      {recommended && (
        <div className="absolute top-0 right-0 left-0 bg-gold text-black text-xs font-medium text-center py-1">
          MOST POPULAR
        </div>
      )}

      <CardHeader className={`${recommended ? 'pt-8' : 'pt-6'}`}>
        <div className="text-center mb-4">
          <h3 className="text-xl font-bold text-white mb-1">{title}</h3>
          <p className="text-gray-400 text-sm">{description}</p>
        </div>
        <div className="text-center">
          <div className="flex items-center justify-center">
            <span className="text-4xl font-bold text-white">{price}</span>
            {discountBadge && (
              <span className="ml-2 bg-cyan/20 text-cyan text-xs font-medium py-1 px-2 rounded">
                {discountBadge}
              </span>
            )}
          </div>
          <span className="text-gray-400 text-sm">{period}</span>
        </div>
      </CardHeader>

      <CardContent className="flex-grow">
        <ul className="space-y-3">
          {features.map((feature, i) => (
            <li key={i} className="flex items-start">
              <svg className="w-5 h-5 text-gold mr-2 mt-0.5 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
              </svg>
              <span className="text-gray-300">{feature}</span>
            </li>
          ))}
        </ul>
      </CardContent>

      <CardFooter className="border-t border-gray-800 pt-4">
        <Button className={`w-full ${recommended ? 'bg-gold text-black hover:bg-gold/90' : 'bg-white/10 text-white hover:bg-white/20'}`}>
          {buttonText}
        </Button>
      </CardFooter>
    </Card>
  );
}

// FAQ item component
function FaqItem({ question, answer }: { question: string; answer: string }) {
  return (
    <div className="border border-gray-800 rounded-lg p-6 bg-gray-900/30">
      <h3 className="text-xl font-medium text-white mb-3">{question}</h3>
      <p className="text-gray-300">{answer}</p>
    </div>
  );
}
