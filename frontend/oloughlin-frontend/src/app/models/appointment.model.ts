export class Appointment {
  id: string;
  customerId: string;
  dateTime: string;
  status: string;
  customer: string;
  email: string;

  constructor() {
    this.id = '';
    this.customerId = '';
    this.dateTime = '';
    this.status = '';
    this.customer = '';
    this.email = '';
  }
}
