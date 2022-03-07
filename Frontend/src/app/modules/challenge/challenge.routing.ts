import { Route } from '@angular/router';
import { ChallengeComponent } from 'app/modules/challenge/challenge.component';
import {ChallengeResolver} from "./challenge.resolvers";

export const challengeRoutes: Route[] = [
    {
        path     : '',
        component: ChallengeComponent,
        resolve  : {
            challenges: ChallengeResolver,
        },
    }
];
