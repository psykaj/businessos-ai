# Landing Page Engine

The Landing Page Engine provides a modular way to build high-converting landing pages for SMEs.

## Architecture
- **Pages**: Core entity representing the page (Slug, Title, Status).
- **Sections**: JSON-based ordered components (Hero, Features, Pricing, Testimonials).
- **Drafts & Publishing**: Support for draft iterations before pushing live.

## API Endpoints
- `GET /api/landing-pages` - List organization pages.
- `GET /api/landing-pages/{id}` - Get page details for editing.
- `GET /api/landing-pages/slug/{slug}` - Public access endpoint for live pages.
- `POST /api/landing-pages` - Create a draft page.
- `PUT /api/landing-pages/{id}` - Update sections, title, or status.
- `DELETE /api/landing-pages/{id}` - Archive/Delete a page.
