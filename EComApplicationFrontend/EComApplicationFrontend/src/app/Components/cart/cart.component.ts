import { Component } from '@angular/core';
import { CartService } from '../../Services/Cart/cart.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { RegisterService } from '../../Services/Register/register.service';
import * as bootstrap from 'bootstrap'
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PaymentService } from '../../Services/Payment/payment.service';
import Swal from 'sweetalert2'
import { SharedServiceService } from '../../Services/SharedService/shared-service.service';
import { ProductService } from '../../Services/Product/product.service';


@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,CurrencyPipe],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {

  constructor(private productService:ProductService, private paymentService : PaymentService , private cartService : CartService, private registerService : RegisterService ,private router :Router, private sharedService : SharedServiceService){}

  cartItems:any
  itemsCounts:number = 0;
  isNull:boolean = false;
  outOfStock:boolean= false
  profileData:any;
  address:string = '';
  formDetails:any;
  addressDetails:string = '';
  products:any;


  paymentForm:FormGroup = new FormGroup({
    cardNumber: new FormControl('',[Validators.required, Validators.pattern('^[0-9]{10}$')]),
    year:new FormControl('',Validators.required),
    month: new FormControl('',Validators.required),
    // expiryDate : new FormControl('',[Validators.required]),
    cvv : new FormControl('',Validators.required)
  })
  addressForm:FormGroup = new FormGroup({
    address : new FormControl('',Validators.required)
  })

  ngOnInit():void{
    this.getAllItems();
  }

  getProductDetails(){
    this.productService.getAvailableProductsForCustomer().subscribe({
      next:(value:any)=>{
        this.products = value;
      }
    })
  }

  getAllItems(){

    const cartId = parseInt(sessionStorage.getItem("cartId") || "0",10)
    this.cartService.getAllProducts(cartId).subscribe({
      next:(value:any)=>{

        this.cartItems = value;
        this.itemsCounts = 0
        for(let i=0; i<this.cartItems.length; i++){
          this.itemsCounts = this.itemsCounts + this.cartItems[i].quantity
      }
      this.sharedService.setItemsCount(this.itemsCounts);
      },
      error:(error:any)=>{

        console.log(error);
        
      }
    })
  }

  decreaseQuantity(item:any){

    const cartId = parseInt(sessionStorage.getItem("cartId") || "0", 10);
    this.cartService.reduceQuantity(cartId, item.productId).subscribe({
      next:(value:any)=>{
        
        this.getAllItems();
        this.outOfStock=false;
        // this.itemsCount = this.itemsCount-1
        // this.sharedService.setItemsCount(this.itemCounts);
      }
    })
  }

  increaseQuantity(item:any){

    const cartId = parseInt(sessionStorage.getItem("cartId") || "0", 10);
    const userId = parseInt(sessionStorage.getItem("userId") || "0", 10);
    const details = {
      cartId : cartId,
      productId:item.productId,
      quantity:1
    }

    this.cartService.addToCart(userId,details).subscribe({
      next:(value:any)=>{

        this.getAllItems();
      },
      error:(error:any)=>{
       
   
        this.outOfStock = true;
      }
    })

  }

  removeFromCart(item:any){
    const cartId = parseInt(sessionStorage.getItem("cartId") || "0" , 10)
    const productId = item.productId

    this.cartService.deleteProduct(cartId, productId).subscribe({
      next:(value:any)=>{
        Swal.fire({
          position: "top-end",
          icon: "success",
          title: "Product removed from cart",
          showConfirmButton: false,
          timer: 1500
        });
        this.getAllItems();
      }
    })

  }

  calculateTotal(): number {
    let total = 0;
    
    
    this.cartItems.forEach((item: any) => { 

      total += item.sellingPrice * item.quantity; 
    });
    return total;
  }

  toCheckout(cartItems:any){
    let outOfStockItems: string[] = []; 
    this.getProductDetails();

    cartItems.forEach((item: any) => {
      const product = this.products.find((product: any) => product.id === item.productId);
      if (product && product.stock < item.quantity) {
        outOfStockItems.push(product.name || `Product ID: ${product.id}`);
      }
    });

    if(outOfStockItems.length > 0){
      alert(`following items are out of stock:\n${outOfStockItems.join(", ")}`)
    }
    else{
      const email = sessionStorage.getItem("email") || "";
      this.registerService.getUserByEmail(email).subscribe({
        next:(value:any)=>{
          this.profileData = value;
      
          if(this.profileData.address !== null){
            this.openPaymentModal();
          }
          else{
            this.openAddressModal();
            
          }
        }
      })
        
    }

  }

  updateAddress(){
    this.addressDetails = this.addressForm.value;
    const email = sessionStorage.getItem("email") || ''
    this.registerService.updateAddress(email,this.addressDetails).subscribe({
      next:(value:any)=>{
        Swal.fire({
          icon:"success",
          title:"Address saved",
          showConfirmButton:false,
          timer:1000
      });

      },
      error:(error:any)=>{
        Swal.fire({
          icon: "error",
          title: "Oops...",
          text: "Could not save the address. Try later",
        });
      }
    })
  }

  closeAddressModal() {
    const otpModalElement = document.getElementById('addressModal');
    if (otpModalElement) {
      const otpModalInstance = bootstrap.Modal.getInstance(otpModalElement);
      otpModalInstance?.hide();
    }
  }
  
  openAddressModal() {
    const modalElement = document.getElementById('addressModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }
  
  openPaymentModal() {
    const modalElement = document.getElementById('paymentModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  closePaymentModal() {
    const otpModalElement = document.getElementById('paymentModal');
    if (otpModalElement) {
      const otpModalInstance = bootstrap.Modal.getInstance(otpModalElement);
      otpModalInstance?.hide();
    }
  }

  paymentCheck():void{
    debugger
    this.formDetails=this.paymentForm.value
    
    const date = new Date(`${this.formDetails.year}-01-${this.formDetails.month}`);
    const formattedDate = date.toISOString().split('T')[0];
    const details = {
      cardNumber : this.formDetails.cardNumber,
      expiryDate : formattedDate,
      cvv : this.formDetails.cvv
    }

    this.paymentService.verifyCard(details).subscribe({
      next:(value:any)=>{
        this.closePaymentModal()
        const userId = parseInt(sessionStorage.getItem("userId") || "0",10)
        const details = {
          userId:userId,
          subTotal: this.calculateTotal()
        }
        this.paymentService.generateInvoice(details).subscribe({
          next:(value:any)=>{
            this.router.navigateByUrl("/layout/invoice");
            this.sharedService.setItemsCount(0);
          }
        });
      }
    });

  }

  reRoute(){
    this.router.navigateByUrl('/layout/customer-marketplace')
  }
}

