# White Label Platform UI Architecture

## Overview
The Day 9 White Label Platform UI allows organizations to completely customize their workspaces, public pages, branding, and custom domains. The UI architecture was built utilizing Next.js 15, React, Tailwind CSS, Shadcn UI, React Hook Form, and Zod.

## Component Hierarchy
The implementation consists of 6 primary modules, integrated directly into the `DashboardShell`:

1. **Branding (`/dashboard/branding`)**
   - Form for Company Name, Support Email, Colors, and Fonts.
   - Side-by-side **Live Preview** component rendering visual changes instantly based on `form.watch()`.
   - Separate Media Upload blocks that bypass React Hook Form to directly stream `File` blobs to the backend using `MediaService`.

2. **Custom Domains (`/dashboard/domains`)**
   - Data Table displaying `Connected Domains`, `SSL Status`, and `Verification Status`.
   - A `Dialog` integration presenting DNS configurations (TXT records) for verification.
   - Domain creation and primary assignment handlers relying on `react-query` mutations.

3. **Themes Customizer (`/dashboard/themes`)**
   - Theme switching (Light/Dark/Custom).
   - Dynamic JSON Configuration Editor (`ThemeJson` parsed and rendered as form fields for Colors and Component Layout values).
   - Fully interactive Card preview component visualizing how structural changes (e.g. Card Style = Glassmorphism) apply across the system.

4. **SEO Management (`/dashboard/seo`)**
   - Complex form structure mapping SEO settings to a standard JSON object.
   - Live Search Engine Preview simulating Google SERP rendering.
   - Live Social Media Preview simulating Twitter Cards and OpenGraph layouts.

5. **Landing Page Builder (`/dashboard/landing-pages`)**
   - `page.tsx`: Grid dashboard summarizing pages, statuses, and publish dates.
   - `create/page.tsx`: Form bridging creation to the editor interface with slug auto-generation.
   - `edit/[id]/page.tsx`: The Modular Builder Interface.

## Landing Page Builder Architecture
The builder employs a decoupled layout:
1. **Left Sidebar (Structure)**: Displays active sections. Contains controls to Reorder (Up/Down Arrow handlers manipulating array index) and Delete sections.
2. **Editor Panel**: Dynamically mounts input fields by parsing `contentJson` values associated with the active Section type. Uses generic Input and Textarea components based on text length constraints.
3. **Preview Container**: Iterates over the `sections` array and mounts structural stubs based on the `sectionType` (Hero, Features, Pricing). Incorporates active CSS classes to highlight the currently selected section in real-time.

## API Integration Strategy
We utilize a singleton `apiClient` instance leveraging `axios`. 
- Every network request is prefixed with `Bearer [JWT]` injected dynamically via interceptors.
- All Data Fetching and Mutations are handled asynchronously using `@tanstack/react-query`, ensuring state synchronization and optimistic UI updates without manual re-fetching.
- All endpoints are strictly typed with Data Transfer Objects (DTOs) synced to the C# Backend schema.

## State Management
- **Server State**: Managed exclusively through `React Query` (`useQuery`, `useMutation`).
- **Form State**: Managed via `React Hook Form` using strict `Zod` schemas. Validation fires `onChange` to give users immediate feedback.
- **Local UI State**: Managed via `useState` for transient states (Dialog open/close, active active editor index).

## Responsive Design Decisions
All layouts rely on Tailwind CSS responsive breakpoints (`sm:`, `md:`, `lg:`, `xl:`). 
The Landing Page Builder uses a dual-pane flex layout on Desktop (`lg:col-span-3`, `lg:col-span-9`) which stacks elegantly on mobile viewports.

## Media Management Flow
The Media library uses a file input bound to a standard `<input type="file" />`.
1. Once a file is selected, `FormData` is constructed.
2. An async post request hits `MediaService.uploadMedia`.
3. The server immediately returns an accessible URL, which is hydrated into the local state and displayed within a preview Grid.
