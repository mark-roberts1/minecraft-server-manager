import { ServerProperty } from "./ServerProperty";

export class UpdateServerRequest {
	public constructor() {
		this.newName = "";
		this.version = "";
		this.description = "";
		this.newProperties = [];
	}

	newName: string;
	version: string;
	description: string;
	newProperties: ServerProperty[];
}
