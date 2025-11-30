export const dangerColor = (level: number): string => {
  if (level <= 3) return "#4caf50";
  if (level <= 6) return "#d1a000";
  return "#b01515";
}
