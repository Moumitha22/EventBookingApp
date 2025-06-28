import { Component, OnInit } from '@angular/core';
import { BookingService } from '../../core/services/booking.service';
import { BookingResponse } from '../../models/booking-response.model';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-bookings.html',
  styleUrls: ['./my-bookings.css']
})
export class MyBookingsComponent implements OnInit {
  bookings: BookingResponse[] = [];
  loading = true;

  constructor(
    private bookingService: BookingService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.currentUser?.id;
    if (!userId) return;

    this.bookingService.getMyBookings().subscribe({
      next: (res:any) => {
        console.log(res.data);
        this.bookings = res.data as BookingResponse[];
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load bookings', err);
        this.loading = false;
      }
    });
  }
}