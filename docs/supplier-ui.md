# Supplier Management UI Architecture

## Overview
The Supplier Management UI provides vendor registration, contact details, payment terms tracking, and real-time vendor performance scorecards.

## Key Features & UI Components

### 1. Supplier Directory (`/dashboard/suppliers`)
- Grid cards displaying company name, vendor code, contact person, email, phone, and payment terms (Net 30, Net 15, Immediate).

### 2. Vendor Performance Scorecard
- Displays calculated performance percentage badge:
  - 90%+: High Performance (Emerald badge)
  - 70-89%: Satisfactory (Amber badge)
  - <70%: Needs Review (Rose badge)

### 3. Supplier Onboarding (`supplier-modal.tsx`)
- Form dialog for creating and updating vendor contact profiles, tax IDs, and payment terms.
