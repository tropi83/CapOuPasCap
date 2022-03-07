import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import {ChallengeService} from "../../core/challenge/challenge.service";
import {Challenge} from "../../core/challenge/challenge.models";


@Injectable({
    providedIn: 'root'
})
export class ChallengeResolver implements Resolve<any>
{
    /**
     * Constructor
     */
    constructor(private _challengeService: ChallengeService,
                private _router: Router
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Resolver
     *
     * @param route
     * @param state
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Challenge[]>
    {

        return this._challengeService.getAll()
            .pipe(
                catchError((error) => {

                    // Log the error
                    console.error(error);

                    // Get the parent url
                    const parentUrl = state.url.split('/').slice(0, -1).join('/');

                    // Navigate to there
                    this._router.navigateByUrl(parentUrl);

                    // Throw an error
                    return throwError(error);
                })
            );
    }
}


