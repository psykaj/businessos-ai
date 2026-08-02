"use client";

import React, { useState } from "react";
import {
  Send,
  Paperclip,
  Sparkles,
  Lock,
  MessageSquare,
  FileText,
  HelpCircle,
  CheckCircle,
  Smile,
  Zap,
  Loader2
} from "lucide-react";
import { useSendMessage, useAddInternalNote, useMessageTemplates } from "@/hooks/use-communication";
import { CommunicationChannelType, ConversationDto } from "@/lib/communication-service";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";

interface MessageComposerProps {
  conversation: ConversationDto;
  onSendSuccess?: () => void;
}

export function MessageComposer({ conversation, onSendSuccess }: MessageComposerProps) {
  const [mode, setMode] = useState<"Reply" | "Note">("Reply");
  const [content, setContent] = useState("");
  const [isGeneratingAi, setIsGeneratingAi] = useState(false);
  const [showTemplatePicker, setShowTemplatePicker] = useState(false);

  const sendMessageMutation = useSendMessage();
  const addNoteMutation = useAddInternalNote();

  const { data: templates } = useMessageTemplates(conversation.channelType);

  const isPending = sendMessageMutation.isPending || addNoteMutation.isPending;

  const handleSubmit = (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    if (!content.trim()) return;

    if (mode === "Reply") {
      sendMessageMutation.mutate(
        { conversationId: conversation.id, content: content.trim() },
        {
          onSuccess: () => {
            setContent("");
            if (onSendSuccess) onSendSuccess();
          }
        }
      );
    } else {
      addNoteMutation.mutate(
        { conversationId: conversation.id, content: content.trim() },
        {
          onSuccess: () => {
            setContent("");
            if (onSendSuccess) onSendSuccess();
          }
        }
      );
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if ((e.metaKey || e.ctrlKey) && e.key === "Enter") {
      e.preventDefault();
      handleSubmit();
    }
  };

  const handleApplyTemplate = (templateContent: string) => {
    let replaced = templateContent.replace(/{{customer.name}}/g, conversation.customerName);
    replaced = replaced.replace(/{{customer.company}}/g, conversation.customerCompany || "your company");
    setContent(prev => (prev ? `${prev}\n\n${replaced}` : replaced));
    setShowTemplatePicker(false);
    toast.success("Applied template & substituted dynamic customer variables!");
  };

  const handleTriggerAiReply = () => {
    setIsGeneratingAi(true);
    setTimeout(() => {
      const draft = `Hello ${conversation.customerName}! I reviewed your inquiry regarding our Enterprise SLA terms. Based on your account volume with ${conversation.customerCompany || "us"}, we can approve the custom terms immediately. Let me know if you would like me to finalize the digital agreement today!`;
      setContent(draft);
      setIsGeneratingAi(false);
      toast.success("AI suggested response generated with customer context!");
    }, 900);
  };

  return (
    <div className="w-full border-t border-border/80 bg-card/60 backdrop-blur-md p-4 space-y-3">
      {/* Mode switcher tabs */}
      <div className="flex items-center justify-between gap-2">
        <div className="flex items-center p-1 rounded-xl bg-muted/60 border border-border/40 gap-1 text-xs font-semibold">
          <button
            type="button"
            onClick={() => setMode("Reply")}
            className={`px-3 py-1.5 rounded-lg flex items-center gap-1.5 transition-all ${
              mode === "Reply"
                ? "bg-background text-primary shadow-xs"
                : "text-muted-foreground hover:text-foreground"
            }`}
          >
            <MessageSquare className="w-3.5 h-3.5" />
            <span>Reply ({conversation.channelType})</span>
          </button>
          <button
            type="button"
            onClick={() => setMode("Note")}
            className={`px-3 py-1.5 rounded-lg flex items-center gap-1.5 transition-all ${
              mode === "Note"
                ? "bg-amber-500/15 text-amber-600 dark:text-amber-400 font-bold shadow-xs"
                : "text-muted-foreground hover:text-foreground"
            }`}
          >
            <Lock className="w-3.5 h-3.5 text-amber-500" />
            <span>Internal Team Note</span>
          </button>
        </div>

        <div className="flex items-center gap-2">
          {/* AI Quick Reply Draft Button */}
          {mode === "Reply" && (
            <Button
              type="button"
              variant="outline"
              size="sm"
              onClick={handleTriggerAiReply}
              disabled={isGeneratingAi}
              className="h-8 px-2.5 text-xs font-semibold text-primary bg-primary/5 border-primary/20 hover:bg-primary/10 gap-1.5"
            >
              {isGeneratingAi ? (
                <Loader2 className="w-3.5 h-3.5 animate-spin" />
              ) : (
                <Sparkles className="w-3.5 h-3.5 text-amber-500" />
              )}
              <span>AI Suggest Answer</span>
            </Button>
          )}

          {/* Quick template picker dropdown toggle */}
          <div className="relative">
            <Button
              type="button"
              variant="outline"
              size="sm"
              onClick={() => setShowTemplatePicker(!showTemplatePicker)}
              className="h-8 px-2.5 text-xs text-muted-foreground hover:text-foreground gap-1.5 border-border"
            >
              <Zap className="w-3.5 h-3.5 text-indigo-500" />
              <span>Quick Replies</span>
            </Button>

            {/* Floating Template Selector Modal */}
            {showTemplatePicker && (
              <div className="absolute bottom-10 right-0 w-80 max-h-72 overflow-y-auto rounded-xl bg-card border border-border shadow-xl p-2 z-50 text-xs animate-in fade-in zoom-in-95 duration-150">
                <div className="flex items-center justify-between p-2 font-bold border-b border-border text-muted-foreground">
                  <span>Canned Message Templates</span>
                  <span className="text-2xs font-normal">Click to insert</span>
                </div>
                {templates && templates.length > 0 ? (
                  <div className="divide-y divide-border/40">
                    {templates.map(t => (
                      <div
                        key={t.id}
                        onClick={() => handleApplyTemplate(t.content)}
                        className="p-2.5 hover:bg-muted/70 rounded-lg cursor-pointer transition-colors"
                      >
                        <div className="flex items-center justify-between font-bold text-foreground">
                          <span className="truncate">{t.title}</span>
                          <span className="text-2xs font-mono text-primary px-1.5 py-0.5 rounded bg-primary/10">
                            {t.shortcutCode}
                          </span>
                        </div>
                        <p className="text-2xs text-muted-foreground line-clamp-2 mt-1 font-normal">
                          {t.content}
                        </p>
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="p-4 text-center text-muted-foreground text-2xs">No templates configured yet.</div>
                )}
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Text input box */}
      <div className={`relative rounded-xl border p-2.5 transition-all ${
        mode === "Note"
          ? "bg-amber-500/5 border-amber-500/40 focus-within:ring-2 focus-within:ring-amber-500/30"
          : "bg-background border-border/80 focus-within:border-primary focus-within:ring-2 focus-within:ring-primary/20"
      }`}>
        <textarea
          value={content}
          onChange={e => setContent(e.target.value)}
          onKeyDown={handleKeyDown}
          placeholder={
            mode === "Reply"
              ? `Write your message to ${conversation.customerName} via ${conversation.channelType}... (Cmd + Enter to send)`
              : `Type a confidential team note... (invisible to customer)`
          }
          className="w-full text-xs font-normal bg-transparent border-0 focus:outline-hidden text-foreground resize-none min-h-[80px] placeholder:text-muted-foreground/70"
        />

        <div className="flex items-center justify-between pt-2 border-t border-border/40 mt-1">
          <div className="flex items-center gap-1 text-muted-foreground">
            <Button
              type="button"
              variant="ghost"
              size="icon"
              className="w-7 h-7 rounded-lg hover:text-foreground"
              title="Attach document or screenshot"
              onClick={() => toast.info("Attachment uploading ready via Azure/Firebase storage adapter.")}
            >
              <Paperclip className="w-4 h-4" />
            </Button>
            <Button
              type="button"
              variant="ghost"
              size="icon"
              className="w-7 h-7 rounded-lg hover:text-foreground"
              title="Insert Emoji"
            >
              <Smile className="w-4 h-4" />
            </Button>
            <span className="text-2xs ml-2 opacity-60 hidden sm:inline-block">
              Pro tip: Press ⌘ + Enter to dispatch
            </span>
          </div>

          <Button
            type="button"
            size="sm"
            onClick={() => handleSubmit()}
            disabled={isPending || !content.trim()}
            className={`text-xs font-semibold px-4 h-8 gap-1.5 shadow-sm transition-all ${
              mode === "Note"
                ? "bg-amber-600 hover:bg-amber-700 text-white"
                : "bg-primary hover:bg-primary/90 text-primary-foreground"
            }`}
          >
            {isPending ? (
              <Loader2 className="w-3.5 h-3.5 animate-spin" />
            ) : mode === "Note" ? (
              <Lock className="w-3.5 h-3.5" />
            ) : (
              <Send className="w-3.5 h-3.5" />
            )}
            <span>{mode === "Note" ? "Add Internal Note" : "Send Reply"}</span>
          </Button>
        </div>
      </div>
    </div>
  );
}
