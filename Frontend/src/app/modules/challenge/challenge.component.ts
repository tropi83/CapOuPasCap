import {ChangeDetectionStrategy, ChangeDetectorRef, Component, ViewChild, ViewEncapsulation} from '@angular/core';
import {UserService} from "../../core/user/user.service";
import {Subject, takeUntil} from "rxjs";
import {User} from "../../core/user/user.model";
import {FormBuilder, FormGroup, NgForm, Validators} from "@angular/forms";
import {ChallengeService} from "../../core/challenge/challenge.service";
import {Challenge} from "../../core/challenge/challenge.models";
import {FuseAlertType} from "../../../@fuse/components/alert";
import {FuseConfirmationService} from "../../../@fuse/services/confirmation";

@Component({
    selector       : 'challenge',
    templateUrl    : './challenge.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChallengeComponent
{

    @ViewChild('challengeNgForm') challengeNgForm: NgForm;
    @ViewChild('commentNgForm') commentNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type   : 'success',
        message: ''
    };
    showAlert: boolean = false;

    alertComment: { type: FuseAlertType; message: string } = {
        type   : 'success',
        message: ''
    };
    showAlertComment: boolean = false;

    alertDeleteChallenge: { type: FuseAlertType; message: string } = {
        type   : 'success',
        message: ''
    };
    showAlertDeleteChallenge: boolean = false;

    private _unsubscribeAll: Subject<any> = new Subject<any>();
    user: User;
    challengeForm: FormGroup;
    challenges: Challenge[];

    commentForm: FormGroup;
    selectedChallenge: Challenge;

    selectedChallengeMode = "all";
    createMode: boolean = false;

    /**
     * Constructor
     */
    constructor(private _changeDetectorRef: ChangeDetectorRef,
                private _userService: UserService,
                private _formBuilder: FormBuilder,
                private _challengeService: ChallengeService,
                private _fuseConfirmationService: FuseConfirmationService
    )
    {
        // Subscribe to user changes
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user: User) => {
                this.user = user;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Subscribe to challenges changes
        this._challengeService.challenges$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((challenges) => {
                this.challenges = challenges;
                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Create the challenge form
        this.challengeForm = this._formBuilder.group({
                name      : ['', [Validators.required, Validators.minLength(2), Validators.maxLength(63)]],
                description  : ['', [Validators.required, Validators.minLength(4), Validators.maxLength(139)]]
            }
        );

        // Create the comment form
        this.commentForm = this._formBuilder.group({
                texte  : ['', [Validators.required, Validators.minLength(4), Validators.maxLength(139)]]
            }
        );
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Create challenge
     */
    createChallenge(): void
    {
        // Return if the form is invalid
        if ( this.challengeForm.invalid )
        {
            return;
        }

        // Disable the form
        this.challengeForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Add Challenge
        this._challengeService.createChallenge(this.challengeForm.controls['name'].value, this.challengeForm.controls['description'].value)
            .subscribe(
                () => {

                    this.alert = {
                        type   : 'success',
                        message: 'Défi crée avec succès.',

                    };

                    this.challengeForm.reset();
                },
                (error) => {

                    let errorMessage = error.error || error.errors.toString() || error.statusText || 'Une erreur est survenue. Veuillez re-essayer plus tard.';
                    this.alert = {
                        type   : 'error',
                        message: errorMessage
                    };

                },
                () => {
                    // Re-enable the form
                    this.challengeForm.enable();

                    // Show the alert
                    this.showAlert = true;

                    // Mark for check
                    this._changeDetectorRef.markForCheck();
                }

            );
    }

    /**
     * Switch challenge
     */
    switchSelectedChallenge(challenge){
        this.selectedChallenge = challenge;

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Switch On create mode
     */
    switchOnCreateMode(){
        this.createMode = true;

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Switch Off create mode
     */
    switchOffCreateMode(){
        this.createMode = false;

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Get all challenge realised
     */
    getAll(){
        this.selectedChallengeMode = "all";

        this._challengeService.getAll()
            .subscribe(
                (challenges) => {
                    this.challenges = challenges;
                    this._changeDetectorRef.markForCheck();
                }
            );

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Get all challenge by creation date most old
     */
    getOldest(){
        this.selectedChallengeMode = "oldest";

        this._challengeService.getAll('asc')
            .subscribe(
                (challenges) => {
                    this.challenges = challenges;
                    this._changeDetectorRef.markForCheck();
                }
            );

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Get all challenge realised
     */
    getAllRealised(){

        this.selectedChallengeMode = "realised";

        this._challengeService.getAllRealised()
            .subscribe(
                () => {
                    this._changeDetectorRef.markForCheck();
                }
            );

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Get all challenge order by like DESC
     */
    getAllByLikeDesc(){

        this.selectedChallengeMode = "bestLike";

        this._challengeService.getAllByLike()
            .subscribe(
                () => {
                    this._changeDetectorRef.markForCheck();
                }
            );

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Get all challenge order by like ASC
     */
    getAllByLikeAsc(){

        this.selectedChallengeMode = "minusLike";

        this._challengeService.getAllByLike('asc')
            .subscribe(
                () => {
                    this._changeDetectorRef.markForCheck();
                }
            );

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Like challenge
     */
    like(challenge){

        this._challengeService.likeChallenge(challenge)
            .subscribe(
                () => {
                    challenge.like = true;
                },
                (error) => {
                    challenge.like = false;
                },()=>{

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                }
            );
    }

    /**
     * Unlike challenge
     */
    unlike(challenge){
        this._challengeService.unlikeChallenge(challenge)
            .subscribe(
                () => {
                    challenge.like = false;
                },
                (error) => {
                    challenge.like = true;
                },()=>{

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                }
            );
    }

    /**
     * Hide challenge
     */
    hideChallenge(challenge){

        this._challengeService.toggleHideChallenge(challenge, true)
            .subscribe(
                () => {
                    challenge.cache = true;
                },
                (error) => {
                    challenge.cache = false;
                },()=>{

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                }
            );
    }

    /**
     * UnHide challenge
     */
    unHideChallenge(challenge){
        this._challengeService.toggleHideChallenge(challenge, false)
            .subscribe(
                () => {
                    challenge.cache = false;
                },
                (error) => {
                    challenge.cache = true;
                },()=>{

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                }
            );
    }

    /**
     * Realised challenge
     */
    realisedChallenge(challenge){

        this._challengeService.realisedChallenge(challenge)
            .subscribe(
                () => {
                    challenge.realise = true;
                },
                (error) => {
                    challenge.realise = false;
                },()=>{

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                }
            );
    }

    /**
     * UnRealisedchallenge
     */
    unrealisedChallenge(challenge){
        this._challengeService.unrealisedChallenge(challenge)
            .subscribe(
                () => {
                    challenge.realise = false;
                },
                (error) => {
                    challenge.realise = true;
                },()=>{

                    // Mark for check
                    this._changeDetectorRef.markForCheck();

                }
            );
    }

    /**
     * Create comment
     */
    createComment(): void
    {
        // Return if the comment form is invalid
        if ( this.commentForm.invalid )
        {
            return;
        }

        // Disable the comment form
        this.commentForm.disable();

        // Hide the comment alert
        this.showAlertComment = false;

        // Add Comment
        this._challengeService.createComment(this.commentForm.controls['texte'].value, this.selectedChallenge.id)
            .subscribe(
                () => {
                    this.alertComment = {
                        type   : 'success',
                        message: 'Commentaire crée avec succès.',
                    };

                    this.commentForm.reset();
                },
                (error) => {

                    let errorMessage = error.error || error.errors.toString() || error.statusText || 'Une erreur est survenue. Veuillez re-essayer plus tard.';
                    this.alertComment = {
                        type   : 'error',
                        message: errorMessage
                    };

                },
                () => {
                    // Re-enable the comment form
                    this.commentForm.enable();

                    // Show the comment alert
                    this.showAlertComment = true;

                    // Mark for check
                    this._changeDetectorRef.markForCheck();
                }

            );
    }

    /**
     * Delete challenge
     */
    deleteChallenge(challenge): void
    {
        // Open the confirmation dialog
        const confirmation = this._fuseConfirmationService.open({
            title  : 'Suppression défi: "' + challenge.nom + '"',
            message: 'Etes-vous sûr de vouloir supprimer ce défi ?',
            actions: {
                confirm: {
                    label: 'Supprimer'
                }
            }
        });

        // Subscribe to the confirmation dialog closed action
        confirmation.afterClosed().subscribe((result) => {

            // If the confirm button pressed...
            if ( result === 'confirmed' )
            {

                // Delete the command on the server
                this._challengeService.deleteChallenge(challenge).subscribe(
                    () => {
                        this.alertDeleteChallenge = {
                            type   : 'success',
                            message: 'Défi supprimé.',
                        };
                    },
                    error => {
                        let errorMessage = error.error || error.errors.toString() || error.statusText || 'Une erreur est survenue. Veuillez re-essayer plus tard.';
                        this.alertDeleteChallenge = {
                            type   : 'error',
                            message: errorMessage
                        };

                    },
                    () => {
                        this.showAlertDeleteChallenge = true;

                        // Mark for check
                        this._changeDetectorRef.markForCheck();
                    }
                );
            }
        });
    }

}
