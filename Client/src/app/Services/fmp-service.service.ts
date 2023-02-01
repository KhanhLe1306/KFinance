import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class FMPServiceService {
  private url: string = 'https://localhost:7035/api/fmp/';
  Routes = {
    getROI: () => 'getROI',
  }
  constructor(private http: HttpClient) { }
  
  getROI(body: any) {
    let url = this.url + this.Routes.getROI();
    return this.http.post(url, body);
  }
}
