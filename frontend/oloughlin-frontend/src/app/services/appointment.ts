import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Appointment } from '../models/appointment.model';
import { AppConstants } from '../shared/app-constants.constants';

export interface ApiAppointmentResponse {
  result: Appointment[];
  hasError: boolean;
  errorDetails: any;
}

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly apiUrl = AppConstants.APPOINTENTS_ENDPOINT;

  constructor(private http: HttpClient) { }

  getAppointments(): Observable<ApiAppointmentResponse> {
    return this.http.get<ApiAppointmentResponse>(this.apiUrl);
  }

  addAppointment(appointment: Appointment): Observable<Appointment> {
    return this.http.post<Appointment>(this.apiUrl, appointment);
  }

  updateAppointment(appointment: Appointment): Observable<Appointment> {
    return this.http.put<Appointment>(`${this.apiUrl}/${appointment.id}`, appointment);
  }

  deleteAppointment(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }  
}
