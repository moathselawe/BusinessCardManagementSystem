import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AboutUsComponent } from './about-us.component/about-us.component';
import { FqasComponent } from './fqas.component/fqas.component';
import { PublicHomeComponent } from './public-home.component/public-home.component';

const routes: Routes = [
  { path: 'home', component: PublicHomeComponent },
  { path: 'aboutUs', component: AboutUsComponent },
  { path: 'FQAS', component: FqasComponent },
  { path: '', redirectTo: 'public', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
