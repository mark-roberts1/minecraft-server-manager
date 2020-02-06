import React from 'react';
import './App.scss';
import Header from './components/Header'
import Login from './components/Login'
import ServerList from './components/ServerList'
import TemplateList from './components/TemplateList'
import UserList from './components/UserList';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom'

const App: React.FC = () => {
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
