import { Component, EventEmitter, Input, Output } from '@angular/core';
import { environment } from '../../environments/environment';
import { EventModel } from '../../models/event.model';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-event-card',
  imports: [CommonModule,RouterLink],
  templateUrl: './event-card.html',
  styleUrls: ['./event-card.css']
})
export class EventCardComponent {
  @Input() event!: EventModel;
  @Input() role: 'Admin' | 'User' | null = null;

  @Output() delete = new EventEmitter<string>();

  imageBaseUrl = environment.apiBaseUrl;

  deleteEvent(): void {
    if (confirm('Are you sure you want to delete this event?')) {
      this.delete.emit(this.event.id);
    }
  }
}