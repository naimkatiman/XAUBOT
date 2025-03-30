"use client";

import React, { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Card } from '@/components/ui/card';

export default function ContactPage() {
  const [formState, setFormState] = useState({
    name: '',
    email: '',
    subject: '',
    message: '',
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isSubmitted, setIsSubmitted] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormState(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);

    // Simulate form submission
    setTimeout(() => {
      setIsSubmitting(false);
      setIsSubmitted(true);
      setFormState({
        name: '',
        email: '',
        subject: '',
        message: '',
      });
    }, 1500);
  };

  return (
    <div className="pt-20 pb-32">
      {/* Page header */}
      <div className="container mx-auto px-4 mb-16">
        <div className="text-center max-w-3xl mx-auto">
          <h1 className="text-4xl md:text-5xl font-bold mb-6 text-white">
            Contact <span className="text-gold">Us</span>
          </h1>
          <p className="text-gray-300 text-lg md:text-xl mb-8">
            Have questions or need assistance? Our team is here to help you with anything related to XAUBOT.
          </p>
        </div>
      </div>

      {/* Contact section */}
      <div className="container mx-auto px-4">
        <div className="max-w-5xl mx-auto">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
            <div>
              <h2 className="text-2xl font-bold mb-6 text-white">Get in Touch</h2>

              {isSubmitted ? (
                <Card className="bg-gradient-to-br from-gray-900/70 to-indigo-900/30 border-green-500/30 p-8">
                  <div className="text-center">
                    <div className="mx-auto w-16 h-16 bg-green-500/20 rounded-full flex items-center justify-center mb-4">
                      <svg className="w-8 h-8 text-green-500" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" />
                      </svg>
                    </div>
                    <h3 className="text-xl font-bold text-white mb-2">Message Sent!</h3>
                    <p className="text-gray-300 mb-6">
                      Thank you for contacting us. Our team will review your message and get back to you shortly.
                    </p>
                    <Button
                      className="bg-white/10 text-white hover:bg-white/20"
                      onClick={() => setIsSubmitted(false)}
                    >
                      Send Another Message
                    </Button>
                  </div>
                </Card>
              ) : (
                <form onSubmit={handleSubmit} className="space-y-6">
                  <div>
                    <Label htmlFor="name" className="text-white">Name</Label>
                    <Input
                      id="name"
                      name="name"
                      value={formState.name}
                      onChange={handleChange}
                      placeholder="Your name"
                      required
                      className="bg-gray-900/50 border-gray-800 text-white mt-2"
                    />
                  </div>

                  <div>
                    <Label htmlFor="email" className="text-white">Email</Label>
                    <Input
                      id="email"
                      name="email"
                      type="email"
                      value={formState.email}
                      onChange={handleChange}
                      placeholder="Your email address"
                      required
                      className="bg-gray-900/50 border-gray-800 text-white mt-2"
                    />
                  </div>

                  <div>
                    <Label htmlFor="subject" className="text-white">Subject</Label>
                    <Input
                      id="subject"
                      name="subject"
                      value={formState.subject}
                      onChange={handleChange}
                      placeholder="Subject of your inquiry"
                      required
                      className="bg-gray-900/50 border-gray-800 text-white mt-2"
                    />
                  </div>

                  <div>
                    <Label htmlFor="message" className="text-white">Message</Label>
                    <textarea
                      id="message"
                      name="message"
                      value={formState.message}
                      onChange={handleChange}
                      placeholder="Your message or inquiry"
                      required
                      rows={5}
                      className="w-full bg-gray-900/50 border border-gray-800 text-white mt-2 rounded-md p-3 focus:outline-none focus:ring-2 focus:ring-gold/50 focus:border-transparent"
                    />
                  </div>

                  <div>
                    <Button
                      type="submit"
                      className="bg-gold text-black hover:bg-gold/90 w-full"
                      disabled={isSubmitting}
                    >
                      {isSubmitting ? (
                        <span className="flex items-center">
                          <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-black" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                          </svg>
                          Sending...
                        </span>
                      ) : "Send Message"}
                    </Button>
                  </div>
                </form>
              )}
            </div>

            <div>
              <div className="bg-gray-900/30 rounded-xl border border-gray-800 p-8 h-full">
                <h2 className="text-2xl font-bold mb-8 text-white">Contact Information</h2>

                <div className="space-y-8">
                  <div className="flex items-start">
                    <div className="flex-shrink-0 w-10 h-10 rounded-lg bg-gold/20 flex items-center justify-center mr-4">
                      <svg className="w-5 h-5 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                      </svg>
                    </div>
                    <div>
                      <h3 className="text-lg font-medium text-white mb-1">Email Us</h3>
                      <p className="text-gray-300 mb-1">For general inquiries:</p>
                      <a href="mailto:info@xaubot.com" className="text-gold hover:underline">info@xaubot.com</a>
                      <p className="text-gray-300 mt-3 mb-1">For technical support:</p>
                      <a href="mailto:support@xaubot.com" className="text-gold hover:underline">support@xaubot.com</a>
                    </div>
                  </div>

                  <div className="flex items-start">
                    <div className="flex-shrink-0 w-10 h-10 rounded-lg bg-gold/20 flex items-center justify-center mr-4">
                      <svg className="w-5 h-5 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 18h.01M8 21h8a2 2 0 002-2V5a2 2 0 00-2-2H8a2 2 0 00-2 2v14a2 2 0 002 2z" />
                      </svg>
                    </div>
                    <div>
                      <h3 className="text-lg font-medium text-white mb-1">Call Us</h3>
                      <p className="text-gray-300 mb-1">Support Hotline:</p>
                      <a href="tel:+18005555555" className="text-gold hover:underline">+1 (800) 555-5555</a>
                      <p className="text-gray-300 mt-3 mb-1">Business Hours:</p>
                      <p className="text-gray-300">Monday - Friday: 9am - 5pm EST</p>
                    </div>
                  </div>

                  <div className="flex items-start">
                    <div className="flex-shrink-0 w-10 h-10 rounded-lg bg-gold/20 flex items-center justify-center mr-4">
                      <svg className="w-5 h-5 text-gold" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8h2a2 2 0 012 2v6a2 2 0 01-2 2h-2v4l-4-4H9a1.994 1.994 0 01-1.414-.586m0 0L11 14h4a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2v4l.586-.586z" />
                      </svg>
                    </div>
                    <div>
                      <h3 className="text-lg font-medium text-white mb-1">Live Chat</h3>
                      <p className="text-gray-300 mb-3">
                        Get immediate assistance from our support team through our live chat service.
                      </p>
                      <Button className="bg-white/10 text-white hover:bg-white/20">
                        Start Live Chat
                      </Button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* FAQ section */}
      <div className="container mx-auto px-4 mt-24">
        <div className="max-w-3xl mx-auto">
          <h2 className="text-3xl font-bold mb-10 text-center text-white">
            Frequently Asked <span className="text-gold">Questions</span>
          </h2>

          <div className="space-y-6">
            <FaqItem
              question="How quickly can I expect a response to my inquiry?"
              answer="We strive to respond to all inquiries within 24 hours during business days. For urgent technical issues, our support team typically responds within a few hours."
            />

            <FaqItem
              question="I'm having trouble with my XAUBOT installation. How can I get help?"
              answer="For technical support, please contact our support team at support@xaubot.com with details about your issue, including your operating system, MetaTrader version, and any error messages you're receiving. You can also use our live chat for real-time assistance."
            />

            <FaqItem
              question="Do you offer personalized consulting services?"
              answer="Yes, we offer personalized consulting services for enterprise clients and professional traders. Please contact us at info@xaubot.com with details about your specific needs, and our team will get back to you with a customized solution."
            />
          </div>
        </div>
      </div>
    </div>
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
