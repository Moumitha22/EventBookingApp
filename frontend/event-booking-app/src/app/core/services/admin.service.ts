import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminDashboardSummaryModel } from '../../models/admin-dashboard-summary.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = `${environment.apiBaseUrl}/api/Admin`;

  constructor(private http: HttpClient) {}

  getDashboardSummary(): Observable<AdminDashboardSummaryModel> {
    return this.http.get<AdminDashboardSummaryModel>(`${this.baseUrl}/dashboard-summary`);
  }
}
