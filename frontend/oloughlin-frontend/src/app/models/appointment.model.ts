export class Appointment {
  id: string;
  dateTime: string;
  status: string;
  customerName: string;
  customerEmail: string;

  constructor() {
    this.id = '';
    this.dateTime = '';
    this.status = '';
    this.customerName = '';
    this.customerEmail = '';
  }
}
