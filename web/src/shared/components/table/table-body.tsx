import { Column } from "./table-types";

export const TableBody = ({ tableData, columns }: {tableData :any[], columns: Column[]}) => {
  return (
    <tbody>
      {tableData.map((data: any) => {
        return (
          <tr key={data.id}>
            {columns.map(({ accessor, render}: Column) => {
              const tData = data[accessor] ? data[accessor] : "";
              const tRender = render ? render(tData) : tData;
              return <td key={accessor}>{tRender}</td>;
            })}
          </tr>
        );
      })}
    </tbody>
  );
};
