import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private http : HttpClient) { }

  private cartUrl = 'https://localhost:7040/api/Cart'

  getAllProducts(cartId:number):Observable<any>{  
    return this.http.get<any>(`${this.cartUrl}/get-cart-items?cartId=${cartId}`);
  }
  addToCart(userId:number, productDetails:any):Observable<any>{
    return this.http.post<any>(`${this.cartUrl}/add-to-cart?userId=${userId}`,productDetails)
  }
  reduceQuantity(cartId:number, productId:number):Observable<any>{
    return this.http.put<any>(`${this.cartUrl}/reduce-quantity?cartId=${cartId}&productId=${productId}`,null)
  }
  deleteProduct(cartId:number,productId:number):Observable<any>{
    return this.http.delete<any>(`${this.cartUrl}/remove-item?cartId=${cartId}&productId=${productId}`)
  }

}
