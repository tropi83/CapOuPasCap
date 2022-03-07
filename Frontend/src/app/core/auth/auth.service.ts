import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, switchMap, throwError } from 'rxjs';
import { AuthUtils } from 'app/core/auth/auth.utils';
import { UserService } from 'app/core/user/user.service';
import {environment} from '../../../environments/environment';
import {User} from "../user/user.model";

@Injectable()
export class AuthService
{
    private _authenticated: boolean = false;

    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private _userService: UserService,
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for access token
     */
    set accessToken(token: string)
    {
        localStorage.setItem('accessToken', token);
    }

    get accessToken(): string
    {
        return localStorage.getItem('accessToken') ?? '';
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Sign in
     *
     * @param pseudo
     * @param motdepasse
     */
    signIn(pseudo: string, motdepasse: string ): Observable<any>
    {
        // Throw error, if the user is already logged in
        if ( this._authenticated )
        {
            return throwError('Utilisateur déjà connecté.');
        }

        return this._httpClient.post(environment.backendUrl + 'Utilisateurs/Connexion', { pseudo: pseudo, motdepasse: motdepasse }).pipe(
            switchMap((response: any) => {

                if(response && response.id && response.pseudo){

                    // Store the access token in the local storage
                    this.accessToken = response.id;

                    // Set the authenticated flag to true
                    this._authenticated = true;

                    const user = new User(response.id, response.pseudo)


                    // Store the user on the user service
                    this._userService.user = user;


                }

                // Return a new observable with the response
                return of(response);
            })
        );
    }

    /**
     * Sign in using the access token
     */
    signInUsingToken(): Observable<any>
    {
        // Renew token
        return this._httpClient.get(environment.backendUrl + 'Utilisateurs/Connexion/' + this.accessToken
        ).pipe(
            catchError(() =>

                // Return false
                of(false)
            ),
            switchMap((response: any) => {

                if(response.success) {
                    if (response.data) {
                        // Store the access token in the local storage
                        this.accessToken = response.data.token;

                        // Set the authenticated flag to true
                        this._authenticated = true;

                        // Store the user on the user service
                        this._userService.user = response.data.user;

                        // Return true
                        return of(true);
                    }
                }else{
                    // Return true
                    return of(false);
                }

            })
        );
    }

    /**
     * Sign up
     *
     * @param pseudo
     * @param motdepasse
     */
    signUp(pseudo: string, motdepasse: string ): Observable<any>
    {
        return this._httpClient.post(environment.backendUrl + 'Utilisateurs/Inscription/',
            { pseudo: pseudo, motdepasse: motdepasse }
        );
    }


    /**
     * Sign out
     */
    signOut(): Observable<any>
    {
        // Remove the access token from the local storage
        localStorage.removeItem('accessToken');

        // Set the authenticated flag to false
        this._authenticated = false;

        // Return the observable
        return of(true);
    }

    /**
     * Check the authentication status
     */
    check(): Observable<boolean>
    {
        // Check if the user is logged in
        if ( this._authenticated )
        {
            return of(true);
        }

        // Check the access token availability
        if ( !this.accessToken )
        {
            return of(false);
        }

        // Check the access token expire date
        if ( AuthUtils.isTokenExpired(this.accessToken) )
        {
            return of(false);
        }

        // If the access token exists and it didn't expire, sign in using it
        return this.signInUsingToken()
    }
}
