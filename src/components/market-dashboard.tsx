"use client";

import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { motion } from 'framer-motion';

interface PriceData {
  price: number;
  change: number;
  changePercent: number;
  high: number;
  low: number;
  timestamp: string;
}

// Mock data - in a real implementation, this would come from an API
const mockGoldData: PriceData = {
  price: 2323.45,
  change: 12.80,
  changePercent: 0.55,
  high: 2330.20,
  low: 2305.10,
  timestamp: new Date().toISOString()
};

interface ChartDataPoint {
  time: string;
  value: number;
}

// Mock chart data - would be fetched from API in real implementation
const mockChartData: ChartDataPoint[] = Array(24).fill(0).map((_, i) => ({
  time: `${i}:00`,
  value: 2300 + Math.random() * 50
}));

export default function MarketDashboard() {
  const [goldData, setGoldData] = useState<PriceData>(mockGoldData);
  const [chartData, setChartData] = useState<ChartDataPoint[]>(mockChartData);
  const [timeframe, setTimeframe] = useState<string>("24h");

  // Simulate real-time updates
  useEffect(() => {
    const interval = setInterval(() => {
      const priceChange = (Math.random() - 0.5) * 2;
      setGoldData((prevData: PriceData) => ({
        ...prevData,
        price: Number((prevData.price + priceChange).toFixed(2)),
        change: Number((prevData.change + priceChange).toFixed(2)),
        changePercent: Number(((prevData.change + priceChange) / prevData.price * 100).toFixed(2)),
        timestamp: new Date().toISOString()
      }));
    }, 5000);

    return () => clearInterval(interval);
  }, []);

  // In a real implementation, this would fetch new data based on the timeframe
  const updateTimeframe = (newTimeframe: string) => {
    setTimeframe(newTimeframe);
    // Simulate different data for different timeframes
    const points = newTimeframe === "24h" ? 24 : newTimeframe === "7d" ? 7 : 30;
    const baseValue = newTimeframe === "24h" ? 2300 : newTimeframe === "7d" ? 2250 : 2150;
    
    const newChartData = Array(points).fill(0).map((_, i) => ({
      time: newTimeframe === "24h" ? `${i}:00` : 
            newTimeframe === "7d" ? `Day ${i+1}` : `Week ${i+1}`,
      value: baseValue + Math.random() * 100
    }));
    
    setChartData(newChartData);
  };

  return (
    <section className="py-20 bg-gradient-to-b from-black to-darkBlue">
      <div className="container mx-auto px-4">
        <div className="text-center mb-16">
          <h2 className="text-3xl md:text-4xl font-bold mb-4">
            <span className="text-white">Gold </span>
            <span className="text-gold">Market Dashboard</span>
          </h2>
          <p className="text-gray-300 max-w-2xl mx-auto">
            Real-time gold price data and market analysis powered by our AI trading algorithms.
          </p>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 mb-10">
          <motion.div 
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5 }}
            className="lg:col-span-2"
          >
            <Card className="border-gray-800 bg-gray-900/50 backdrop-blur-sm h-full">
              <CardHeader>
                <CardTitle className="text-xl font-bold text-white flex justify-between">
                  <span>XAU/USD Price Chart</span>
                  <span className={goldData.change >= 0 ? "text-green-500" : "text-red-500"}>
                    ${goldData.price.toFixed(2)}
                  </span>
                </CardTitle>
                <div className="flex items-center space-x-2">
                  <span className={`text-sm ${goldData.change >= 0 ? "text-green-500" : "text-red-500"}`}>
                    {goldData.change >= 0 ? "+" : ""}{goldData.change.toFixed(2)} ({goldData.change >= 0 ? "+" : ""}{goldData.changePercent.toFixed(2)}%)
                  </span>
                  <span className="text-xs text-gray-400">
                    Updated: {new Date(goldData.timestamp).toLocaleTimeString()}
                  </span>
                </div>
              </CardHeader>
              <CardContent>
                <Tabs defaultValue="24h" onValueChange={(value: string) => updateTimeframe(value)}>
                  <TabsList className="bg-gray-800/50 mb-4">
                    <TabsTrigger value="24h">24H</TabsTrigger>
                    <TabsTrigger value="7d">7D</TabsTrigger>
                    <TabsTrigger value="30d">30D</TabsTrigger>
                  </TabsList>
                  <TabsContent value="24h" className="h-[300px] relative">
                    <div className="h-full bg-gradient-to-b from-gold/5 to-transparent relative">
                      {/* Chart visualization - simplified for this example */}
                      <div className="absolute bottom-0 left-0 w-full h-[250px] flex items-end">
                        {chartData.map((point: ChartDataPoint, index: number) => (
                          <div key={index} className="flex-1 flex flex-col items-center">
                            <div 
                              className="w-4/5 bg-gold/70 rounded-t-sm" 
                              style={{ 
                                height: `${((point.value - 2300) / 50) * 200}px`,
                                transition: 'height 0.5s ease-in-out'
                              }}
                            ></div>
                            {index % 4 === 0 && (
                              <span className="text-xs text-gray-400 mt-1">{point.time}</span>
                            )}
                          </div>
                        ))}
                      </div>
                    </div>
                  </TabsContent>
                  <TabsContent value="7d" className="h-[300px]">
                    {/* 7 day chart would render here */}
                    <div className="h-full bg-gradient-to-b from-gold/5 to-transparent flex items-center justify-center">
                      <p className="text-gold">7-Day Price History</p>
                    </div>
                  </TabsContent>
                  <TabsContent value="30d" className="h-[300px]">
                    {/* 30 day chart would render here */}
                    <div className="h-full bg-gradient-to-b from-gold/5 to-transparent flex items-center justify-center">
                      <p className="text-gold">30-Day Price History</p>
                    </div>
                  </TabsContent>
                </Tabs>
              </CardContent>
            </Card>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.2 }}
          >
            <Card className="border-gray-800 bg-gray-900/50 backdrop-blur-sm h-full">
              <CardHeader>
                <CardTitle className="text-xl font-bold text-white">Market Insights</CardTitle>
                <CardDescription className="text-gray-300">
                  AI-powered analysis for XAU/USD
                </CardDescription>
              </CardHeader>
              <CardContent className="space-y-4">
                <div className="p-4 bg-gray-800/50 rounded-lg">
                  <h4 className="font-semibold text-white mb-2">Technical Indicators</h4>
                  <div className="space-y-2">
                    <div className="flex justify-between">
                      <span className="text-gray-300">RSI (14)</span>
                      <span className="text-yellow-400">58.3</span>
                    </div>
                    <div className="flex justify-between">
                      <span className="text-gray-300">MACD</span>
                      <span className="text-green-500">Bullish</span>
                    </div>
                    <div className="flex justify-between">
                      <span className="text-gray-300">Moving Avg (50)</span>
                      <span className="text-green-500">Above</span>
                    </div>
                    <div className="flex justify-between">
                      <span className="text-gray-300">Moving Avg (200)</span>
                      <span className="text-green-500">Above</span>
                    </div>
                  </div>
                </div>
                
                <div className="p-4 bg-gray-800/50 rounded-lg">
                  <h4 className="font-semibold text-white mb-2">XAUBOT Prediction</h4>
                  <div className="text-center my-2">
                    <span className="text-sm text-gold bg-gold/10 rounded-full px-3 py-1">
                      Bullish for next 24 hours
                    </span>
                  </div>
                  <p className="text-gray-300 text-sm">
                    Our AI algorithm detects strong upward momentum with potential resistance at $2,340.
                  </p>
                </div>
              </CardContent>
            </Card>
          </motion.div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.3 }}
          >
            <Card className="border-gray-800 bg-gray-900/50 backdrop-blur-sm">
              <CardHeader className="pb-2">
                <CardTitle className="text-lg font-bold text-white">Support Levels</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-2">
                  <div className="flex justify-between">
                    <span className="text-gray-300">Strong</span>
                    <span className="text-gold font-mono">$2,295.00</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-300">Medium</span>
                    <span className="text-gold font-mono">$2,310.50</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-300">Weak</span>
                    <span className="text-gold font-mono">$2,318.20</span>
                  </div>
                </div>
              </CardContent>
            </Card>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.4 }}
          >
            <Card className="border-gray-800 bg-gray-900/50 backdrop-blur-sm">
              <CardHeader className="pb-2">
                <CardTitle className="text-lg font-bold text-white">Resistance Levels</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="space-y-2">
                  <div className="flex justify-between">
                    <span className="text-gray-300">Weak</span>
                    <span className="text-gold font-mono">$2,330.60</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-300">Medium</span>
                    <span className="text-gold font-mono">$2,342.80</span>
                  </div>
                  <div className="flex justify-between">
                    <span className="text-gray-300">Strong</span>
                    <span className="text-gold font-mono">$2,355.00</span>
                  </div>
                </div>
              </CardContent>
            </Card>
          </motion.div>

          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.5 }}
          >
            <Card className="border-gray-800 bg-gray-900/50 backdrop-blur-sm">
              <CardHeader className="pb-2">
                <CardTitle className="text-lg font-bold text-white">Market Sentiment</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="flex flex-col items-center">
                  <div className="w-full bg-gray-700 rounded-full h-4 mb-2">
                    <div className="bg-gradient-to-r from-green-500 to-gold h-4 rounded-full" style={{ width: '68%' }}></div>
                  </div>
                  <div className="flex justify-between w-full text-xs">
                    <span className="text-red-500">Bearish</span>
                    <span className="text-green-500">Bullish</span>
                  </div>
                  <p className="text-center mt-3 text-gold">68% Bullish</p>
                </div>
              </CardContent>
            </Card>
          </motion.div>
        </div>
      </div>
    </section>
  );
}
