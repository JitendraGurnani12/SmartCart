import { Component } from '@angular/core';
import { LoginRequest } from '../../models/login-request.model';
import { AuthApiService } from '../../services/auth-api.service';
import { Route, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(private authApiService: AuthApiService,
    private authService: AuthService,
    private router: Router
  ) {

  }
  loginDto: LoginRequest = {
    email: '',
    password: ''
  };
   submitted = false;
  ngOnInit(){

  }
  onSubmit(): void {

    this.submitted = true;

    if (!this.loginDto.email || !this.loginDto.password) {
      return;
    }
    console.log('Login Request', this.loginDto);
    // Call API here later
    this.authApiService.login(this.loginDto).subscribe(data=>{
      // localStorage.setItem('token', data.token); 
      this.authService.setToken(data.token);
      alert('Login Successful'); 
      this.router.navigate(['/products']);
    })
  }
  logout() {
   this.authService.removeToken();
   this.router.navigate(['/auth/login']);
  }
}
