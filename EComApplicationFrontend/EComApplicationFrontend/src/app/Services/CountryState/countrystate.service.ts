import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { count, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CountrystateService {

  constructor(private http : HttpClient) { }

  private url = 'https://localhost:7040/api/CountryState';

  getCountries():Observable<any>{
    return this.http.get<any>(`${this.url}/countries`)
  }
  getStateByCountryId(countryId:number|string):Observable<any>{
    return this.http.get<any>(`${this.url}/country/states?countryId=${countryId}`)
  }

}
