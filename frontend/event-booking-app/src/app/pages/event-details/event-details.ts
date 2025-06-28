// import { Component, OnInit, inject } from '@angular/core';
// import { ActivatedRoute, Router, RouterLink } from '@angular/router';
// import { EventService } from '../../core/services/event.service';
// import { AuthService } from '../../core/services/auth.service';
// import { CommonModule } from '@angular/common';
// import { EventModel } from '../../models/event.model';
// import { environment } from '../../environments/environment';

// @Component({
//   selector: 'app-event-details',
//   standalone: true,
//   imports: [CommonModule, RouterLink],
//   templateUrl: './event-details.html',
//   styleUrls: ['./event-details.css']
// })
// export class EventDetailsComponent implements OnInit {
//   event!: EventModel;
//   currentIndex = 0;
//   apiBaseUrl = environment.apiBaseUrl;

//   private route = inject(ActivatedRoute);
//   private router = inject(Router);
//   private eventService = inject(EventService);
//   private authService = inject(AuthService);

//   ngOnInit(): void {
//     const id = this.route.snapshot.paramMap.get('id');
//     if (id) {
//       this.eventService.getEventById(id).subscribe({
//         next: (res) => this.event = res.data,
//         error: () => console.error('Failed to load event details')
//       });
//     }
//   }

//   get images(): string[] {
//     if (!this.event?.imageUrl) return [];
//     return [this.apiBaseUrl + this.event.imageUrl];
//   }

//   nextImage() {
//     if (this.currentIndex < this.images.length - 1) this.currentIndex++;
//   }

//   prevImage() {
//     if (this.currentIndex > 0) this.currentIndex--;
//   }

//   handleBookTickets() {
//     const user = this.authService.currentUser;
//     if (!user) {
//       alert('Please login to book tickets.');
//       this.router.navigate(['/login']);
//       return;
//     }

//     if (user.role !== 'User') {
//       alert('Only users can book tickets.');
//       return;
//     }

//     this.router.navigate(['/book-event', this.event.id]);
//   }
// }


import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { EventService } from '../../core/services/event.service';
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { EventModel } from '../../models/event.model';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './event-details.html',
  styleUrls: ['./event-details.css']
})
export class EventDetailsComponent implements OnInit {
  event!: EventModel;
  role: 'Admin' | 'User' | null = null;
  apiBaseUrl = environment.apiBaseUrl;

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private eventService = inject(EventService);
  private authService = inject(AuthService);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.eventService.getEventById(id).subscribe({
        next: (res) => this.event = res.data,
        error: () => console.error('Failed to load event details')
      });
    }
  }

  get image(): string {
    return this.event?.imageUrl
      ? this.apiBaseUrl + this.event.imageUrl
      : 'https://via.placeholder.com/600x300?text=No+Image';
  }

  canEditProperty(): boolean {
    const user = this.authService.currentUser;
    if (!user) 
      return false;
    return user.role === 'Admin';
  }

  handleBookTickets() {
    const user = this.authService.currentUser;
    if (!user) {
      alert('Please login to book tickets.');
      this.router.navigate(['/login']);
      return;
    }

    if (user.role !== 'User') {
      alert('Only users can book tickets.');
      return;
    }

    this.router.navigate(['/book-event', this.event.id]);
  }

  goToEdit() {
    this.router.navigate(['/edit-event', this.event.id]);
  }

  goToBookings() {
    this.router.navigate(['/bookings-summary'], { queryParams: { eventId: this.event.id } });
  }
}
