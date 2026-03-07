import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageJobsComponent } from './manage-Jobs-component';

const routes: Routes = [
  { path: '', component: ManageJobsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageJobsRoutingModule { }
