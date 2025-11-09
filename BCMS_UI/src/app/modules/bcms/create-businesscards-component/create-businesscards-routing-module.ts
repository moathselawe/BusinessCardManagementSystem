import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateBusinesscardsComponent } from './create-businesscards-component';

const routes: Routes = [
  { path: '', component: CreateBusinesscardsComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateBusinesscardsRoutingModule { }
