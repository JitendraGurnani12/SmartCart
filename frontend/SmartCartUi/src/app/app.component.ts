import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './core/services/auth-service';
import { SpinnerLoadingService } from './shared/spinner/service/spinnerLoading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SmartCartUi';
  isUserLogedIn : boolean = false;
  isLoading : boolean  = false;
  constructor(private router:Router,
              private spinnerLoadingService : SpinnerLoadingService,
              private authService : AuthService){

  }
  ngOnInit() {
    this.spinnerLoadingService.loading$.subscribe(data=>{
      this.isLoading = data;
    })
    this.authService.isUserLogedInObservable.subscribe(status=>{
      this.isUserLogedIn = status;
    })
  }

  logout(): void 
  {
    localStorage.removeItem('token');
    this.router.navigate(['/auth/login']);
  }

}
