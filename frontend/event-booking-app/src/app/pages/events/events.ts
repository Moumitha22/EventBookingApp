import { Component, inject, OnInit } from '@angular/core';
import { EventService } from '../../core/services/event.service';
import { EventModel } from '../../models/event.model';
import { CommonModule } from '@angular/common';
import { EventCardComponent } from '../../components/event-card/event-card';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-events',
  imports: [CommonModule, EventCardComponent],
  templateUrl: './events.html',
  styleUrls: ['./events.css']
})
export class EventsComponent implements OnInit {
  events: EventModel[] = [];
  isLoading = true;
  errorMessage = '';
  
  private eventService= inject(EventService);
  private authService= inject(AuthService);

  userRole: 'Admin' | 'User' = 'User';


  ngOnInit(): void {
    this.loadEvents();
    this.authService.userRole$.subscribe(role => {
    if (role === 'Admin') {
      this.userRole = 'Admin';
    } else {
      this.userRole = 'User';
    }
  });
  }

  loadEvents(): void {
    this.isLoading = true;
    this.eventService.getAllEvents().subscribe({
      next: (res:any) => {
        this.events = res.data as EventModel[];
        this.isLoading = false;
      },
      error: (err:any) => {
        this.errorMessage = 'Failed to load events.';
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  deleteEvent(eventId: string): void {
    this.eventService.deleteEvent(eventId).subscribe({
      next: () => {
        this.events = this.events.filter(e => e.id !== eventId);
      },
      error: () => {
        alert('Failed to delete event.');
      }
    });
  }

}
