import {ErrorHandler, Injectable, LOCALE_ID, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ExtraOptions, PreloadAllModules, RouterModule } from '@angular/router';
import { MarkdownModule } from 'ngx-markdown';
import { FuseModule } from '@fuse';
import { FuseConfigModule } from '@fuse/services/config';
import { FuseMockApiModule } from '@fuse/lib/mock-api';
import { CoreModule } from 'app/core/core.module';
import { appConfig } from 'app/core/config/app.config';
import { mockApiServices } from 'app/mock-api';
import { LayoutModule } from 'app/layout/layout.module';
import { AppComponent } from 'app/app.component';
import { appRoutes } from 'app/app.routing';
import {MAT_DATE_LOCALE} from "@angular/material/core";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {ErrorInterceptor} from "./modules/error/error.interceptor";
import {registerLocaleData} from "@angular/common";
import {environment} from "../environments/environment";
import fr from '@angular/common/locales/fr';
import * as Sentry from '@sentry/browser';

registerLocaleData(fr);

if (environment.sentry.dsn) {
    Sentry.init({
        dsn: environment.sentry.dsn
    });
}

@Injectable()
export class SentryErrorHandler implements ErrorHandler {
    constructor() {
    }

    handleError(error) {
        Sentry.captureException(error.originalError || error);
        throw error;
    }
}

const routerConfig: ExtraOptions = {
    preloadingStrategy       : PreloadAllModules,
    scrollPositionRestoration: 'enabled'
};

@NgModule({
    declarations: [
        AppComponent
    ],
    imports     : [
        BrowserModule,
        BrowserAnimationsModule,
        RouterModule.forRoot(appRoutes, routerConfig),

        // Fuse, FuseConfig & FuseMockAPI
        FuseModule,
        FuseConfigModule.forRoot(appConfig),
        FuseMockApiModule.forRoot(mockApiServices),

        // Core module of your application
        CoreModule,

        // Layout module of your application
        LayoutModule,

        // 3rd party modules that require global configuration via forRoot
        MarkdownModule.forRoot({})
    ],
    providers: [
        {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
        {provide: LOCALE_ID, useValue: 'fr-FR'},
        {provide: MAT_DATE_LOCALE, useValue: 'fr-FR'},
        {provide: ErrorHandler, useClass: SentryErrorHandler}
    ],
    bootstrap   : [
        AppComponent
    ]
})
export class AppModule
{
}
