import { ReactNode } from "react";

export type SortOrder = "asc" | "desc";

export type Column = {
  label?: string | ReactNode,
  accessor: string, 
  sortable?: boolean, 
  sortbyOrder?: SortOrder,
  render?(arg: any): string | ReactNode,
}