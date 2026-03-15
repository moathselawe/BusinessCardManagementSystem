import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageLookupsComponent } from './manage-lookups-component/manage-lookups-component';

const routes: Routes = [
  { path: 'ManageLookups', component: ManageLookupsComponent },
  //{ path: 'JobApplication/:id', component: JobApplicationAssistantComponent },
  //{ path: 'CreateJob', component: JobFormComponent },
  //{ path: 'ModifyJob/:id', component: JobFormComponent },
  //{ path: 'PreviewJob/:id', component: JobFormComponent },
  { path: '', redirectTo: 'JobApplication', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReusableRoutingModule { }
