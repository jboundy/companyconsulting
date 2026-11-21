1. User Profile & Localization Self-Service
Profile editing: Updating display name, first/last name, and avatar (uploading a custom image or resetting/syncing from an external provider).

Regional & display settings: Updating Locale, TimeZone, and Preferences (theme, default page size, notification toggles) with immediate feedback for client apps.

Email management: Changing email addresses with a verification step, resending confirmation links, and updating EmailConfirmed state via UserManager.

2. Authentication & Security Controls
Password lifecycle: Changing an existing password (for local credential accounts) and running forgot-password / reset-password token flows.

Multi-Factor Authentication (MFA / 2FA): Enrolling/disabling authenticator apps (TOTP), generating/regenerating recovery codes, and viewing MFA status.

Linked external providers: Viewing connected providers (e.g., Google, Microsoft), linking an additional provider while signed in, and unlinking a provider—ideally guarding against unlinking the only sign-in method when no local password is set.

Active sessions & tokens: Listing active sessions/refresh tokens or triggering a "sign out of all devices / revoke tokens" action (rotating SecurityStamp and revoking grants/tokens in your OIDC store).

3. Account Lifecycle & Privacy
Account deactivation & deletion: Self-service account deactivation/close flows (setting account status to deactivated, locking/disabling login in Identity, and revoking active tokens) or hard/soft deletion workflows depending on compliance needs.

Consent & data export: Viewing granted OAuth client consents/permissions and exporting personal profile/account data.

4. Administrative & Support Features (If Needed)
User lookup & inspection: Searching users by ID, email, or external login, and viewing account status, linked providers, and recent audit activity.

Account moderation & recovery: Locking/unlocking accounts, disabling/re-enabling users, triggering password resets or email verifications on a user's behalf, and managing roles/claims.

Audit log viewer: Querying immutable account.audit_logs records (who changed what, when, and from where) to troubleshoot support tickets and security events.