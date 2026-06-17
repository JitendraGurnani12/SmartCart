import { Injectable } from '@angular/core';
import { Toast } from '../models/toast.model';


@Injectable({
  providedIn: 'root'
})
export class ToasterService {

  constructor() { }
  toasts: any[] = [];

	showSuccessToast(template: string) {
		this.toasts.push({ template, classname: 'bg-success text-light', delay: 3000 });
	}

	showWarningToast(template: string) {
		this.toasts.push({ template, classname: 'bg-danger text-light', delay: 3000 });
	}

	remove(toast: Toast) {
		this.toasts = this.toasts.filter((t) => t !== toast);
	}

	clear() { 
		this.toasts.splice(0, this.toasts.length);
	}
}
