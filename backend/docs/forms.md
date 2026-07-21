# Forms Module

The Forms Module enables businesses to dynamically create and manage forms for lead capture, surveys, and customer feedback.

## Features
- **Dynamic Form Builder**: Support for Text, Email, Phone, Number, Dropdown, Radio, Checkbox, Date, Textarea.
- **Form Submissions**: Captures device, browser, and source tracking (UTM).
- **Public API**: Allows embedding forms on external websites via a dedicated public endpoint.
- **CRM Integration**: Automatically routes submissions to the Lead Capture engine.

## Key APIs
- `GET /api/forms`: List all forms
- `POST /api/forms`: Create a new form
- `POST /api/forms/{id}/publish`: Publish a draft form
- `POST /api/public/forms/{id}/submit`: Public endpoint to submit data without authentication
