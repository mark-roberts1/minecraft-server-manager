import React, { useEffect, useState } from 'react';
import { useParams, Redirect, Link } from 'react-router-dom';
import { User } from '../models/User';
import api from '../Controller';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import locked from '../media/locked.png';
import unlocked from '../media/unlocked.png';
import edit from '../media/edit.png';
import './UserPage.scss';
import { UpdatePasswordRequest } from '../models/UpdatePasswordRequest';
import { UpdateEmailRequest } from '../models/UpdateEmailRequest';
import { UpdateRoleRequest } from '../models/UpdateRoleRequest';

interface UserState {
    user: User;
    didFetchInit: boolean;
    editMode: boolean;
}

interface UserEditState {
    updatePassword: UpdatePasswordRequest;
    updateEmail: UpdateEmailRequest;
    updateRole: UpdateRoleRequest;
    passwordConfirm: string;
    deleteUserConfirm: boolean;
}

const UserPage: React.FC = () => {
    const [userState, setUserState] = useState<UserState>({
        user: new User(),
        didFetchInit: false,
        editMode: false
    });

    const [editUserState, setEditUser] = useState<UserEditState>({
        updatePassword: new UpdatePasswordRequest(),
        updateEmail: new UpdateEmailRequest(),
        updateRole: new UpdateRoleRequest(),
        passwordConfirm: "",
        deleteUserConfirm: false
    });

    let {userId} = useParams();
    
    const doLockToggle = async () => {
        await api.toggleUserLock(userState.user.userId).then(result => {
            setUserState({
                ...userState,
                user: {
                    ...userState.user,
                    isLocked: result.isUserLocked
                }
            });
        });
    }
    
    useEffect(() => {
        if(!userState.didFetchInit){
            api.getUser(parseInt(userId ?? "0")).then(result => {
                setUserState({...userState, didFetchInit: true, user: result});
            })
        }
    });

    return (
        
        <div className="user">
            {
                userState.editMode &&
                <div className="edit-mode">
                </div>
            }
            {
                !userState.editMode &&
                <div className="row p-4">
                    <h2 className="col-lg-10 col-md-10 col-sm-10">{userState.user.username}</h2>
                    <div className="col-lg-2 col-md-2 col-sm-2">
                        <img className="lock-btn float-right" src={userState.user.isLocked ? locked : unlocked} onClick={() => {doLockToggle()}} />
                    </div>
                </div>
            }
        </div>
    )
}

export default UserPage;