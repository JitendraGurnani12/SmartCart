import { Component, OnInit } from '@angular/core';
import { CartService } from '../../cart/services/cart.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html'
})
export class CheckoutComponent implements OnInit {

  cart: any;

  constructor(
    private cartService: CartService
  ) { }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {

    this.cartService
      .getCartByUserId()
      .subscribe({

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

  placeOrder(): void {

    console.log('Place Order Clicked');

    // Next step:
    // Call Order API

  }

}