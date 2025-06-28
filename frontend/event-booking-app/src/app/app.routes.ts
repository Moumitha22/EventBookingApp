import { Routes } from '@angular/router';
import { Landing } from './pages/landing/landing';
import { LoginComponent } from './pages/login/login';
import { SignupComponent } from './pages/signup/signup';
import { EventsComponent } from './pages/events/events';
import { PostEventComponent } from './pages/post-event/post-event';
import { MyBookingsComponent } from './pages/my-bookings/my-bookings';
import { BookingsSummary } from './pages/bookings-summary/bookings-summary';

export const routes: Routes = [
  { path: '', component: Landing, pathMatch: 'full' },  
  { path: 'landing', component: Landing },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'events', component: EventsComponent },
  { path: 'my-events', component: MyBookingsComponent },
  { path: 'bookings-summary', component: BookingsSummary },
  { path: 'post-event', component: PostEventComponent},
];
