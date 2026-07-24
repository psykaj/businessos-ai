# AI Command Engine

## Overview
The **AI Command Engine** interprets natural language business requests from SME owners and maps them to internal system tools.

## Supported Command Intents
- **Show Revenue**: "Show today's revenue", "How much money made to date" -> `AnalyticsTool`
- **Show Dashboard**: "Show dashboard", "Executive summary" -> `AnalyticsTool`
- **Create Invoice**: "Create an invoice for $500", "Generate invoice" -> `InvoiceTool`
- **Show Unpaid Invoices**: "Which invoices are unpaid?", "Outstanding invoices" -> `InvoiceTool`
- **Assign Lead**: "Assign new leads to Rahul" -> `CrmTool`
- **Pending Tasks**: "Show my pending tasks", "What are my todos?" -> `CrmTool`
- **Schedule Follow-up**: "Schedule follow-up call with client" -> `CrmTool`
- **Send Email**: "Send payment reminder email" -> `EmailTool`
- **Send WhatsApp**: "WhatsApp client regarding update" -> `WhatsAppTool`
- **Generate Report**: "Generate this week's executive report" -> `ReportTool`
- **Create QR Code**: "Create QR code for my landing page" -> `QRTool`
- **Customer Search**: "Which customers haven't been contacted?" -> `CustomerTool`
- **Trigger Workflow**: "Run lead nurture workflow" -> `WorkflowTool`

## Architecture & Provider Independence
The engine is structured around the `IAiCommandEngine` interface. It employs a dual-tier processing mechanism:
1. **Deterministic Intent & Parameter Extractor**: Ensures 100% reliable local matching and zero cloud latency when standard business phrases are used.
2. **LLM Provider Abstraction**: Can delegate complex phrasing to OpenAI, Anthropic, or Gemini when cloud credentials are configured.
