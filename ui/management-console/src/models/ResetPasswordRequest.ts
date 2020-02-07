export class ResetPasswordRequest {
	public constructor() {
		this.link = "";
		this.newPassword = "";
	}
	
	link: string;
	newPassword: string;
}
