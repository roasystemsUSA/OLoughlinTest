import { Component } from '@angular/core';
import { AppointmentService, ApiAppointmentResponse } from '../services/appointment';
import { Appointment } from '../models/appointment.model';
import { CustomerService, ApiCustomerResponse } from '../services/customer';
import { Customer } from '../models/customer.model';
import { NgIf, NgFor, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-appointments',
  imports: [NgIf, NgFor, FormsModule, DatePipe],
  templateUrl: './appointments.html',
  styleUrls: ['./appointments.scss']
})
export class Appointments {
  appointments: Appointment[] = [];
  filteredAppointments: Appointment[] = [];
  pagedAppointments: Appointment[] = [];
  customers: Customer[] = [];
  filterCustomer: string = '';
  filterStatus: string = '';
  filterStartDate: string = '';
  filterEndDate: string = '';
  showAddModal = false;
  showEditModal = false;
  showDeleteModal = false;
  selectedAppointment: Appointment | null = null;

  // Autocomplete for customer selection in add modal
  customerSearch: string = '';
  showCustomerDropdown = false;
  filteredCustomers: Customer[] = [];

  // Paginación
  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;

  // Ordenamiento
  sortColumn: string = 'dateTime';
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(
    private appointmentService: AppointmentService,
    private customerService: CustomerService
  ) { }

  ngOnInit() {
    this.loadAppointments();
    this.loadCustomers();
  }

  loadAppointments() {
    this.appointmentService.getAppointments().subscribe((data: ApiAppointmentResponse) => {
      this.appointments = data.result ?? [];
      this.applyFilters();
    });
  }

  loadCustomers() {
    this.customerService.getCustomers().subscribe((data: ApiCustomerResponse) => {
      this.customers = data.result ?? [];
      this.filteredCustomers = [...this.customers];
    });
  }

  applyFilters() {
    this.filteredAppointments = this.appointments.filter(a => {
      const customerName = a.customerName ? a.customerName.toLowerCase() : '';
      const customerMatch = customerName.includes(this.filterCustomer.toLowerCase());
      const statusMatch = this.filterStatus ? a.status === this.filterStatus : true;
      const dateMatch =
        (!this.filterStartDate || new Date(a.dateTime) >= new Date(this.filterStartDate)) &&
        (!this.filterEndDate || new Date(a.dateTime) <= new Date(this.filterEndDate));
      return customerMatch && statusMatch && dateMatch;
    });

    // Ordenamiento seguro
    this.filteredAppointments.sort((a, b) => {
      let valueA = '';
      let valueB = '';
      if (this.sortColumn === 'customer') {
        valueA = a.customerName ? a.customerName.toLowerCase() : '';
        valueB = b.customerName ? b.customerName.toLowerCase() : '';
      } else if (this.sortColumn === 'status') {
        valueA = a.status ? a.status.toLowerCase() : '';
        valueB = b.status ? b.status.toLowerCase() : '';
      } else if (this.sortColumn === 'dateTime') {
        valueA = a.dateTime || '';
        valueB = b.dateTime || '';
      }
      if (valueA < valueB) return this.sortDirection === 'asc' ? -1 : 1;
      if (valueA > valueB) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });

    this.totalPages = Math.ceil(this.filteredAppointments.length / this.pageSize) || 1;
    this.currentPage = Math.min(this.currentPage, this.totalPages);
    this.updatePagedAppointments();
  }

  updatePagedAppointments() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.pagedAppointments = this.filteredAppointments.slice(start, end);
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePagedAppointments();
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
    this.selectedAppointment = new Appointment();
    this.selectedAppointment.status = 'scheduled';
    this.customerSearch = '';
    this.filteredCustomers = [...this.customers];
    this.showAddModal = true;
  }

  openEditModal(appointment: Appointment) {
    this.selectedAppointment = { ...appointment };
    this.showEditModal = true;
  }

  openDeleteModal(appointment: Appointment) {
    this.selectedAppointment = appointment;
    this.showDeleteModal = true;
  }

  closeModals() {
    this.showAddModal = false;
    this.showEditModal = false;
    this.showDeleteModal = false;
    this.selectedAppointment = null;
    this.customerSearch = '';
    this.showCustomerDropdown = false;
  }

  saveAppointment(appointment: Appointment) {
    if (appointment.id) {
      this.appointmentService.updateAppointment(appointment).subscribe(() => {
        this.loadAppointments();
        this.closeModals();
      });
    } else {
      appointment.status = 'scheduled';
      this.appointmentService.addAppointment(appointment).subscribe(() => {
        this.loadAppointments();
        this.closeModals();
      });
    }
  }

  deleteAppointment() {
    if (this.selectedAppointment) {
      this.appointmentService.deleteAppointment(this.selectedAppointment.id).subscribe(() => {
        this.loadAppointments();
        this.closeModals();
      });
    }
  }

  // Autocomplete logic for customer selection
  filterCustomerOptions() {
    const search = this.customerSearch.toLowerCase();
    this.filteredCustomers = this.customers.filter(c =>
      c.name.toLowerCase().includes(search)
    );
    this.showCustomerDropdown = true;
  }

  selectCustomer(customer: Customer) {
    if (this.selectedAppointment) {
      this.selectedAppointment.customerName = customer.name;
      this.selectedAppointment.customerEmail = customer.email;
      this.customerSearch = customer.name;
      this.showCustomerDropdown = false;
    }
  }

  hideDropdownWithDelay() {
    setTimeout(() => this.showCustomerDropdown = false, 200);
  }
}
