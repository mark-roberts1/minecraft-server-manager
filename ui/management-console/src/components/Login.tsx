import React, { useState, useEffect } from 'react';
import './Login.scss';
import { LoginRequest } from '../models/LoginRequest';
import { Link, Redirect } from 'react-router-dom';
import api from '../Controller'
import LoggedOnUser from '../models/LoggedOnUser';

const Login: React.FC<LoggedOnUser> = (user: LoggedOnUser) => {
    const logonUser = user;
    const [login, setLogin] = useState(new LoginRequest());
    
    let loginChecked: boolean = false;

    const loginApi = async (e: any) => {
        await api.login(login);
        window.location.reload();
    }

    useEffect(() => {
        if (!loginChecked) {
            api.getCurrentUser()
                .then(user => {
                    // loginChecked = true;
                    logonUser.setAuthenticated(user, true);
                })
                .catch(ex => {
                    console.log("user must authenticate");
                    // loginChecked = true;
                });
        }
    })

    return (
        <div className="login">
            {
                logonUser.isAuthenticated &&
                <Redirect to="/" />
            }
            <div className="login-form">
                <h2>Login</h2>
                <div className="input-wrapper">
                    <div className="field-wrapper">
                        <input type="textbox" className="field" placeholder="Username" name="username" 
                            value={login.username} onChange={e => setLogin({...login, username: e.target.value})} required />
                        <label htmlFor="username" className="field-label">Username</label>
                    </div>
                    <div className="field-wrapper">
                        <input type="password" className="field" placeholder="Password" name="password" 
                            value={login.password} onChange={e => setLogin({...login, password: e.target.value})} required />
                        <label htmlFor="password" className="field-label">Password</label>
                    </div>
                </div>
                <button itemType="submit" className="login-btn" onClick={e => loginApi(e)}>Login</button>
                <br/>
                <Link className="link" to="/forgotpassword">Forgot Password</Link>|
                <Link className="link" to="/forgotusername">Forgot Username</Link>
            </div>
        </div>
    )
}

export default Login;