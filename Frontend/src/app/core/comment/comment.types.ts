import { User } from "../user/user.types";

export interface Comment
{
    id: string;
    description: string;
    dateDeCreation: string;
    createur: User;
}

