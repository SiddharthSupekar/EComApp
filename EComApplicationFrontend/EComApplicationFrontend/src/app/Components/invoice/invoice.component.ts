import { Component } from '@angular/core';
import { PaymentService } from '../../Services/Payment/payment.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-invoice',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css'
})
export class InvoiceComponent {

  constructor(private paymentService : PaymentService){}

  invoiceData : any;

  ngOnInit():void{
    this.getInvoiceData();
  }

  getInvoiceData():void{
    const userId = parseInt(sessionStorage.getItem("userId") || "0", 10);
    this.paymentService.generateReciept(userId).subscribe({
      next:(value:any)=>{
        this.invoiceData = value
      }
    })
  }

}
