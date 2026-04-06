import axios, { type AxiosError } from "axios";

const baseURL = process.env.NEXT_PUBLIC_API_BASE_URL ?? "";

export const httpClient = axios.create({
  baseURL,
  headers: { "Content-Type": "application/json" },
});

export type ApiErrorBody = { message?: string; title?: string };

/** POST multipart (e.g. file upload). Avoids axios default JSON Content-Type on FormData. */
export async function postFormData<T>(urlPath: string, formData: FormData): Promise<T> {
  const base = baseURL.replace(/\/$/, "");
  const path = urlPath.startsWith("/") ? urlPath : `/${urlPath}`;
  const res = await fetch(`${base}${path}`, { method: "POST", body: formData });
  if (!res.ok) {
    const text = await res.text();
    let message = `Request failed (${res.status})`;
    try {
      const body = JSON.parse(text) as unknown;
      if (body && typeof body === "object" && body !== null && "message" in body) {
        const m = (body as ApiErrorBody).message;
        if (typeof m === "string") message = m;
      }
    } catch {
      if (text.length > 0 && text.length < 500) message = text;
    }
    throw new Error(message);
  }
  return res.json() as Promise<T>;
}

export function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError(error)) {
    const ax = error as AxiosError<unknown>;
    const data = ax.response?.data;
    if (typeof data === "string" && data.length > 0) return data;
    if (data && typeof data === "object" && data !== null && "message" in data) {
      const m = (data as ApiErrorBody).message;
      if (typeof m === "string") return m;
    }
    return ax.message || "Request failed";
  }
  if (error instanceof Error) return error.message;
  return "Something went wrong";
}
