import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ProductsComponent } from './pages/products/products.component';
import { LoginComponent } from './pages/login/login.component';
import { CartComponent } from './pages/cart/cart.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { UsersComponent } from './pages/users/users.component';
import { ProductDetailComponent } from './pages/product-detail/product-detail.component';
import { PurchaseDetailComponent } from './pages/purchase-detail/purchase-detail.component';
import { PurchasesComponent } from './pages/purchases/purchases.component';

const routes: Routes = [
  { path: 'products', component: ProductsComponent},
  { path: 'products/:id', component: ProductDetailComponent },
  { path: '', component: ProductsComponent},
  { path: 'login', component: LoginComponent},
  { path: 'cart', component: CartComponent},
  { path: 'profile', component: ProfileComponent},
  { path: 'users', component: UsersComponent},
  { path: 'purchases/:id', component: PurchaseDetailComponent},
  { path: 'purchases', component: PurchasesComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
