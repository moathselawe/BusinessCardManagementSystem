import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageBusinesscardsComponent } from './manage-businesscards-component/manage-businesscards-component';
import { CreateBusinesscardsComponent } from './create-businesscards-component/create-businesscards-component';

const routes: Routes = [
  { path: 'ManageBusinesscards', component: ManageBusinesscardsComponent },
  { path: 'CreateBusinesscard', component: CreateBusinesscardsComponent }, 
  { path: 'ModifyBusinesscard/:id', component: CreateBusinesscardsComponent }, 
  { path: 'PreviewBusinesscard/:id', component: CreateBusinesscardsComponent },
  { path: 'CreateMulipleBusinesscards', component: CreateBusinesscardsComponent },
  { path: '', redirectTo: 'ManageBusinesscards', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BCMSRoutingModule { }
