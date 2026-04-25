import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { AccordionModule } from 'primeng/accordion';
import { CarouselModule } from 'primeng/carousel';
import { ScrollerModule } from 'primeng/scroller';
import { TabsModule } from 'primeng/tabs';
import { AboutUsComponent } from './about-us.component/about-us.component';
import { FqasComponent } from './fqas.component/fqas.component';
import { PublicHomeComponent } from './public-home.component/public-home.component';
import { PublicRoutingModule } from './public-routing-module';

@NgModule({
  declarations: [
    AboutUsComponent,
     FqasComponent,
     PublicHomeComponent,
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    PublicRoutingModule,
    TranslateModule,
    CarouselModule ,
    AccordionModule,
    TabsModule,
    ScrollerModule
  ]
})
export class PublicModule { }

