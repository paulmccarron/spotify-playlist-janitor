import React from 'react';
import styled from 'styled-components';
import { TextInput } from '../../shared/components/text-input/text-input';

export const Catalogue = () => {
  return (
    <Content>
      <div>
        <div><>Text Box</><TextInput {...{ placeholder: 'Example placeholder...' }} /></div>
        <div>Button Primary</div>
        <div>Button Secondary</div>
        <div>Number Input</div>
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
