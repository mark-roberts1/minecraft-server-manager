import React from 'react';
import './Header.scss';
import icon from '../media/header-icon.png';
import { Link } from 'react-router-dom';
import LoggedOnUser from '../models/LoggedOnUser';

const Header : React.FC<LoggedOnUser> = (user: LoggedOnUser) => {
    return (
        <header>
            <Link to="/home">
                <img src={icon} alt=""/>
            </Link>
            {
                user.isAuthenticated &&
                <nav>
                    <ul>
                        <li>
                            <Link to="/servers">SERVERS</Link>
                        </li>
                        <li>
                            <Link to="/templates">TEMPLATES</Link>
                        </li>
                        <li>
                            <Link to="/users">USERS</Link>
                        </li>
                        <li>
                            <Link to="/logout">LOG OUT</Link>
                        </li>
                    </ul>
                </nav>
            }
        </header>
    )
}

export default Header;