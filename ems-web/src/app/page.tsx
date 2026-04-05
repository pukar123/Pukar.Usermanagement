import Link from "next/link";

const cards = [
  { href: "/employees", title: "Employees", description: "Browse and manage employees." },
  { href: "/departments", title: "Departments", description: "Organization departments." },
  { href: "/positions", title: "Positions", description: "Job titles and roles per organization." },
] as const;

export default function Home() {
  return (
    <main className="mx-auto flex max-w-3xl flex-1 flex-col gap-10 px-4 py-12 sm:px-6">
      <div>
        <h1 className="text-3xl font-semibold tracking-tight text-zinc-900 dark:text-zinc-50">
          Employee Management System
        </h1>
        <p className="mt-2 text-zinc-600 dark:text-zinc-400">
          Next.js frontend for EMS.API.           Set <code className="rounded bg-zinc-100 px-1 dark:bg-zinc-800">NEXT_PUBLIC_API_BASE_URL</code> in{" "}
          <code className="rounded bg-zinc-100 px-1 dark:bg-zinc-800">.env.local</code> and complete organization setup on first run; enable CORS on the API.
        </p>
      </div>
      <ul className="grid gap-4 sm:grid-cols-3">
        {cards.map(({ href, title, description }) => (
          <li key={href}>
            <Link
              href={href}
              className="block rounded-xl border border-zinc-200 bg-white p-5 shadow-sm transition hover:border-zinc-300 hover:shadow dark:border-zinc-700 dark:bg-zinc-950 dark:hover:border-zinc-600"
            >
              <span className="font-medium text-zinc-900 dark:text-zinc-50">{title}</span>
              <span className="mt-1 block text-sm text-zinc-600 dark:text-zinc-400">{description}</span>
            </Link>
          </li>
        ))}
      </ul>
    </main>
  );
}
