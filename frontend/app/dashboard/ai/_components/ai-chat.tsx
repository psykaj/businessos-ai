"use client";

import { useState, useEffect, useRef } from "react";
import { useRouter } from "next/navigation";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import { Send, Bot, User, Loader2 } from "lucide-react";
import { getConversation, createConversation, sendMessage, AIMessage } from "@/lib/api/ai";
import { Button } from "@/components/ui/button";
import { Textarea } from "@/components/ui/textarea";
import { ScrollArea } from "@/components/ui/scroll-area";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

interface AiChatProps {
  conversationId?: string;
}

export function AiChat({ conversationId }: AiChatProps) {
  const router = useRouter();
  const [messages, setMessages] = useState<AIMessage[]>([]);
  const [input, setInput] = useState("");
  const [loading, setLoading] = useState(false);
  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (conversationId) {
      const fetchChat = async () => {
        try {
          setLoading(true);
          const data = await getConversation(conversationId);
          setMessages(data.messages || []);
        } catch (error) {
          toast.error("Failed to load conversation");
        } finally {
          setLoading(false);
        }
      };
      fetchChat();
    } else {
      setMessages([]);
    }
  }, [conversationId]);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollTop = scrollRef.current.scrollHeight;
    }
  }, [messages, loading]);

  const handleSubmit = async (e?: React.FormEvent) => {
    e?.preventDefault();
    if (!input.trim() || loading) return;

    const userMessage = input.trim();
    setInput("");

    // Optimistic update
    const tempId = Date.now().toString();
    setMessages(prev => [...prev, {
      id: tempId,
      conversationId: conversationId || "",
      role: 'user',
      content: userMessage,
      tokenUsage: 0,
      createdAt: new Date().toISOString()
    }]);

    try {
      setLoading(true);
      let targetId = conversationId;
      
      if (!targetId) {
        // Create new conversation
        const newConv = await createConversation({ title: userMessage.substring(0, 30) });
        targetId = newConv.id;
        // The router push will unmount this and mount the new route, but we still await sendMessage
      }

      await sendMessage(targetId, userMessage);
      
      if (!conversationId) {
        router.push(`/dashboard/ai/${targetId}`);
      } else {
        // Refresh messages
        const data = await getConversation(targetId);
        setMessages(data.messages || []);
      }
    } catch (error) {
      toast.error("Failed to send message");
      setMessages(prev => prev.filter(m => m.id !== tempId)); // Remove optimistic message on error
    } finally {
      setLoading(false);
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSubmit();
    }
  };

  return (
    <div className="flex flex-col h-full w-full max-w-4xl mx-auto">
      <ScrollArea className="flex-1 p-4" ref={scrollRef}>
        <div className="space-y-6 pb-20">
          {messages.length === 0 && !loading && (
            <div className="flex flex-col items-center justify-center h-full text-center mt-32">
              <div className="h-16 w-16 bg-primary/10 rounded-full flex items-center justify-center mb-6">
                <Bot className="h-8 w-8 text-primary" />
              </div>
              <h2 className="text-2xl font-bold mb-2">How can I help you today?</h2>
              <p className="text-muted-foreground max-w-md">
                I'm your BusinessOS AI assistant. I can help you draft emails, analyze customer data, write code, or automate workflows.
              </p>
            </div>
          )}

          {messages.map((msg) => (
            <div key={msg.id} className={cn("flex gap-4 w-full", msg.role === 'user' ? "justify-end" : "justify-start")}>
              {msg.role === 'assistant' && (
                <div className="h-8 w-8 rounded-full bg-primary flex items-center justify-center shrink-0 mt-1">
                  <Bot className="h-5 w-5 text-primary-foreground" />
                </div>
              )}
              
              <div className={cn(
                "px-4 py-3 rounded-2xl max-w-[85%]", 
                msg.role === 'user' ? "bg-primary text-primary-foreground rounded-tr-sm" : "bg-muted rounded-tl-sm"
              )}>
                {msg.role === 'user' ? (
                  <p className="whitespace-pre-wrap">{msg.content}</p>
                ) : (
                  <div className="prose prose-sm dark:prose-invert max-w-none break-words">
                    <ReactMarkdown remarkPlugins={[remarkGfm]}>
                      {msg.content}
                    </ReactMarkdown>
                  </div>
                )}
              </div>
              
              {msg.role === 'user' && (
                <div className="h-8 w-8 rounded-full bg-secondary flex items-center justify-center shrink-0 mt-1">
                  <User className="h-5 w-5 text-secondary-foreground" />
                </div>
              )}
            </div>
          ))}

          {loading && messages.length > 0 && (
             <div className="flex gap-4 w-full justify-start">
               <div className="h-8 w-8 rounded-full bg-primary flex items-center justify-center shrink-0 mt-1">
                 <Bot className="h-5 w-5 text-primary-foreground" />
               </div>
               <div className="px-4 py-3 rounded-2xl bg-muted rounded-tl-sm flex items-center gap-2">
                 <Loader2 className="h-4 w-4 animate-spin text-muted-foreground" />
                 <span className="text-sm text-muted-foreground">AI is thinking...</span>
               </div>
             </div>
          )}
        </div>
      </ScrollArea>

      <div className="p-4 bg-background border-t">
        <form 
          onSubmit={handleSubmit}
          className="relative flex items-center shadow-sm border rounded-xl bg-background overflow-hidden focus-within:ring-1 focus-within:ring-primary"
        >
          <Textarea
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={handleKeyDown}
            placeholder="Type your message..."
            className="min-h-[56px] max-h-32 resize-none border-0 focus-visible:ring-0 shadow-none py-4 px-4 bg-transparent"
            disabled={loading}
          />
          <div className="absolute right-2 bottom-2">
            <Button 
              type="submit" 
              size="icon" 
              disabled={!input.trim() || loading}
              className="h-10 w-10 rounded-lg"
            >
              <Send className="h-4 w-4" />
            </Button>
          </div>
        </form>
        <p className="text-center text-[10px] text-muted-foreground mt-2">
          AI can make mistakes. Verify important information.
        </p>
      </div>
    </div>
  );
}
