import { Component } from '@angular/core';
import { CustomerService } from '../services/customer';
import { Customer } from '../models/customer.model';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-customers',
  imports: [NgIf, NgFor, FormsModule, HttpClientModule],
  templateUrl: './customers.html',
  styleUrls: ['./customers.scss']
})
export class Customers {
  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];
  filterName: string = '';
  filterEmail: string = '';
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  selectedCustomer: Customer | null = null;

  constructor(private customerService: CustomerService) { }

  ngOnInit() {
    this.loadCustomers();
  }

  loadCustomers() {
    this.customerService.getCustomers().subscribe(data => {
      this.customers = data.result ?? [];
      this.applyFilters();
    });
  }

  applyFilters() {
    this.filteredCustomers = this.customers.filter(c =>
      c.name.toLowerCase().includes(this.filterName.toLowerCase()) &&
      c.email.toLowerCase().includes(this.filterEmail.toLowerCase())
    );
  }

  openAddModal() {
    this.selectedCustomer = new Customer();
    this.showAddModal = true;
  }

  openEditModal(customer: Customer) {
    this.selectedCustomer = { ...customer };
    this.showEditModal = true;
  }

  openDeleteModal(customer: Customer) {
    this.selectedCustomer = customer;
    this.showDeleteModal = true;
  }

  closeModals() {
    this.showAddModal = false;
    this.showEditModal = false;
    this.showDeleteModal = false;
    this.selectedCustomer = null;
  }

  saveCustomer(customer: Customer) {
    if (customer.id) {
      this.customerService.updateCustomer(customer).subscribe(() => {
        this.loadCustomers();
        this.closeModals();
      });
    } else {
      this.customerService.addCustomer(customer).subscribe(() => {
        this.loadCustomers();
        this.closeModals();
      });
    }
  }

  deleteCustomer() {
    if (this.selectedCustomer) {
      this.customerService.deleteCustomer(this.selectedCustomer.id).subscribe(() => {
        this.loadCustomers();
        this.closeModals();
      });
    }
  }
}
