"use client";

import { useState, useRef, useEffect } from "react";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import {
  Send,
  Sparkles,
  Bot,
  User,
  Copy,
  RefreshCw,
  Plus,
  Trash2,
  Check,
  AlertCircle,
  Command as CommandIcon,
  ChevronRight,
  MessageSquare,
  Search,
} from "lucide-react";
import { useCopilot } from "@/hooks/use-copilot";
import { Message, ExecutionResponse } from "@/lib/copilot-service";
import { CommandCenter } from "./CommandCenter";
import { ActionConfirmationModal } from "./ActionConfirmationModal";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

export function CopilotWorkspace() {
  const [activeConversationId, setActiveConversationId] = useState<string | null>(null);
  const [prompt, setPrompt] = useState("");
  const [copiedMsgId, setCopiedMsgId] = useState<string | null>(null);
  const [showCommandCenter, setShowCommandCenter] = useState(true);
  const [pendingConfirmation, setPendingConfirmation] = useState<{
    command: string;
    toolName: string;
    description: string;
  } | null>(null);

  const messagesEndRef = useRef<HTMLDivElement>(null);

  const {
    useConversations,
    useConversation,
    createConversation,
    deleteConversation,
    executeCommand,
    isExecuting,
  } = useCopilot();

  const { data: conversationsData, isLoading: isLoadingConversations } = useConversations();
  const { data: activeConversation, isLoading: isLoadingConversation } = useConversation(activeConversationId);

  const conversations = conversationsData?.items || [];
  const currentMessages: Message[] = activeConversation?.messages || [];

  // Auto scroll to bottom
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [currentMessages, isExecuting]);

  // Handle prompt submit
  const handleSubmitPrompt = async (customCommand?: string, isConfirmed: boolean = false) => {
    const textToExecute = customCommand || prompt;
    if (!textToExecute.trim() || isExecuting) return;

    let targetConvId = activeConversationId;
    if (!targetConvId) {
      const newConv = await createConversation("Business Assistant Chat");
      targetConvId = newConv.id;
      setActiveConversationId(newConv.id);
    }

    if (!customCommand) setPrompt("");
    setShowCommandCenter(false);

    try {
      const res: ExecutionResponse = await executeCommand({
        command: textToExecute,
        conversationId: targetConvId,
        isConfirmed: isConfirmed,
      });

      if (res.status === "RequiresConfirmation") {
        setPendingConfirmation({
          command: textToExecute,
          toolName: res.toolInvoked,
          description: res.resultSummary || "This action mutates business data and requires explicit approval.",
        });
      }
    } catch (err) {
      console.error("Execution error:", err);
    }
  };

  const handleCopyMessage = (content: string, id: string) => {
    navigator.clipboard.writeText(content);
    setCopiedMsgId(id);
    toast.success("Copied to clipboard");
    setTimeout(() => setCopiedMsgId(null), 2000);
  };

  const handleConfirmAction = async () => {
    if (!pendingConfirmation) return;
    const cmd = pendingConfirmation.command;
    setPendingConfirmation(null);
    await handleSubmitPrompt(cmd, true);
  };

  return (
    <div className="flex h-[calc(100vh-8.5rem)] w-full overflow-hidden rounded-2xl border border-border/60 bg-card shadow-xl">
      {/* Sidebar: Conversation History */}
      <div className="hidden w-72 flex-col border-r border-border/50 bg-muted/20 md:flex">
        <div className="flex items-center justify-between border-b border-border/50 p-4">
          <span className="text-xs font-bold uppercase tracking-wider text-muted-foreground">
            Threads
          </span>
          <button
            onClick={async () => {
              const conv = await createConversation("New Conversation");
              setActiveConversationId(conv.id);
            }}
            className="flex items-center gap-1.5 rounded-lg bg-indigo-600 px-2.5 py-1 text-xs font-semibold text-white shadow-sm hover:bg-indigo-700 transition-colors"
          >
            <Plus className="h-3.5 w-3.5" />
            New Thread
          </button>
        </div>

        <div className="flex-1 overflow-y-auto p-2 space-y-1">
          {isLoadingConversations ? (
            <div className="p-4 text-center text-xs text-muted-foreground animate-pulse">
              Loading conversations...
            </div>
          ) : conversations.length === 0 ? (
            <div className="p-4 text-center text-xs text-muted-foreground">
              No conversations yet.
            </div>
          ) : (
            conversations.map((conv: any) => {
              const isActive = activeConversationId === conv.id;
              return (
                <div
                  key={conv.id}
                  onClick={() => setActiveConversationId(conv.id)}
                  className={cn(
                    "group flex items-center justify-between rounded-xl px-3 py-2.5 text-xs font-medium cursor-pointer transition-all",
                    isActive
                      ? "bg-indigo-600/10 text-indigo-600 dark:text-indigo-400 font-semibold"
                      : "text-muted-foreground hover:bg-muted hover:text-foreground"
                  )}
                >
                  <div className="flex items-center gap-2.5 min-w-0">
                    <MessageSquare className="h-3.5 w-3.5 shrink-0" />
                    <span className="truncate">{conv.title}</span>
                  </div>
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                      deleteConversation(conv.id);
                      if (activeConversationId === conv.id) setActiveConversationId(null);
                    }}
                    className="opacity-0 group-hover:opacity-100 p-1 text-muted-foreground hover:text-red-500 transition-opacity"
                  >
                    <Trash2 className="h-3.5 w-3.5" />
                  </button>
                </div>
              );
            })
          )}
        </div>
      </div>

      {/* Main Chat Workspace */}
      <div className="flex flex-1 flex-col justify-between bg-background/50">
        {/* Top Chat Header */}
        <div className="flex items-center justify-between border-b border-border/50 px-6 py-3 bg-card/40 backdrop-blur-sm">
          <div className="flex items-center gap-2">
            <Bot className="h-5 w-5 text-indigo-500" />
            <h2 className="text-sm font-semibold text-foreground">
              {activeConversation?.title || "AI Business Assistant"}
            </h2>
          </div>
          <button
            onClick={() => setShowCommandCenter(!showCommandCenter)}
            className="flex items-center gap-1.5 rounded-lg border border-border/60 bg-background px-3 py-1.5 text-xs font-medium text-muted-foreground hover:bg-muted hover:text-foreground transition-colors"
          >
            <CommandIcon className="h-3.5 w-3.5 text-indigo-500" />
            {showCommandCenter ? "Hide Shortcuts" : "Command Center"}
          </button>
        </div>

        {/* Message Thread Container */}
        <div className="flex-1 overflow-y-auto p-6 space-y-6">
          {showCommandCenter && (
            <div className="mb-4">
              <CommandCenter
                onSelectCommand={(cmd) => {
                  setPrompt(cmd);
                  handleSubmitPrompt(cmd);
                }}
              />
            </div>
          )}

          {currentMessages.length === 0 && !showCommandCenter && (
            <div className="flex flex-col items-center justify-center py-16 text-center">
              <div className="flex h-16 w-16 items-center justify-center rounded-2xl bg-indigo-500/10 text-indigo-500 mb-4 shadow-inner">
                <Sparkles className="h-8 w-8 animate-pulse" />
              </div>
              <h3 className="text-base font-bold text-foreground">
                How can I assist your business today?
              </h3>
              <p className="text-xs text-muted-foreground max-w-sm mt-1">
                Ask about revenue, leads, invoices, marketing campaigns, or execute workflows in plain English.
              </p>
            </div>
          )}

          {currentMessages.map((msg) => {
            const isUser = msg.role === "User";
            return (
              <div
                key={msg.id}
                className={cn(
                  "flex gap-4 max-w-3xl",
                  isUser ? "ml-auto flex-row-reverse" : "mr-auto"
                )}
              >
                <div
                  className={cn(
                    "flex h-8 w-8 shrink-0 items-center justify-center rounded-xl text-xs font-bold shadow-sm",
                    isUser
                      ? "bg-indigo-600 text-white"
                      : "bg-gradient-to-tr from-indigo-500 to-purple-600 text-white"
                  )}
                >
                  {isUser ? <User className="h-4 w-4" /> : <Bot className="h-4 w-4" />}
                </div>

                <div className="group relative flex flex-col gap-1 min-w-0">
                  <div
                    className={cn(
                      "rounded-2xl px-4 py-3 text-xs leading-relaxed shadow-sm",
                      isUser
                        ? "bg-indigo-600 text-white rounded-tr-none"
                        : "border border-border/60 bg-card text-foreground rounded-tl-none"
                    )}
                  >
                    <ReactMarkdown remarkPlugins={[remarkGfm]}>
                      {msg.content}
                    </ReactMarkdown>

                    {msg.toolInvoked && (
                      <div className="mt-2 flex items-center gap-1.5 rounded-md bg-indigo-500/10 px-2 py-1 text-[11px] font-mono text-indigo-500">
                        <span>Invoked Tool:</span>
                        <span className="font-semibold">{msg.toolInvoked}</span>
                      </div>
                    )}
                  </div>

                  <div className="flex items-center gap-2 px-1 text-[10px] text-muted-foreground opacity-0 group-hover:opacity-100 transition-opacity">
                    <button
                      onClick={() => handleCopyMessage(msg.content, msg.id)}
                      className="flex items-center gap-1 hover:text-foreground"
                    >
                      {copiedMsgId === msg.id ? (
                        <Check className="h-3 w-3 text-emerald-500" />
                      ) : (
                        <Copy className="h-3 w-3" />
                      )}
                      Copy
                    </button>
                  </div>
                </div>
              </div>
            );
          })}

          {isExecuting && (
            <div className="flex gap-4 mr-auto max-w-3xl animate-in fade-in duration-200">
              <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-xl bg-gradient-to-tr from-indigo-500 to-purple-600 text-white shadow-sm">
                <Bot className="h-4 w-4 animate-spin" />
              </div>
              <div className="rounded-2xl border border-border/60 bg-card px-4 py-3 text-xs text-muted-foreground flex items-center gap-2">
                <Sparkles className="h-4 w-4 text-indigo-500 animate-bounce" />
                <span>AI Agent is interpreting request and executing tools...</span>
              </div>
            </div>
          )}

          <div ref={messagesEndRef} />
        </div>

        {/* Input Bar */}
        <div className="border-t border-border/50 p-4 bg-card/40 backdrop-blur-sm">
          <form
            onSubmit={(e) => {
              e.preventDefault();
              handleSubmitPrompt();
            }}
            className="flex items-center gap-2 rounded-xl border border-border/60 bg-background px-4 py-2.5 shadow-sm focus-within:border-indigo-500 focus-within:ring-2 focus-within:ring-indigo-500/20"
          >
            <input
              type="text"
              value={prompt}
              onChange={(e) => setPrompt(e.target.value)}
              placeholder="Ask AI Copilot to execute business tasks (e.g. 'Show revenue', 'Create invoice')..."
              className="flex-1 bg-transparent text-xs text-foreground placeholder:text-muted-foreground focus:outline-none"
              disabled={isExecuting}
            />
            <button
              type="submit"
              disabled={!prompt.trim() || isExecuting}
              className="flex h-8 w-8 items-center justify-center rounded-lg bg-indigo-600 text-white shadow-md shadow-indigo-500/20 hover:bg-indigo-700 transition-all disabled:opacity-40"
            >
              <Send className="h-4 w-4" />
            </button>
          </form>
        </div>
      </div>

      {/* Safety Confirmation Modal */}
      <ActionConfirmationModal
        isOpen={!!pendingConfirmation}
        onClose={() => setPendingConfirmation(null)}
        onConfirm={handleConfirmAction}
        title={pendingConfirmation?.command || ""}
        description={pendingConfirmation?.description || ""}
        toolName={pendingConfirmation?.toolName}
        isPending={isExecuting}
      />
    </div>
  );
}
