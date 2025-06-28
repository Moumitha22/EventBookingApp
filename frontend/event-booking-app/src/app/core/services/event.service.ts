import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { EventUpdateRequest } from '../../models/event-update-request.model';
import { EventModel } from '../../models/event.model';

@Injectable()
export class EventService {
  private baseUrl = `${environment.apiBaseUrl}/api/Event`;

  private http = inject(HttpClient);

  getEventById(id: string): Observable<{ data: EventModel }> {
    return this.http.get<{ data: EventModel }>(`${this.baseUrl}/${id}`);
  }

  getAllEvents(): Observable<{ data: EventModel[] }> {
    return this.http.get<{ data: EventModel[] }>(`${this.baseUrl}`);
  }

  getUpcomingEvents(): Observable<{ data: EventModel[] }> {
    return this.http.get<{ data: EventModel[] }>(`${this.baseUrl}/upcoming`);
  }

  getEventsByCategory(categoryId: string): Observable<{ data: EventModel[] }> {
    return this.http.get<{ data: EventModel[] }>(`${this.baseUrl}/category/${categoryId}`);
  }

  // addEvent(request: EventCreateRequest): Observable<{ data: EventModel }> {
  //   return this.http.post<{ data: EventModel }>(this.baseUrl, request);
  // }

  updateEvent(id: string, request: EventUpdateRequest): Observable<{ data: EventModel }> {
    return this.http.put<{ data: EventModel }>(`${this.baseUrl}/${id}`, request);
  }

  deleteEvent(id: string): Observable<{ data: any }> {
    return this.http.delete<{ data: any }>(`${this.baseUrl}/${id}`);
  }

  uploadImage(eventId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('EventId', eventId);
    formData.append('File', file);

    return this.http.post(`${this.baseUrl}/upload-image`, formData);
  }

  // uploadEventWithImage(event: any, imageFile?: File): Observable<{ data: any }> {
  //   const formData = new FormData();

  //   const eventJson = JSON.stringify(event);
  //   formData.append('EventJson', eventJson);

  //   if (imageFile) {
  //     formData.append('Image', imageFile);
  //   }

  //   return this.http.post<{ data: any }>(`${this.baseUrl}/upload`, formData);
  // }
  uploadEventWithImage(formData: FormData): Observable<{ data: any }> {
    return this.http.post<{ data: any }>(`${this.baseUrl}/upload`, formData);
  }


}
