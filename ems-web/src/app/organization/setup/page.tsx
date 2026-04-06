"use client";

import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { toast } from "sonner";
import { useUpdateOrganization } from "@/features/organizations/hooks";
import { useOrganizationContext } from "@/providers/OrganizationProvider";
import { Button } from "@/shared/components/Button";
import { getErrorMessage } from "@/shared/api/http-client";

const inputClass =
  "mt-1 w-full rounded-lg border border-zinc-300 bg-white px-3 py-2 text-sm text-zinc-900 shadow-sm focus:border-zinc-500 focus:outline-none focus:ring-1 focus:ring-zinc-500 dark:border-zinc-600 dark:bg-zinc-900 dark:text-zinc-100";

export default function OrganizationSetupEditPage() {
  const router = useRouter();
  const { currentOrganization, needsSetup, organizationId } = useOrganizationContext();
  const updateMut = useUpdateOrganization(organizationId);

  const [name, setName] = useState("");
  const [code, setCode] = useState("");
  const [isActive, setIsActive] = useState(true);

  useEffect(() => {
    if (currentOrganization) {
      setName(currentOrganization.name);
      setCode(currentOrganization.code ?? "");
      setIsActive(currentOrganization.isActive);
    }
  }, [currentOrganization]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) {
      toast.error("Organization name is required.");
      return;
    }

    try {
      await updateMut.mutateAsync({
        name: name.trim(),
        code: code.trim() ? code.trim() : null,
        isActive,
      });
      toast.success("Organization updated.");
      router.replace("/");
    } catch (err) {
      toast.error(getErrorMessage(err));
    }
  };

  if (needsSetup || !currentOrganization) {
    return null;
  }

  return (
    <main className="mx-auto max-w-lg flex-1 px-4 py-12 sm:px-6">
      <div className="rounded-xl border border-zinc-200 bg-white p-6 shadow-sm dark:border-zinc-700 dark:bg-zinc-950">
        <h1 className="text-2xl font-semibold text-zinc-900 dark:text-zinc-50">Organization settings</h1>
        <p className="mt-2 text-sm text-zinc-600 dark:text-zinc-400">
          Update your organization name, code, and active status.
        </p>

        <form onSubmit={(e) => void handleSubmit(e)} className="mt-8 space-y-4">
          <div>
            <label className="text-xs font-medium uppercase tracking-wide text-zinc-500 dark:text-zinc-400">Name</label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className={inputClass}
              required
              autoFocus
            />
          </div>
          <div>
            <label className="text-xs font-medium uppercase tracking-wide text-zinc-500 dark:text-zinc-400">Code</label>
            <input
              type="text"
              value={code}
              onChange={(e) => setCode(e.target.value)}
              className={inputClass}
              placeholder="Optional short code"
            />
          </div>
          <label className="flex items-center gap-2 text-sm text-zinc-800 dark:text-zinc-200">
            <input
              type="checkbox"
              checked={isActive}
              onChange={(e) => setIsActive(e.target.checked)}
              className="rounded border-zinc-300 dark:border-zinc-600"
            />
            Active
          </label>
          <div className="flex flex-wrap gap-3 pt-2">
            <Button type="submit" disabled={updateMut.isPending}>
              {updateMut.isPending ? "Saving…" : "Save changes"}
            </Button>
            <Button type="button" variant="secondary" onClick={() => router.push("/")}>
              Cancel
            </Button>
          </div>
        </form>
      </div>
    </main>
  );
}
