# Report Generation & Export Architecture

The **Report Generator** enables on-demand creation and export of comprehensive business reports across all modules.

## Supported Report Types
- Executive Summary Report
- Sales Pipeline & Performance Report
- Marketing Campaign & Lead ROI Report
- CRM Activity & Deal Report
- Workflow Automation & Integration Report
- AI Assistant Usage Report

## Export Formats & Streaming
- **PDF**: Formatted document layout stream.
- **Excel**: Multi-column XML spreadsheet layout compatible with Microsoft Excel and Google Sheets.
- **CSV**: Standard comma-separated data output for downstream processing.

## API Endpoints
- `GET /api/v1/bi/reports`: List generated reports
- `GET /api/v1/bi/reports/{id}`: View report details
- `POST /api/v1/bi/reports/generate`: Generate a new report
- `GET /api/v1/bi/reports/{id}/export?format={pdf|excel|csv}`: Download formatted export stream
