import { HTMLAttributes } from "react";
import styled from "styled-components";

const StyledTitle = styled.div`
  font-size: 2rem;
  font-weight: 700;
`;

export const Title = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <StyledTitle {...props}>{children}</StyledTitle>;
};

Title.displayName = "Title";

const StyledSubTitle = styled.div`
  font-size: 1.5rem;
  font-weight: 600;
`;

export const SubTitle = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <StyledSubTitle {...props}>{children}</StyledSubTitle>;
};

SubTitle.displayName = "SubTitle";

const StyledText = styled.div`
  font-size: 1rem;
  font-weight: 600;
`;

export const Text = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <StyledText {...props}>{children}</StyledText>;
};

Text.displayName = "Text";

const StyledSecondaryText = styled.div`
  font-size: 1rem;
`;

export const SecondaryText = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <StyledSecondaryText {...props}>{children}</StyledSecondaryText>;
};

SecondaryText.displayName = "SecondaryText";

const StyledSubText = styled.div`
  font-size: 0.75rem;
`;

export const SubText = ({
  children,
  ...props
}: HTMLAttributes<HTMLDivElement>) => {
  return <StyledSubText {...props}>{children}</StyledSubText>;
};

StyledSubText.displayName = "SubTextDiv";
