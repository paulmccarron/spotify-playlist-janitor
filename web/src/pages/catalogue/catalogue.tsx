import React, { useCallback, useState } from "react";
import styled from "styled-components";
import { TextInput } from "../../shared/components/text-input";
import { Button } from "../../shared/components/button";
import { Select } from "../../shared/components/select";
import { Toggle } from "../../shared/components/toggle";
import { Tabs, Tab, TabPanel, TabList } from "../../shared/components/tabs";
import { Table } from "../../shared/components/table";
import { selectOptions, tableColumns, tableData } from "./data";
import {
  Title,
  SubTitle,
  Text,
  SecondaryText,
  SubText,
} from "../../shared/components/typography";
import { useModal, Modal } from "../../shared/components/modal";
import { VscGithub, VscQuestion, VscSmiley } from "react-icons/vsc";
// import { Tooltip } from "react-tooltip";
import { Tooltip } from "../../shared/components/tooltip";

export const Catalogue = () => {
  const [textValue, setTextInputValue] = useState("");
  const [passwordValue, setPasswordInputValue] = useState("");
  const [numberValue, setNumberInputValue] = useState<number | undefined>();
  const [selectValue, setSelectValue] = useState<
    { label: string; value: string } | undefined
  >(undefined);
  const [toggleValue, setToggleValue] = useState(false);

  const { isOpen, onOpen, onClose } = useModal();
  const { isOpen: isOpen2, onOpen: onOpen2, onClose: onClose2 } = useModal();

  const onTextChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setTextInputValue(e.target.value);
    },
    [setTextInputValue]
  );

  const onPasswordChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      setPasswordInputValue(e.target.value);
    },
    [setPasswordInputValue]
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
        <thead>
          <tr>
            <th>Component</th>
            <th>Example</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Title</td>
            <td>
              <Title
                {...{
                  id: "title-test",
                  "data-testid": "title-test-data-testid",
                }}
              >
                Title Text
              </Title>
            </td>
          </tr>
          <tr>
            <td>Subtitle</td>
            <td>
              <SubTitle
                {...{
                  id: "subtitle-test",
                  "data-testid": "subtitle-test-data-testid",
                }}
              >
                Subtitle Text
              </SubTitle>
            </td>
          </tr>
          <tr>
            <td>Text</td>
            <td>
              <Text
                {...{
                  id: "text-test",
                  "data-testid": "text-test-data-testid",
                }}
              >
                Normal Text
              </Text>
              <Text
                {...{
                  id: "text-test",
                  "data-testid": "text-test-data-testid",
                  style: { color: "#1ed760" },
                }}
              >
                Normal Text
              </Text>
            </td>
          </tr>
          <tr>
            <td>Secondary Text</td>
            <td>
              <SecondaryText
                {...{
                  id: "secondary-text-test",
                  "data-testid": "secondary-text-test-data-testid",
                }}
              >
                Secondary Text
              </SecondaryText>
            </td>
          </tr>
          <tr>
            <td>Sub Text</td>
            <td>
              <SubText
                {...{
                  id: "sub-text-test",
                  "data-testid": "sub-text-test-data-testid",
                }}
              >
                Sub Text
              </SubText>
            </td>
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
                  value: passwordValue,
                  onChange: onPasswordChange,
                }}
              />
              <>Value: {passwordValue}</>
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
                  options: selectOptions,
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
            <td style={{ display: "flex", justifyContent: "space-evenly" }}>
              <>
                <Button
                  {...{
                    className: "primary",
                    id: "modal-button-example",
                    "data-testid": "modal-button-example",
                    onClick: onOpen,
                  }}
                >
                  Open Modal
                </Button>
                <Modal {...{ isOpen, onClose, label: "Test Label" }}>
                  <>
                    <Title style={{ marginBottom: 8 }}>Sample Modal</Title>
                    <Text style={{ marginBottom: 8 }}>Modal Content</Text>
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: "flex-end",
                      }}
                    >
                      <Button
                        {...{
                          className: "secondary",
                          style: { margin: "0px 4px" },
                          id: "cancel-modal-button-example",
                          "data-testid": "cancel-modal-button-example",
                          onClick: onClose,
                        }}
                      >
                        Cancel
                      </Button>
                      <Button
                        {...{
                          className: "primary",
                          style: { margin: "0px 4px" },
                          id: "confirm-modal-button-example",
                          "data-testid": "confirm-modal-button-example",
                          onClick: onClose,
                        }}
                      >
                        Confirm
                      </Button>
                    </div>
                  </>
                </Modal>
              </>
              <>
                <Button
                  {...{
                    className: "primary",
                    id: "modal-button-example-2",
                    "data-testid": "modal-button-example-2",
                    onClick: onOpen2,
                  }}
                >
                  Open Modal 2
                </Button>
                <Modal
                  {...{
                    isOpen: isOpen2,
                    onClose: onClose2,
                    label: "Test Label",
                  }}
                >
                  <>
                    <Title style={{ marginBottom: 8 }}>Sample Modal 2</Title>
                    <Text style={{ marginBottom: 8 }}>Modal Content 2</Text>
                    <div
                      style={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: "flex-end",
                      }}
                    >
                      <Button
                        {...{
                          className: "secondary",
                          style: { margin: "0px 4px" },
                          id: "cancel-modal-button-example",
                          "data-testid": "cancel-modal-button-example",
                          onClick: onClose2,
                        }}
                      >
                        Cancel
                      </Button>
                      <Button
                        {...{
                          className: "primary",
                          style: { margin: "0px 4px" },
                          id: "confirm-modal-button-example",
                          "data-testid": "confirm-modal-button-example",
                          onClick: onClose2,
                        }}
                      >
                        Confirm
                      </Button>
                    </div>
                  </>
                </Modal>
              </>
            </td>
          </tr>
          <tr>
            <td>Tooltip</td>
            <td>
              <Tooltip content={"Tooltip Content"}>
                <VscQuestion />
              </Tooltip>
              <Tooltip
                content={
                  <>
                    <Text>Happy Tooltip Content</Text>
                    <SecondaryText>Helpful Explanation</SecondaryText>
                  </>
                }
              >
                <VscSmiley />
              </Tooltip>
              <Tooltip
                content={<SecondaryText>Github Tooltip Content</SecondaryText>}
              >
                <VscGithub
                  style={{ cursor: "pointer" }}
                  onClick={() => alert("You did a GitHub &#127881;!!!")}
                />
              </Tooltip>
            </td>
          </tr>
          <tr>
            <td>Popover</td>
            <td></td>
          </tr>
        </tbody>
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
