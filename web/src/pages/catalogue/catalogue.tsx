import React, { useCallback, useState } from 'react';
import styled from 'styled-components';
import { TextInput } from '../../shared/components/text-input';
import { Button } from '../../shared/components/button';
import { Select } from '../../shared/components/select';

const options = [
  {label: 'Option 1', value: 'option1'},
  {label: 'Option 2', value: 'option2'},
  {label: 'Option 3', value: 'option3'},
]

export const Catalogue = () => {
  const [textValue, setTextInputValue] = useState('');
  const [numberValue, setNumberInputValue] = useState<number | undefined>();
  const [selectValue, setSelectValue] = useState('');

  const onTextChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setTextInputValue(e.target.value);
  }, [setTextInputValue])

  const onNumberChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = parseInt(e.target.value)
    setNumberInputValue(newValue);
  }, [setNumberInputValue])

  const onSelectChange = useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectValue(e.target.value);
  }, [setSelectValue])

  return (
    <Content>
      <div>
        <div>
          <>Text Box</>
          <TextInput {...{ placeholder: 'Example placeholder...', value: textValue, onChange: onTextChange }} />
        </div>
        <div>
          <>Password Box</>
          <TextInput {...{ type: 'password', placeholder: 'Enter password...', value: textValue, onChange: onTextChange }} />
        </div>
        <div>
          <>Number Box</>
          <TextInput {...{ type: 'number', placeholder: 'Enter number...', value: numberValue, onChange: onNumberChange }} />
        </div>
        <div>
          <>Select</>
          <Select {...{ value: selectValue, placeholder: 'Select option...', options, onChange: onSelectChange }} />
        </div>
        <div>
          <>Button Primary</>
          <Button {...{ text: 'Primary', onClick: () => { alert('Primary Button Clicked!') } }} />
        </div>
        <div>
          <>Button Primary</>
          <Button {...{ text: 'Secondary', type: 'secondary', onClick: () => { alert('Secondary Button Clicked!') } }} />
        </div>
        <div>Toggle</div>
        <div>Tabs</div>
        <div>List</div>
        <div>Modal</div>
      </div>
    </Content>
  );
}

const Content = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: white;
`;
