import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrationService } from '../../../services/hiremind/registration.service';

@Component({
  selector: 'app-verify-email-component',
  standalone: false,
  templateUrl: './verify-email-component.html',
  styleUrl: './verify-email-component.css',
})
export class VerifyEmailComponent {

  isValied: boolean = false;
  isCanResendVerfication: boolean = false;
  loading: boolean = true;
  isSending: boolean = false;
  isResendEmailSuccess: boolean = false;
  message: string = '';
  email: string = '';

  constructor(
    private route: ActivatedRoute,
    private service: RegistrationService
  ) { }

  ngOnInit(): void {

    const token = this.route.snapshot.paramMap.get('token');

    if (!token) {
      this.isValied = false;
      this.loading = false;
      return;
    }

    this.service.verifyEmail(token).subscribe({
      next: (res: any) => {
        this.isValied = res.isSuccess;
        this.isCanResendVerfication = res.isCanResendVerfication;
        this.message = res.message;
        this.email = res.email;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  resendEmail() {
    if (!this.email || this.isSending) return;
    this.isSending = true;
    this.service.resendVerification(this.email).subscribe({
      next: (res: any) => {

        if (res.isSuccess) {
          this.isResendEmailSuccess = true;
          this.isCanResendVerfication = false;

          this.message = "Verification email sent again please check your email";
        }

        this.isSending = false;
      },
      error: () => {
        this.isSending = false;
      }
    });
  }
}
