import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { cn } from "@/lib/utils";
import { ThemeProvider } from "@/hooks/use-theme";

const inter = Inter({
  subsets: ["latin"],
  variable: "--font-sans",
});

export const metadata: Metadata = {
  title: "BusinessOS AI — Dashboard",
  description:
    "AI-powered Business Operating System for SMEs. Manage QR codes, reviews, invoices, customers, and more.",
};

/**
 * Inline script that runs before React hydration to prevent a flash of the
 * wrong theme. Reads the stored preference from localStorage, falls back to
 * the system preference, and applies the `.dark` class immediately.
 */
const themeScript = `
(function(){
  try {
    var stored = localStorage.getItem('businessos-theme');
    var theme = stored || 'system';
    var dark = theme === 'dark' || (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches);
    if (dark) document.documentElement.classList.add('dark');
  } catch(e) {}
})();
`;

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en" className={cn("h-full antialiased", inter.variable)} suppressHydrationWarning>
      <head>
        <script dangerouslySetInnerHTML={{ __html: themeScript }} />
      </head>
      <body className="min-h-full font-sans" suppressHydrationWarning>
        <ThemeProvider>{children}</ThemeProvider>
      </body>
    </html>
  );
}
