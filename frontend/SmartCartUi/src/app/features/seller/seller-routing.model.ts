import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AuthGuard } from 'src/app/core/guards/auth.guard';
import { SellerMyProductListComponent } from './pages/my-products/my-products.component';
import { SellerProductAddEditComponent } from './pages/seller-product-add-edit/seller-product-add-edit.component';

const routes: Routes = [
    {
    path: '',
    children: [
      { path: 'dashboard', component: DashboardComponent, },
      { path: 'my-Products', component: SellerMyProductListComponent, },
      
      { path: '', redirectTo: 'dashboard', pathMatch: 'full', }
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SellerRoutingModule { }
