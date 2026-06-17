import { Component } from '@angular/core';
import { LoginRequest } from '../../models/login-request.model';
import { AuthApiService } from '../../services/auth-api.service';
import { Route, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth-service';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(private authApiService: AuthApiService,
    private authService: AuthService,
    private router: Router,
    private toasterService: ToasterService
  ) {

  }
  loginDto: LoginRequest = {
    email: '',
    password: ''
  };
   submitted = false;
  ngOnInit(){
    if(this.authService.isLoggedIn()){
      this.router.navigate(['/products']);
    }
  }
  onSubmit(): void {

    this.submitted = true;

    if (!this.loginDto.email || !this.loginDto.password) {
      return;
    }
    console.log('Login Request', this.loginDto);

    this.authApiService.login(this.loginDto).subscribe(data => {

      this.authService.setToken(data.token);

      localStorage.setItem('logedInUserDisplayName', data.displayName);
      this.router.navigate(['/products']);
    },
      (error) => {
        debugger;
        this.toasterService.showWarningToast(error.error);
      })
  }

  logout() {
   this.authService.logout();
   this.router.navigate(['/auth/login']);
  }
}
