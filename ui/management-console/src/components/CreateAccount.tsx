import React, { useState } from 'react';
import './CreateAccount.scss';
import { useParams } from 'react-router-dom';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { CreateUserRequest } from '../models/CreateUserRequest';
import api from '../Controller';

interface CreateUserState {
    request: CreateUserRequest;
    passwordConfirm: string;

    passwordMatch: boolean;
    usernameValid: boolean;
    emailValid: boolean;
    passwordValid: boolean;
    linkValid: boolean;
}

const CreateAccount: React.FC = () => {
    const [createState, setCreateState] = useState<CreateUserState>({
        request: new CreateUserRequest(),
        passwordConfirm: "",
        passwordMatch: true,
        usernameValid: true,
        emailValid: true,
        passwordValid: true,
        linkValid: true
    });

    const stateValid = () => {
        let usernameValid = true;
        let emailValid = true;
        let passwordValid = true;
        let linkValid = true;

        usernameValid = createState.request.username.length > 0;
        emailValid = createState.request.email.length > 0;
        passwordValid = createState.request.password.length > 0;
        linkValid = createState.request.invitationLink.length === 32;
        
        setCreateState({...createState, 
            usernameValid: usernameValid,
            emailValid: emailValid,
            passwordValid: passwordValid,
            linkValid: linkValid
        })

        return usernameValid && emailValid && passwordValid && linkValid;
    }

    const createUser = () => {
        if (!stateValid()) return;

        api.registerUser(createState.request)
            .then(res => {
                if (res.userCreated) {
                    window.location.href = "/login";
                }
            })

    }

    const checkPasswordState = (pass: string, confirm: string) => {
        return pass === confirm;
    }

    const handlePasswordChange = (value: string) => {
        let match = checkPasswordState(value, createState.passwordConfirm);

        setCreateState({...createState, passwordMatch: match, request: {...createState.request, password: value}});
    }

    const handleUsernameChange = (value: string) => {
        setCreateState({...createState, usernameValid: true, request: {...createState.request, username: value}});
    }

    const handleEmailChange = (value: string) => {
        setCreateState({...createState, emailValid: true, request: {...createState.request, email: value}});
    }

    const handleLinkChange = (value: string) => {
        setCreateState({...createState, linkValid: true, request: {...createState.request, invitationLink: value}});
    }

    const handlePasswordConfirmChange = (value: string) => {
        let match = checkPasswordState(createState.request.password, value);

        setCreateState({...createState, passwordMatch: match, passwordConfirm: value});
    }

    return (
        <div className="create-account">
            <Modal.Dialog size="lg" className="pt-5">
                <Modal.Header>
                    <h1>Create Account</h1>
                </Modal.Header>
                <Modal.Body>
                    <Form onSubmit={(e: any) => createUser()}>
                        <Form.Control className={`mt-4 ${createState.usernameValid ? "" : "is-invalid"}`} required type="username" placeholder="Username" 
                            value={createState.request.username} 
                            onChange={(e: any) => handleUsernameChange(e.target.value)} />
                        <div className="invalid-feedback">Username is required</div>

                        <Form.Control className={`mt-4 ${createState.emailValid ? "" : "is-invalid"}`} required type="email" placeholder="Email" 
                            value={createState.request.email} 
                            onChange={(e: any) => handleEmailChange(e.target.value)} />
                        <div className="invalid-feedback">Email is required</div>

                        <Form.Control className={`mt-4 ${createState.passwordMatch ? "" : "is-invalid"}`} required type="password" placeholder="Password" 
                            value={createState.request.password} 
                            onChange={(e: any) => handlePasswordChange(e.target.value)} />
                        <div className="invalid-feedback">Passwords do not match</div>
                        
                        <Form.Control className={`mt-4 ${createState.passwordMatch ? "" : "is-invalid"}`} required type="password" placeholder="Confirm password" 
                            value={createState.passwordConfirm} 
                            onChange={(e: any) => handlePasswordConfirmChange(e.target.value)} />
                        <div className="invalid-feedback">Passwords do not match</div>
                        
                        <Form.Control className={`mt-4 ${createState.linkValid ? "" : "is-invalid"}`} required type="text" placeholder="Secret key" 
                            value={createState.request.invitationLink} 
                            onChange={(e: any) => handleLinkChange(e.target.value)} />
                        <div className="invalid-feedback">The secret key should have 32 characters</div>
                    </Form>
                </Modal.Body>
                <Modal.Footer className="d-flex justify-content-center">
                    <Button size="lg" onClick={(e: any) => createUser()}>Create</Button>
                </Modal.Footer>
            </Modal.Dialog>
        </div>
    );
}

export default CreateAccount;