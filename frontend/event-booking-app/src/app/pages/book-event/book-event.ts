import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EventService } from '../../core/services/event.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BookingService } from '../../core/services/booking.service';
import { EventModel } from '../../models/event.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-book-event',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-event.html',
  styleUrl: './book-event.css'
})
export class BookEventComponent implements OnInit {
  event!: EventModel;
  bookingForm!: FormGroup;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private eventService = inject(EventService);
  private bookingService = inject(BookingService);

  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('id')!;
    this.bookingForm = this.fb.group({
      seatCount: [1, [Validators.required, Validators.min(1)]]
    });

    this.eventService.getEventById(eventId).subscribe({
      next: res => {
        this.event = res.data;
        this.bookingForm.get('seatCount')?.addValidators(Validators.max(this.event.availableSeats));
        this.bookingForm.get('seatCount')?.updateValueAndValidity();
      },
      error: () => {
        this.errorMessage = 'Failed to load event.';
      }
    });
  }

  onSubmit(): void {
  if (this.bookingForm.invalid) return;
  this.isSubmitting = true;
  this.successMessage = '';
  this.errorMessage = '';

  const seatCount = this.bookingForm.value.seatCount;

  this.bookingService.bookEvent(this.event.id, seatCount).subscribe({
    next: () => {
      this.successMessage = 'Tickets booked successfully!';
      this.router.navigate(['/my-events']);
      this.isSubmitting = false;
    },
    error: (err) => {
      if (err.error?.Message) {
        this.errorMessage = err.error.Message;
      } else if (err.error?.Errors?.general?.length) {
        this.errorMessage = err.error.Errors.general[0];
      } else {
        this.errorMessage = 'Booking failed. Please try again.';
      }
      this.isSubmitting = false;
    }
  });
}


}
