import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { MyProductsComponent } from './pages/my-products/my-products.component';
import { AuthGuard } from 'src/app/core/guards/auth.guard';
import { ProductformComponent } from './pages/productform/productform.component';

const routes: Routes = [
    {
    path: '',
    children: [
      { path: 'dashboard', component: DashboardComponent, },
      { path: 'my-Products', component: MyProductsComponent, },
      { path: 'product/add',component: ProductformComponent },
      { path: 'product/edit/:id',component: ProductformComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full', }
      ]
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SellerRoutingModule { }
