import { create } from "zustand";
import type { Employee } from "../types/employee.types";

export type EmployeeFormMode = "create" | "edit" | null;

type EmployeeUiState = {
  selectedEmployee: Employee | null;
  setSelectedEmployee: (e: Employee | null) => void;
  formMode: EmployeeFormMode;
  openCreateForm: () => void;
  openEditForm: (employee: Employee) => void;
  closeForm: () => void;
  isDeleteOpen: boolean;
  employeeToDelete: Employee | null;
  openDeleteDialog: (employee: Employee) => void;
  closeDeleteDialog: () => void;
};

export const useEmployeeUiStore = create<EmployeeUiState>((set) => ({
  selectedEmployee: null,
  setSelectedEmployee: (employee) => set({ selectedEmployee: employee }),

  formMode: null,
  openCreateForm: () => set({ formMode: "create", selectedEmployee: null }),
  openEditForm: (employee) => set({ formMode: "edit", selectedEmployee: employee }),
  closeForm: () => set({ formMode: null, selectedEmployee: null }),

  isDeleteOpen: false,
  employeeToDelete: null,
  openDeleteDialog: (employee) => set({ isDeleteOpen: true, employeeToDelete: employee }),
  closeDeleteDialog: () => set({ isDeleteOpen: false, employeeToDelete: null }),
}));
