import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { MyProductsComponent } from './pages/my-products/my-products.component';
import { SellerRoutingModule } from './seller-routing.model';
import { ProductformComponent } from './pages/productform/productform.component';



@NgModule({
  declarations: [
    DashboardComponent,
    MyProductsComponent,
    ProductformComponent
  ],
  imports: [
    CommonModule,
    SellerRoutingModule
  ]
})
export class SellerModule { }
