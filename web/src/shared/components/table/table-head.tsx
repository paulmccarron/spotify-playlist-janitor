import { useState } from "react";
import styled from "styled-components";
import { VscArrowUp, VscArrowDown } from "react-icons/vsc";

import { TABLE_HOVER } from "shared/constants";

import { Column, SortOrder } from "./table-types";
import { Text } from "../typography";

type TableHeadProps = {
  columns: Column[];
  handleSorting(accessor: string, sortOrder: SortOrder): void;
};

const Tr = styled.tr`
  .sortable {
    cursor: pointer !important;
  }

  .sortable:hover {
    background-color: ${TABLE_HOVER};
  }

  .content {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
  }

  svg {
    padding-top: 3px;
    margin-bottom: -3px;
  }
`;

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
      <Tr>
        {columns.map(({ label, accessor, sortable }: any) => {
          let icon = undefined;

          if (accessor === sortField) {
            icon = order === "asc" ? <VscArrowUp /> : <VscArrowDown />;
          }
          return (
            <th
              key={accessor}
              onClick={
                sortable ? () => handleSortingChange(accessor) : () => {}
              }
              className={sortable ? "sortable" : ""}
            >
              <div className="content">
                <Text>{label}</Text>
                {icon}
              </div>
            </th>
          );
        })}
      </Tr>
    </thead>
  );
};

TableHead.displayName = "TableHead";
