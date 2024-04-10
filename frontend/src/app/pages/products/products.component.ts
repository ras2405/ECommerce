import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/services/products.service';
import { NgbOffcanvas, NgbModal, ModalDismissReasons } from '@ng-bootstrap/ng-bootstrap';
import { LoginService } from 'src/app/services/session.service';
import { Router } from '@angular/router';
import { Product } from 'src/app/Interfaces/product';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  constructor(
    private productsService: ProductsService,
    private offcanvasService: NgbOffcanvas,
    private login: LoginService,
    private modalService: NgbModal,
    private router: Router,
    private errorModalService: ErrorModalService
  ) { }
  products?: Product[] = this.productsService.returnedProducts;

  errorResponse: HttpErrorResponse["error"];
  newProd: Product = {
    id: '',
    name: '',
    description: '',
    price: 0,
    stock: 0,
    category: { categoryName: '' },
    brand: { brandName: '' },
    colors: [{ colorName: '' }],
    promotionExcluded: false
  };
  closeResult = '';

  currentCategory?: string;
  currentBrand?: string;
  currentText?: string;
  min?: number;
  max?: number;
  promo?: boolean;

  categories: string[] = ['Pants', 'Shirts', 'Shoes', 'Accesories', 'Top', 'Bottoms', 'Fullbody'];
  brands: string[] = ['Zara', 'Daniel Cassin', 'Piece of Cake', 'Indian', 'Legacy', 'Lacoste', 'Adidas'];
  colors: string[] = ['Red', 'Blue', 'Green', 'Yellow', 'Black', 'White',
    'Gray', 'Brown', 'Purple', 'Orange', 'Pink'];

  selectedColors: { [key: string]: boolean } = {};

  ngOnInit(): void {
    this.productsService.getProducts().subscribe(data => { this.products = data; });
  }

  open(offcanvas: any) {
    this.offcanvasService.open(offcanvas);
  }

  selectCategory(category?: string): void {
    this.currentCategory = category;
  }

  selectBrand(category?: string): void {
    this.currentCategory = category;
  }

  dismiss() {
    this.offcanvasService.dismiss();
  }

  filter() {
    this.productsService.getFilteredProducts(
      this.currentCategory, this.currentBrand, this.currentText, this.min, this.max, this.promo
    ).subscribe(data => {
      this.offcanvasService.dismiss();
      this.products = data;
    },
    (error: HttpErrorResponse) => {
      if (error.status == 0 || error.status == 500){
        this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
      }
    });
  }

  isAdmin(): boolean {
    return this.login.isAdmin();
  }

  openModal(content: any) {
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
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

  saveSelectedColors() {
    const selectedColorsArray = Object.keys(this.selectedColors).filter(color => this.selectedColors[color]);
    console.log('Selected colors:', selectedColorsArray);
    for(let i = 0; i < selectedColorsArray.length; i++) {
      this.newProd.colors[i] = { colorName: selectedColorsArray[i] };
    }
  }

  saveProduct() {
    console.log('Saving product:', this.newProd);
    this.saveSelectedColors();
    this.productsService.addProduct(this.newProd).subscribe(
      (data) => {
        console.log('Product added successfully', data);
        this.errorResponse = null;
        this.modalService.dismissAll();
        this.router.navigate(['/products']);
        window.location.reload();
      },
      (error: HttpErrorResponse) => {
        if (error.status == 0 || error.status == 500){
          this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
        }
        console.error('An error occurred adding the product, try again', error);
        this.errorResponse = error.error;
      }
    );
  }
}

