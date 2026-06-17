import { Component, OnInit } from '@angular/core';
import { CartService } from '../../cart/services/cart.service';
import { CheckOutService } from '../services/checkout.service';
import { OrdersModule } from '../../orders/orders.module';
import { Cart } from '../../cart/models/cart.model';
import { Router } from '@angular/router';
import { OrderRequest } from '../../orders/models/orderRequest.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html'
})
export class CheckoutComponent implements OnInit {

  cart!: Cart;
  orderRequest : OrderRequest= new OrderRequest();
  

  constructor(
    private cartService: CartService,
    private checkOutService : CheckOutService,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void 
  {
    this.cartService.getCartByUserId().subscribe({
        next: (response) => {
          this.cart = response.data;
        },
        error: (error) => {
          console.error(error);
        }
      });
  }

  getTotalAmount(): number {

    if (!this.cart?.cartItems) {
      return 0;
    }

    return this.cart.cartItems.reduce(
      (total: number, item: any) =>
        total + (item.price * item.quantity),
      0
    );
  }

  
  placeOrder(): void 
  {
    console.log('Place Order Clicked');
    this.orderRequest ={
      cartId : this.cart.id
    };
    this.checkOutService.placeOrder(this.orderRequest).subscribe(data=>{
      
      this.router.navigate(['/orders']);
    });
  }

}