import { UserRole } from "./User"

export class UpdateRoleRequest {
	public constructor() {
		this.userRole = UserRole.Normal;
	}

	userRole: UserRole;
}
