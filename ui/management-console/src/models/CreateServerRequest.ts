import { ServerProperty } from "./ServerProperty";

export class CreateServerRequest {
	public constructor() {
		this.Name = "";
		this.Version = "";
		this.Description = "";
		this.Properties = [];
	}
	
	Name: string;
	Version: string;
	Description: string;
	Properties: ServerProperty[];
}
