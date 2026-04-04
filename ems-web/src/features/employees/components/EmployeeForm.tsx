"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
import { Button } from "@/shared/components/Button";
import { getErrorMessage } from "@/shared/api/http-client";
import { useCreateEmployee, useUpdateEmployee } from "../hooks";
import {
  employeeFormSchema,
  type EmployeeFormInput,
  type EmployeeFormValues,
} from "../types/employee-form.schema";
import type { Employee } from "../types/employee.types";
import { employmentStatusLabels } from "../types/employment-status";
import { dateInputToApiIso, toDateInputValue } from "../utils/date-format";
import type { CreateEmployeeRequest, UpdateEmployeeRequest } from "../types/employee.types";
import { useEmployeeUiStore } from "../store/employee-ui-store";

const defaultOrgId = Number(process.env.NEXT_PUBLIC_DEFAULT_ORGANIZATION_ID) || 1;

const inputClass =
  "mt-1 w-full rounded-lg border border-zinc-300 bg-white px-3 py-2 text-sm text-zinc-900 shadow-sm focus:border-zinc-500 focus:outline-none focus:ring-1 focus:ring-zinc-500 dark:border-zinc-600 dark:bg-zinc-900 dark:text-zinc-100";

function emptyDefaults(): EmployeeFormInput {
  return {
    organizationId: defaultOrgId,
    employeeNumber: "",
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    dateOfBirth: "",
    dateJoined: "",
    employmentStatus: 0,
    isActive: true,
    departmentId: "",
    locationId: "",
    managerId: "",
    jobPositionId: "",
  };
}

function employeeToFormInput(emp: Employee): EmployeeFormInput {
  const idStr = (n: number | null) => (n != null ? String(n) : "");
  return {
    organizationId: emp.organizationId,
    employeeNumber: emp.employeeNumber,
    firstName: emp.firstName,
    lastName: emp.lastName,
    email: emp.email,
    phoneNumber: emp.phoneNumber ?? "",
    dateOfBirth: toDateInputValue(emp.dateOfBirth),
    dateJoined: toDateInputValue(emp.dateJoined),
    employmentStatus: emp.employmentStatus,
    isActive: emp.isActive,
    departmentId: idStr(emp.departmentId),
    locationId: idStr(emp.locationId),
    managerId: idStr(emp.managerId),
    jobPositionId: idStr(emp.jobPositionId),
  };
}

function toApiPayload(values: EmployeeFormValues): CreateEmployeeRequest {
  return {
    organizationId: values.organizationId,
    departmentId: values.departmentId,
    locationId: values.locationId,
    managerId: values.managerId,
    jobPositionId: values.jobPositionId,
    employeeNumber: values.employeeNumber,
    firstName: values.firstName,
    lastName: values.lastName,
    email: values.email,
    phoneNumber: values.phoneNumber?.trim() ? values.phoneNumber.trim() : null,
    dateOfBirth: dateInputToApiIso(values.dateOfBirth),
    dateJoined: dateInputToApiIso(values.dateJoined),
    employmentStatus: values.employmentStatus,
    isActive: values.isActive,
  };
}

type EmployeeFormProps = {
  mode: "create" | "edit";
  employee: Employee | null;
  onSuccess: () => void;
  onCancel: () => void;
};

export function EmployeeForm({ mode, employee, onSuccess, onCancel }: EmployeeFormProps) {
  const createMutation = useCreateEmployee();
  const updateMutation = useUpdateEmployee();
  const closeForm = useEmployeeUiStore((s) => s.closeForm);

  const form = useForm<EmployeeFormInput, unknown, EmployeeFormValues>({
    resolver: zodResolver(employeeFormSchema),
    defaultValues: emptyDefaults(),
  });

  useEffect(() => {
    if (mode === "edit" && employee) {
      form.reset(employeeToFormInput(employee));
    } else {
      form.reset(emptyDefaults());
    }
  }, [mode, employee, form.reset]);

  const onSubmit = (values: EmployeeFormValues) => {
    const payload = toApiPayload(values) as UpdateEmployeeRequest;

    if (mode === "create") {
      createMutation.mutate(payload as CreateEmployeeRequest, {
        onSuccess: () => {
          toast.success("Employee created");
          closeForm();
          onSuccess();
        },
        onError: (e) => toast.error(getErrorMessage(e)),
      });
    } else if (employee) {
      updateMutation.mutate(
        { id: employee.id, data: payload },
        {
          onSuccess: () => {
            toast.success("Employee updated");
            closeForm();
            onSuccess();
          },
          onError: (e) => toast.error(getErrorMessage(e)),
        },
      );
    }
  };

  const pending = createMutation.isPending || updateMutation.isPending;

  return (
    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
      <div className="grid gap-4 sm:grid-cols-2">
        <Field label="Organization ID" error={form.formState.errors.organizationId?.message}>
          <input
            type="number"
            className={inputClass}
            {...form.register("organizationId", { valueAsNumber: true })}
          />
        </Field>
        <Field label="Employee number" error={form.formState.errors.employeeNumber?.message}>
          <input type="text" className={inputClass} {...form.register("employeeNumber")} />
        </Field>
        <Field label="First name" error={form.formState.errors.firstName?.message}>
          <input type="text" className={inputClass} {...form.register("firstName")} />
        </Field>
        <Field label="Last name" error={form.formState.errors.lastName?.message}>
          <input type="text" className={inputClass} {...form.register("lastName")} />
        </Field>
        <Field label="Email" error={form.formState.errors.email?.message}>
          <input type="email" className={inputClass} {...form.register("email")} />
        </Field>
        <Field label="Phone" error={form.formState.errors.phoneNumber?.message}>
          <input type="text" className={inputClass} {...form.register("phoneNumber")} />
        </Field>
        <Field label="Date of birth" error={form.formState.errors.dateOfBirth?.message}>
          <input type="date" className={inputClass} {...form.register("dateOfBirth")} />
        </Field>
        <Field label="Date joined" error={form.formState.errors.dateJoined?.message}>
          <input type="date" className={inputClass} {...form.register("dateJoined")} />
        </Field>
        <Field label="Employment status" error={form.formState.errors.employmentStatus?.message}>
          <select className={inputClass} {...form.register("employmentStatus", { valueAsNumber: true })}>
            {Object.entries(employmentStatusLabels).map(([value, label]) => (
              <option key={value} value={value}>
                {label}
              </option>
            ))}
          </select>
        </Field>
        <Field label="Active" error={form.formState.errors.isActive?.message}>
          <label className="mt-1 flex items-center gap-2 text-sm">
            <input type="checkbox" {...form.register("isActive")} />
            <span>Is active</span>
          </label>
        </Field>
        <Field label="Department ID (optional)" error={form.formState.errors.departmentId?.message}>
          <input type="text" className={inputClass} placeholder="e.g. 1" {...form.register("departmentId")} />
        </Field>
        <Field label="Location ID (optional)" error={form.formState.errors.locationId?.message}>
          <input type="text" className={inputClass} {...form.register("locationId")} />
        </Field>
        <Field label="Manager ID (optional)" error={form.formState.errors.managerId?.message}>
          <input type="text" className={inputClass} {...form.register("managerId")} />
        </Field>
        <Field label="Job position ID (optional)" error={form.formState.errors.jobPositionId?.message}>
          <input type="text" className={inputClass} {...form.register("jobPositionId")} />
        </Field>
      </div>

      <div className="flex justify-end gap-2 pt-2">
        <Button type="button" variant="secondary" onClick={onCancel} disabled={pending}>
          Cancel
        </Button>
        <Button type="submit" disabled={pending}>
          {pending ? "Saving…" : mode === "create" ? "Create" : "Save changes"}
        </Button>
      </div>
    </form>
  );
}

function Field({
  label,
  error,
  children,
}: {
  label: string;
  error?: string;
  children: import("react").ReactNode;
}) {
  return (
    <div className="sm:col-span-1">
      <label className="block text-xs font-medium uppercase tracking-wide text-zinc-500 dark:text-zinc-400">
        {label}
      </label>
      {children}
      {error ? <p className="mt-1 text-xs text-red-600 dark:text-red-400">{error}</p> : null}
    </div>
  );
}
