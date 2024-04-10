import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ProductsService } from 'src/app/services/products.service';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { LoginService } from 'src/app/services/session.service';
import { Product } from 'src/app/Interfaces/product';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent {
  currentProduct?: Observable<Product>;
  currentId: string = '';
  promotionExcluded: string = '';

  errorResponse?: HttpErrorResponse["error"];
  closeResult = '';
  newProd: Product = {} as Product;
  selectedProduct?: Product;
  modalRef?: NgbModalRef;

  categories: string[] = ['Pants', 'Shirts', 'Shoes', 'Accesories'];
  brands: string[] = ['Zara', 'Daniel Cassin'];
  colors: string[] = ['Red', 'Blue', 'Green', 'Yellow', 'Black', 'White',
    'Gray', 'Brown', 'Purple', 'Orange', 'Pink'];

  product?: Product;

  constructor(
    private route: ActivatedRoute,
    private productsService: ProductsService,
    private login: LoginService,
    private modalService: NgbModal,
    private router: Router,
    private errorModalService: ErrorModalService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.currentId = params['id'];
      this.currentProductDetails();
    });
  }

  currentProductDetails(): Observable<any> {
    this.currentProduct = this.productsService.getSpecificProduct(this.currentId);
    this.currentProduct.subscribe(
      (product: any) => {
        console.log('Current Product:', product);
        this.product = product;
        this.newProd = product;
        console.log('new Product:', this.newProd);
      },
      (error: any) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('Error fetching current product:', error);
      }
    );
    this.promotion();
    return this.currentProduct;
  }

  promotion() {
    this.currentProduct?.subscribe((product: Product) => {
      if (!product.promotionExcluded) {
        this.promotionExcluded = "Sorry, this product has no promotion available";
      } else {
        this.promotionExcluded = "This product has a promotion available!";
      }
    });
  }

  isAdmin(): boolean {
    return this.login.isAdmin();
  }

  openModal(modal: any) {
    this.modalService.open(modal, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      (result) => {
        this.closeResult = `Closed with: ${result}`;
      },
      (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      },
    );
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  editProduct() {
    this.newProd.id = this.currentId;
    this.currentProduct = this.productsService.updateProduct(this.newProd);
    window.location.reload();
  }

  openDeleteModal(modal: any) {
    this.modalService.open(modal, { ariaLabelledBy: 'modal-basic-title' }).result.then(
      (result) => {
        this.closeResult = `Closed with: ${result}`;
      },
      (reason) => {
        this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      },
    );
  }

  deleteProduct(): void {
    this.productsService.deleteProduct(this.currentId).subscribe(
      (data: Product) => {
        console.log('Product deleted successfully', data);
        this.errorResponse = null;
        this.modalService.dismissAll();
        this.router.navigate(['/products']);
      },
      (error) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error occurred while deleting the product, try again', error);
        this.errorResponse = error;
        this.modalService.dismissAll();
        this.router.navigate(['/products']);   
      }
    );
  }

  addToCart() {
    if (this.product) {
      let cart: Product[] = JSON.parse(localStorage.getItem('cart') || '[]');
      cart.push(this.product);
      localStorage.setItem('cart', JSON.stringify(cart));
      this.router.navigate(['/cart']);
    }
  }

  isBuyerOrBoth(): boolean {
    return this.login.isBuyerOrBoth();
  }
}
