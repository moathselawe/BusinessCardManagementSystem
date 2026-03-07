import { Component } from '@angular/core';

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

  sendVerfication() {
    this.isVerficationSent = true;
    this.disableResendButton();
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

    this.disableResendButton();
  }

  confirmVerification() {
    if (this.otp?.length === 6) {
      this.isConfirmed = true;
      this.isInvalidOTP = false;
    } else {
      this.isConfirmed = false;
      this.isInvalidOTP = true;
      return;
    }

    // call api here
    // API will deside
    this.isCorrectOTP = true; //incase its incorrect
   // this.isCorrectOTP = false; //incase its correct
  }

  saveNewPassword() {
    //check password befor assginging true to the next step
    this.isPasswordSaved = true;
  }
}
