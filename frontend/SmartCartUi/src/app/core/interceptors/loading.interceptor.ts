import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { SpinnerLoadingService } from 'src/app/shared/spinner/service/spinnerLoading.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private spinnerLoadingService : SpinnerLoadingService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      finalize(()=>{
      this.spinnerLoadingService.hide()
    })
  )
  }
}
