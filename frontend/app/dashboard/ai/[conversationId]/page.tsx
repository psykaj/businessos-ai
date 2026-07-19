import { AiChat } from "../_components/ai-chat";

export default async function ConversationPage({
  params,
}: {
  params: Promise<{ conversationId: string }>;
}) {
  const { conversationId } = await params;
  return <AiChat conversationId={conversationId} />;
}
