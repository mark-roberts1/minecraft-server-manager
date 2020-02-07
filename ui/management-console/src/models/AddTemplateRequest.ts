export class AddTemplateRequest {
	public constructor() {
		this.name = "";
		this.description = "";
		this.version = "";
		this.downloadLink = "";
	}

	name: string;
	description: string;
	version: string;
	downloadLink: string;
}
