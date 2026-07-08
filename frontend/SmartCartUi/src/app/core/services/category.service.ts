import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.development";

import { ApiResponse } from "src/app/models/api-response.model";
import { BehaviorSubject } from "rxjs";
import { Category } from "src/app/models/category.model";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(
    private http: HttpClient
  ) { }
  private baseUrl = environment.baseApiUrl;
  

  
  getAllCategories() {
    return this.http.get<ApiResponse<Category[]>>(
      `${this.baseUrl}category/GetAll`
    );
  }
  
}