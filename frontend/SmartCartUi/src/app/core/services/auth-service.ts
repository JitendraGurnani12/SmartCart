import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";
import { jwtDecode } from 'jwt-decode';
import { Role } from "src/app/models/roles.model";
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly TOKEN_KEY = 'token';
  private isUserLogedInSubject = new BehaviorSubject<boolean>(this.isLoggedIn());
   isUserLogedInObservable = this.isUserLogedInSubject.asObservable();
  constructor(router:ActivatedRoute){
    
  }

  setToken(token: string): void {
    this.isUserLogedInSubject.next(true);
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean 
  {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    if (this.isTokenExpired()) {
      
      return false;
    }
    return true;
  }

  logout() {
    this.isUserLogedInSubject.next(false);
    this.removeToken();
    localStorage.removeItem('role');
  }
  isAdmin(): boolean {
    return localStorage.getItem('role') === Role.Admin;
  }

  isSeller(): boolean {
    return localStorage.getItem('role') === Role.Seller;
  }

  isCustomer(): boolean {
    return localStorage.getItem('role') === Role.Customer;
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) {
      return true;
    }
    const decoded: any = jwtDecode(token);
    const currentTime = Math.floor(Date.now() / 1000);
    return decoded.exp < currentTime;
  }
}