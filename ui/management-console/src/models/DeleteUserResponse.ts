export class DeleteUserResponse {
	public constructor() {
		this.UserId = 0;
		this.UserDeleted = false;
	}
	
	UserId: number;
	UserDeleted: boolean;
}
