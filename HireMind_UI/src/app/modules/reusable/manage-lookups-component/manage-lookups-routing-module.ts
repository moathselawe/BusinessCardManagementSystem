import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageLookupsComponent } from './manage-lookups-component';

const routes: Routes = [
  { path: '', component: ManageLookupsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageLookupsRoutingModule { }
