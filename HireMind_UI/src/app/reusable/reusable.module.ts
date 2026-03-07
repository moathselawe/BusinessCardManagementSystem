//import { NgModule } from "@angular/core";
//import { ToastModule } from "primeng/toast";
//import { ToastMessageComponent } from "./toast-message/toast-message.component";

//@NgModule({
//  declarations: [ToastMessageComponent],
//  imports: [ToastModule],
//  exports: [ToastMessageComponent]
//})
//export class ReusableModule { }
import { NgModule } from "@angular/core";
import { ToastModule } from "primeng/toast";
import { ToastMessageComponent } from "./toast-message/toast-message.component";

@NgModule({
  declarations: [ToastMessageComponent],
  imports: [ToastModule],
  exports: [ToastMessageComponent]
})
export class ReusableModule { }
