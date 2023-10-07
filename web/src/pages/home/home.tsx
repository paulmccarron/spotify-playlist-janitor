import React from 'react';
import logo from '../../logo.svg';
import './home.css';

export const Home = () => {
  return (
    <div className="Home">
      <header className="Home-header">
        <img src={logo} className="Home-logo" alt="logo" />
        <p>
          Edit <code>src/Home.tsx</code> and save to reload.
        </p>
        <a
          className="Home-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
        <p>
          API URL: {process.env.REACT_APP_API_URL}
        </p>
      </header>
    </div>
  );
}
