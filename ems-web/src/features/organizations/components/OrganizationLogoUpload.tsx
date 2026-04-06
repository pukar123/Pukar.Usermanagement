"use client";

import { useUploadOrganizationLogo } from "../hooks";
import { resolveOrganizationLogoUrl } from "@/shared/utils/organization-logo-url";
import { getErrorMessage } from "@/shared/api/http-client";
import { toast } from "sonner";

type Props = {
  organizationId: number;
  logoRelativePath: string | null;
};

export function OrganizationLogoUpload({ organizationId, logoRelativePath }: Props) {
  const uploadMut = useUploadOrganizationLogo(organizationId);
  const src = resolveOrganizationLogoUrl(logoRelativePath);
  const inputId = `org-logo-upload-${organizationId}`;

  const onChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    e.target.value = "";
    if (!file) return;
    try {
      await uploadMut.mutateAsync(file);
      toast.success("Logo updated.");
    } catch (err) {
      toast.error(getErrorMessage(err));
    }
  };

  return (
    <div className="flex flex-col items-start gap-2">
      <div className="flex h-32 w-32 shrink-0 items-center justify-center overflow-hidden rounded-xl border border-zinc-200 bg-zinc-50 dark:border-zinc-700 dark:bg-zinc-900">
        {src ? (
          <img src={src} alt="" className="max-h-full max-w-full object-contain" />
        ) : (
          <span className="px-2 text-center text-xs text-zinc-400">No logo</span>
        )}
      </div>
      <input
        type="file"
        accept="image/png,image/jpeg,image/jpg,image/webp,image/gif,image/svg+xml"
        className="sr-only"
        id={inputId}
        onChange={(e) => void onChange(e)}
        disabled={uploadMut.isPending}
      />
      <label
        htmlFor={inputId}
        className="cursor-pointer rounded-lg border border-zinc-300 bg-white px-3 py-1.5 text-xs font-medium text-zinc-800 shadow-sm hover:bg-zinc-50 disabled:cursor-not-allowed disabled:opacity-50 dark:border-zinc-600 dark:bg-zinc-900 dark:text-zinc-200 dark:hover:bg-zinc-800"
      >
        {uploadMut.isPending ? "Uploading…" : "Change logo"}
      </label>
    </div>
  );
}
