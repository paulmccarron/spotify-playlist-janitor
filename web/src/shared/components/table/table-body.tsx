import { SecondaryText, Text } from "../typography";
import { Column } from "./table-types";

export const TableBody = ({
  tableData,
  columns,
}: {
  tableData: any[];
  columns: Column[];
}) => {
  return (
    <tbody>
      {tableData.map((data: any, index: number) => {
        return (
          <tr key={data.id}>
            {columns.map(({ accessor, primary, render }: Column) => {
              const tData = data[accessor] ? data[accessor] : "";
              const tRender = render ? render(tData) : tData;
              return (
                <td
                  key={accessor}
                  id={`table-body-cell-${index}-${accessor}`}
                  data-testid={`table-body-cell-${index}-${accessor}`}
                >
                  {primary ? (
                    <Text>{tRender}</Text>
                  ) : (
                    <SecondaryText>{tRender}</SecondaryText>
                  )}
                </td>
              );
            })}
          </tr>
        );
      })}
    </tbody>
  );
};

TableBody.displayName = "TableBody";
