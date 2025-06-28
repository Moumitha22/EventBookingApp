import { Component } from '@angular/core';
import { EventBookingSummary } from '../../models/event-booking-summary';
import { BookingService } from '../../core/services/booking.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bookings-summary',
  imports: [CommonModule],
  templateUrl: './bookings-summary.html',
  styleUrl: './bookings-summary.css'
})
export class BookingsSummary {
 summaries: EventBookingSummary[] = [];
  loading = true;

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    this.fetchBookingSummary();
  }

  fetchBookingSummary(): void {
    this.bookingService.getBookingSummary().subscribe({
      next: (res) => {
        console.log(res.data);
        this.summaries = res.data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load booking summary', err);
        this.loading = false;
      }
    });
  }
}