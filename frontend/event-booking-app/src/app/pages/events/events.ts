import { Component, OnInit } from '@angular/core';
import { EventService } from '../../core/services/event.service';
import { EventModel } from '../../models/event.model';
import { CommonModule } from '@angular/common';
import { EventCardComponent } from '../../components/event-card/event-card';

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

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadEvents();
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
}
