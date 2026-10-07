import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import {  SellerMyProductListComponent } from './pages/my-products/my-products.component';
import { SellerRoutingModule } from './seller-routing.model';
import {  SellerProductAddEditComponent } from './pages/seller-product-add-edit/seller-product-add-edit.component';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    DashboardComponent,
    SellerMyProductListComponent,
    SellerProductAddEditComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SellerRoutingModule
  ]
})
export class SellerModule { }
