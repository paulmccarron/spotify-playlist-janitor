import { useState } from "react";
export const TableHead = ({ columns, handleSorting }: any) => {
  const [sortField, setSortField] = useState("");
  const [order, setOrder] = useState("asc");

  const handleSortingChange = (accessor: any) => {
    const sortOrder =
      accessor === sortField && order === "asc" ? "desc" : "asc";
    setSortField(accessor);
    setOrder(sortOrder);
    handleSorting(accessor, sortOrder);
    alert("woooooo")
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
          return (
            <th
              key={accessor}
              onClick={sortable ? () => handleSortingChange(accessor) : () => {}}
              className={cl}
            >
              {label}
            </th>
          );
        })}
      </tr>
    </thead>
  );
};