import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ToastMessageService {
    _$ub: Subject<{ messageType: string, messageTitle: string, messageBody?: string }> =
      new Subject<{ messageType: string, messageTitle: string, messageBody?: string }>();

    constructor() { }

    showMessage(prps: { messageType: string, messageTitle: string, messageBody?: string }) {
      this._$ub.next(prps);
    }
}
