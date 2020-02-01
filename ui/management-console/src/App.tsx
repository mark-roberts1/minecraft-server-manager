import React from 'react';
import icon from './minecraft-flat-icon.png';
import './App.scss';
import { Button } from './Components/Button';

const App: React.FC = () => {
  return (
    <div className="App">
      <header>
      <div className="header-buttons">
          <span className="button">
          <Button type="" onClick="#" buttonStyle="btn-header" buttonSize="">
            Server List 
          </Button>
          </span>
          <span className="button">
          <Button type="" onClick="#" buttonStyle="btn-header" buttonSize="">
            User List 
          </Button>
          </span>
          <span className="button">
          <Button type="" onClick="#" buttonStyle="btn-header" buttonSize="">
            Profile
          </Button>
          </span>
          <span className="button">
          <Button type="" onClick="#" buttonStyle="btn-header" buttonSize="">
            Login 
          </Button>
          </span>
        </div>
      <img src={icon} alt=""/>
        
      </header>
      <div className="app-content">
        <div className="right-panel">

        </div>
      </div>
    </div>
  );
}

export default App;
