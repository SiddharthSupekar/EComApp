import { Component } from '@angular/core';
import { ProductService } from '../../Services/Product/product.service';
import { CartService } from '../../Services/Cart/cart.service';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedServiceService } from '../../Services/SharedService/shared-service.service';
import Swal from 'sweetalert2'
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-marketplace',
  standalone: true,
  imports: [CurrencyPipe, ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './user-marketplace.component.html',
  styleUrls: ['./user-marketplace.component.css']
})
export class UserMarketplaceComponent {

  productDetails: any = [];
  cartItems: any = [];
  itemCounts: number = 0;
  itemAdded: { [key: number]: boolean } = {}; 
  isAvailable:boolean = false;

  constructor(private toastr : ToastrService,
              private productService: ProductService,
              private cartService: CartService, 
              private sharedService : SharedServiceService) {}

  ngOnInit(): void {

    this.getProducts();
    this.loadCartItems();
    
  }

  getProducts(): void {
    this.productService.getAvailableProductsForCustomer().subscribe({
      next: (value: any) => {
        this.isAvailable = true
        this.productDetails = value;
      },
      error: (err) => {
        console.error("Error fetching products", err);
      }
    });
  }

  loadCartItems(): void {

    const cartId = parseInt(sessionStorage.getItem('cartId') || '0', 10);
    this.cartService.getAllProducts(cartId).subscribe({
      next: (value: any) => {
        this.isAvailable = true
        this.cartItems = value;
        this.itemCounts = 0
        for(let i=0; i<this.cartItems.length; i++){
            this.itemCounts = this.itemCounts + this.cartItems[i].quantity
        }
        this.sharedService.setItemsCount(this.itemCounts);
        
      },
      error: (err) => {

        console.error("Error fetching cart items", err);

      }
    });
  }

  // Add product to cart
  addToCart(product: any): void {
    const cartId = parseInt(sessionStorage.getItem('cartId') || '0', 10);
    const userId = parseInt(sessionStorage.getItem('userId') || '0', 10);

    const details = { cartId: cartId, productId: product.id, quantity: 1 };

    this.cartService.addToCart(userId, details).subscribe({
      next: () => {
        // Swal.fire({
        //   position:'top-right',
        //   icon: "success",
        //   title: "Item Added",
        //   showConfirmButton: false,
        //   timer: 700
        // });
        this.toastr.success("Item Added successfully")
        this.loadCartItems();
        this.itemAdded[product.id] = true; 
        this.isAvailable = true
      },
      error: (err) => {
        this.isAvailable = false
        console.error("Error adding to cart", err);
      }
    });
  }

  cartQuantity(product: any): number {
    const item = this.cartItems.find((item: any) => item.productId === product.id);
    return item ? item.quantity : 0; // Return the quantity if found, else 0
  }
  
  // Check if the product is already added to the cart
  checkIfAdded(product: any): boolean {
    return this.itemAdded[product.id] || false; // Return true if the product was added
  }
}
