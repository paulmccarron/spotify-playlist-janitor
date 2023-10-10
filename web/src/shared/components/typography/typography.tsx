import React, { HTMLAttributes } from "react";
import styled from "styled-components";

export const Title = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <TitleDiv {...props}>{children}</TitleDiv>;
};

const TitleDiv = styled.div`
  font-size: 2rem;
  font-weight: 700;
`;

export const SubTitle = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <SubTitleDiv {...props}>{children}</SubTitleDiv>;
};

const SubTitleDiv = styled.div`
  font-size: 1.5rem;
  font-weight: 600;
`;

export const Text = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <TextDiv {...props}>{children}</TextDiv>;
};

const TextDiv = styled.div`
  font-size: 1rem;
  font-weight: 600;
`;

export const SecondaryText = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <SecondaryTextDiv {...props}>{children}</SecondaryTextDiv>;
};

const SecondaryTextDiv = styled.div`
  font-size: 1rem;
`;
