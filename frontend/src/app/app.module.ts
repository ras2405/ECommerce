import { NgModule  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { SortableModule } from 'ngx-bootstrap/sortable';
import { NgbModal, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProductsComponent } from './pages/products/products.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { LoginComponent } from './pages/login/login.component';
import { CartComponent } from './pages/cart/cart.component';
import { ProfileComponent } from './pages/profile/profile.component';

import { ProductDetailComponent } from './pages/product-detail/product-detail.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { SignupFormComponent } from './components/signup-form/signup-form.component';
import { UsersComponent } from './pages/users/users.component';
import { CartItemComponent } from './components/cart-item/cart-item.component';
import { CartCheckoutComponent } from './components/cart-checkout/cart-checkout.component';
import { PurchaseDetailComponent } from './pages/purchase-detail/purchase-detail.component';
import { PurchaseItemComponent } from './components/purchase-item/purchase-item.component';
import { PurchaseSummaryComponent } from './components/purchase-summary/purchase-summary.component';
import { PurchasesComponent } from './pages/purchases/purchases.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ErrorModalComponent } from './components/error-modal/error-modal.component';
import { ErrorModalService } from './services/error-modal.service';

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent,
    NavBarComponent,
    LoginComponent,
    CartComponent,
    ProfileComponent,
    UsersComponent,
    ProductDetailComponent,
    LoginFormComponent,
    SignupFormComponent,
    CartItemComponent,
    CartCheckoutComponent,
    PurchaseDetailComponent,
    PurchaseItemComponent,
    PurchaseSummaryComponent,
    PurchasesComponent,
    ErrorModalComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModalModule,
    SortableModule.forRoot(),
    NgbModule,
    BrowserAnimationsModule
  ],
  providers: [ErrorModalService],
  bootstrap: [AppComponent]
})
export class AppModule { }
