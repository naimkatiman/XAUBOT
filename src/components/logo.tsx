import React from 'react';
import Link from 'next/link';

interface LogoProps {
  className?: string;
  width?: number;
  height?: number;
}

export const Logo: React.FC<LogoProps> = ({
  className = '',
  width = 150,
  height = 40
}) => {
  return (
    <Link href="/" className={`flex items-center ${className}`}>
      <div className="flex items-center">
        <div className="relative flex items-center">
          <div className="flex justify-center items-center rounded-full overflow-hidden bg-indigo-900 mr-2 w-10 h-10">
            <div className="logo-icon relative flex items-center justify-center">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 500 500"
                width="30"
                height="30"
                className="text-white"
              >
                <g fill="currentColor">
                  <circle cx="250" cy="200" r="100" fill="transparent" stroke="currentColor" strokeWidth="12"/>
                  <g transform="translate(245, 195) scale(0.8)">
                    <path d="M-30,-40 L30,-40 L30,-35 L0,0 L30,35 L30,40 L-30,40 L-30,35 L0,0 L-30,-35 Z" />
                    <rect x="-5" y="-40" width="10" height="80" />
                  </g>
                  <path d="M160,250 Q250,220 340,250 L320,330 Q250,310 180,330 Z" />
                  <path d="M170,260 Q140,280 130,310 Q120,330 140,340 Q160,330 165,315 Z" />
                  <path d="M330,260 Q360,280 370,310 Q380,330 360,340 Q340,330 335,315 Z" />
                  <path d="M170,330 Q250,350 330,330 Q300,380 200,380 Z" />
                  <path d="M360,160 L365,160 L365,120 L395,130 L365,140 Z" />
                  <rect x="354" y="120" width="6" height="80" />
                </g>
              </svg>
            </div>
          </div>
          <span className="font-bold tracking-wide">
            <span className="text-gold">XAU</span>
            <span className="text-white">BOT</span>
          </span>
        </div>
      </div>
    </Link>
  );
};

export default Logo;
