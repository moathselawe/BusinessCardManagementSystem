import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManageBusinesscardsComponent } from './manage-businesscards-component';

const routes: Routes = [
  { path: '', component: ManageBusinesscardsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageBusinesscardsRoutingModule { }
