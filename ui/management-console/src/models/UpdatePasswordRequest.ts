export class UpdatePasswordRequest {
	public constructor() {
		this.originalPassword = "";
		this.newPassword = "";
	}

	originalPassword: string;
	newPassword: string;
}
