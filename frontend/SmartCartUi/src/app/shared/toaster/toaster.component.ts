import { Component } from '@angular/core';
import { ToasterService } from './service/toaster.service';

@Component({
  selector: 'app-toaster',
  template:`<ngb-toast *ngFor="let toast of toastService.toasts"
				[class]="toast.classname"
				[autohide]="true"
				[delay]="toast.delay || 1500"
				(hidden)="toastService.remove(toast)"
			>
				{{toast.template}}
			</ngb-toast>`,
      host: { class: 'toast-container position-fixed top-0 end-0 p-3', style: 'z-index: 1200' }
})
export class ToasterComponent {

  constructor(public toastService:ToasterService){ }
}
