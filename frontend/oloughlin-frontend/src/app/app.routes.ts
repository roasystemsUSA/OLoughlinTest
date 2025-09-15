import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Customers } from './customers/customers';
import { Appointments } from './appointments/appointments';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' }, // Redirección
  { path: 'home', component: Home },
  { path: 'customers', component: Customers },
  { path: 'appointments', component: Appointments }
];
