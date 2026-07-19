# Branding Module

The Branding module allows organizations to configure their brand identity on the White Label platform.

## Features
- **Company Name**: Configurable primary company name.
- **Media Assets**: Support for custom Logo and Favicon uploads.
- **Colors**: Primary, Secondary, and Accent colors for UI styling.
- **Typography**: Configurable font family for all outward-facing pages.
- **Footer**: Custom footer text and support email.

## API Endpoints
- `GET /api/branding` - Retrieves current branding config.
- `PUT /api/branding` - Updates textual and color configurations.
- `POST /api/branding/logo` - Uploads a logo image.
- `POST /api/branding/favicon` - Uploads a favicon image.

## Security
All files are validated and stored securely using the isolated Media Service. Organizations can only read/write their own branding.
