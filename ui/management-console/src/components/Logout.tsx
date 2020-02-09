import React, { useEffect } from 'react';
import api from '../Controller';
import { Redirect } from 'react-router-dom'
const Logoutapi = async () => {let logout = await api.logout();}
const Logout: React.FC = () => {
    useEffect(() => {
        Logoutapi();
        console.log("yeet");
    })
    return (
        <Redirect  to='/Home'/>
    )
}

export default Logout;