import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SmartCartUi';
  constructor(private router:Router){

  }
  logout(): void {

  localStorage.removeItem('token');

  this.router.navigate(['/auth/login']);
}
// gotoCartPage(){
//   this.router.navigate(['/cart']);
// }
}
