# 3-Column Conversation Workspace UI

## Overview
The Live Conversation Workspace (`/dashboard/conversations`) provides an immersive, multi-pane communication terminal where agents engage with customers, insert canned variable responses, consult AI recommendations, and coordinate internally.

## Layout Architecture
1. **Left Pane (Active Threads Queue)**: Rapid context switching between open customer inquiries without leaving the active chat view.
2. **Center Pane (Message Engine & Composer)**:
   - **Transcript Stream**: Displays inbound/outbound bubbles, attachments, delivery checkmarks, and clearly separated **Confidential Internal Team Notes** (styled in safety amber to prevent accidental external transmission).
   - **Message Composer**: Supports shortcut keyboard commands (`Cmd + Enter`), live quick-reply template selector, and one-click **BusinessOS AI Suggested Answers**.
3. **Right Pane (Customer 360 Sidebar)**:
   - Surfaces lifetime revenue value (LTV), customer verification tags, contact coordinates, assignment controls, and past CSAT satisfaction ratings to give agents complete context before replying.
