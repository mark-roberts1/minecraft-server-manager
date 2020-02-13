import React, { useEffect, useState } from 'react';
import { useParams, Redirect, Link } from 'react-router-dom';
import { User } from '../models/User';
import api from '../Controller';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import { ToggleUserLockResponse } from '../models/ToggleUserLockResponse';

const UserPage: React.FC = () => {
    class UserState{
        constructor(){
            this.User = new User();
            this.didFetchInit = false;
        }
        User: User;
        didFetchInit: boolean;
    }
    let isUserLocked = false;
    const doLockToggle = async () => {
        await api.toggleUserLock(userState.User.userId).then(result => {
            isUserLocked = result.isUserLocked;
        });
    }
    const [userState, setUserState] = useState(new UserState());
    let {userId} = useParams();
    let isUserAuthenticatedOrSelf : boolean = false;
    useEffect(() => {
        if(!userState.didFetchInit){
            api.getCurrentUser().then(result => {
                isUserAuthenticatedOrSelf = (result.userRole == 1 || result.userId == parseInt(userId ?? "0"));
            })
            api.getUser(parseInt(userId ?? "0")).then(result => {
                setUserState({didFetchInit: true, User: result});
                isUserLocked = result.isLocked;
            })
        }
    })
    return (
        
        <div className="user">
            
            <h2 className="col-lg-12 col-md-12 col-sm-12">{userState.User.username}</h2>
            <div className="col-lg-12 col-md-12 col-sm-12">
                {!isUserLocked &&
                <Button variant="danger" onClick={() => {doLockToggle()}}>Lock User</Button>}
                {isUserLocked &&
                <Button variant="danger" onClick={() => {doLockToggle()}}>Unlock User</Button>}
                
            </div>
            <Table responsive hover variant="light" className="pt-3">
            <tbody>
                <tr key="ID">
                    <td>ID</td> 
                    <td>{userState.User.userId}</td>
                </tr>
                <tr key="MCUser">
                    <td>MC Username</td> 
                    <td>{userState.User.minecraftUsername}</td>
                    <td><Link key={userState.User.userId} to={`${userState.User.userId}/updatemcusername`} ><Button className="col-lg-7 col-md-7 col-sm-7">Update MC Username</Button></Link></td>
                </tr>
                <tr key="Email">
                    <td>Email</td> 
                    <td>{userState.User.email}</td>
                    <td><Link key={userState.User.userId} to={`${userState.User.userId}/updateemail`} ><Button className="col-lg-7 col-md-7 col-sm-7">Update Email</Button></Link></td>
                </tr>
            </tbody>
            </Table>
        </div>
    )
}

export default UserPage;