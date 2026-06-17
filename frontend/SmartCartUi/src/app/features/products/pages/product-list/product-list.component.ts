import { Component } from '@angular/core';
import { ProductService } from '../../services/product-service';
import { Product } from '../../models/product.model';
import { CartService } from 'src/app/features/cart/services/cart.service';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {

  page :number = 1;
  pageSize : number = 10;
  products: Product[] = [];
  constructor(private productService : ProductService,
    private cartService: CartService,
    private toasterService : ToasterService
  )
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
  addToCart(productId :number): void {
  
    this.cartService.addToCart(productId).subscribe({ next: () => {
          this.toasterService.showSuccessToast("Product Added into cart");
          this.cartService.refreshCartCount();
        },
        error: (error) => {
          console.error(error);
          this.toasterService.showWarningToast(error);
        }
      });
  }
}
