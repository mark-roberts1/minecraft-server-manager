export class AddTemplateRequest {
	public constructor() {
		this.Name = "";
		this.Description = "";
		this.Version = "";
		this.DownloadLink = "";
	}

	Name: string;
	Description: string;
	Version: string;
	DownloadLink: string;
}
