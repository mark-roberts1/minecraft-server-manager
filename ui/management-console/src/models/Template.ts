import { ServerProperty } from "./ServerProperty";

export class Template {
	public constructor() {
		this.TemplateId = 0;
		this.Name = "";
		this.Description = "";
		this.Version = "";
		this.DownloadLink = "";
		this.Properties = [];
	}

	TemplateId: number;
	Name: string;
	Description: string;
	Version: string;
	DownloadLink: string;
	Properties: ServerProperty[];
}
