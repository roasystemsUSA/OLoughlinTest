import { Component } from '@angular/core';
import { CustomerService } from '../services/customer';
import { Customer } from '../models/customer.model';
import { NgIf, NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customers',
  imports: [NgIf, NgFor, FormsModule],
  templateUrl: './customers.html',
  styleUrls: ['./customers.scss']
})
export class Customers {
  customers: Customer[] = [];
  filteredCustomers: Customer[] = [];
  pagedCustomers: Customer[] = [];
  filterName: string = '';
  filterEmail: string = '';
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  selectedCustomer: Customer | null = null;

  // Paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;

  // Ordenamiento
  sortColumn: string = 'name';
  sortDirection: 'asc' | 'desc' = 'asc';

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
    // Ordenamiento seguro
    this.filteredCustomers.sort((a, b) => {
      let valueA = '';
      let valueB = '';
      if (this.sortColumn === 'name') {
        valueA = a.name.toLowerCase();
        valueB = b.name.toLowerCase();
      } else if (this.sortColumn === 'email') {
        valueA = a.email.toLowerCase();
        valueB = b.email.toLowerCase();
      }
      if (valueA < valueB) return this.sortDirection === 'asc' ? -1 : 1;
      if (valueA > valueB) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });
    this.totalPages = Math.ceil(this.filteredCustomers.length / this.pageSize) || 1;
    this.currentPage = Math.min(this.currentPage, this.totalPages);
    this.updatePagedCustomers();
  }

  updatePagedCustomers() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedCustomers = this.filteredCustomers.slice(start, end);
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePagedCustomers();
  }

  sortBy(column: string) {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }
    this.applyFilters();
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
