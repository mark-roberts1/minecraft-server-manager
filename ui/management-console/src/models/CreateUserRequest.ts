export class CreateUserRequest {
	public constructor() {
		this.username = "";
		this.email = "";
		this.password = "";
		this.invitationLink = "";
	}

	username: string;
	email: string;
	password: string;
	invitationLink: string;
}
