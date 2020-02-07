import { ServerProperty } from "./ServerProperty";

export class ServerInfo {
	public constructor() {
		this.serverId = 0;
		this.name = "";
		this.version = "";
		this.description = "";
		this.status = ServerStatus.Stopped;
		this.properties = [];
	}

	serverId: number;
	name: string;
	version: string;
	description: string;
	status: ServerStatus;
	properties: ServerProperty[];
}

export enum ServerStatus
{
	Stopped,
	Started
}