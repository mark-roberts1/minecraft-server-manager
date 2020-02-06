export class ResetPasswordRequest {
	public constructor() {
		this.Link = "";
		this.NewPassword = "";
	}
	
	Link: string;
	NewPassword: string;
}
