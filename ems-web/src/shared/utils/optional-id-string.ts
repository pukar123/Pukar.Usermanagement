/** Parses optional FK text fields used in forms before Zod transform. */
export function stringIdFieldToNullableNumber(s: string): number | null {
  const t = s.trim();
  if (t === "") return null;
  const n = Number(t);
  return Number.isFinite(n) ? n : null;
}

export function nullableNumberToStringId(n: number | null | undefined): string {
  return n == null ? "" : String(n);
}
