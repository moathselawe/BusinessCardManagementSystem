import { DragDropModule } from '@angular/cdk/drag-drop';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { TranslateModule } from '@ngx-translate/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import Aura from '@primeuix/themes/aura';
import { AvatarModule } from 'primeng/avatar';
import { BadgeModule } from 'primeng/badge';
import { ButtonModule } from 'primeng/button';
import { providePrimeNG } from 'primeng/config';
import { DialogModule } from 'primeng/dialog';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { PopoverModule } from 'primeng/popover';
import { RippleModule } from 'primeng/ripple';
import { TooltipModule } from 'primeng/tooltip';
import { App } from './app';
import { AppRoutingModule } from './app-routing-module';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { MainLayout } from './layouts/main-layout/main-layout';
import { PublicLayout } from './layouts/public-layout/public-layout';
import { ReusableModule } from './reusable/reusable.module';
import { TemplateConversation } from './shared/template-conversation/template-conversation';
import { AppSideMenu } from './menus/admin-menus/admin-side-bar/admin-side-bar';
import { AppTopMenu } from './menus/admin-menus/admin-top-bar/admin-top-bar';
import { PublicTopbar } from './menus/public-top-bar/public-top-bar';


@NgModule({
  declarations: [
    App,
    AppSideMenu,
    AppTopMenu,
    PublicTopbar,
    MainLayout,
    PublicLayout
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
    PopoverModule,

    TranslateModule.forRoot({
      loader: provideTranslateHttpLoader({
        prefix: './assets/i18n/',
        suffix: '.json'
      })
    })
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
