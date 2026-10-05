tracks income and expenses, sends professional invoices, runs financial reports like balance sheets

1. Customer & Account Data
   Customer / Account Profile: Core details identifying who is being billed, including organization name, billing contact, tax IDs (e.g., VAT or GST numbers), and currency preferences.

Payment Methods (Tokenized): Securely stored references or tokens from payment gateways (like Stripe or PayPal) representing credit cards, bank accounts, or SEPA mandates. Never store raw credit card numbers.

Billing Addresses: Physical and mailing addresses used for regional tax calculations (such as sales tax or VAT nexus).

2. Product & Catalog Data
   Products: Definitions of what is being sold (e.g., "SaaS Pro Tier", "API Add-on").

Prices / Rate Plans: Pricing configurations attached to products, specifying whether an item is a flat fee, tiered, volume-based, or usage-based (metered).

Tax Categories: Metadata mapping products to specific tax rules or rates depending on jurisdiction.

3. Subscriptions & Contracts
   Subscriptions: The core contract linking a customer to one or more products/prices. It tracks state (e.g., active, past_due, canceled, trialing), current billing period start/end dates, and renewal settings.

Subscription Items: Individual line items within a subscription, especially useful for multi-product bundles or tiered quantities.

4. Metered & Usage Data
   Usage Events / Meters: Raw event logs or aggregated counters for usage-based billing (e.g., API calls made, gigabytes stored, seat counts).

Billing Metric Definitions: Rules defining how raw events translate into billable units during the invoice generation cycle.

5. Invoicing & Financial Records
   Invoices: Point-in-time documents detailing what charges are due. This includes line items, discounts applied, taxes calculated, subtotal, and total amount due. Statuses typically include draft, open, paid, void, or uncollectible.

Invoice Line Items: Individual charges, adjustments, or prorated amounts tied to a specific invoice.

Credit Notes / Debit Notes: Records used to adjust invoice balances downward (refunds, write-offs) or upward.

6. Payments & Ledger
   Transactions / Payments: Records of money movement attempt results (e.g., succeeded, failed, pending, refunded), gateway transaction IDs, and associated error codes.

Double-Entry Ledger (Accounts & Entries): An immutable financial ledger tracking debits and credits across accounts (e.g., Accounts Receivable, Cash, Revenue, Unearned/Deferred Revenue) to ensure financial reporting integrity.

Key Architectural Best Practices
Immutability: Once an invoice is finalized or a transaction completes, the core financial records should be immutable (append-only). Corrections should be made via credit notes or adjustment entries rather than mutating historical records.

Idempotency Keys: When persisting payment requests or invoice generation triggers, store and enforce idempotency keys to prevent accidental double-charging or duplicate invoices caused by network retries.
