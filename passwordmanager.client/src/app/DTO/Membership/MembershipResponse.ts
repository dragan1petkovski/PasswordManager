import { MemberItem } from "./MemberItem";

export interface MembershipResponse
{
	id: string,
	members: MemberItem[]
	nonMembers: MemberItem[]
}