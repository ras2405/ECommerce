import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Purchase } from 'src/app/Interfaces/purchase';
import { ErrorModalService } from 'src/app/services/error-modal.service';
import { PurchasesService } from 'src/app/services/purchases.service';

@Component({
  selector: 'app-purchase-detail',
  templateUrl: './purchase-detail.component.html',
  styleUrls: ['./purchase-detail.component.css']
})
export class PurchaseDetailComponent {
  purchase?: Purchase;

  constructor(
    private route: ActivatedRoute,
    public purchases: PurchasesService,
    private errorModalService: ErrorModalService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const purchaseId = params['id'];
      this.purchases.getPurchase(purchaseId).subscribe(
        (purchase) => {
          this.purchase = purchase
        },
        (error: HttpErrorResponse) => {
          if (error.status == 0 || error.status == 500){
            this.errorModalService.openErrorModal('Oops! The server is experiencing some difficulties. Please try again later');
          }
          console.error('Error fetching purchase:', error);
        }
      )
    })
  }
}
