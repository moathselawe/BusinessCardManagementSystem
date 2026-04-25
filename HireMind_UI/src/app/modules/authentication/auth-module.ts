import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AccordionModule } from 'primeng/accordion';
import { CarouselModule } from 'primeng/carousel';
import { ScrollerModule } from 'primeng/scroller';
import { TabsModule } from 'primeng/tabs';
import { AuthRoutingModule } from './auth-routing-module';
import { LoginComponent } from './login/login-component';
import { ResetPasswordComponent } from './reset-password-component/reset-password-component';
import { VerifyEmailComponent } from './verify-email-component/verify-email-component';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';
import { InputGroupAddonModule } from 'primeng/inputgroupaddon';
import { DialogModule } from 'primeng/dialog';
import { PasswordModule } from 'primeng/password';
import { InputGroupModule } from 'primeng/inputgroup';
import { InputOtpModule } from 'primeng/inputotp';

@NgModule({
  declarations: [
    LoginComponent,
    ResetPasswordComponent,
    VerifyEmailComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    AuthRoutingModule,
    TranslateModule,
    FormsModule,
    ButtonModule,
    DividerModule,
    InputTextModule,
    CheckboxModule,
    InputGroupAddonModule,
    DialogModule,
    PasswordModule,
    InputGroupModule,
    InputOtpModule
  ]
})
export class AuthModule { }

