1. Core Domain Entities & Concepts
   To properly separate concerns, a product system usually organizes its domain into a few core building blocks:

Product / Abstract Product: Represents the high-level concept (e.g., "Pro Plan" or "Ergonomic Office Chair"). It contains marketing descriptions, brand, images, tags, and category classifications.

SKU / Variant: Represents a specific, sellable item (e.g., "Pro Plan - Annual" or "Ergonomic Office Chair - Mesh / Blue"). This holds distinct identifiers (GTIN/UPC/SKU), physical dimensions, or digital access parameters.

Price & Pricing Rules: Defines how a SKU is monetized (flat price, recurring tier, volume discount, multi-currency).

Tax Class / Category: Maps products to tax engines (e.g., Avalara/TaxJar codes) for digital vs. physical tax rules.

2. Key System Features
   A. Catalog & Classification Management
   Hierarchical Categorization: Multi-level category trees (e.g., Electronics -> Workstations -> Monitors) and cross-category tagging.

Dynamic Attributes & Schemas: Ability to attach custom key-value metadata to products without modifying database schemas (e.g., Color, RAM Size, Storage Capacity, License Seats).

Bundle & Package Management: Creating composite products made up of multiple child SKUs (e.g., "Starter Kit" containing Product A + Product B at a discount).

B. Pricing & Currency Engine
Multi-Currency Support: Storing prices in base and regional currencies.

Flexible Pricing Models:

One-time: Standard physical/digital goods pricing.

Recurring / Subscriptions: Flat-rate, per-seat, usage-based / metered pricing tiers.

Tiered & Volume Pricing: Discounting based on quantity breaks (e.g., 1–10 items @ $10, 11–50 @ $8).

Promotions & Discount Rules: Coupon codes, limited-time promotional pricing, and user-group-specific pricing (e.g., B2B wholesale prices vs. retail).

C. Lifecycle & Version Control
Drafting & Publishing: Staging products in a Draft state, sending them through an In Review approval workflow, and setting Published or Archived states.

Scheduled Visibility: Automatically publishing or unpublishing products on specific dates (for seasonal items, launches, or sales).

Audit Trail / History: Tracking who changed a product description, price, or SKU status, with the ability to inspect past versions.

D. Integration & Event Hooks
Integration Events: Emitting domain events (e.g., ProductCreated, PriceUpdated, ProductArchived) so downstream systems (Billing, Search/Solr/Elasticsearch, Inventory, Storefront) can update their projections.

Localization / i18n: Storing translated product titles and descriptions for multi-region support.

3. Capabilities & Permissions by User Role
   Role-Based Access Control (RBAC) ensures different actors can perform only their relevant operations.

🛠️ Product / Catalog Manager (Internal Business User)
CRUD Products & Variants: Create, update, duplicate, and archive products and SKUs.

Manage Categories & Taxonomy: Rearrange category trees and define custom attribute schemas.

Set Pricing Rules: Assign prices, currency overrides, promotional tiers, and billing frequencies.

Manage Assets: Upload and sequence media (images, product manuals, spec sheets).

Publishing Controls: Schedule release dates or move items from draft to live status.

💰 Finance & Revenue Operations Manager
Tax Code Mapping: Assign appropriate tax category codes (e.g., SaaS vs. Physical Item) to products.

GL (General Ledger) Account Mapping: Link SKUs or product categories to accounting ledger codes for revenue recognition.

Audit Price Changes: Review historical price changes and sign off on price updates before they go live.

🔌 System Integrations / Developers (API Consumers)
Query Catalog (Read-Only Public): Fetch published products, active pricing, and media for display on storefronts or mobile apps.

Sync Extensions (Admin API): Import bulk products from ERP/PIM systems or external feeds (e.g., CSV imports, Shopify/ERP syncs).

Webhook Subscriptions: Listen for price and product availability updates.

👤 End User / Customer (Read & Select Only)
Browse & Search Catalog: View published products, filter by attributes, compare variants, and see current pricing.

Select Options: Pick specific SKU variants (e.g., Monthly vs. Annual subscription, Red vs. Blue).

Note: Customers do not modify product data; they consume read-only snapshots when creating quotes, carts, or invoices.
