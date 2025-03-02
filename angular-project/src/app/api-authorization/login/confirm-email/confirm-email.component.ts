import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from 'src/services/user.service';
import { confirmEmail } from '../../user-registration';
import { User } from '@angular/fire/auth';
import { take } from 'rxjs';

@Component({
  selector: 'app-confirm-email',
  standalone: true,
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent implements OnInit{
  debugger
  constructor(private accountService: AuthenticationService,
    private router: Router,
    private userService: UserService,
    private activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.accountService.user$.pipe(take(1)).subscribe({
      next: (user: User | null) =>{
        if (user) {
          this.router.navigateByUrl('/');
        } else {
          this.activatedRoute.queryParamMap.subscribe({
            next: (params: any) => {
              const confirmEmail: confirmEmail  = {
                token: params.get('token'),
                email: params.get('email'),
              }
              this.accountService.confirmEmail(confirmEmail).subscribe();
            }
          })
        }
      }
    })
  }

  resendEmailConfirmationLink() {
    this.router.navigateByUrl('/account/send-email/resend-email-confirmation-link');
  }
}