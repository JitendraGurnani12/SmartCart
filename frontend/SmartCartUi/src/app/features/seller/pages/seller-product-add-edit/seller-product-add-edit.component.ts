import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SellerProductService } from '../../services/seller-product.service';
import { CategoryService } from 'src/app/core/services/category.service';
import { ToasterService } from 'src/app/shared/toaster/service/toaster.service';
import { Category } from 'src/app/models/category.model';
import { Product } from 'src/app/features/products/models/product.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-seller-product-add-edit',
  templateUrl: './seller-product-add-edit.component.html',
  styleUrls: ['./seller-product-add-edit.component.css']
})
export class SellerProductAddEditComponent
  implements OnInit {

  product: Product = {
    id: 0,
    name: '',
    description: '',
    price: 0,
    imageUrl: '',
    categoryId: 0,
    sellerId: ''
  };

  categories: Category[] = [];

  isEditMode = false;
  editId : number = 0;

  productId: number | null = null;
  selectedImage: File | null = null;
imagePreview: string | null = null;

  isSubmitting = false;


  constructor(
    private sellerProductService:
      SellerProductService,
      private modal : NgbActiveModal,

    private categoryService:
      CategoryService,

    private toasterService:
      ToasterService,

    private route:
      ActivatedRoute,

    private router:
      Router
  ) {}


  ngOnInit(): void {

    this.loadCategories();
    this.editId = this.sellerProductService.getEditId();
    if (this.editId > 0) {
      this.isEditMode = true;
      
      this.loadProduct(this.editId);
    }
  }

  loadCategories(): void 
  {
    this.categoryService.getAllCategories().subscribe({

        next: (response) => {

          this.categories =
            response.data;

        },
        error: (error) => {
          this.toasterService.showWarningToast(
            error?.error?.message ||
            'Unable to load categories'
          );
        }
      });
  }


  loadProduct(id: number): void 
  {
    this.sellerProductService.getProductById(id).subscribe({next: (response) => {
      debugger;
          this.product = response;
          if (this.product.imageUrl) {
            this.imagePreview =
              this.product.imageUrl;
          }
        },
        error: (error) => {
          this.toasterService.showWarningToast(
            error?.error?.message ||
            'Unable to load product'
          );
          this.goBack();
        }
      });
  }


  saveProduct(form: NgForm): void 
  {
    if (form.invalid) {
      form.control.markAllAsTouched();
      return;
    }
    this.isSubmitting = true;
    if (this.isEditMode) {
      this.updateProduct();
    }
    else {
      this.addProduct();
    }
  }

  private createProductFormData(): FormData {

  const formData = new FormData();

  formData.append(
    'Name',
    this.product.name || ''
  );

  formData.append(
    'Description',
    this.product.description || ''
  );

  formData.append(
    'Price',
    this.product.price?.toString() || '0'
  );

  formData.append(
    'CategoryId',
    this.product.categoryId?.toString() || '0'
  );


  // Add Id only during Edit
  if (this.isEditMode) {

    formData.append(
      'Id',
      this.product.id.toString()
    );
  }


  // Add image only if user selected a new image
  if (this.selectedImage) {

    formData.append(
      'image',
      this.selectedImage,
      this.selectedImage.name
    );
  }


  return formData;
}


  addProduct(): void {

    const formData = this.createProductFormData();

    this.sellerProductService.addProduct(formData).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.toasterService.showSuccessToast(
          response.message ||
          'Product added successfully'
        );
        this.goBack();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.toasterService.showWarningToast(
          error?.error?.message ||
          'Unable to add product'
        );
      }
    });
  }


  updateProduct(): void {

    const formData =
      this.createProductFormData();

    this.sellerProductService.updateProduct(formData).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.toasterService.showSuccessToast(
          response.message ||
          'Product updated successfully'
        );
        this.goBack();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.toasterService.showWarningToast(
          error?.error?.message ||
          'Unable to update product'
        );
      }
    });
  }


  goBack(): void {
    this.modal.dismiss();
  }
  onImageSelected(event: Event): void {

    const input = event.target as HTMLInputElement;
    if (!input.files ||
      input.files.length === 0) {
      return;
    }
    const file = input.files[0];
    // Validate file type
    const allowedTypes = ['image/jpeg','image/png'];
    if (!allowedTypes.includes(file.type)) {
      this.selectedImage = null;
      this.toasterService.showWarningToast(
        'Only JPG, JPEG and PNG images are allowed.'
      );
      input.value = '';
      return;
    }
    // Validate file size
    const maxSize = 5 * 1024 * 1024;
    if (file.size > maxSize) {
      this.selectedImage = null;
      this.toasterService.showWarningToast('Image size cannot exceed 5 MB.');
      input.value = '';
      return;
    }
    // Store selected file
    this.selectedImage = file;
    
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

}