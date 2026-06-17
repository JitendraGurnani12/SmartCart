import { Injectable } from '@angular/core';
 
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.development';
import { OrderRequest } from '../../orders/models/orderRequest.model';

// import { RegisterRequest } from '../models/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class CheckOutService {

  private baseUrl = environment.baseApiUrl;

  constructor(private http: HttpClient) { }

   placeOrder(orderRequest : OrderRequest) {

    return this.http.post(`${this.baseUrl}order/PlaceOrder`, orderRequest);
  }
  
}
