export class UpdateTemplateRequest {
	public constructor() {
		this.TemplateId = 0;
		this.Name = "";
		this.Description = "";
		this.Version = "";
		this.DownloadLink = "";
	}

	TemplateId: number;
	Name: string;
	Description: string;
	Version: string;
	DownloadLink: string;
}
