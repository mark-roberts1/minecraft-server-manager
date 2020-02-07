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

const App: React.FC = () => {
  useEffect(() => {
    async function executeAsync(controller: Controller) {
      let request: LoginRequest = {
        Username: "startupAdmin",
        Password: "QWEasdqwe123!"
      };

      console.log(await controller.login(request));
    }

    let controller = new Controller("https://api.marksgamedomain.net", null);

    executeAsync(controller);
  }, []);
  
  return (
    <Router>
    <div className="App">
      <Header></Header>
      <div className="app-content">
        <Switch>
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
