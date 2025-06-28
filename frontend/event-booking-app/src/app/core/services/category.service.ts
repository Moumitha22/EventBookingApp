import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable()
export class CategoryService {
  private baseUrl = `${environment.apiBaseUrl}/api/Category`;

  constructor(private http: HttpClient) {}

  getAllCategories() {
    return this.http.get<{ data: { id: string; name: string }[] }>(`${this.baseUrl}`);
  }
}
