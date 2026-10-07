import { Component } from '@angular/core';

import { CartService } from 'src/app/features/cart/services/cart.service';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Category } from 'src/app/models/category.model';
import { CategoryService } from 'src/app/core/services/category.service';
import { SellerProductService } from '../../services/seller-product.service';
import { Product } from 'src/app/features/products/models/product.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SellerProductAddEditComponent } from '../seller-product-add-edit/seller-product-add-edit.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './my-products.component.html',
  styleUrls: ['./my-products.component.css']
})
export class SellerMyProductListComponent {

  page: number = 1;
  pageSize: number = 8;

  products: Product[] = [];
  categories: Category[] = [];

  searchText: string = '';
  selectedCategoryId: number = 0;

  totalRecords: number = 0;

  private searchSubject = new Subject<string>();

  constructor(
    private sellerProductService: SellerProductService,
    private modalService : NgbModal,
    private toasterService: ToasterService,
    private categoryService: CategoryService
  ) {}

  ngOnInit(): void {

    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {

        this.page = 1;
        this.searchText = search;

        this.loadProducts();
      });

    this.loadCategories();
    this.loadProducts();
  }


  loadCategories(): void {

    this.categoryService
      .getAllCategories()
      .subscribe({

        next: (response) => {

          this.categories = response.data;

        },

        error: (error) => {

          this.toasterService.showWarningToast(
            error?.error?.message ||
            'Unable to load categories'
          );

        }

      });
  }


  loadProducts(): void {

    this.sellerProductService
      .getProducts(
        this.page,
        this.pageSize,
        this.searchText,
        this.selectedCategoryId
      )
      .subscribe({

        next: (response) => {

          this.products = response.data.data;

          this.totalRecords =
            response.data.totalCount;

        },

        error: (error) => {

          this.products = [];

          this.totalRecords = 0;

          this.toasterService.showWarningToast(
            error?.error?.message ||
            'Unable to load products'
          );

        }

      });
  }


  onSearch(): void {

    this.searchSubject.next(
      this.searchText
    );

  }


  onCategoryChange(): void {

    this.page = 1;

    this.loadProducts();

  }


  get totalPages(): number {

    return Math.ceil(
      this.totalRecords / this.pageSize
    );

  }


  get pageNumbers(): number[] {

    return Array.from(
      { length: this.totalPages },
      (_, i) => i + 1
    );

  }


  changePage(page: number): void {

    if (
      page < 1 ||
      page > this.totalPages
    ) {
      return;
    }

    this.page = page;

    this.loadProducts();

  }


  getCategoryName(
    categoryId: number
  ): string {

    const category =
      this.categories.find(
        c => c.id === categoryId
      );

    return category
      ? category.name
      : 'N/A';

  }


  addProduct(): void {
    this.sellerProductService.setEditId(0);
    this.modalService.open(SellerProductAddEditComponent,{ centered: true }).dismissed.subscribe(x=>{
      this.loadProducts();
    })
  }


  editProduct(id: number): void {
    this.sellerProductService.setEditId(id);
    this.modalService.open(SellerProductAddEditComponent,{ centered: true }).dismissed.subscribe(x=>{
      this.loadProducts();
    })

  }


  deleteProduct(id: number): void 
  {

    const confirmed = confirm('Are you sure you want to delete this product?');

    if (!confirmed) {
      return;
    }

    this.sellerProductService.deleteProduct(id).subscribe({
        next: (response) => {
          this.toasterService
            .showSuccessToast(
              response.message ||
              'Product deleted successfully'
            );

          this.loadProducts();
        },
        error: (error) => {
          this.toasterService
            .showWarningToast(
              error?.error?.message ||
              'Unable to delete product'
            );
        }
      });
  }

}