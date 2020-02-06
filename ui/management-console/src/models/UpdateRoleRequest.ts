import { UserRole } from "./User"

export class UpdateRoleRequest {
	public constructor() {
		this.UserRole = UserRole.Normal;
	}

	UserRole: UserRole;
}
