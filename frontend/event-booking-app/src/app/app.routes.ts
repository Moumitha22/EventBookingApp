import { Routes } from '@angular/router';
import { Landing } from './pages/landing/landing';
import { LoginComponent } from './pages/login/login';
import { SignupComponent } from './pages/signup/signup';
import { EventsComponent } from './pages/events/events';
import { PostEventComponent } from './pages/post-event/post-event';
import { MyBookingsComponent } from './pages/my-bookings/my-bookings';
import { BookingsSummary } from './pages/bookings-summary/bookings-summary';
import { EditEventComponent } from './pages/edit-event/edit-event';
import { BookEventComponent } from './pages/book-event/book-event';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { EventDetailsComponent } from './pages/event-details/event-details';
import { RoleGuard } from './core/guards/role-guard';

export const routes: Routes = [
  { path: '', component: Landing, pathMatch: 'full' },
  { path: 'landing', component: Landing },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'events', component: EventsComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [RoleGuard],
    data: { expectedRoles: ['Admin'] }
  },
  {
    path: 'post-event',
    component: PostEventComponent,
    canActivate: [RoleGuard],
    data: { expectedRoles: ['Admin'] }
  },
  {
    path: 'edit-event/:id',
    component: EditEventComponent,
    canActivate: [RoleGuard],
    data: { expectedRoles: ['Admin'] }
  },
  {
    path: 'bookings-summary',
    component: BookingsSummary,
    canActivate: [RoleGuard],
    data: { expectedRoles: ['Admin'] }
  },
  {
    path: 'my-events',
    component: MyBookingsComponent,
    canActivate: [RoleGuard],
    data: { expectedRoles: ['User'] }
  },
  {
    path: 'book-event/:id',
    component: BookEventComponent,
    canActivate: [RoleGuard],
    data: { expectedRoles: ['User'] }
  },

  {
    path: 'event-details/:id',
    component: EventDetailsComponent 
  }
];
