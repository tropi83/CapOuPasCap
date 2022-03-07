export class User
{
    id: string;
    pseudo: string;


    /**
     * Constructor
     */
    constructor(id, pseudo)
    {
        this.id = id || null;
        this.pseudo = pseudo || null;
    }

}
