"use client";

import React, { useState, useEffect } from 'react';
import Link from 'next/link';
import { Button } from '@/components/ui/button';
import Logo from '@/components/logo';
import { usePathname } from 'next/navigation';

interface NavItemProps {
  href: string;
  children: React.ReactNode;
  isActive?: boolean;
  className?: string;
}

const NavItem = ({ href, children, isActive = false, className = '' }: NavItemProps) => (
  <li className={`mx-2 ${className}`}>
    <Link
      href={href}
      className={`text-gray-200 hover:text-gold transition-colors duration-200 py-2 block
        ${isActive ? 'text-gold font-medium' : ''}`}
    >
      {children}
    </Link>
  </li>
);

export default function Header() {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const pathname = usePathname();

  // Close mobile menu when route changes
  useEffect(() => {
    setIsMenuOpen(false);
  }, [pathname]);

  return (
    <header className="bg-darkBlue border-b border-gray-800 sticky top-0 z-50 backdrop-blur-sm bg-opacity-80">
      <div className="container mx-auto px-4 py-4">
        <div className="flex justify-between items-center">
          <Logo />

          {/* Mobile menu button */}
          <button
            onClick={() => setIsMenuOpen(!isMenuOpen)}
            className="md:hidden text-gray-200 focus:outline-none"
          >
            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
              {isMenuOpen ? (
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              ) : (
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
              )}
            </svg>
          </button>

          {/* Desktop Navigation */}
          <nav className="hidden md:block">
            <ul className="flex items-center">
              <NavItem href="/" isActive={pathname === '/'}>Home</NavItem>
              <NavItem href="/products" isActive={pathname === '/products'}>Products</NavItem>
              <NavItem href="/pricing" isActive={pathname === '/pricing'}>Pricing</NavItem>
              <NavItem href="/blog" isActive={pathname.startsWith('/blog')}>Blog</NavItem>
              <NavItem href="/about" isActive={pathname === '/about'}>About</NavItem>
              <NavItem href="/contact" isActive={pathname === '/contact'}>Contact</NavItem>
              <li className="ml-4">
                <Button className="bg-gold text-black hover:bg-gold/90">
                  Get Started
                </Button>
              </li>
            </ul>
          </nav>
        </div>

        {/* Mobile Navigation */}
        {isMenuOpen && (
          <nav className="md:hidden mt-4 border-t border-gray-800 pt-4 animate-fadeIn">
            <ul className="flex flex-col space-y-3">
              <NavItem href="/" isActive={pathname === '/'}>Home</NavItem>
              <NavItem href="/products" isActive={pathname === '/products'}>Products</NavItem>
              <NavItem href="/pricing" isActive={pathname === '/pricing'}>Pricing</NavItem>
              <NavItem href="/blog" isActive={pathname.startsWith('/blog')}>Blog</NavItem>
              <NavItem href="/about" isActive={pathname === '/about'}>About</NavItem>
              <NavItem href="/contact" isActive={pathname === '/contact'}>Contact</NavItem>
              <li className="pt-2">
                <Button className="bg-gold text-black hover:bg-gold/90 w-full">
                  Get Started
                </Button>
              </li>
            </ul>
          </nav>
        )}
      </div>
    </header>
  );
}
