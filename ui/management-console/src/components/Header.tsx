import React from 'react';
import './Header.scss';
import icon from '../media/header-icon.png';
import { Link } from 'react-router-dom';

const Header : React.FC = () => {
    return (
        <header>
            <img src={icon} alt=""/>
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
        </header>
    )
}

export default Header;