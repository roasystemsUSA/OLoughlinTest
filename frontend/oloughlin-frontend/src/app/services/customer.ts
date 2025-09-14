import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer.model';
import { AppConstants } from '../shared/app-constants.constants';

export interface ApiCustomerResponse {
  result: Customer[];
  hasError: boolean;
  errorDetails: any;
}

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private readonly apiUrl = AppConstants.CUSTOMERS_ENDPOINT;

  constructor(private http: HttpClient) { }

  getCustomers(): Observable<ApiCustomerResponse> {
    return this.http.get<ApiCustomerResponse>(this.apiUrl);
  }

  addCustomer(customer: Customer): Observable<Customer> {
    return this.http.post<Customer>(this.apiUrl, customer);
  }

  updateCustomer(customer: Customer): Observable<Customer> {
    return this.http.put<Customer>(`${this.apiUrl}/${customer.id}`, customer);
  }

  deleteCustomer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
