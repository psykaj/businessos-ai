# Marketing & Lead Capture UI Architecture

The Day 12 frontend adds a complete workspace for marketing teams and business owners to capture leads, track campaigns, and monitor customer journeys.

## Key Modules

### 1. Dynamic Form Builder (`/dashboard/forms`)
A sophisticated interface for building custom forms.
- **Architecture**: A state-driven approach (`FormBuilder.tsx`) rather than a heavy drag-and-drop library. This provides a highly responsive, bug-free experience. 
- **Components**: The Canvas displays a simulated view of the fields, while the Properties panel allows editing labels, internal API names, required flags, and dropdown options.
- **Public Renderer**: Unauthenticated form submissions are handled at `/f/[id]`. This is completely decoupled from the dashboard layout, allowing it to be embedded in iframes or linked directly.

### 2. Submissions Dashboard (`/dashboard/forms/submissions`)
A central data table for all captured leads. Submissions are instantly parsed (using `JSON.parse` on the payload) to display name, email, and lead source. Crucially, it links directly to the CRM Lead record created by the backend automation engine.

### 3. Campaign Management (`/dashboard/campaigns`)
Marketing analytics overview to track total spend versus return on investment (ROI). Allows creation of UTM-tagged campaigns to accurately attribute where leads and revenue originate.

### 4. Customer Journey Funnel (`/dashboard/customer-journey`)
A visual representation of the conversion funnel (`Visitor -> Lead -> Qualified -> Customer`). Built using clean CSS transitions and semantic layout to instantly show drop-off points.

### 5. Lead Source Analytics (`/dashboard/lead-sources`)
Aggregates performance by traffic channel (e.g., Google Ads, Organic, WhatsApp). Crucial for business owners to decide where to allocate their marketing budget.

## State Management & API Integration
All data fetching and mutations are handled by **React Query** (`@tanstack/react-query`) with custom hooks located in `hooks/use-forms.ts`, `hooks/use-campaigns.ts`, etc. This provides automatic caching, optimistic updates, and loading state management.

## Responsive Strategy
- Grid layouts adapt from 1 column on mobile to 3-4 columns on desktop.
- The Form Builder collapses properties into standard scrollable sidebars.
- Data tables use horizontal scrolling on small screens to prevent layout breakage.
