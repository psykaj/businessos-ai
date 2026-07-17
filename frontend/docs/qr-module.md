# QR Code Management Module

## Overview
The QR Code Management module allows BusinessOS AI users to generate, manage, and track dynamic QR codes directly from their dashboard.

## Folder Structure
```
frontend/
├── app/dashboard/qr/
│   ├── page.tsx               # Dashboard table listing all QR codes
│   ├── create/page.tsx        # Create new QR code form
│   ├── [id]/page.tsx          # QR code details, preview, and download options
│   └── edit/[id]/page.tsx     # Edit existing QR code configuration
├── components/qr/
│   ├── qr-form.tsx            # Reusable form component using react-hook-form + zod
│   ├── qr-preview.tsx         # Live visualization component for QR generation
│   └── qr-table.tsx           # Reusable data table with actions menu
├── lib/
│   └── qr-service.ts          # API Client wrapper for backend communication
└── types/
    └── qr.ts                  # Shared DTO interfaces and Enums
```

## Frontend Architecture
This module strictly follows **Clean Architecture** patterns applied to React/Next.js:
- **Presentation Layer**: Pages under `app/dashboard/qr` handle layout routing and URL parameters.
- **Component Layer**: Stateless/Stateful components under `components/qr` render the UI and handle internal validation without coupling to data fetching.
- **Data Access Layer**: `lib/qr-service.ts` encapsulates all Axios requests to the .NET backend API, ensuring components remain ignorant of specific REST URL paths or HTTP verbs.

## Supported QR Types
The backend and frontend both support the following 17 Enum types:
Website, PDF, Image, Video, Text, Email, Phone, SMS, WhatsApp, WiFi, GoogleMaps, BusinessCard, Event, SocialMedia, AppDownload, Menu, PaymentLink.

## API Integration Flow
1. API responses arrive wrapped in a global `ApiResponse<T>` from the .NET backend.
2. `lib/api-client.ts` intercepts this (and attaches the Bearer Token).
3. `qr-service.ts` unwraps the `.data` payload, returning clean interfaces (e.g., `QRCodeDto` or `PagedResult<QRCodeDto>`) to React.
4. If a 401 Unauthorized occurs, the global interceptor automatically handles logout redirection.

## Future Enhancements
- **Analytics Charts**: The details page currently has a placeholder for scan metrics. Once the tracking backend is implemented, this can be swapped with Recharts or Chart.js graphs.
- **PDF Export**: PDF generation can be added easily in `qr-service.ts` by appending another format query parameter to the image generation endpoint.
