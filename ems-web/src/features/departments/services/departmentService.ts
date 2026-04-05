import { httpClient } from "@/shared/api/http-client";
import type {
  CreateDepartmentRequest,
  Department,
  UpdateDepartmentRequest,
} from "../types/department.types";

const PATH = "/api/Departments";

export const departmentService = {
  getDepartments: async (): Promise<Department[]> => {
    const { data } = await httpClient.get<Department[]>(PATH);
    return data;
  },

  createDepartment: async (body: CreateDepartmentRequest): Promise<Department> => {
    const { data } = await httpClient.post<Department>(PATH, body);
    return data;
  },

  updateDepartment: async (id: number, body: UpdateDepartmentRequest): Promise<Department> => {
    const { data } = await httpClient.put<Department>(`${PATH}/${id}`, body);
    return data;
  },

  deleteDepartment: async (id: number): Promise<void> => {
    await httpClient.delete(`${PATH}/${id}`);
  },
};
