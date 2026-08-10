"use client";

import { useState } from "react";
import { useAskBusinessOS } from "@/lib/command-center-service";
import { Card, CardContent } from "@/components/ui/card";
import { Button, buttonVariants } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Bot, Send, ArrowRight, Loader2, Sparkles } from "lucide-react";
import Link from "next/link";

export function CommandCenterAskAi() {
  const [question, setQuestion] = useState("");
  const { mutate, data: response, isPending, error } = useAskBusinessOS();

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!question.trim() || isPending) return;
    mutate({ Question: question });
  };

  return (
    <Card className="overflow-hidden border-primary/20 shadow-md">
      <div className="bg-primary/5 p-4 border-b border-primary/10">
        <form onSubmit={handleSubmit} className="flex gap-2 relative">
          <div className="absolute left-3 top-1/2 -translate-y-1/2 text-primary">
            <Sparkles className="w-5 h-5" />
          </div>
          <Input
            value={question}
            onChange={(e) => setQuestion(e.target.value)}
            placeholder="Ask anything about your business... (e.g. 'Why did sales drop?' or 'What invoices need attention?')"
            className="pl-10 py-6 text-base bg-background shadow-sm border-primary/20 focus-visible:ring-primary"
            disabled={isPending}
          />
          <Button type="submit" disabled={!question.trim() || isPending} className="py-6 px-6 shadow-sm">
            {isPending ? <Loader2 className="w-5 h-5 animate-spin" /> : <Send className="w-5 h-5" />}
          </Button>
        </form>
      </div>

      {/* Results Area */}
      {(isPending || response || error) && (
        <CardContent className="p-6 bg-background animate-in fade-in slide-in-from-top-4 duration-300">
          {isPending && (
            <div className="flex items-center space-x-4 text-muted-foreground">
              <Bot className="w-8 h-8 p-1.5 bg-primary/10 rounded-full text-primary animate-pulse" />
              <div className="space-y-2 flex-1">
                <div className="h-4 bg-muted rounded animate-pulse w-3/4"></div>
                <div className="h-4 bg-muted rounded animate-pulse w-1/2"></div>
              </div>
            </div>
          )}

          {error && !isPending && (
            <div className="text-sm text-red-500">
              I'm currently unable to process your request. Please try again later.
            </div>
          )}

          {response && !isPending && (
            <div className="space-y-4">
              <div className="flex gap-4 items-start">
                <div className="shrink-0">
                  <div className="w-8 h-8 rounded-full bg-primary flex items-center justify-center text-primary-foreground">
                    <Bot className="w-5 h-5" />
                  </div>
                </div>
                <div className="space-y-3 flex-1">
                  <div className="text-sm leading-relaxed">{response.Answer}</div>
                  
                  {response.SuggestedActions && response.SuggestedActions.length > 0 && (
                    <div className="mt-4 p-4 rounded-lg border bg-muted/30">
                      <h4 className="text-xs font-semibold uppercase tracking-wider text-muted-foreground mb-3">
                        Suggested Actions
                      </h4>
                      <div className="flex flex-wrap gap-2">
                        {response.SuggestedActions.map((action, idx) => (
                          <Link href="/dashboard/action-center" key={idx} className={buttonVariants({ size: "sm", variant: "secondary", className: "text-xs" })}>
                            {action.Description}
                            <ArrowRight className="w-3 h-3 ml-2" />
                          </Link>
                        ))}
                      </div>
                    </div>
                  )}
                </div>
              </div>
            </div>
          )}
        </CardContent>
      )}
    </Card>
  );
}
