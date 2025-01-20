import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private loginUrl = 'https://localhost:7040/api/Login';
          
  constructor(private https : HttpClient) { }

  postLogin(  loginDetails : any):Observable<any>{
    return this.https.post<any>(this.loginUrl,loginDetails)
  }

  verifyOtp(verifyDetails:any):Observable<any>{
    return this.https.post<any>(`${this.loginUrl}/otpVerify`,verifyDetails)
  }

  forgotPassword(email:string):Observable<any>{
    return this.https.get<any>(`${this.loginUrl}/forgotPassword?email=${email}`)
  }
}
