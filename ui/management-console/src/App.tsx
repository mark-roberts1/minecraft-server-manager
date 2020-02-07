import React, { useCallback, useEffect } from 'react';
import './App.scss';
import Header from './components/Header'
import Login from './components/Login'
import ServerList from './components/ServerList'
import TemplateList from './components/TemplateList'
import UserList from './components/UserList';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'
import { Controller } from './Controller';
import { LoginRequest } from './models/LoginRequest';
import Home from './components/Home';

const App: React.FC = () => {
  
  return (
    <Router>
    <div className="App">
      <Header></Header>
      <div className="app-content">
        <Switch>
          <Route path="/home">
            <Home/>
          </Route>
          <Route path="/login">
            <Login/>
          </Route>
          <Route path="/servers">
            <ServerList/>
          </Route>
          <Route path="/templates">
            <TemplateList/>
          </Route>
          <Route path="/users">
            <UserList/>
          </Route>
        </Switch>
      </div>
    </div>
    </Router>
  );
}

export default App;
