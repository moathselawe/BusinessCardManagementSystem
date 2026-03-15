import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageApplicationsComponent } from './manage-applications.component';

const routes: Routes = [
  { path: '', component: ManageApplicationsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageApplicationsModule { }
