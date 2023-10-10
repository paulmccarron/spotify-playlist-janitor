import { ReactNode } from "react";

export type SortOrder = "asc" | "desc";

export type Column = {
  label?: string, 
  accessor: string, 
  sortable?: boolean, 
  sortbyOrder?: SortOrder,
  render?(arg: any): string | ReactNode,
}