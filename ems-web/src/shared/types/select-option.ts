/**
 * Generic option row for searchable selects (value + display label).
 * TValue is typically number (FK) or string (codes / external ids).
 */
export type SelectOption<TValue> = {
  value: TValue;
  label: string;
  disabled?: boolean;
};
