import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageLookupsComponent } from './manage-lookups-component/manage-lookups-component';

const routes: Routes = [
  { path: 'ManageLookups', component: ManageLookupsComponent },
  { path: '', redirectTo: 'ManageLookups', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReusableRoutingModule { }
