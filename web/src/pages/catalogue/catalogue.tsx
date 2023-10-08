import React, { useCallback, useState } from 'react';
import styled from 'styled-components';
import { TextInput } from '../../shared/components/text-input';
import { Button } from '../../shared/components/button';
import { Select } from '../../shared/components/select';
import { Toggle } from '../../shared/components/toggle';

const options = [
  { label: 'Option 1', value: 'option1' },
  { label: 'Option 2', value: 'option2' },
  { label: 'Option 3', value: 'option3' },
]

export const Catalogue = () => {
  const [textValue, setTextInputValue] = useState('');
  const [numberValue, setNumberInputValue] = useState<number | undefined>();
  const [selectValue, setSelectValue] = useState('');
  const [toggleValue, setToggleValue] = useState(false);

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

  const onToggleChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setToggleValue(e.target.checked);
  }, [setToggleValue])

  return (
    <Content>
      <div>
        <div>
          <>Text Box</>
          <TextInput {...{ label: 'Email', placeholder: 'Example placeholder...', value: textValue, onChange: onTextChange }} />
          <>Value: {textValue}</>
        </div>
        <div>
          <>Password Box</>
          <TextInput {...{ label: 'Password', type: 'password', placeholder: 'Enter password...', value: textValue, onChange: onTextChange }} />
          <>Value: {textValue}</>
        </div>
        <div>
          <>Number Box</>
          <TextInput {...{ type: 'number', placeholder: 'Enter number...', value: numberValue, onChange: onNumberChange }} />
          <>Value: {numberValue}</>
        </div>
        <div>
          <>Select</>
          <Select {...{ value: selectValue, placeholder: 'Select option...', options, onChange: onSelectChange }} />
          <>Value: {selectValue}</>
        </div>
        <div>
          <>Button Primary</>
          <Button {...{
            className: 'primary',
            id: 'primary-example',
            'data-testid': 'primary-example',
            onClick: () => { alert('Primary Button Clicked!') }
          }}
          >
            Primary
          </Button>
        </div>
        <div>
          <>Button Secondary</>
          <Button {...{
            className: 'secondary',
            id: 'secondary-example',
            'data-testid': 'secondary-example',
            onClick: () => { alert('Secondary Button Clicked!') }
          }}
          >
            Secondary
          </Button>
        </div>
        <div>
          <>Toggle</>
          <Toggle {...{ label: 'Toggle Example', onChange: onToggleChange, checked: toggleValue }} /></div>
        <div>Tabs</div>
        <div>Table</div>
        <div>Modal</div>
        <div>Title</div>
        <div>Subtitle</div>
        <div>Text</div>
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
