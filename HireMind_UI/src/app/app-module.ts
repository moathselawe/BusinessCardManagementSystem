import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
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
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AppTopMenu } from './menus/app-top-menu/app-top-menu';
import { MenubarModule } from 'primeng/menubar';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { TooltipModule } from 'primeng/tooltip';
import { PopoverModule } from 'primeng/popover';

@NgModule({
  declarations: [
    App,
    AppSideMenu,
    AppTopMenu,
    MainLayout,
  ],
  imports: [
    BrowserModule, DragDropModule, TooltipModule,
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
    MenubarModule,
    PopoverModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi()),
    provideBrowserGlobalErrorListeners(),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    }),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [App]
})
export class AppModule { }
