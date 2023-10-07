import React from 'react';
import styled from 'styled-components';

type ButtonProps = {
    text: string;
    type?: 'primary' | 'secondary' | 'none';
    onClick(): void;
}

export const Button = ({ text, type = 'primary', onClick }: ButtonProps) => {
    return (
        <Container>
            <button className={type} onClick={onClick}>{text}</button>
        </Container>
    );
}

const Container = styled.div`
display : flex;
flex-direction: column;
align-items: center;
justify-content: center;
img{
    height: 50vh;
}
button{
    border-radius: 5rem;
    height: 50px;
    width: 112px;
    color: black;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    border:none;
    &:hover {
        transform: scale(1.04);
    }
}
button.primary{
    background-color: #1ed760;
}
button.secondary{
    background-color: white;
}
`;