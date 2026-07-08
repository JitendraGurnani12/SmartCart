import { Injectable } from '@angular/core';
 
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { ApiResponse } from 'src/app/models/api-response.model';
import { PageResponse } from 'src/app/models/page-response.model';
import { Product } from '../models/product.model';

// import { RegisterRequest } from '../models/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) { }

  getProducts(page: number, pageSize: number,search:string = '',categoryId : number) {
    return this.http.get<ApiResponse<PageResponse<Product>>>(`${this.baseUrl}product/GetAll?page=${page}&pageSize=${pageSize}&search=${search}&categoryId=${categoryId}`);
  }
  getProductById(productId : number){
    return this.http.get<Product>(`${this.baseUrl}product/GetById?productId=${productId}`);
  }
}



