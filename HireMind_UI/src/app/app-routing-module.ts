import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayout } from './layouts/main-layout/main-layout';
import { PublicLayout } from './layouts/public-layout/public-layout';

const routes: Routes = [

  // 🌐 PUBLIC (NO AUTH)
  {
    path: '',
    component: PublicLayout,
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./modules/public/public-module')
            .then(m => m.PublicModule)
      },
      {
        path: '',
        loadChildren: () =>
          import('./modules/authentication/auth-module')
            .then(m => m.AuthModule)
      }
    ]
  },

  // 🔐 APP (AUTH REQUIRED)
  {
    path: '',
    component: MainLayout,
    children: [
      {
        path: 'BCMS',
        loadChildren: () =>
          import('./modules/bcms/bcms-module')
            .then(m => m.BCMSModule)
      },
      {
        path: 'HireMind',
        loadChildren: () =>
          import('./modules/hiremind/hiremind-module')
            .then(m => m.HireMindModule)
      },
      {
        path: 'Auth',
        loadChildren: () =>
          import('./modules/admin/admin-module')
            .then(m => m.AdminModule)
      },
      {
        path: 'Shared',
        loadChildren: () =>
          import('./modules/reusable/reusable-module')
            .then(m => m.ReusableModule)
      },

      { path: '', redirectTo: 'BCMS/ManageBusinesscards', pathMatch: 'full' }
    ]
  }
];

//const routes: Routes = [
//  {
//    path: '',
//    loadChildren: () =>
//      import('./modules/authentication/auth-module')
//        .then(m => m.AuthModule)
//  },

//  {
//    path: '',
//    loadChildren: () =>
//      import('./modules/public/public-module')
//        .then(m => m.PublicModule)
//  },

//  {
//    path: '',
//    component: MainLayout,
//    children: [
//      {
//        path: 'BCMS',
//        loadChildren: () =>
//          import('./modules/bcms/bcms-module')
//            .then(m => m.BCMSModule)
//      },
//      {
//        path: 'HireMind',
//        loadChildren: () =>
//          import('./modules/hiremind/hiremind-module')
//            .then(m => m.HireMindModule)
//      },
//      {
//        path: 'Auth',
//        loadChildren: () =>
//          import('./modules/admin/admin-module')
//            .then(m => m.AdminModule)
//      },
//      {
//        path: 'Shared',
//        loadChildren: () =>
//          import('./modules/reusable/reusable-module')
//            .then(m => m.ReusableModule)
//      },

//      { path: '', redirectTo: 'BCMS/ManageBusinesscards', pathMatch: 'full' }
//    ]
//  }
//];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
