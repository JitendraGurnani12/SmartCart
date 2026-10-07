import { Injectable } from '@angular/core';
 
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { ApiResponse } from 'src/app/models/api-response.model';
import { PageResponse } from 'src/app/models/page-response.model';
import { Product } from '../../products/models/product.model';


// import { RegisterRequest } from '../models/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class SellerProductService {

  private baseUrl = environment.baseApiUrl;
  private editId : number = 0;

  constructor(private http: HttpClient) { }

  getProducts(page: number, pageSize: number,search:string = '',categoryId : number) {
    return this.http.get<ApiResponse<PageResponse<Product>>>(`${this.baseUrl}product/GetMyProducts?page=${page}&pageSize=${pageSize}&search=${search}&categoryId=${categoryId}`);
  }
  getProductById(productId : number){
    return this.http.get<Product>(`${this.baseUrl}product/GetMyProductById?productId=${productId}`);
  }

  // addProduct(product: Product) {
  //   return this.http.post< ApiResponse<Product>>(`${this.baseUrl}product/Add`,product);
  // }

  // updateProduct(product: Product) {
  //   return this.http.put<ApiResponse<Product>>(`${this.baseUrl}product/Update`,product);
  // }
  addProduct(formData: FormData) {
  return this.http.post<ApiResponse<Product>>( `${this.baseUrl}product/Add`, formData);
  }

updateProduct(formData: FormData) {
  return this.http.put<ApiResponse<Product>>(`${this.baseUrl}product/Update`,formData);
  }
  public setEditId(id: number) {
    this.editId = id;
  }

  public getEditId(): number {
    return this.editId;
  }
  deleteProduct(productId: number) {
    return this.http.delete<ApiResponse<null>>(`${this.baseUrl}product/Delete/${productId}`);
  }
}



