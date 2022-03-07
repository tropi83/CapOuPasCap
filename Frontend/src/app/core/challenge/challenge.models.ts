import { User } from "../user/user.types";
import { Comment } from "../comment/comment.types";

// -----------------------------------------------------------------------------------------------------
// @ Challenge
// -----------------------------------------------------------------------------------------------------
export class Challenge
{
    id: string;
    nom: string;
    description: string;
    dateDeCreation: string;
    createur: User;
    commentaires: Comment[];
    like: boolean;
    likeId: string;
    nbLike: number;
    realise: boolean;
    realiseId: string;
    nbRealise: number;
    cache: boolean;


    /**
     * Constructor
     */
    constructor(id, nom, description, dateDeCreation, createur, commentaires, like, likeId, nbLike, realise, realiseId, nbRealise, cache )
    {
        this.id = id || null;
        this.nom = nom || null;
        this.description = description || null;
        this.dateDeCreation = dateDeCreation || null;
        this.createur = createur || null;
        this.commentaires = commentaires || null;
        this.like  = like || null;
        this.likeId = likeId || null;
        this.nbLike = nbLike || null;
        this.realise = realise || null;
        this.realiseId = realiseId || null;
        this.nbRealise = nbRealise || null;
        this.cache = cache || null;
    }
}
