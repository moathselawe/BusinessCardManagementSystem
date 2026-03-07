import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { JobApplicationAssistantComponent } from './job-application-assistant-component';

const routes: Routes = [
  { path: '', component: JobApplicationAssistantComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class JobApplicationAssistantRoutingModule { }
