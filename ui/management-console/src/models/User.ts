export enum UserRole {
	Normal,
	Admin
}

export class User {
	public constructor() {
		this.userId = 0;
		this.username = "";
		this.minecraftUsername = "";
		this.email = "";
		this.userRole = UserRole.Normal;
		this.isLocked = false;
	}
	
	userId: number;
	username: string;
	minecraftUsername: string;
	email: string;
	userRole: UserRole;
	isLocked: boolean;

}
