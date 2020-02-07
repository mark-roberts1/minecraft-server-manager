import { ServerProperty } from "./ServerProperty";

export class CreateServerRequest {
	public constructor() {
		this.name = "";
		this.version = "";
		this.description = "";
		this.properties = [];
	}
	
	name: string;
	version: string;
	description: string;
	properties: ServerProperty[];
}
