import React, { useEffect, useState } from 'react';
import api from '../Controller';
import { User } from '../models/User';
import './UserList.scss';
import './AddModal.scss';
import { Link } from 'react-router-dom';

class UsersState {
    constructor(users: User[]){
        this.users = users;
        this.loaded = false;
    }
    loaded: boolean;
    users: User[];
}
const UserList: React.FC = () => {
    const [usersState, setUsers] = useState(new UsersState([]));
    useEffect(() => {
        if (!usersState.loaded) {
            api.listUsers()
                .then(list => {
                    setUsers({...usersState, users: list, loaded: true});
                });
        }
    });
    return (
        <div className="user-list">
            <div className="list">
                {usersState.users.length > 0 &&
                    usersState.users.map((user, index) => {
                        return (
                            
                            <Link key={user.userId} to={`user/${user.userId}`}>
                                <div id={user.userId.toString()} key={user.userId} className="user">
                                    <div className="user-field">
                                        <span className="username">
                                            Username: {user.username}
                                        </span>
                                    </div>
                                    <div className="user-field">
                                        {user.minecraftUsername != null && 
                                            <span className="mc-username">
                                            MC Username:
                                        </span>}
                                        {user.minecraftUsername == null && 
                                            <span className="mc-username">
                                            MC Username: unset
                                            </span>}
                                    </div>
                                </div>
                            </Link>
                        )
                    })
                    
                }
                
            </div>
        </div>
    )
}

export default UserList;