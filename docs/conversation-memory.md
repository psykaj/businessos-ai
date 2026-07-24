# Conversation Memory Architecture

## Overview
**Conversation Memory** manages organization-isolated chat sessions and message logs for the AI Business Agent.

## Entity Structure
- **Conversation**: Represents a chat thread belonging to an `OrganizationId` and `UserId`.
  - Properties: `Id`, `OrganizationId`, `UserId`, `Title`, `Status` (Active/Archived), `CreatedAt`, `UpdatedAt`.
- **Message**: Represents an individual user prompt or assistant response in a thread.
  - Properties: `Id`, `ConversationId`, `Role` (User/Assistant/System), `Content`, `ToolInvoked`, `ExecutionId`, `CreatedAt`.

## API Endpoints
- `POST /api/ai-agent/conversations`: Create a new conversation thread.
- `GET /api/ai-agent/conversations`: List tenant conversations (supports search, status filtering, pagination).
- `GET /api/ai-agent/conversations/{id}`: Retrieve conversation metadata and full message history.
- `PUT /api/ai-agent/conversations/{id}`: Rename or update status of conversation.
- `POST /api/ai-agent/conversations/{id}/archive`: Archive conversation thread.
- `DELETE /api/ai-agent/conversations/{id}`: Soft delete conversation.
- `GET /api/ai-agent/conversations/{id}/messages`: Fetch message history.
