import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './core/services/auth-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SmartCartUi';
  isUserLogedIn : boolean = false;
  constructor(private router:Router,
              private authService : AuthService){

  }
  ngOnInit() {
    this.authService.isUserLogedInObservable.subscribe(status=>{
      debugger;
      this.isUserLogedIn = status;
    })
  }

  logout(): void 
  {
    localStorage.removeItem('token');
    this.router.navigate(['/auth/login']);
  }

}
