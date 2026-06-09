import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.module')
        .then(m => m.AuthModule)
  },
  {
    path: '',
    redirectTo: 'auth/login',
    pathMatch: 'full'
  },
  {
    path: 'products',
    loadChildren:()=>import('./features/products/products.module').then(m=>m.ProductsModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'cart',
    loadChildren: () =>
      import('./features/cart/cart.module')
        .then(m => m.CartModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'checkout',
    loadChildren: () =>
      import('./features/checkout/checkout.module')
        .then(m => m.CheckoutModule),
    canActivate: [AuthGuard]
  },

  {
    path: 'orders',
    loadChildren: () =>
      import('./features/orders/orders.module')
        .then(m => m.OrdersModule),
    canActivate: [AuthGuard]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
