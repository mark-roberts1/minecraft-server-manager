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
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';

interface UserState {
    user: User;
    didFetchInit: boolean;
    editMode: boolean;
}

interface UserEditState {
    updatePassword: UpdatePasswordRequest;
    updateEmail: UpdateEmailRequest;
    updateRole: UpdateRoleRequest;
    emailConfirm: string;
    emailMatch: boolean;
    emailValid: boolean;
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
        deleteUserConfirm: false,
        emailConfirm: "",
        emailMatch: true,
        emailValid: false
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

    const stringsMatch = (string1: string, string2: string) => {
        return string1 === string2;
    }

    const handleEmailChange = (value: string) => {
        let emailMatch = stringsMatch(value, editUserState.emailConfirm);

        setEditUser({
            ...editUserState,
            updateEmail: {
                newEmail: value
            },
            emailMatch: emailMatch,
            emailValid: value.length > 0
        });
    }

    const handleEmailConfirmChange = (value: string) => {
        let emailMatch = stringsMatch(value, editUserState.updateEmail.newEmail);

        setEditUser({
            ...editUserState,
            emailConfirm: value,
            emailMatch: emailMatch,
            emailValid: value.length > 0
        });
    }
    
    useEffect(() => {
        if(!userState.didFetchInit){
            api.getUser(parseInt(userId ?? "0")).then(result => {
                setUserState({...userState, didFetchInit: true, user: result});
                setEditUser({
                    ...editUserState, 
                    updateEmail: { 
                        newEmail: result.email 
                    },
                    emailConfirm: result.email,
                    emailValid: true,
                    emailMatch: true
                });
            })
        }
    });

    const submitEmailChange = () => {
        if (!editUserState.emailValid || !editUserState.emailMatch) return;

        api.updateEmail(userState.user.userId, editUserState.updateEmail)
            .then(res => {
                if (res.emailUpdated) {
                    alert("Email updated");
                }
            })
    }
    return (
        
        <div className="user">
            <div className="row p-4">
                <h2 className="col-lg-10 col-md-10 col-sm-10">{userState.user.username}</h2>
                <div className="col-lg-2 col-md-2 col-sm-2">
                    <img className="edit-btn float-right" src={edit} onClick={() => setUserState({...userState, editMode: true})} />
                </div>
            </div>
            <Modal size="xl" show={userState.editMode} onHide={() => setUserState({...userState, editMode: false})}>
                <Modal.Header closeButton>
                    <Modal.Title>Edit {userState.user.username}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Table variant="light" striped responsive>
                        <tbody>
                            <tr>
                                <td className="align-middle">{userState.user.isLocked ? "Unlock User" : "Lock User"}</td>
                                <td className="align-middle"></td>
                                <td className="align-middle"></td>
                                <td className="align-middle">
                                    <img className="lock-btn float-right" 
                                        src={userState.user.isLocked ? locked : unlocked} 
                                        onClick={() => {doLockToggle()}} />
                                </td>
                            </tr>
                            <tr>
                                <td className="align-middle">Update Email</td>
                                <td>
                                    <Form.Control 
                                        className={`${editUserState.emailMatch && editUserState.emailValid ? "" : "is-invalid"}`} 
                                        required type="email" placeholder="Email" 
                                        value={editUserState.updateEmail.newEmail} 
                                        onChange={(e: any) => handleEmailChange(e.target.value)} />
                                    <div className="invalid-feedback">
                                        {editUserState.emailMatch ? "Email is required" : "Email does not match confirmation"}
                                    </div>
                                </td>
                                <td>
                                    <Form.Control 
                                        className={`${editUserState.emailMatch && editUserState.emailValid ? "" : "is-invalid"}`} 
                                        required type="email" placeholder="Confirm Email" 
                                        value={editUserState.emailConfirm} 
                                        onChange={(e: any) => handleEmailConfirmChange(e.target.value)} />
                                    <div className="invalid-feedback">
                                        {editUserState.emailMatch ? "Email is required" : "Email does not match confirmation"}
                                    </div>
                                </td>
                                <td className="align-middle">
                                    <Button className="float-right" 
                                        disabled={!editUserState.emailMatch || !editUserState.emailValid} 
                                        onClick={() => submitEmailChange()}>Update</Button>
                                </td>
                            </tr>
                            
                        </tbody>
                    </Table>
                </Modal.Body>
            </Modal>
        </div>
    )
}

export default UserPage;