import {ChangeDetectorRef, Injectable} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
    BehaviorSubject,
    filter,
    map,
    Observable,
    of,
    ReplaySubject,
    Subject,
    switchMap,
    take, takeUntil,
    tap,
    throwError
} from 'rxjs';
import { environment } from "../../../environments/environment";

import {cloneDeep} from "lodash-es";
import {Challenge} from "./challenge.models";
import {UserService} from "../user/user.service";
import {User} from "../user/user.model";

@Injectable({
    providedIn: 'root'
})
export class ChallengeService
{
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    private _challenges: BehaviorSubject<Challenge[] | null> = new BehaviorSubject(null);
    user: User;

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient,
                private _userService: UserService)
    {
        // Subscribe to user changes
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                this.user = user;

            });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for challenges
     */
    get challenges$(): Observable<Challenge[]>
    {
        return this._challenges.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get all challenges order by creation date
     */
    getAll(sort = 'desc'): Observable<Challenge[]>
    {
        return this._httpClient.get<any>(environment.backendUrl + 'Defis/Utilisateur/'  + this.user.id + '/DateCreation/' + sort).pipe(
            tap((challenges) => {
                if(challenges) {
                    this._challenges.next(challenges);
                }
            })
        );
    }

    /**
     * Get all challenges realised
     */
    getAllRealised(): Observable<Challenge[]>
    {
        return this._httpClient.get<any>(environment.backendUrl + 'Defis/Utilisateur/'  + this.user.id + '/DateCreation/desc').pipe(
            tap((challenges) => {
                if(challenges) {
                    challenges = challenges.filter(challenge => challenge.realise && challenge.realise === true);
                    this._challenges.next(challenges);

                    return challenges;
                }
            })
        );
    }

    /**
     * Get all challenges order by like
     */
    getAllByLike(sort = 'desc'): Observable<Challenge[]>
    {
        return this._httpClient.get<any>(environment.backendUrl + 'Defis/Utilisateur/'  + this.user.id + '/Like/' + sort).pipe(
            tap((challenges) => {
                if(challenges) {
                    this._challenges.next(challenges);

                    return challenges;
                }
            })
        );
    }


    /**
     * Create challenge
     */
    createChallenge(name: string, description :string): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.post<any>(environment.backendUrl + 'Defis',
                    {
                        nom         : name,
                        description  : description,
                        utilisateurId  : this.user.id,
                        }
                ).pipe(
                    map((response) => {

                        if(response) {
                            // Update the challenges with the new challenge
                            this._challenges.next([response, ...challenges]);

                            // Return the new challenge from observable
                            return response;
                        }
                    })
                ))
        );
    }

    /**
     * Like challenge
     */
    likeChallenge(challenge): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.post<any>(environment.backendUrl + 'Likes',
                    {
                        defiId  : challenge.id,
                        utilisateurId  : this.user.id,
                        }
                ).pipe(
                    map((response) => {
                        if(response) {
                            // Get the challenges value
                            const challenges1 = this._challenges.value;

                            // Find the index of the updated challenge
                            const indexChallenge = challenges1.findIndex(item => item.id === challenge.id);

                            // Update the like state
                            challenges1[indexChallenge].like = false;
                            challenges1[indexChallenge].likeId = response.id;

                            // Update the challenge
                            this._challenges.next(challenges1);

                            return challenges1;
                        }else{
                            return response;
                        }
                    })
                ))
        );
    }

    /**
     * UnLike challenge
     */
    unlikeChallenge(challenge): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.delete<any>(environment.backendUrl + 'Likes/' + challenge.likeId,
                ).pipe(
                    map((response) => {

                        if(response) {
                            // Get the challenges value
                            const challenges1 = this._challenges.value;

                            // Find the index of the updated challenge
                            const indexChallenge = challenges1.findIndex(item => item.id === challenge.id);

                            // Update the like state
                            challenges1[indexChallenge].like = true;

                            // Update the challenge
                            this._challenges.next(challenges1);

                            return challenges1;
                        }else{
                            return response;
                        }
                    })
                ))
        );
    }



    /**
     * Hide/UnHide challenge
     */
    toggleHideChallenge(challenge, hideState: boolean): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.put<any>(environment.backendUrl + 'Defis/' + challenge.id + '/Utilisateur/' + this.user.id ,
                    {
                        cache  : hideState
                    }
                ).pipe(
                    map((response) => {
                        if(response) {
                            // Get the challenges value
                            const challenges1 = this._challenges.value;

                            // Find the index of the updated challenge
                            const indexChallenge = challenges1.findIndex(item => item.id === challenge.id);

                            // Update the hide state
                            challenges1[indexChallenge].cache = true;

                            // Update the challenge
                            this._challenges.next(challenges1);

                            return challenges1;
                        }else{
                            return response;
                        }
                    })
                ))
        );
    }


    /**
     * Realised challenge
     */
    realisedChallenge(challenge): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.post<any>(environment.backendUrl + 'DefisRealises',
                    {
                        defiId  : challenge.id,
                        utilisateurId  : this.user.id,
                    }
                ).pipe(
                    map((response) => {
                        if(response) {
                            // Get the challenges value
                            const challenges1 = this._challenges.value;

                            // Find the index of the updated challenge
                            const indexChallenge = challenges1.findIndex(item => item.id === challenge.id);

                            // Update the comments
                            challenges1[indexChallenge].realise = false;
                            challenges1[indexChallenge].realiseId = response.id;

                            // Update the challenge
                            this._challenges.next(challenges1);

                            return challenges1;
                        }else{
                            return response;
                        }
                    })
                ))
        );
    }

    /**
     * UnRealised challenge
     */
    unrealisedChallenge(challenge): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.delete<any>(environment.backendUrl + 'DefisRealises/' + challenge.realiseId,
                ).pipe(
                    map((response) => {

                        if(response) {
                            // Get the challenges value
                            const challenges1 = this._challenges.value;

                            // Find the index of the updated challenge
                            const indexChallenge = challenges1.findIndex(item => item.id === challenge.id);

                            // Update the like state
                            challenges1[indexChallenge].realise = true;

                            // Update the challenge
                            this._challenges.next(challenges1);

                            return challenges1;
                        }else{
                            return response;
                        }
                    })
                ))
        );
    }

    /**
     * Create comment
     */
    createComment(text: string, challengeId :string): Observable<Challenge>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges =>
                this._httpClient.post<any>(environment.backendUrl + 'Commentaires',
                    {
                        texte          : text,
                        defiId         : challengeId,
                        utilisateurId  : this.user.id,
                        }
                ).pipe(
                    map((response) => {

                        if(response) {

                            // Get the challenges value
                            const challenges1 = this._challenges.value;

                            // Find the index of the updated challenge
                            const indexChallenge = challenges1.findIndex(item => item.id === challengeId);

                            // Update the comments
                            challenges1[indexChallenge].commentaires.push(response);

                            // Update the challenge
                            this._challenges.next(challenges1);

                            // Update the challenges with the new challenge
                            this._challenges.next([response, ...challenges]);

                            // Return the new challenge from observable
                            return response;
                        }
                    })
                ))
        );
    }


    /**
     * Delete challenge
     *
     * @param challenge
     */
    deleteChallenge(challenge): Observable<boolean>
    {
        return this.challenges$.pipe(
            take(1),
            switchMap(challenges => this._httpClient.delete(environment.backendUrl + 'Defis/' + challenge.id, ).pipe(
                map((isDeleted: boolean) => {

                    // Find the index of the deleted challenge
                    const index = challenges.findIndex(item => item.id === challenge.id);

                    // Delete the challenge
                    challenges.splice(index, 1);

                    // Update the challenges
                    this._challenges.next(challenges);

                    // Return the deleted status
                    return isDeleted;
                })
            ))
        );
    }

}
