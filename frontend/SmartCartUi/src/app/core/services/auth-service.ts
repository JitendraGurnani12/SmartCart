import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { BehaviorSubject } from "rxjs";

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

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  logout() {
    this.isUserLogedInSubject.next(false);
   localStorage.clear();
  }
}