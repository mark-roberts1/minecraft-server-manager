import React from 'react';
import icon from './minecraft-flat-icon.png';
import './App.scss';

const App: React.FC = () => {
  return (
    <div className="App">
      <header>
        <img src={icon} alt=""/>
      </header>
      <div className="app-content">

      </div>
    </div>
  );
}

export default App;
