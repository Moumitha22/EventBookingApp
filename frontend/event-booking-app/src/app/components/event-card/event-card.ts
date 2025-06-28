import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { environment } from '../../environments/environment';
import { EventModel } from '../../models/event.model';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

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
  private router = inject(Router);

  imageBaseUrl = environment.apiBaseUrl;

  goToDetails(event: MouseEvent) {
    this.router.navigate(['/event-details', this.event.id]);
  }

  deleteEvent(): void {
    if (confirm('Are you sure you want to delete this event?')) {
      this.delete.emit(this.event.id);
    }
  }
}