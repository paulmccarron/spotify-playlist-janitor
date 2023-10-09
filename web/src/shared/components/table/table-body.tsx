export const TableBody = ({ tableData, columns }: any) => {
    return (
      <tbody>
        {tableData.map((data: any) => {
          return (
            <tr key={data.id}>
              {columns.map(({ accessor }: any) => {
                const tData = data[accessor] ? data[accessor] : "——";
                return <td key={accessor}>{tData}</td>;
              })}
            </tr>
          );
        })}
      </tbody>
    );
  };