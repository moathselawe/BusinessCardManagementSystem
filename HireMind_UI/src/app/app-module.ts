import { provideHttpClient } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import Aura from '@primeuix/themes/aura';
import { providePrimeNG } from 'primeng/config';
import { App } from './app';
import { AppRoutingModule } from './app-routing-module';
import { AppSideMenu } from './menus/app-side-menu/app-side-menu';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { MenuModule } from 'primeng/menu';
import { RippleModule } from 'primeng/ripple';
import { MainLayout } from './main-layout/main-layout';
import { ReusableModule } from './reusable/reusable.module';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { TemplateConversation } from './shared/template-conversation/template-conversation';

@NgModule({
  declarations: [
    App,
    AppSideMenu,
    MainLayout,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MenuModule,
    BadgeModule,
    AvatarModule,
    RippleModule,
    ReusableModule,
    ButtonModule,
    DialogModule,
    TemplateConversation,
  ],
  providers: [
    provideHttpClient(),
    provideBrowserGlobalErrorListeners(),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    })
  ],
  bootstrap: [App]
})
export class AppModule { }
