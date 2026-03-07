import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobApplicationAssistantComponent } from './job-application-assistant-component/job-application-assistant-component';
import { ManageJobsComponent } from './manage-Jobs-component/manage-Jobs-component';
import { JobFormComponent } from './job-form-component/job-form-component';

const routes: Routes = [
  { path: 'ManageJobs', component: ManageJobsComponent },
  { path: 'JobApplication/:id', component: JobApplicationAssistantComponent },
  { path: 'CreateJob', component: JobFormComponent },
  { path: 'ModifyJob/:id', component: JobFormComponent },
  { path: 'PreviewJob/:id', component: JobFormComponent },
  { path: '', redirectTo: 'JobApplication', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HireMindRoutingModule { }
