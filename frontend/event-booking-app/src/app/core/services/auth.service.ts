import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../../models/login-request.model';
import { LoginResponse } from '../../models/login-response.mode';
import { RegisterRequest } from '../../models/register-request.model';
import { Observable, BehaviorSubject, tap, map, distinctUntilChanged } from 'rxjs';
import { environment } from '../../environments/environment';

export interface AuthUser {
  id: string;
  email: string;
  role: string;
}

@Injectable()
export class AuthService {
  private baseUrl = `${environment.apiBaseUrl}/api/Authentication`;
  private accessTokenKey = 'accessToken';
  private loggingOut = false;

  private userSubject = new BehaviorSubject<AuthUser | null>(this.getUserFromToken());
  user$ = this.userSubject.asObservable();

  userRole$ = this.user$.pipe(
    map(u => u?.role ?? null),
    distinctUntilChanged()
  );

  isLoggedIn$ = this.user$.pipe(
    map(u => !!u),
    distinctUntilChanged()
  );


  private http =  inject(HttpClient);

  register(data: RegisterRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, data);
  }

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, data, { withCredentials: true })
      .pipe(tap(res => {
        const token = (res as any)?.data?.accessToken;
        if (token) this.setAccessToken(token);
      }));
  }

  refreshToken(): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/refresh-token`, {}, { withCredentials: true })
      .pipe(tap(res => {
        const token = (res as any)?.data?.accessToken;
        if (token) this.setAccessToken(token);
      }));
  }

  logout(): Observable<any> {
    return this.http.post(`${this.baseUrl}/logout`, {}, { withCredentials: true });
  }

  setLoggingOut(value: boolean): void {
    this.loggingOut = value;
  }

  isLoggingOut(): boolean {
    return this.loggingOut;
  }

  clearLocalState(): void {
    localStorage.removeItem(this.accessTokenKey);
    this.userSubject.next(null);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  get currentUser(): AuthUser | null {
    return this.userSubject.value;
  }

  get currentUserRole(): string | null {
    return this.currentUser?.role ?? null;
  }

  private setAccessToken(token: string): void {
    localStorage.setItem(this.accessTokenKey, token);
    const user = this.getUserFromToken(token);
    this.userSubject.next(user);
  }

  private getUserFromToken(token?: string): AuthUser | null {
    token = token ?? localStorage.getItem(this.accessTokenKey) ?? undefined;
    if (!token) return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload.nameid,
        email: payload.email,
        role: payload.role
      };
    } catch {
      return null;
    }
  }
}
