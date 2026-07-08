import { Component } from '@angular/core';
import { ProductService } from '../../services/product-service';
import { Product } from '../../models/product.model';
import { CartService } from 'src/app/features/cart/services/cart.service';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Category } from 'src/app/models/category.model';
import { CategoryService } from 'src/app/core/services/category.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent {

  page :number = 1;
  pageSize : number = 8;
  products: Product[] = [];
  searchText : string = "";
  totalRecords = 0;
  categories: Category[] = [];
  selectedCategoryId: number = 0;
  private searchSubject = new Subject<string>();
  constructor(private productService : ProductService,
    private cartService: CartService,
    private toasterService : ToasterService,
    private categoryService : CategoryService 
  )
  {

  }
  

// searchProducts() {
//   this.productService.getAllProducts(this.searchText).subscribe(res => {
//     this.products = res.data;
//   });
// }
  ngOnInit(){
    this.loadCategories();
    this.loadProducts();

  this.searchSubject
    .pipe(
      debounceTime(300),
      distinctUntilChanged()
    )
    .subscribe(search => {
      this.page = 1; // Reset to first page
      this.searchText = search;
      this.loadProducts();

    });
  }
  loadCategories() 
  {
    this.categoryService.getAllCategories().subscribe({
        next: (response) => {
          this.categories = response.data;

        }
      });
  }
  onCategoryChange() 
  {
    this.page = 1;
    this.loadProducts();
  }

  onSearch() {
    this.searchSubject.next(this.searchText);
  }

  loadProducts(){
    this.productService.getProducts(this.page,this.pageSize,this.searchText,this.selectedCategoryId).subscribe({
      next:(response)=>{
        
        this.products = response.data.data;
        console.log(this.products);
        this.totalRecords = response.data.totalCount;
      }
    });
  }

  get totalPages(): number {
    return Math.ceil(this.totalRecords / this.pageSize);
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

  get pageNumbers(): number[] {
    return Array.from(
      { length: this.totalPages },
      (_, i) => i + 1
    );
  }

  changePage(page: number) 
  {
    if (page < 1 || page > this.totalPages) {
      return;
    }
    this.page = page;
    this.loadProducts();
  }
}
