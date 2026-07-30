# Business Goals UI

## Overview
The Business Goals tracking page (`/app/dashboard/business-goals`) provides visibility into strategic objectives.

## Key Features
- **Goal Cards**: Displays the goal title, category, description, and status.
- **Progress Tracking**: Visual progress bars mapping the `currentValue` against the `targetValue`.
- **Status Badges**: Color-coded badges indicating if a goal is "On Track", "At Risk", "Behind", or "Completed".
- **Deadlines**: Clear visibility of the target completion date.

## Technical Implementation
- Utilizes `useBusinessGoals` React Query hook for data fetching.
- Heavy use of Shadcn UI `Card` and `Badge` components.
- Tailwind CSS handles the dynamic width and color-coding of the progress bars.
