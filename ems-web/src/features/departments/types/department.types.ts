/** Aligns with EMS.Application.DTOs.Department (camelCase JSON). */
export type Department = {
  id: number;
  organizationId: number;
  name: string;
  code: string | null;
  parentDepartmentId: number | null;
  isActive: boolean;
};

export type CreateDepartmentRequest = {
  organizationId: number;
  name: string;
  code?: string | null;
  parentDepartmentId?: number | null;
  isActive: boolean;
};

export type UpdateDepartmentRequest = {
  name: string;
  code?: string | null;
  parentDepartmentId?: number | null;
  isActive: boolean;
};
