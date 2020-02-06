export class User {
	public constructor() {
		this.UserId = 0;
		this.Username = "";
		this.MinecraftUsername = "";
		this.Email = "";
		this.UserRole = UserRole.Normal;
		this.IsLocked = false;
	}

	UserId: number;
	Username: string;
	MinecraftUsername: string;
	Email: string;
	UserRole: UserRole;
	IsLocked: boolean;

}

export enum UserRole {
	Normal,
	Admin
}