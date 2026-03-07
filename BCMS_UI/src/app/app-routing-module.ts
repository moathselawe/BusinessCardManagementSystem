import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayout } from './main-layout/main-layout';

const routes: Routes = [
  {
    path: 'login',
    loadChildren: () =>
      import('./modules/authentication/login/login-module')
        .then(m => m.LoginModule)
  },
  {
    path: 'resetpassword',
    loadChildren: () =>
      import('./modules/authentication/reset-password-component/reset-password-module')
        .then(m => m.ResetPasswordModule)
  },
  {
    path: '',
    component: MainLayout,
    children: [
      {
        path: 'BCMS',
        loadChildren: () =>
          import('./modules/bcms/bcms-module')
            .then(m => m.BCMSModule)
      }
    ]
  },
  {
    path: '',
    component: MainLayout,
    children: [
      {
        path: 'HireMind',
        loadChildren: () =>
          import('./modules/hiremind/hiremind-module')
            .then(m => m.HireMindModule)
      }
    ]
  },
  { path: '', redirectTo: 'BCMS/ManageBusinesscards', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
