import Link from "next/link";

export default function Home() {
  return (
    <main className="mx-auto flex max-w-3xl flex-1 flex-col justify-center gap-8 px-4 py-16 sm:px-6">
      <div>
        <h1 className="text-3xl font-semibold tracking-tight text-zinc-900 dark:text-zinc-50">
          Employee Management System
        </h1>
        <p className="mt-2 text-zinc-600 dark:text-zinc-400">
          Next.js frontend for the EMS.API backend. Configure <code className="rounded bg-zinc-100 px-1 dark:bg-zinc-800">NEXT_PUBLIC_API_BASE_URL</code>{" "}
          and ensure CORS is enabled on the API.
        </p>
      </div>
      <Link
        href="/employees"
        className="inline-flex w-fit rounded-lg bg-zinc-900 px-5 py-2.5 text-sm font-medium text-white hover:bg-zinc-800 dark:bg-zinc-100 dark:text-zinc-900 dark:hover:bg-white"
      >
        Open employees
      </Link>
    </main>
  );
}
