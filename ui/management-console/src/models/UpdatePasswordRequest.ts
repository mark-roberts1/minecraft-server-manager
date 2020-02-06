export class UpdatePasswordRequest {
	public constructor() {
		this.OriginalPassword = "";
		this.NewPassword = "";
	}

	OriginalPassword: string;
	NewPassword: string;
}
