export const locationKeys = {
  all: ["locations"] as const,
  list: () => [...locationKeys.all, "list"] as const,
};
