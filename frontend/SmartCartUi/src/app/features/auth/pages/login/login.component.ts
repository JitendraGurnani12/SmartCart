import { Component } from '@angular/core';
import { LoginRequest } from '../../models/login-request.model';
import { AuthApiService } from '../../services/auth-api.service';
import { Route, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth-service';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';
import { jwtDecode } from 'jwt-decode';
import { Role } from 'src/app/models/roles.model';

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
      const role = this.getRoleFromToken();
      localStorage.setItem('role',role);
      if(role == Role.Seller){
        this.router.navigateByUrl('/seller')
      }
      else if(role == Role.Admin){

      }
      else{
        this.router.navigate(['/products']);
      }
      localStorage.setItem('logedInUserDisplayName', data.displayName);
    },
      (ex) => {
        this.toasterService.showWarningToast(ex.error.message);
      })
  }

  logout() {
   this.authService.logout();
   this.router.navigate(['/auth/login']);
  }
  getRoleFromToken(){
    const token = this.authService.getToken();
    if(!token){
      return '';
    }
    const decoded :any = jwtDecode(token);
    return decoded[
    'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    ];
  }
}
