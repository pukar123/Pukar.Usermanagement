import type { EmploymentStatusValue } from "./employment-status";

/** Aligns with EMS.Application.DTOs.Employee.EmployeeResponseModel (camelCase JSON). */
export type Employee = {
  id: number;
  organizationId: number;
  departmentId: number | null;
  locationId: number | null;
  managerId: number | null;
  jobPositionId: number | null;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string | null;
  dateOfBirth: string;
  dateJoined: string;
  employmentStatus: EmploymentStatusValue;
  isActive: boolean;
  createdAtUtc: string;
  updatedAtUtc: string;
};

export type CreateEmployeeRequest = {
  organizationId: number;
  departmentId?: number | null;
  locationId?: number | null;
  managerId?: number | null;
  jobPositionId?: number | null;
  employeeNumber: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string | null;
  dateOfBirth: string;
  dateJoined: string;
  employmentStatus: EmploymentStatusValue;
  isActive: boolean;
};

export type UpdateEmployeeRequest = CreateEmployeeRequest;
