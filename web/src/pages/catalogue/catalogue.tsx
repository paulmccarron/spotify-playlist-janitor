import React, { useCallback, useState } from 'react';
import styled from 'styled-components';
import { TextInput } from '../../shared/components/text-input';
import { Button } from '../../shared/components/button';

export const Catalogue = () => {
  const [textValue, setTextInputValue] = useState('');
  const [numberValue, setNumberInputValue] = useState<number | undefined>();

  const onTextChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    setTextInputValue(e.target.value);
  }, [setTextInputValue])

  const onNumberChange = useCallback((e: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = parseInt(e.target.value)
    setNumberInputValue(newValue);
  }, [setTextInputValue])

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
          <TextInput {...{ type: 'number', placeholder: 'Enter number...', value: numberValue, onChange: onNumberChange }} /></div>
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
