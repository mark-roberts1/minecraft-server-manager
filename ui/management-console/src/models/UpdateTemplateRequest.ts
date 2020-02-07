export class UpdateTemplateRequest {
	public constructor() {
		this.templateId = 0;
		this.name = "";
		this.description = "";
		this.version = "";
		this.downloadLink = "";
	}

	templateId: number;
	name: string;
	description: string;
	version: string;
	downloadLink: string;
}
