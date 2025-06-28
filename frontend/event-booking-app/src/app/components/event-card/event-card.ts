import { Component, Input } from '@angular/core';
import { environment } from '../../environments/environment';
import { EventModel } from '../../models/event.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-event-card',
  imports: [CommonModule],
  templateUrl: './event-card.html',
  styleUrls: ['./event-card.css']
})
export class EventCardComponent {
  @Input() event!: EventModel;

  imageBaseUrl = environment.apiBaseUrl;
}
