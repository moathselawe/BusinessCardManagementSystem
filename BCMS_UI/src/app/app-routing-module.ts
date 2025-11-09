import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayout } from './main-layout/main-layout';

const routes: Routes = [
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
  { path: '', redirectTo: 'BCMS/ManageBusinesscards', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
