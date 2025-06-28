import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import { RoleGuard } from './core/guards/role-guard';
import { AuthService } from './core/services/auth.service';
import { EventService } from './core/services/event.service';
import { BookingService } from './core/services/booking.service';
import { CategoryService } from './core/services/category.service';
import { AdminService } from './core/services/admin.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthService,
    EventService,
    BookingService,
    CategoryService,
    AdminService,
    RoleGuard
  ]
};
