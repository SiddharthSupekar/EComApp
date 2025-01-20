import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http : HttpClient) { }

  private productUrl = 'https://localhost:7040/api/Product';

  getAllProductsByUser(userId:number):Observable<any>{
    return this.http.get<any>(`${this.productUrl}/userId?userId=${userId}`);
  }
  getAllProductsForCustomer():Observable<any>{
    return this.http.get<any>(`${this.productUrl}/all`)
  }
  getAvailableProducts():Observable<any>{
    return this.http.get<any>(`${this.productUrl}/available-products`)
  }
  getAvailableProductsForCustomer():Observable<any>{
    return this.http.get<any>(`${this.productUrl}/all-for-customer`)
  }
  addProduct(userId:number, productDetails:any):Observable<any>{
    return this.http.post<any>(`${this.productUrl}/add-product?userId=${userId}`,productDetails)
  }
  updateProduct(userId:number, productId:number,productDetails:any):Observable<any>{
    return this.http.put<any>(`${this.productUrl}/update?productId=${productId}`,productDetails)
  }
  deleteProduct(productId:number):Observable<any>{
    return this.http.delete<any>(`${this.productUrl}/delete-product?productId=${productId}`)
  }

}
