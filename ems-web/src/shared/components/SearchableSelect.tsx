"use client";

import { useCallback, useEffect, useId, useMemo, useRef, useState } from "react";
import type { SelectOption } from "@/shared/types/select-option";
import { cn } from "@/shared/utils/cn";

const triggerClass =
  "flex w-full items-center justify-between gap-2 rounded-lg border border-zinc-300 bg-white px-3 py-2 text-left text-sm text-zinc-900 shadow-sm focus:border-zinc-500 focus:outline-none focus:ring-1 focus:ring-zinc-500 dark:border-zinc-600 dark:bg-zinc-900 dark:text-zinc-100";

const inputClass =
  "w-full rounded-lg border border-zinc-300 bg-white px-3 py-2 text-sm text-zinc-900 shadow-sm focus:border-zinc-500 focus:outline-none focus:ring-1 focus:ring-zinc-500 dark:border-zinc-600 dark:bg-zinc-900 dark:text-zinc-100";

export type SearchableSelectProps<TValue extends string | number> = {
  id?: string;
  options: SelectOption<TValue>[];
  value: TValue | null;
  onChange: (value: TValue | null) => void;
  placeholder?: string;
  /** Label for the cleared / optional state (default "None"). */
  emptyLabel?: string;
  allowClear?: boolean;
  disabled?: boolean;
  className?: string;
  "aria-invalid"?: boolean;
};

export function SearchableSelect<TValue extends string | number>({
  id: idProp,
  options,
  value,
  onChange,
  placeholder = "Select…",
  emptyLabel = "None",
  allowClear = true,
  disabled = false,
  className,
  "aria-invalid": ariaInvalid,
}: SearchableSelectProps<TValue>) {
  const reactId = useId();
  const listboxId = `${reactId}-listbox`;
  const [open, setOpen] = useState(false);
  const [query, setQuery] = useState("");
  const rootRef = useRef<HTMLDivElement>(null);

  const selectedLabel = useMemo(() => {
    if (value === null) return null;
    const hit = options.find((o) => o.value === value);
    return hit?.label ?? String(value);
  }, [options, value]);

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return options;
    return options.filter((o) => {
      if (o.disabled) return false;
      const label = o.label.toLowerCase();
      const val = String(o.value).toLowerCase();
      return label.includes(q) || val.includes(q);
    });
  }, [options, query]);

  useEffect(() => {
    if (!open) return;
    const onDoc = (e: MouseEvent) => {
      if (rootRef.current && !rootRef.current.contains(e.target as Node)) {
        setOpen(false);
        setQuery("");
      }
    };
    document.addEventListener("mousedown", onDoc);
    return () => document.removeEventListener("mousedown", onDoc);
  }, [open]);

  const handlePick = useCallback(
    (v: TValue | null) => {
      onChange(v);
      setOpen(false);
      setQuery("");
    },
    [onChange],
  );

  const id = idProp ?? reactId;

  return (
    <div ref={rootRef} className={cn("relative", className)}>
      <button
        type="button"
        id={id}
        disabled={disabled}
        aria-expanded={open}
        aria-haspopup="listbox"
        aria-controls={listboxId}
        aria-invalid={ariaInvalid}
        className={cn(triggerClass, disabled && "cursor-not-allowed opacity-60")}
        onClick={() => !disabled && setOpen((o) => !o)}
      >
        <span className={cn("truncate", !selectedLabel && "text-zinc-500 dark:text-zinc-400")}>
          {selectedLabel ?? placeholder}
        </span>
        <span className="shrink-0 text-zinc-400" aria-hidden>
          ▾
        </span>
      </button>

      {open ? (
        <div
          id={listboxId}
          role="listbox"
          className="mt-2 space-y-2 rounded-lg border border-zinc-200 bg-white p-2 shadow-md dark:border-zinc-700 dark:bg-zinc-950"
        >
          <input
            type="search"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="Search…"
            className={inputClass}
            autoFocus
            aria-label="Filter options"
          />
          <ul className="max-h-48 overflow-y-auto rounded-md border border-zinc-100 dark:border-zinc-800">
            {allowClear ? (
              <li role="option" aria-selected={value === null}>
                <button
                  type="button"
                  className="w-full px-3 py-2 text-left text-sm text-zinc-600 hover:bg-zinc-50 dark:text-zinc-400 dark:hover:bg-zinc-900"
                  onClick={() => handlePick(null)}
                >
                  {emptyLabel}
                </button>
              </li>
            ) : null}
            {filtered.length === 0 ? (
              <li className="px-3 py-2 text-sm text-zinc-500">No matches.</li>
            ) : (
              filtered.map((o) => (
                <li key={String(o.value)} role="option" aria-selected={value === o.value}>
                  <button
                    type="button"
                    disabled={o.disabled}
                    className={cn(
                      "w-full px-3 py-2 text-left text-sm hover:bg-zinc-50 dark:hover:bg-zinc-900",
                      value === o.value && "bg-zinc-100 font-medium dark:bg-zinc-800",
                      o.disabled && "cursor-not-allowed opacity-50",
                    )}
                    onClick={() => !o.disabled && handlePick(o.value)}
                  >
                    {o.label}
                  </button>
                </li>
              ))
            )}
          </ul>
        </div>
      ) : null}
    </div>
  );
}
