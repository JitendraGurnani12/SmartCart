import { Injectable } from '@angular/core';
 
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { ApiResponse } from 'src/app/models/api-response.model';
import { PageResponse } from 'src/app/models/page-response.model';
import { OrderRequest } from '../models/orderRequest.model';
import { Order } from '../models/order.model';

// import { RegisterRequest } from '../models/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) { }

 getOrders() {
  return this.http.get<ApiResponse<Order[]>>(`${this.baseUrl}order/GetByUserId`);
  } 
  getOrderDetailById( id :number ) {
  return this.http.get<ApiResponse<Order>>(`${this.baseUrl}order/GetOrderDetailById?id=${id}`);
  } 
}



