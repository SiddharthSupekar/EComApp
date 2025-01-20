import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private url = 'https://localhost:7040/api/Payment/'


  constructor(private http : HttpClient) { }

  verifyCard(cardDetails:any):Observable<any>{
    return this.http.post<any>(`${this.url}card-details`,cardDetails)
  }
  generateInvoice(details:any):Observable<any>{
    return this.http.post<any>(`${this.url}generate-invoice`,details)
  }
  generateReciept(userId:number):Observable<any>{
    return this.http.get<any>(`${this.url}generate-reciept?userId=${userId}`)
  }

}
