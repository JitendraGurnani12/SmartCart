import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { BehaviorSubject } from 'rxjs';
import { CartService } from 'src/app/features/cart/services/cart.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {


  constructor(private router: Router,
    private authService: AuthService,
    private cartService : CartService
  ) {

  }
  cartBadageCount:number = 0;
  isUserLogedIn: boolean = false;

  logedInUserDisplayName : string = "User"

  ngOnInit() {
    this.authService.isUserLogedInObservable.subscribe(status => {
      this.isUserLogedIn = status
      if(this.isUserLogedIn){
        const displayName = localStorage.getItem('logedInUserDisplayName');
        this.logedInUserDisplayName = displayName ?? 'User';
      }
    })
    this.cartService.cartCountObservable.subscribe(count=>{
      this.cartBadageCount = count;
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
