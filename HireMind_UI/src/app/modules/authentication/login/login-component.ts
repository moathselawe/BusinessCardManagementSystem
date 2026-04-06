import { Component } from '@angular/core';
import { RegisterUser } from '../../../models/hiremind/RegisterUser';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { TokenService } from '../../../services/hiremind/token.service';
import { RegistrationService } from '../../../services/hiremind/registration.service';

@Component({
  selector: 'app-login-component',
  standalone: false,
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  constructor(
    public registrationService: RegistrationService,
    public tokenService: TokenService,
    public router: Router,
    public toastService: ToastMessageService
  ) { }


  isNewAccount: boolean = false;
  isInfoDialog: boolean = false;

  newUser: RegisterUser = new RegisterUser();
  submit(form: NgForm) {

    form.control.markAllAsTouched();

    const notValid = this.validateForm(form);
    if (notValid) {
      return;
    }

    // call API here later
    this.registrationService.register(this.newUser).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Updated',
          messageBody: 'Job updated successfully.'
        });
        this.isNewAccount = false;
      },
      error: () => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to update Job.'
        });
      }
    });
  }

  validateForm(form: NgForm): boolean {

    if (form.invalid) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Validation Error',
        messageBody: 'Please fill all required fields correctly.'
      });
      return true;
    }

    // Arabic Name validation
    if (!this.newUser.arabicName || this.newUser.arabicName.length < 3 || this.newUser.arabicName.length > 100) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Invalid Name',
        messageBody: 'Arabic name must be between 3 and 100 characters.'
      });
      return true;
    }


    // English Name validation
    if (!this.newUser.englishName || this.newUser.englishName.length < 3 || this.newUser.englishName.length > 100) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Invalid Name',
        messageBody: 'English name must be between 3 and 100 characters.'
      });
      return true;
    }

    // Email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!this.newUser.email || !emailRegex.test(this.newUser.email) || this.newUser.email.length > 150) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Invalid Email',
        messageBody: 'Please enter a valid email address.'
      });
      return true;
    }

    // Mobile validation
    const mobileRegex = /^[0-9]{8,15}$/;
    if (!this.newUser.mobile || !mobileRegex.test(this.newUser.mobile)) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Invalid Mobile',
        messageBody: 'Mobile number must be 8–15 digits.'
      });
      return true;
    }

    // Password validation
    const passwordRegex = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*(),.?":{}|<>]).{8,50}$/;

    if (!this.newUser.password || !passwordRegex.test(this.newUser.password)) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Weak Password',
        messageBody: 'Password must contain at least 8 characters, one uppercase letter, one number and one symbol.'
      });
      return true;
    }

    // Confirm password match
    if (this.newUser.password !== this.newUser.confirmPassword) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Password Mismatch',
        messageBody: 'Password and Confirm Password must match.'
      });
      return true;
    }

    return false;
  }




  //login
  ngOnInit() {

    const savedLogin = localStorage.getItem('remember_login');

    if (savedLogin) {
      const data = JSON.parse(savedLogin);

      this.email = data.email;
      this.password = data.password;
      this.rememberMe = true;
    }

  }

  email: string = '';
  password: string = '';
  rememberMe: boolean = false;

  isLoading: boolean = false;
    
  login() {

    if (!this.email || !this.password) {
      this.toastService.showMessage({
        messageType: 'warn',
        messageTitle: 'Validation',
        messageBody: 'Email and password are required'
      });
      return;
    }

    this.isLoading = true;

    //const payload = {
    //  email: this.email,
    //  password: this.password
    //};
    const payload = {
      body: {
        email: this.email,
        password: this.password
      }
    };

    this.tokenService.login(payload).subscribe({
      next: (res: any) => { 
        this.isLoading = false;

        if (res.isSuccess) {
          const token = res.accessToken;
          const refreshToken = res.refreshToken;

          this.tokenService.saveTokens(token, refreshToken, this.rememberMe);

          // Save login credentials if needed
          if (this.rememberMe) {
            localStorage.setItem('remember_login', JSON.stringify({
              email: this.email,
              password: this.password
            }));
          } else {
            localStorage.removeItem('remember_login');
          }

          this.toastService.showMessage({
            messageType: 'success',
            messageTitle: 'Login Successful',
            messageBody: 'Welcome back!'
          });

          this.router.navigate(['/BCMS/ManageBusinesscards']);

        }
        else {
          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Login Failed',
            messageBody: res.message
          });

        }
      },

      error: () => {
        this.isLoading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Unable to login. Please try again.'
        });
      }
    });

  }
}
