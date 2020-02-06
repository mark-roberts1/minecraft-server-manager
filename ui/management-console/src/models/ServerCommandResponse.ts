export class ServerCommandResponse {
	public constructor() {
		this.Succeeded = false;
		this.Log = "";
	}

	Succeeded: boolean;
	Log: string;
}
