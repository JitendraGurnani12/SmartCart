import { Component } from '@angular/core';
import { ProductService } from '../../services/product-service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../models/product.model';
import { CartService } from 'src/app/features/cart/services/cart.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent 
{

  productDetail! : Product;
  constructor(private productService : ProductService,
              private route: ActivatedRoute,
              private cartService: CartService
  ){

  }
  ngOnInit(){
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.productService.getProductById(id).subscribe(data=>{
      this.productDetail = data;
      console.log(this.productDetail);
    })
  }

  addToCart(): void {
  
    this.cartService.addToCart(this.productDetail.id).subscribe({ next: () => {
          alert('Product added to cart');
        },
        error: (error) => {
          console.error(error);
        }
      });
  }
}
