import { ServerProperty } from "./ServerProperty";

export class UpdateServerRequest {
	public constructor() {
		this.NewName = "";
		this.Version = "";
		this.Description = "";
		this.NewProperties = [];
	}

	NewName: string;
	Version: string;
	Description: string;
	NewProperties: ServerProperty[];
}
