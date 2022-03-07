import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map, Observable, of, ReplaySubject, tap, throwError} from 'rxjs';
import { User } from 'app/core/user/user.types';
import {environment} from "../../../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class UserService
{
    private _user: ReplaySubject<User> = new ReplaySubject<User>(1);

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: User)
    {
        // Store the value
        this._user.next(value);
    }

    get user$(): Observable<User>
    {
        return this._user.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): Observable<User>
    {
        let token = localStorage.getItem('accessToken');
        return this._httpClient.get<any>(environment.backendUrl + '/Utilisateurs/Connexion' + token).pipe(
            tap((response) => {
                    if(response.success) {
                        if (response.data) {
                            if(response && response.data && response.data.user){
                                this._user.next(response.data.user);
                            }else{
                                this._user.error('User not found');
                            }
                        }
                    }
            },
            (error) => {
                this._user.error(error);
            })
        );
    }
}
