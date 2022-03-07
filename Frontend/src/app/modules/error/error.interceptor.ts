import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {EMPTY, Observable, throwError} from 'rxjs';
import {catchError, timeout} from 'rxjs/operators';
import {Router} from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private _router: Router,
    ) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(err => {
            if ([0, 500].indexOf(err.status) !== -1) {

                if (err.status === 0){
                    this._router.navigate(['/500']);
                } else{
                    const error = err.error.error || err.statusText;
                    //this._flashService.error('Erreur ' + err.status + ': ' + error);
                }
            }
            return throwError(err);
        }));
    }
}
