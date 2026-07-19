'use client';

import { useState } from 'react';
import { billingService } from '@/lib/billing-service';
import { Button } from '@/components/ui/button';
import { toast } from 'sonner';

interface RazorpayCheckoutProps {
  planId: string;
  planName: string;
  amount: number;
  billingCycle: 'monthly' | 'yearly';
  onSuccess?: () => void;
  buttonText?: string;
  className?: string;
}

export function RazorpayCheckout({ planId, planName, amount, billingCycle, onSuccess, buttonText = "Subscribe Now", className }: RazorpayCheckoutProps) {
  const [isLoading, setIsLoading] = useState(false);

  const loadRazorpayScript = (): Promise<boolean> => {
    return new Promise((resolve) => {
      if (document.getElementById('razorpay-script')) {
        resolve(true);
        return;
      }
      const script = document.createElement('script');
      script.id = 'razorpay-script';
      script.src = 'https://checkout.razorpay.com/v1/checkout.js';
      script.onload = () => resolve(true);
      script.onerror = () => resolve(false);
      document.body.appendChild(script);
    });
  };

  const handlePayment = async () => {
    setIsLoading(true);
    
    try {
      const res = await loadRazorpayScript();
      if (!res) {
        toast.error('Failed to load Razorpay SDK. Are you online?');
        setIsLoading(false);
        return;
      }

      // Create Order on Backend
      const order = await billingService.createRazorpayOrder(planId, billingCycle);

      const options = {
        key: order.key, 
        amount: order.amount,
        currency: 'USD', 
        name: 'BusinessOS AI',
        description: `Subscription to ${planName} Plan (${billingCycle})`,
        image: '/logo.png', // Fallback to a logo if available
        order_id: order.orderId,
        handler: async function (response: { razorpay_payment_id: string; razorpay_order_id: string; razorpay_signature: string }) {
          try {
            toast.loading('Verifying payment...', { id: 'payment-verification' });
            // Verify payment
            const isSuccess = await billingService.verifyRazorpayPayment(
              response.razorpay_payment_id,
              response.razorpay_order_id,
              response.razorpay_signature
            );
            
            if (isSuccess) {
              toast.success('Subscription activated successfully!', { id: 'payment-verification' });
              if (onSuccess) onSuccess();
            } else {
              toast.error('Payment verification failed. Please contact support.', { id: 'payment-verification' });
            }
          } catch (error) {
            toast.error('Error verifying payment.', { id: 'payment-verification' });
          }
        },
        prefill: {
          name: 'BusinessOS User',
          email: 'user@example.com',
          contact: '9999999999'
        },
        theme: {
          color: '#000000'
        }
      };

      const Razorpay = (window as unknown as { Razorpay: new (opts: unknown) => { on: (evt: string, cb: (res: { error: { description: string } }) => void) => void; open: () => void } }).Razorpay;
      const paymentObject = new Razorpay(options);
      
      paymentObject.on('payment.failed', function (response: { error: { description: string } }) {
        toast.error(`Payment failed: ${response.error.description}`);
      });
      
      paymentObject.open();

    } catch (error) {
      toast.error('An error occurred during checkout setup.');
      console.error(error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Button 
      onClick={handlePayment} 
      disabled={isLoading}
      className={className}
    >
      {isLoading ? 'Processing...' : buttonText}
    </Button>
  );
}
