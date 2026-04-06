import type { SelectOption } from "@/shared/types/select-option";

/**
 * Maps a collection to {@link SelectOption} rows (same idea as server-side DropdownHelper projections).
 */
export function toSelectOptions<TItem, TValue extends string | number>(
  items: TItem[],
  valueSelector: (item: TItem) => TValue,
  labelSelector: (item: TItem) => string,
): SelectOption<TValue>[] {
  return items.map((item) => ({
    value: valueSelector(item),
    label: labelSelector(item),
  }));
}
