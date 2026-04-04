import axios, { type AxiosError } from "axios";

const baseURL = process.env.NEXT_PUBLIC_API_BASE_URL ?? "";

export const httpClient = axios.create({
  baseURL,
  headers: { "Content-Type": "application/json" },
});

export type ApiErrorBody = { message?: string; title?: string };

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
