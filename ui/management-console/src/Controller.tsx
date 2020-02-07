import axios, { AxiosRequestConfig } from 'axios'
import { User } from './models/User';
import { UpdatePasswordRequest } from './models/UpdatePasswordRequest';
import { UpdatePasswordResponse } from './models/UpdatePasswordResponse';
import { UpdateEmailRequest } from './models/UpdateEmailRequest';
import { UpdateEmailResponse } from './models/UpdateEmailResponse';
import { InviteUserRequest } from './models/InviteUserRequest';
import { InviteUserResponse } from './models/InviteUserResponse';
import { LinkValidationResponse } from './models/LinkValidationResponse';
import { CreateUserRequest } from './models/CreateUserRequest';
import { CreateUserResponse } from './models/CreateUserResponse';
import { UpdateRoleRequest } from './models/UpdateRoleRequest';
import { UpdateRoleResponse } from './models/UpdateRoleResponse';
import { DeleteUserResponse } from './models/DeleteUserResponse';
import { ToggleUserLockResponse } from './models/ToggleUserLockResponse';
import { ServerInfo } from './models/ServerInfo';
import { CreateServerRequest } from './models/CreateServerRequest';
import { CreateServerResponse } from './models/CreateServerResponse';
import { DeleteServerResponse } from './models/DeleteServerResponse';
import { UpdateServerRequest } from './models/UpdateServerRequest';
import { UpdateServerResponse } from './models/UpdateServerResponse';
import { Template } from './models/Template';
import { AddTemplateRequest } from './models/AddTemplateRequest';
import { AddTemplateResponse } from './models/AddTemplateResponse';
import { UpdateTemplateRequest } from './models/UpdateTemplateRequest';
import { UpdateTemplateResponse } from './models/UpdateTemplateResponse';
import { StartResponse } from './models/StartResponse';
import { ServerCommandRequest } from './models/ServerCommandRequest';
import { ServerCommandResponse } from './models/ServerCommandResponse';
import { LoginRequest } from './models/LoginRequest';
import { TokenResponse } from './models/TokenResponse';
import { ForgotPasswordRequest } from './models/ForgotPasswordRequest';
import { ForgotPasswordResponse } from './models/ForgotPasswordResponse';
import { ResetPasswordRequest } from './models/ResetPasswordRequest';
import { ResetPasswordResponse } from './models/ResetPasswordResponse';

export class Controller {
    public constructor(baseUrl: string, sessionToken: string | null) {
        this.baseUrl = baseUrl;
        this.sessionToken = sessionToken;
    }

    private baseUrl: string;
    private sessionToken: string | null;
    
    private authConfig: AxiosRequestConfig = {
        
        headers: { 
            "SessionToken": this.sessionToken ,
            "Content-Type": "application/json"
        }
    };

    private unauthConfig: AxiosRequestConfig = {
        headers: { 
            "Content-Type": "application/json"
        }
    };

    public async getCurrentUser() : Promise<User> {
        return (await axios.get<User>(`${this.baseUrl}/api/user`, this.authConfig)).data;
    }

    public async listUsers() : Promise<User[]> {
        return (await axios.get<User[]>(`${this.baseUrl}/api/user/list`, this.authConfig)).data;
    }

    public async getUser(userId: number) : Promise<User> {
        return (await axios.get<User>(`${this.baseUrl}/api/user/${userId}/get`, this.authConfig)).data;
    }

    public async updatePassword(userId: number, request: UpdatePasswordRequest) : Promise<UpdatePasswordResponse> {
        return (await axios.put<UpdatePasswordResponse>(`${this.baseUrl}/api/user/${userId}/updatePassword`, request, this.authConfig)).data;
    }

    public async updateEmail(userId: number, request: UpdateEmailRequest) : Promise<UpdateEmailResponse> {
        return (await axios.put<UpdateEmailResponse>(`${this.baseUrl}/api/user/${userId}/updateEmail`, request, this.authConfig)).data;
    }

    public async inviteUser(request: InviteUserRequest) : Promise<InviteUserResponse> {
        return (await axios.post<InviteUserResponse>(`${this.baseUrl}/api/user/invite`, request, this.authConfig)).data;
    }

    public async validateLink(link: string) : Promise<LinkValidationResponse> {
        return (await axios.get<LinkValidationResponse>(`${this.baseUrl}/api/user/${link}/validate`, this.authConfig)).data;
    }

    public async registerUser(request: CreateUserRequest) : Promise<CreateUserResponse> {
        return (await axios.post<CreateUserResponse>(`${this.baseUrl}/api/user/register`, request, this.unauthConfig)).data;
    }

    public async updateUserRole(userId: number, request: UpdateRoleRequest) : Promise<UpdateRoleResponse> {
        return (await axios.put<UpdateRoleResponse>(`${this.baseUrl}/api/user/${userId}/updateRole`, request, this.authConfig)).data;
    }

    public async deleteUser(userId: number) : Promise<DeleteUserResponse> {
        return (await axios.delete<DeleteUserResponse>(`${this.baseUrl}/api/user/${userId}/delete`, this.authConfig)).data;
    }

    public async toggleUserLock(userId: number) : Promise<ToggleUserLockResponse> {
        return (await axios.post<ToggleUserLockResponse>(`${this.baseUrl}/api/user/${userId}/togglelock`, null, this.authConfig)).data;
    }

    public async listServers() : Promise<ServerInfo[]> {
        return (await axios.get<ServerInfo[]>(`${this.baseUrl}/api/server/list`, this.authConfig)).data;
    }

    public async getServer(serverId: number) : Promise<ServerInfo> {
        return (await axios.get<ServerInfo>(`${this.baseUrl}/api/server/${serverId}`, this.authConfig)).data;
    }

    public async createServer(request: CreateServerRequest) : Promise<CreateServerResponse> {
        return (await axios.post<CreateServerResponse>(`${this.baseUrl}/api/server/create`, request, this.authConfig)).data;
    }

    public async deleteServer(serverId: number) : Promise<DeleteServerResponse> {
        return (await axios.delete<DeleteServerResponse>(`${this.baseUrl}/api/server/${serverId}/delete`, this.authConfig)).data;
    }

    public async updateServer(serverId: number, request: UpdateServerRequest) : Promise<UpdateServerResponse> {
        return (await axios.put<UpdateServerResponse>(`${this.baseUrl}/api/server/${serverId}/update`, request, this.authConfig)).data;
    }

    public async listTemplates() : Promise<Template[]> {
        return (await axios.get<Template[]>(`${this.baseUrl}/api/server/templates`, this.authConfig)).data;
    }

    public async addTemplate(request: AddTemplateRequest) : Promise<AddTemplateResponse> {
        return (await axios.post<AddTemplateResponse>(`${this.baseUrl}/api/server/template/add`, request, this.authConfig)).data;
    }

    public async getTemplate(templateId: number) : Promise<Template> {
        return (await axios.get<Template>(`${this.baseUrl}/api/server/template/${templateId}`, this.authConfig)).data;
    }

    public async updateTemplate(templateId: number, request: UpdateTemplateRequest) : Promise<UpdateTemplateResponse> {
        return (await axios.put<UpdateTemplateResponse>(`${this.baseUrl}/api/server/template/${templateId}/update`, request, this.authConfig)).data;
    }

    public async startServer(serverId: number) : Promise<StartResponse> {
        return (await axios.post<StartResponse>(`${this.baseUrl}/api/server/${serverId}/start`, null, this.authConfig)).data;
    }

    public async stopServer(serverId: number) : Promise<boolean> {
        return (await axios.post<boolean>(`${this.baseUrl}/api/server/${serverId}/stop`, null, this.authConfig)).data;
    }

    public async executeCommand(serverId: number, command: ServerCommandRequest) : Promise<ServerCommandResponse> {
        return (await axios.post<ServerCommandResponse>(`${this.baseUrl}/api/server/${serverId}/executecommand`, command, this.authConfig)).data;
    }

    public async login(request: LoginRequest) : Promise<boolean> {
        let response = (await axios.post<TokenResponse>(`${this.baseUrl}/api/auth/token`, request, this.unauthConfig)).data;

        console.log(response);

        return true;
    }

    public async forgotPassword(request: ForgotPasswordRequest) : Promise<ForgotPasswordResponse> {
        return (await axios.post<ForgotPasswordResponse>(`${this.baseUrl}/api/auth/forgotpassword`, request, this.unauthConfig)).data;
    }

    public async resetPassword(request: ResetPasswordRequest) : Promise<ResetPasswordResponse> {
        return (await axios.post<ResetPasswordResponse>(`${this.baseUrl}/api/auth/resetpassword`, request, this.unauthConfig)).data;
    }
}