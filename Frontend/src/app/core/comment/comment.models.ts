
import { User } from "../user/user.types";

// -----------------------------------------------------------------------------------------------------
// @ Comment
// -----------------------------------------------------------------------------------------------------
export class Comment
{
    id: string;
    description: string;
    dateDeCreation: string;
    createur: User;

    /**
     * Constructor
     */
    constructor(id, description, dateDeCreation, createur)
    {
        this.id = id || null;
        this.description = description || null;
        this.dateDeCreation = dateDeCreation || null;
        this.createur = createur || null;
    }
}
