import { ServerProperty } from "./ServerProperty";

export class ServerInfo {
	public constructor() {
		this.ServerId = 0;
		this.Name = "";
		this.Version = "";
		this.Description = "";
		this.Status = ServerStatus.Stopped;
		this.Properties = [];
	}

	ServerId: number;
	Name: string;
	Version: string;
	Description: string;
	Status: ServerStatus;
	Properties: ServerProperty[];
}

export enum ServerStatus
{
	Stopped,
	Started
}