import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { ResetPasswordService } from '../../../services/hiremind/resetPassword.service';

@Component({ 
  selector: 'app-reset-password-component',
  standalone: false,
  templateUrl: './reset-password-component.html',
  styleUrl: './reset-password-component.css',
})
export class ResetPasswordComponent {

  isVerficationSent: boolean = false;
  isResendDisabled: boolean = false;
  isConfirmed: boolean = false;
  isInvalidOTP: boolean = false;
  isCorrectOTP: boolean = false;
  isPasswordSaved: boolean = false;
  resendCountdown: number = 30; 
  otp: any;
  email: string = '';
  password: string = '';
  confirmPassword: string = '';
  isDisableVerficationSent: boolean = false;
  constructor(
    private route: ActivatedRoute,
    public toastService: ToastMessageService,
    private service: ResetPasswordService
  ) { }

  sendVerfication() {

    if (!this.email) return;

    this.isDisableVerficationSent = true;

    this.service.sendResetCode(this.email).subscribe({
      next: (res: any) => {

        if (res.isSuccess) {
          this.isVerficationSent = true;
          this.disableResendButton();
        }
        else {

          this.isVerficationSent = false;
          this.isDisableVerficationSent = false;

          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: res.message
          });

        }

      },
      error: (err) => {

        this.isVerficationSent = false;
        this.isDisableVerficationSent = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Something went wrong. Please try again.'
        });

        console.error(err);
      }
    });

  }

  disableResendButton() {
    this.isResendDisabled = true;
    this.resendCountdown = 30;
    this.isConfirmed = false;
    this.isInvalidOTP = false;
    this.otp = null;

    const interval = setInterval(() => {
      this.resendCountdown--;
      if (this.resendCountdown <= 0) {
        this.isResendDisabled = false;
        clearInterval(interval);
      }
    }, 1000);
  }

  resendVerification() {

    if (this.isResendDisabled) return;

    this.service.sendResetCode(this.email).subscribe({
      next: (res: any) => {

        if (res.isSuccess) {
          this.disableResendButton();
        }
        else {

          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: res.message
          });

        }

      },
      error: () => {

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to resend verification code.'
        });

      }
    });

  }

  confirmVerification() {

    if (this.otp && this.otp.length !== 6) {
      this.isInvalidOTP = true;
      return;
    }

    this.service.verifyResetCode(this.email, this.otp).subscribe({
      next: (res: any) => {

        if (res.isSuccess) {
          this.isCorrectOTP = true;
        }
        else {

          this.isConfirmed = true;
          this.isInvalidOTP = true;

          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: res.message
          });

        }

      },
      error: () => {

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Verification failed. Please try again.'
        });

      }
    });

  }

  saveNewPassword() {

    if (!this.password || this.password !== this.confirmPassword) {

      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Error',
        messageBody: 'Passwords do not match'
      });

      return;
    }

    const param = {
      email: this.email,
      otp: this.otp,
      password: this.password,
      confirmPassword: this.confirmPassword
    };

    this.service.saveNewPassword(param).subscribe({
      next: (res: any) => {

        if (res.isSuccess) {
          this.isPasswordSaved = true;
        }
        else {

          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: res.message
          });

        }

      },
      error: () => {

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Password reset failed.'
        });

      }
    });

  }
}
