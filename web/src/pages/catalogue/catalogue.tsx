import React, { useCallback, useState } from "react";
import styled from "styled-components";
import { TextInput } from "../../shared/components/text-input";
import { Button } from "../../shared/components/button";
import { Select } from "../../shared/components/select";
import { Toggle } from "../../shared/components/toggle";
import { Tabs, Tab, TabPanel, TabList } from "../../shared/components/tabs";
import { Column, Table } from "../../shared/components/table";
import { VscTrash } from "react-icons/vsc";

const options = [
  { label: "Option 1", value: "option1" },
  { label: "Option 2", value: "option2" },
  { label: "Option 3", value: "option3" },
];

const tableData = [
  {
    id: 1,
    image: "https://i.scdn.co/image/ab67616d00004851835a9e77dae1c928f871ac73",
    title: "Longshot",
    artist: "Catfish and the Bottlemen",
    album: "The Balance",
    length: 232960,
    date_skipped: new Date(2023, 9, 15, 15, 30, 45, 20),
    delete: undefined,
  },
  {
    id: 2,
    image: "https://i.scdn.co/image/ab67616d0000485158406b3f1ac3ceaff7a64fef",
    title: "Dark Necessities",
    artist: "Red Hot Chili Peppers",
    album: "The Getaway",
    length: 302000,
    date_skipped: new Date(2023, 9, 15, 15, 30, 45, 25),
    delete: undefined,
  },
];

const tableColumns: Column[] = [
  {
    label: "",
    accessor: "image",
    sortable: false,
    render: (url: string) => <img alt="" width={40} height={40} src={url} />,
  },
  { label: "Title", accessor: "title", sortable: true, sortbyOrder: "desc" },
  { label: "Artist", accessor: "artist", sortable: true },
  { label: "Album", accessor: "album", sortable: true },
  {
    label: "Length",
    accessor: "length",
    sortable: true,
    render: (millis: number) => {
      var minutes = Math.floor(millis / 60000);
      var seconds = parseInt(((millis % 60000) / 1000).toFixed(0));
      return seconds === 60
        ? minutes + 1 + ":00"
        : minutes + ":" + (seconds < 10 ? "0" : "") + seconds;
    },
  },
  {
    label: "Date Skipped",
    accessor: "date_skipped",
    sortable: true,
    render: (date: Date) => date.toISOString(),
  },
  {
    label: "",
    accessor: "delete",
    sortable: false,
    render: (id: string) => (
      <VscTrash onClick={() => alert(`Deleted track id: ${id}`)} />
    ),
  },
];

export const Catalogue = () => {
  const [textValue, setTextInputValue] = useState("");
  const [numberValue, setNumberInputValue] = useState<number | undefined>();
  const [selectValue, setSelectValue] = useState<
    { label: string; value: string } | undefined
  >(undefined);
  const [toggleValue, setToggleValue] = useState(false);

  const onTextChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setTextInputValue(e.target.value);
    },
    [setTextInputValue]
  );

  const onNumberChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      const newValue = parseInt(e.target.value);
      setNumberInputValue(newValue);
    },
    [setNumberInputValue]
  );

  const onSelectChange = useCallback(
    (newValue: { label: string; value: string }) => {
      setSelectValue(newValue);
    },
    [setSelectValue]
  );

  const onToggleChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setToggleValue(e.target.checked);
    },
    [setToggleValue]
  );

  return (
    <Content>
      <CatalogueTable>
        <tr>
          <th>Component</th>
          <th>Example</th>
        </tr>
        <tr>
          <td>Text Box</td>
          <td>
            <TextInput
              {...{
                label: "Email",
                placeholder: "Enter email...",
                id: "example",
                value: textValue,
                onChange: onTextChange,
              }}
            />
            <>Value: {textValue}</>
          </td>
        </tr>
        <tr>
          <td>Password Box</td>
          <td>
            <TextInput
              {...{
                type: "password",
                label: "Password",
                placeholder: "Enter password...",
                value: textValue,
                onChange: onTextChange,
              }}
            />
            <>Value: {textValue}</>
          </td>
        </tr>
        <tr>
          <td>Number Box</td>
          <td>
            <TextInput
              {...{
                type: "number",
                label: "Minutes",
                placeholder: "Enter number of minutes...",
                value: numberValue,
                onChange: onNumberChange,
              }}
            />
            <>Value: {numberValue}</>
          </td>
        </tr>
        <tr>
          <td>Select</td>
          <td>
            <Select
              {...{
                value: selectValue,
                label: "Select Label",
                placeholder: "Select option...",
                options,
                onChange: onSelectChange,
              }}
            />
            <>Value: {JSON.stringify(selectValue)}</>
          </td>
        </tr>
        <tr>
          <td>Button Primary</td>
          <td>
            <Button
              {...{
                className: "primary",
                id: "primary-example",
                "data-testid": "primary-example",
                onClick: () => {
                  alert("Primary Button Clicked!");
                },
              }}
            >
              Primary
            </Button>
          </td>
        </tr>
        <tr>
          <td>Button Secondary</td>
          <td>
            <Button
              {...{
                className: "secondary",
                id: "secondary-example",
                "data-testid": "secondary-example",
                onClick: () => {
                  alert("Secondary Button Clicked!");
                },
              }}
            >
              Secondary
            </Button>
          </td>
        </tr>
        <tr>
          <td>Toggle</td>
          <td>
            <Toggle
              {...{
                label: "Toggle Example",
                onChange: onToggleChange,
                checked: toggleValue,
              }}
            />
          </td>
        </tr>
        <tr>
          <td>Tabs</td>
          <td>
            <Tabs>
              <TabList>
                <Tab>Tab 1</Tab>
                <Tab>Tab 2</Tab>
                <Tab>Longer Tab Title 3</Tab>
              </TabList>

              <TabPanel>
                <>Tab 1 content</>
              </TabPanel>
              <TabPanel>
                <>Tab 2 content</>
              </TabPanel>
              <TabPanel>
                <>Tab 3 content</>
              </TabPanel>
            </Tabs>
          </td>
        </tr>
        <tr>
          <td>Table</td>
          <td>
            <Table
              caption="Table caption."
              data={tableData}
              columns={tableColumns}
            />
          </td>
        </tr>
        <tr>
          <td>Modal</td>
          <td></td>
        </tr>
        <tr>
          <td>Title</td>
          <td></td>
        </tr>
        <tr>
          <td>Subtitle</td>
          <td></td>
        </tr>
        <tr>
          <td>Text</td>
          <td></td>
        </tr>
      </CatalogueTable>
    </Content>
  );
};

const Content = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
`;

const CatalogueTable = styled.table`
  align-items: center;
  justify-content: center;
  color: white;
  margin-top: 8px;
  th,
  td {
    border: 1px solid #ddd;
    padding: 8px;
  }
`;
