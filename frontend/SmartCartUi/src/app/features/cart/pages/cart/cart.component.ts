import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { Cart } from '../../models/cart.model';
import { CartItem } from '../../models/cart-item.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {

  cart!: Cart;

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
      (total, item) =>
        total + (item.price * item.quantity),
      0
    );
  }
  increaseQuantity(item : CartItem){
    item.quantity = item.quantity + 1;
  }
  decreaseQuantity(item:CartItem){
    item.quantity = item.quantity - 1;
  }

}