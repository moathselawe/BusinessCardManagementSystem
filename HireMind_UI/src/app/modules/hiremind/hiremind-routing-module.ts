import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobApplicationAssistantComponent } from './job-application-assistant-component/job-application-assistant-component';
import { JobFormComponent } from './job-form-component/job-form-component';
import { ManageApplicationsComponent } from './manage-applications.component/manage-applications.component';
import { ManageJobsComponent } from './manage-jobs-component/manage-jobs-component';

const routes: Routes = [
  { path: 'ManageJobs', component: ManageJobsComponent },
  { path: 'JobApplication/:id', component: JobApplicationAssistantComponent },
  { path: 'CreateJob', component: JobFormComponent },
  { path: 'ModifyJob/:id', component: JobFormComponent },
  { path: 'PreviewJob/:id', component: JobFormComponent },
  { path: 'ManageApplications/:id', component: ManageApplicationsComponent },
  { path: '', redirectTo: 'JobApplication', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HireMindRoutingModule { }
