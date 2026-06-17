import { Component, OnInit } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { Cart } from '../../models/cart.model';
import { CartItem } from '../../models/cart-item.model';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html'
})
export class CartComponent implements OnInit {

  cart!: Cart;

  constructor(
    private cartService: CartService,
    private toasterService : ToasterService
  ) { }

  ngOnInit(): void {
    this.loadCart();
    this.cartService.refreshCartCount();
  }

  loadCart(): void {

    this.cartService
      .getCartByUserId()
      .subscribe({

        next: (response) => {
          this.cart = response.data;
          console.log(JSON.stringify(this.cart));
          this.cartService.refreshCartCount();

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
    this.updateQuantityInDb(item);
    
  }
  updateQuantityInDb(item:CartItem){
    this.cartService.updateQuantityInCart(item).subscribe({
      next:(response)=>{
        this.loadCart();
      }
    })
  }
  decreaseQuantity(item:CartItem){
    if (item.quantity > 1) {
      item.quantity--;
       this.updateQuantityInDb(item);
      
    }
  }
  removeItemFromCart(item:CartItem){
    this.cartService.removeItemFromCart(item).subscribe({
      next :(response)=>{
        this.loadCart();
        
      },
      error:(error)=>{
       this.toasterService.showWarningToast(error);
      }
    })
  }

}