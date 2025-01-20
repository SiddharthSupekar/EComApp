import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private https : HttpClient) { }

  private registerUrl = 'https://localhost:7040/api/Register';


  postUser(formData:any):Observable<any>{
    return this.https.post<any>(`${this.registerUrl}`,formData);
  }
  updateUser(username:string, formData:any):Observable<any>{
    return this.https.put<any>(`${this.registerUrl}/update?username=${username}`,formData);
  }
  getUserByEmail(email:string):Observable<any>{
    return this.https.get<any>(`${this.registerUrl}/email?email=${email}`)
  }
  getRoles():Observable<any>{
    return this.https.get<any>(`${this.registerUrl}/roles`)
  }
  changePassword(userDetails:any):Observable<any>{
    return this.https.put<any>(`${this.registerUrl}/changePassword`, userDetails);
  }
  updateAddress(email:string, address:string):Observable<any>{
    return this.https.put<any>(`${this.registerUrl}/update-adderss?email=${email}&address=${address}`, null)
  }
}
