import React, { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import { User, UserRole } from '../models/User';
import { ServerInfo, ServerStatus } from '../models/ServerInfo';
import { Template } from '../models/Template';
import api from '../Controller';
import './Home.scss';
import { InviteUserRequest } from '../models/InviteUserRequest';

class HomeState {
    constructor() {
        this.currentUser = new User();
        this.servers = [];
        this.templates = [];
        this.users = [];
        this.loaded = false;
    }

    currentUser: User;
    servers: ServerInfo[];
    templates: Template[];
    users: User[];
    loaded: boolean;
}

const Home: React.FC = () => {
    const [homeState, setHomeState] = useState(new HomeState());
    const [inviteUserState, setInviteUserState] = useState(new InviteUserRequest());

    useEffect(() => {
        if (!homeState.loaded) {
            api.getCurrentUser()
                .then(user => {
                    api.listServers()
                        .then(serverList => {
                            api.listTemplates()
                                .then(templateList => {
                                    if (user.userRole === UserRole.Admin){
                                        api.listUsers()
                                            .then(list => {
                                                setHomeState({users: list, currentUser: user, servers: serverList, templates: templateList, loaded: true});
                                            });
                                    }
                                    else {
                                        setHomeState({...homeState, currentUser: user, servers: serverList, templates: templateList, loaded: true});
                                    }
                                });
                        });
                });
        }
    })

    const countUserType = (role: UserRole) => {
        let count = 0;

        for (let i = 0; i < homeState.users.length; i++) {
            if (homeState.users[i].userRole === role) {
                count++;
            }
        }

        return count;
    }

    const countLockedUsers = () => {
        let count = 0;

        for (let i = 0; i < homeState.users.length; i++) {
            if (homeState.users[i].isLocked) {
                count++;
            }
        }

        return count;
    }

    const countTemplatesWhereUsed = (used: boolean) => {
        let count = 0;

        for (let i = 0; i < homeState.templates.length; i++) {
            if (isVersionUsed(homeState.templates[i].version) === used) {
                count++;
            }
        }

        return count;
    }

    const isVersionUsed = (version: string) => {
        for (let i = 0; i < homeState.servers.length; i++) {
            if (homeState.servers[i].version === version) {
                return true;
            }
        }

        return false;
    }

    const countServersForStatus = (status: ServerStatus) => {
        let count = 0;

        for (let i = 0; i < homeState.servers.length; i++) {
            if (homeState.servers[i].status === status) {
                count++;
            }
        }

        return count;
    }

    const inviteUser = () => {
        api.inviteUser(inviteUserState)
            .then(res => {
                if (res.userInvited) {
                    setInviteUserState(new InviteUserRequest());
                }
            });
    }

    return (
        <div className="home-page">
            <div className="text-center row pt-5">
                <h2 className="col-md-12 col-lg-12 col-sm-12">Welcome, {homeState.currentUser.username}!</h2>
            </div>
            <div className="row p-3">
                <div className="text-center user-metrics col-md-6 col-lg-6 col-sm-12 pr-3">
                    <h4>Users</h4>
                    <Table responsive striped hover variant="dark">
                        <thead>
                            <tr>
                                <th>Total</th>
                                <th>Admins</th>
                                <th>Normal</th>
                                <th>Locked</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>{homeState.users.length}</th>
                                <th>{countUserType(UserRole.Admin)}</th>
                                <th>{countUserType(UserRole.Normal)}</th>
                                <th>{countLockedUsers()}</th>
                            </tr>
                        </tbody>
                    </Table>
                </div>
                <div className="text-center server-metrics col-md-6 col-lg-6 col-sm-12 pl-3">
                    <h4>Servers</h4>
                    <Table responsive striped hover variant="dark">
                        <thead>
                            <tr>
                                <th>Total</th>
                                <th>Running</th>
                                <th>Stopped</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>{homeState.servers.length}</th>
                                <th>{countServersForStatus(ServerStatus.Started)}</th>
                                <th>{countServersForStatus(ServerStatus.Stopped)}</th>
                            </tr>
                        </tbody>
                    </Table>
                </div>
            </div>
            <div className="row p-3">
                <div className="text-center user-invite col-md-6 col-lg-6 col-sm-12 pr-3">
                    <h4>Invite a user</h4>
                    <div className="input-group pt-4">
                        <input type="text" 
                            className="form-control" 
                            placeholder="Email address" 
                            aria-label="Email address" 
                            value={inviteUserState.emailAddress} 
                            onChange={e => setInviteUserState({ emailAddress: e.target.value })}
                            onKeyUp={e => {
                                if (e.key === "Enter") {
                                    inviteUser();
                                }
                            }} />
                        <div className="input-group-append">
                            <Button variant="dark" onClick={(e: any) => inviteUser()}>Invite</Button>
                        </div>
                    </div>
                </div>
                <div className="text-center template-metrics col-md-6 col-lg-6 col-sm-12 pl-3">
                    <h4>Templates</h4>
                    <Table responsive striped hover variant="dark">
                        <thead>
                            <tr>
                                <th>Total</th>
                                <th>Used</th>
                                <th>Unused</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <th>{homeState.templates.length}</th>
                                <th>{countTemplatesWhereUsed(true)}</th>
                                <th>{countTemplatesWhereUsed(false)}</th>
                            </tr>
                        </tbody>
                    </Table>
                </div>
            </div>
        </div>
    )
};

export default Home;