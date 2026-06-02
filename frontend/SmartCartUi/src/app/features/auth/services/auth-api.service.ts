import { Injectable } from '@angular/core';
import { AuthResponse } from '../models/auth-response.model';
import { LoginRequest } from '../models/login-request.model'; 
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { RegisterRequest } from '../models/register-request.model';
// import { RegisterRequest } from '../models/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthApiService {

  private baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) { }

  login(model: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.baseUrl}auth/login`,
      model
    );
  }

  register(model: RegisterRequest): Observable<any> {
    return this.http.post(
      `${this.baseUrl}auth/Register`,
      model
    );
  }
  
}


