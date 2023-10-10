import { useState } from "react";
import { Column, SortOrder } from "./table-types";
import { VscArrowUp, VscArrowDown } from "react-icons/vsc";

type TableHeadProps = {
  columns: Column[];
  handleSorting(accessor: string, sortOrder: SortOrder): void;
};

export const TableHead = ({ columns, handleSorting }: TableHeadProps) => {
  const [sortField, setSortField] = useState("");
  const [order, setOrder] = useState<SortOrder>("asc");

  const handleSortingChange = (accessor: any) => {
    const sortOrder =
      accessor === sortField && order === "asc" ? "desc" : "asc";
    setSortField(accessor);
    setOrder(sortOrder);
    handleSorting(accessor, sortOrder);
  };

  return (
    <thead>
      <tr>
        {columns.map(({ label, accessor, sortable }: any) => {
          const cl = sortable
            ? sortField === accessor && order === "asc"
              ? "up"
              : sortField === accessor && order === "desc"
              ? "down"
              : "default"
            : "";

          let symbol = undefined;

          if(accessor === sortField){
            symbol = order === "asc" ? <VscArrowUp/> : <VscArrowDown/>
          }
          return (
            <th
              key={accessor}
              onClick={
                sortable ? () => handleSortingChange(accessor) : () => {}
              }
              className={cl}
            >
              {label} {symbol}
            </th>
          );
        })}
      </tr>
    </thead>
  );
};
