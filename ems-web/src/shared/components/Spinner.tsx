import { cn } from "@/shared/utils/cn";

export function Spinner({ className }: { className?: string }) {
  return (
    <div
      className={cn(
        "h-8 w-8 animate-spin rounded-full border-2 border-zinc-200 border-t-zinc-800 dark:border-zinc-700 dark:border-t-zinc-200",
        className,
      )}
      role="status"
      aria-label="Loading"
    />
  );
}
