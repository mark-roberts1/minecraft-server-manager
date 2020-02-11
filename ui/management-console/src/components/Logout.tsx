import React, { useEffect } from 'react';
import api from '../Controller';
import { Redirect } from 'react-router-dom'
import LoggedOnUser from '../models/LoggedOnUser';
import { User } from '../models/User';

const Logout: React.FC<LoggedOnUser> = (user: LoggedOnUser) => {
    useEffect(() => {
        api.logout()
            .then(res => user.setAuthenticated(new User(), false));
    })
    return (
        <div>
            {
                user.isAuthenticated &&
                <Redirect  to='/'/>
            }
        </div>
        
    )
}

export default Logout;