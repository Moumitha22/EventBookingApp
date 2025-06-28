import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookingResponse } from '../../models/booking-response.model';
import { EventBookingSummary } from '../../models/event-booking-summary';
@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private baseUrl = 'http://localhost:5062/api/v1/Booking';

  constructor(private http: HttpClient) {}

  // Book an event
  bookEvent(eventId: string, seatCount: number): Observable<BookingResponse> {
    const params = new HttpParams()
      .set('eventId', eventId)
      .set('seatCount', seatCount);
    return this.http.post<BookingResponse>(`${this.baseUrl}/book`, null, { params });
  }

  getMyBookings(): Observable<BookingResponse[]> {
    return this.http.get<BookingResponse[]>(`${this.baseUrl}/my`);
  }

  getAllBookings(): Observable<BookingResponse[]> {
    return this.http.get<BookingResponse[]>(this.baseUrl);
  }

  getBookingById(id: string): Observable<BookingResponse> {
    return this.http.get<BookingResponse>(`${this.baseUrl}/${id}`);
  }

  getBookingsByEvent(eventId: string): Observable<BookingResponse[]> {
    return this.http.get<BookingResponse[]>(`${this.baseUrl}/event/${eventId}`);
  }

  getBookingSummary(): Observable<{ message: string; data: EventBookingSummary[] }> {
    return this.http.get<{ message: string; data: EventBookingSummary[] }>(`${this.baseUrl}/summary`);
  }
}
