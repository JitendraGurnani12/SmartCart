import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { CartItem } from "../models/cart-item.model";
import { Cart } from "../models/cart.model";
import { ApiResponse } from "src/app/models/api-response.model";
import { BehaviorSubject } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(
    private http: HttpClient
  ) { }
  private baseUrl = environment.baseApiUrl;
  private cartCountBehaviourSubject =new BehaviorSubject<number>(0);
  public cartCountObservable = this.cartCountBehaviourSubject.asObservable();

  addToCart(productId: number) {

    return this.http.post(`${this.baseUrl}cart/add`, { productId: productId}
    );
  }
  getCartByUserId() {
    return this.http.get<ApiResponse<Cart>>(
      `${this.baseUrl}cart/GetCartByUserId`
    );
  }
  removeItemFromCart(item:CartItem){
    return this.http.delete(`${this.baseUrl}cart/RemoveItem`,{body:item});
  }
  updateQuantityInCart(item:CartItem){
    return this.http.put(`${this.baseUrl}cart/UpdateQuantity`,item)
  }
  updateCartBadageCount(count : number){
    this.cartCountBehaviourSubject.next(count);
  }
  refreshCartCount(){
    this.getCartByUserId().subscribe(
      {
      next:(response)=>{
        let count = 0;
        if(response.data?.cartItems?.length > 0){
            response.data.cartItems.forEach(c=>{
              count = count + c.quantity;
            })
        }
        this.cartCountBehaviourSubject.next(count);
      },
      error:()=>{
        this.cartCountBehaviourSubject.next(0);
      }

    })
  }
}