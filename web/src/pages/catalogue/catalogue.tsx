import React, { useCallback, useState } from "react";
import styled from "styled-components";
import { TextInput } from "../../shared/components/text-input";
import { Button } from "../../shared/components/button";
import { Select } from "../../shared/components/select";
import { Toggle } from "../../shared/components/toggle";

const options = [
  { label: "Option 1", value: "option1" },
  { label: "Option 2", value: "option2" },
  { label: "Option 3", value: "option3" },
];

export const Catalogue = () => {
  const [textValue, setTextInputValue] = useState("");
  const [numberValue, setNumberInputValue] = useState<number | undefined>();
  const [selectValue, setSelectValue] = useState("");
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
    (e: React.ChangeEvent<HTMLSelectElement>) => {
      setSelectValue(e.target.value);
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
      <Table>
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
                placeholder: "Example placeholder...",
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
                label: "Password",
                type: "password",
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
                placeholder: "Enter number...",
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
                placeholder: "Select option...",
                options,
                onChange: onSelectChange,
              }}
            />
            <>Value: {selectValue}</>
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
          <td></td>
        </tr>
        <tr>
          <td>Table</td>
          <td></td>
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
      </Table>
    </Content>
  );
};

const Content = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
`;

const Table = styled.table`
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
