export class ServerCommandResponse {
	public constructor() {
		this.succeeded = false;
		this.log = "";
	}

	succeeded: boolean;
	log: string;
}
