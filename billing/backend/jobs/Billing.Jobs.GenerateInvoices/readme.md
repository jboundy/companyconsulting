┌─────────────────────────┐
  │     Product Catalog     │
  └────────────┬────────────┘
               │ 1. Admin selects product
               │    (Returns BasePrice + TaxCode "DIGITAL_SAAS")
               ▼
  ┌─────────────────────────┐         2. Sends Customer Address         ┌─────────────────────────┐
  │     Billing System      │ ────────────────────────────────────────> │       Tax Engine        │
  │                         │ <──────────────────────────────────────── │  (ITaxCalculator)       │
  └────────────┬────────────┘         3. Returns Calculated Tax Amount   └─────────────────────────┘
               │
               │ 4. Creates Invoice & writes immutable 
               │    InvoiceLineTaxSnapshot to Database
               ▼
  ┌─────────────────────────┐
  │    Final Invoice Record │
  └─────────────────────────┘