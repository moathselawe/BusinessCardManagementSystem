//import { Component, OnDestroy, OnInit } from '@angular/core';
//import { MessageService } from 'primeng/api';
//import { Subscription } from 'rxjs';
//import { ToastMessageService } from '../../services/shared/toast-message.service';

//@Component({
//  selector: 'toast-message',
//  templateUrl: './toast-message.component.html',
//  styleUrls: ['./toast-message.component.scss'],
//  providers: [MessageService]
//})
//export class ToastMessageComponent implements OnInit, OnDestroy {

//  propsMessageListener!: Subscription;
//  messageTitle: string = "";
//  messageType: string = "";
//  messageBody: string = "";

//  constructor(private messageService: MessageService,
//    private _toastMessageService: ToastMessageService) { }

//  ngOnInit(): void {
//    this.propsMessageListener = this._toastMessageService._$ub.subscribe({
//      next: (res) => {

//        if (res && res.messageBody) {
//          this.messageTitle = res.messageTitle;
//          this.messageType = res.messageType;
//          this.messageBody = res.messageBody;
//          this.showMessage();
//        }

//      }
//    });
//  }

//  showMessage() {
//    this.messageService.add({
//      severity: this.messageType,
//      summary: this.messageTitle,
//      detail: this.messageBody,
//      life: 3000
//    });
//  }

//  ngOnDestroy(): void {
//    if (this.propsMessageListener && !this.propsMessageListener.closed) {
//      this.propsMessageListener.unsubscribe();
//    }
//  }

//}

import { Component, OnDestroy, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { ToastMessageService } from '../../services/shared/toast-message.service';

@Component({
  selector: 'toast-message',
  standalone: false,
  templateUrl: './toast-message.component.html',
  styleUrls: ['./toast-message.component.scss'],
  providers: [MessageService]
})
export class ToastMessageComponent implements OnInit, OnDestroy {

  propsMessageListener!: Subscription;
  messageTitle: string = "";
  messageType: string = "";
  messageBody: string = "";

  constructor(
    private messageService: MessageService,
    private _toastMessageService: ToastMessageService
  ) { }

  ngOnInit(): void {
    this.propsMessageListener = this._toastMessageService._$ub.subscribe({
      next: (res) => {
        if (res && res.messageBody) {
          this.messageTitle = res.messageTitle;
          this.messageType = res.messageType;
          this.messageBody = res.messageBody;
          this.showMessage();
        }
      }
    });
  }

  showMessage() {
    this.messageService.add({
      severity: this.messageType,
      summary: this.messageTitle,
      detail: this.messageBody,
      life: 3000
    });
  }

  ngOnDestroy(): void {
    if (this.propsMessageListener && !this.propsMessageListener.closed) {
      this.propsMessageListener.unsubscribe();
    }
  }
}

