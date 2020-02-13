import React, { useCallback, useEffect, useState } from 'react';
import './App.scss';
import Header from './components/Header'
import Login from './components/Login'
import ServerList from './components/ServerList'
import TemplateList from './components/TemplateList'
import UserList from './components/UserList';
import { BrowserRouter as Router, Route, Switch, Redirect } from 'react-router-dom'
import Home from './components/Home';
import Logout from './components/Logout';
import LoggedOnUser from './models/LoggedOnUser';
import { User, UserRole } from './models/User';
import Server from './components/Server';
import 'bootstrap/dist/css/bootstrap.min.css';
import UserPage from './components/UserPage';

const App: React.FC = () => {
  const authenticateUser = (authenticatedUser: User, isLoggedIn: boolean) => {
    setUser({...user, isAuthenticated: isLoggedIn});
  }

  console.log = function(){};

  const [user, setUser] = useState<LoggedOnUser>({
    isAuthenticated: false,
    setAuthenticated: authenticateUser,
    userId: 0,
    minecraftUsername: "",
    username: "",
    email: "",
    isLocked: false,
    userRole: UserRole.Normal
  });
  
  return (
    <Router>
    <div className="App">
      <Header {...user}></Header>
      <div className="app-content">
        <Switch>
          {
            !user.isAuthenticated &&
            <Redirect to="/login" {...user} />
          }
          <Route exact path="/">
            <Home/>
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
          <Route path="/logout">
            <Logout {...user} />
          </Route>
        </Switch>
        <Route path="/login">
          <Login {...user} />
        </Route>
        <Route path="/server/:serverId">
          <Server />
        </Route>
        <Route path="/user/:userId">
          <UserPage />
        </Route>
      </div>
    </div>
    </Router>
  );
}

export default App;
