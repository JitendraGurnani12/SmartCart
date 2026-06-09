import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";
import { CartItem } from "../models/cart-item.model";

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(
    private http: HttpClient
  ) { }
  private baseUrl = environment.baseApiUrl;

  addToCart(productId: number) {

    return this.http.post(`${this.baseUrl}cart/add`, { productId: productId}
    );
  }
  getCartByUserId() {
    return this.http.get<any>(
      `${this.baseUrl}cart/GetCartByUserId`
    );
  }
  removeItemFromCart(item:CartItem){
    return this.http.delete(`${this.baseUrl}cart/RemoveItem`,{body:item});
  }
  updateQuantityInCart(item:CartItem){
    return this.http.put(`${this.baseUrl}cart/UpdateQuantity`,item)
  }
}