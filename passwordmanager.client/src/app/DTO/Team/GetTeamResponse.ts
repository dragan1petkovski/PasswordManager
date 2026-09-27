import { Entity } from "../Entity"

export interface GetTeamResponse extends Entity {
	clientid: string;
    clientname: string;
}
