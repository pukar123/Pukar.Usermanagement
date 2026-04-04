"use client";

import type { Employee } from "../types/employee.types";
import { employmentStatusLabels } from "../types/employment-status";
import { Button } from "@/shared/components/Button";
import { cn } from "@/shared/utils/cn";

type EmployeeTableProps = {
  employees: Employee[];
  onEdit: (e: Employee) => void;
  onDelete: (e: Employee) => void;
};

export function EmployeeTable({ employees, onEdit, onDelete }: EmployeeTableProps) {
  return (
    <div className="overflow-x-auto rounded-lg border border-zinc-200 dark:border-zinc-700">
      <table className="min-w-full divide-y divide-zinc-200 text-left text-sm dark:divide-zinc-700">
        <thead className="bg-zinc-50 dark:bg-zinc-900/50">
          <tr>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">#</th>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">Name</th>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">Email</th>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">Status</th>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">Job pos. id</th>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">Active</th>
            <th className="px-4 py-3 font-medium text-zinc-700 dark:text-zinc-300">Actions</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-zinc-200 dark:divide-zinc-700">
          {employees.map((row) => (
            <tr key={row.id} className="bg-white hover:bg-zinc-50 dark:bg-zinc-950 dark:hover:bg-zinc-900">
              <td className="whitespace-nowrap px-4 py-3 font-mono text-zinc-600 dark:text-zinc-400">
                {row.employeeNumber}
              </td>
              <td className="px-4 py-3 text-zinc-900 dark:text-zinc-100">
                {row.firstName} {row.lastName}
              </td>
              <td className="px-4 py-3 text-zinc-700 dark:text-zinc-300">{row.email}</td>
              <td className="px-4 py-3 text-zinc-700 dark:text-zinc-300">
                {employmentStatusLabels[row.employmentStatus] ?? row.employmentStatus}
              </td>
              <td className="px-4 py-3 font-mono text-zinc-600 dark:text-zinc-400">
                {row.jobPositionId ?? "—"}
              </td>
              <td className="px-4 py-3">
                <span
                  className={cn(
                    "inline-flex rounded-full px-2 py-0.5 text-xs font-medium",
                    row.isActive
                      ? "bg-emerald-100 text-emerald-800 dark:bg-emerald-900/40 dark:text-emerald-200"
                      : "bg-zinc-200 text-zinc-700 dark:bg-zinc-800 dark:text-zinc-300",
                  )}
                >
                  {row.isActive ? "Yes" : "No"}
                </span>
              </td>
              <td className="whitespace-nowrap px-4 py-3">
                <div className="flex gap-2">
                  <Button type="button" variant="secondary" className="!py-1 !text-xs" onClick={() => onEdit(row)}>
                    Edit
                  </Button>
                  <Button type="button" variant="danger" className="!py-1 !text-xs" onClick={() => onDelete(row)}>
                    Delete
                  </Button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {employees.length === 0 ? (
        <p className="p-6 text-center text-sm text-zinc-500">No employees match the current filter.</p>
      ) : null}
    </div>
  );
}
