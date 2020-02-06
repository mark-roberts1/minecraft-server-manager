export class CreateUserRequest {
	public constructor() {
		this.Username = "";
		this.Email = "";
		this.Password = "";
		this.InvitationLink = "";
	}

	Username: string;
	Email: string;
	Password: string;
	InvitationLink: string;
}
