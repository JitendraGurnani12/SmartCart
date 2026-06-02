import { Component } from '@angular/core';
import { ProductService } from '../../services/product-service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {

  page :number = 1;
  pageSize : number = 10;
  products: Product[] = [];
  constructor(private productService : ProductService)
  {

  }
  ngOnInit(){
    this.productService.getProducts(this.page,this.pageSize).subscribe({
      next:(response)=>{
        
        this.products = response.data.data;
        console.log(this.products);
      }
    });
  }
}
