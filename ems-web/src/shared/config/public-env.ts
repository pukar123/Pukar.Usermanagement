/** Default org for API calls that require `organizationId` (matches `.env` / Employee forms). */
export const defaultOrganizationId = Number(process.env.NEXT_PUBLIC_DEFAULT_ORGANIZATION_ID) || 1;
