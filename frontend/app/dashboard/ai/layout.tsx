import { Metadata } from "next";
import { AiSidebar } from "./_components/ai-sidebar";

export const metadata: Metadata = {
  title: "AI Assistant | BusinessOS",
  description: "Chat with your AI assistant",
};

export default function AiLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="flex h-[calc(100vh-4rem)] overflow-hidden bg-background">
      <AiSidebar />
      <main className="flex-1 overflow-hidden flex flex-col relative">
        {children}
      </main>
    </div>
  );
}
