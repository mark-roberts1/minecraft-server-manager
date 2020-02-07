export class DeleteUserResponse {
	public constructor() {
		this.userId = 0;
		this.userDeleted = false;
	}
	
	userId: number;
	userDeleted: boolean;
}
