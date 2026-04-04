/** Extract YYYY-MM-DD for date inputs from an API ISO string. */
export function toDateInputValue(iso: string): string {
  if (!iso) return "";
  return iso.slice(0, 10);
}

/** Send calendar date to EMS.API as UTC midnight ISO (System.Text.Json DateTime). */
export function dateInputToApiIso(dateYyyyMmDd: string): string {
  return `${dateYyyyMmDd}T00:00:00.000Z`;
}
