import { ServerProperty } from "./ServerProperty";

export class Template {
	public constructor() {
		this.templateId = 0;
		this.name = "";
		this.description = "";
		this.version = "";
		this.downloadLink = "";
		this.properties = [];
	}

	templateId: number;
	name: string;
	description: string;
	version: string;
	downloadLink: string;
	properties: ServerProperty[];
}
