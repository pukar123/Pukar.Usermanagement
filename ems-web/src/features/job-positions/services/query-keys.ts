export const jobPositionKeys = {
  all: ["jobPositions"] as const,
  list: (organizationId: number) => [...jobPositionKeys.all, "list", organizationId] as const,
};
