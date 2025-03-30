import React from 'react';
import { Button } from '@/components/ui/button';
import Link from 'next/link';

interface BlogPost {
  id: number;
  title: string;
  excerpt: string;
  date: string;
  category: string;
  readTime: string;
  slug: string;
  image: string;
}

export default function BlogPage() {
  const blogPosts: BlogPost[] = [
    {
      id: 1,
      title: "How AI is Revolutionizing Forex Trading",
      excerpt: "Explore how artificial intelligence is transforming the landscape of forex trading, providing traders with powerful tools to analyze markets and make better decisions.",
      date: "May 15, 2024",
      category: "Technology",
      readTime: "5 min read",
      slug: "how-ai-revolutionizing-forex-trading",
      image: "ai-trading"
    },
    {
      id: 2,
      title: "The Ultimate Guide to Gold Trading with XAUBOT",
      excerpt: "Learn how to maximize your gold trading performance using XAUBOT's advanced algorithms and strategies. This comprehensive guide covers everything from basic setup to advanced techniques.",
      date: "May 8, 2024",
      category: "Guides",
      readTime: "8 min read",
      slug: "ultimate-guide-gold-trading-xaubot",
      image: "gold-trading"
    },
    {
      id: 3,
      title: "Risk Management Strategies for Automated Trading",
      excerpt: "Proper risk management is crucial for long-term trading success, especially with automated systems. Discover the key risk management principles used by professional traders.",
      date: "April 29, 2024",
      category: "Strategy",
      readTime: "6 min read",
      slug: "risk-management-strategies-automated-trading",
      image: "risk-management"
    },
    {
      id: 4,
      title: "How to Pass Prop Firm Challenges with XAUBOT",
      excerpt: "Prop firm challenges can be difficult to pass, but with the right approach and tools like XAUBOT, you can significantly increase your chances of success. Here's how.",
      date: "April 22, 2024",
      category: "Tutorials",
      readTime: "7 min read",
      slug: "pass-prop-firm-challenges-xaubot",
      image: "prop-firm"
    },
    {
      id: 5,
      title: "Understanding Market Volatility and How XAUBOT Adapts",
      excerpt: "Market volatility can be both an opportunity and a risk. Learn how XAUBOT's algorithms automatically adapt to changing market conditions to maintain performance.",
      date: "April 15, 2024",
      category: "Market Analysis",
      readTime: "4 min read",
      slug: "understanding-market-volatility-xaubot-adapts",
      image: "volatility"
    },
    {
      id: 6,
      title: "XAUBOT Success Stories: Real Traders, Real Results",
      excerpt: "Read the inspiring stories of traders who have transformed their trading journey with XAUBOT. From retail traders to professional fund managers, these case studies showcase real-world results.",
      date: "April 8, 2024",
      category: "Case Studies",
      readTime: "9 min read",
      slug: "xaubot-success-stories-real-traders-results",
      image: "success-stories"
    }
  ];

  const categories = ["All", "Technology", "Guides", "Strategy", "Tutorials", "Market Analysis", "Case Studies"];

  return (
    <div className="pt-20 pb-32">
      {/* Page header */}
      <div className="container mx-auto px-4 mb-16">
        <div className="text-center max-w-3xl mx-auto">
          <h1 className="text-4xl md:text-5xl font-bold mb-6 text-white">
            The XAUBOT <span className="text-gold">Blog</span>
          </h1>
          <p className="text-gray-300 text-lg md:text-xl mb-8">
            Trading insights, tips, and the latest news from the world of algorithmic trading.
          </p>
        </div>
      </div>

      {/* Categories */}
      <div className="container mx-auto px-4 mb-12">
        <div className="flex flex-wrap justify-center gap-2">
          {categories.map((category, index) => (
            <Button
              key={index}
              variant={index === 0 ? "default" : "outline"}
              className={index === 0 ? "bg-gold text-black hover:bg-gold/90" : "border-gray-700 text-gray-300 hover:bg-gray-800"}
            >
              {category}
            </Button>
          ))}
        </div>
      </div>

      {/* Featured post */}
      <div className="container mx-auto px-4 mb-16">
        <div className="rounded-xl overflow-hidden border border-gray-800 bg-gradient-to-br from-gray-900/70 to-indigo-900/30">
          <div className="grid grid-cols-1 lg:grid-cols-2">
            <div className="p-8 lg:p-12 flex flex-col justify-center">
              <div className="mb-4">
                <span className="inline-block bg-gold/20 text-gold text-xs font-medium py-1 px-2 rounded">
                  Featured
                </span>
                <span className="ml-2 text-gray-400 text-sm">May 20, 2024</span>
              </div>
              <h2 className="text-2xl md:text-3xl font-bold mb-4 text-white">
                XAUBOT v3.0 Release: The Next Generation of AI Trading
              </h2>
              <p className="text-gray-300 mb-6">
                We're excited to announce the release of XAUBOT v3.0, featuring enhanced AI algorithms, multi-timeframe analysis, and improved risk management features. Discover what's new and how it can transform your trading performance.
              </p>
              <div>
                <Button className="bg-gold text-black hover:bg-gold/90">
                  Read Article
                </Button>
              </div>
            </div>
            <div className="h-64 lg:h-auto bg-gradient-to-r from-indigo-900/30 to-cyan-900/30 relative flex items-center justify-center">
              <div className="absolute inset-0 opacity-20 bg-[radial-gradient(#4338ca_1px,transparent_1px)] [background-size:16px_16px]"></div>
              <div className="w-32 h-32 rounded-full bg-black/30 backdrop-blur-sm flex items-center justify-center">
                <svg className="w-16 h-16 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
                </svg>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Blog posts grid */}
      <div className="container mx-auto px-4">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {blogPosts.map((post) => (
            <BlogPostCard key={post.id} post={post} />
          ))}
        </div>

        {/* Pagination */}
        <div className="mt-16 flex justify-center">
          <div className="flex items-center space-x-2">
            <Button variant="outline" className="border-gray-700 text-gray-300 hover:bg-gray-800 w-10 h-10 p-0">
              <span className="sr-only">Previous</span>
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
              </svg>
            </Button>
            <Button className="bg-gold text-black hover:bg-gold/90 w-10 h-10 p-0">1</Button>
            <Button variant="outline" className="border-gray-700 text-gray-300 hover:bg-gray-800 w-10 h-10 p-0">2</Button>
            <Button variant="outline" className="border-gray-700 text-gray-300 hover:bg-gray-800 w-10 h-10 p-0">3</Button>
            <span className="text-gray-500">...</span>
            <Button variant="outline" className="border-gray-700 text-gray-300 hover:bg-gray-800 w-10 h-10 p-0">8</Button>
            <Button variant="outline" className="border-gray-700 text-gray-300 hover:bg-gray-800 w-10 h-10 p-0">
              <span className="sr-only">Next</span>
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
              </svg>
            </Button>
          </div>
        </div>
      </div>

      {/* Newsletter */}
      <div className="container mx-auto px-4 mt-24">
        <div className="max-w-3xl mx-auto bg-gradient-to-r from-indigo-900/40 to-purple-900/40 rounded-xl p-8 md:p-12 border border-gray-800 text-center">
          <h2 className="text-2xl md:text-3xl font-bold mb-4 text-white">
            Subscribe to Our Newsletter
          </h2>
          <p className="text-gray-300 mb-6">
            Stay updated with the latest trading insights, XAUBOT updates, and exclusive tips delivered straight to your inbox.
          </p>
          <div className="flex flex-col sm:flex-row gap-2 max-w-md mx-auto">
            <input
              type="email"
              placeholder="Your email address"
              className="flex-grow bg-gray-900/50 border border-gray-800 text-white rounded-md px-4 py-2 focus:outline-none focus:ring-2 focus:ring-gold/50 focus:border-transparent"
            />
            <Button className="bg-gold text-black hover:bg-gold/90 whitespace-nowrap">
              Subscribe
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}

// Blog post card component
function BlogPostCard({ post }: { post: BlogPost }) {
  return (
    <div className="border border-gray-800 rounded-lg overflow-hidden bg-gray-900/30 hover:bg-gray-900/50 transition-colors group">
      <div className={`h-48 ${getPostBackground(post.image)} relative`}>
        <div className="absolute inset-0 opacity-20 bg-[radial-gradient(#4338ca_1px,transparent_1px)] [background-size:16px_16px]"></div>
        <div className="absolute top-4 left-4">
          <span className="inline-block bg-black/30 backdrop-blur-sm text-white text-xs font-medium py-1 px-2 rounded">
            {post.category}
          </span>
        </div>
      </div>
      <div className="p-6">
        <div className="flex items-center text-sm text-gray-400 mb-3">
          <span>{post.date}</span>
          <span className="mx-2">•</span>
          <span>{post.readTime}</span>
        </div>
        <h3 className="text-xl font-bold text-white mb-3 group-hover:text-gold transition-colors">
          <Link href={`/blog/${post.slug}`}>
            {post.title}
          </Link>
        </h3>
        <p className="text-gray-300 mb-4">
          {post.excerpt}
        </p>
        <Link href={`/blog/${post.slug}`} className="text-gold hover:underline inline-flex items-center">
          Read More
          <svg className="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M14 5l7 7m0 0l-7 7m7-7H3" />
          </svg>
        </Link>
      </div>
    </div>
  );
}

// Helper function to get post background
function getPostBackground(image: string): string {
  switch (image) {
    case 'ai-trading':
      return 'bg-gradient-to-r from-indigo-900/50 to-purple-900/50';
    case 'gold-trading':
      return 'bg-gradient-to-r from-amber-900/50 to-yellow-700/50';
    case 'risk-management':
      return 'bg-gradient-to-r from-red-900/50 to-pink-900/50';
    case 'prop-firm':
      return 'bg-gradient-to-r from-emerald-900/50 to-teal-900/50';
    case 'volatility':
      return 'bg-gradient-to-r from-blue-900/50 to-indigo-900/50';
    case 'success-stories':
      return 'bg-gradient-to-r from-orange-900/50 to-amber-800/50';
    default:
      return 'bg-gradient-to-r from-gray-800/50 to-gray-900/50';
  }
}
