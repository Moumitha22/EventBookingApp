export interface EventBookingSummary {
  eventId: string;
  eventName: string;
  eventDate: Date;
  totalBookings: number;
  totalSeats: number;
  availableSeats: number;
  totalSeatsBooked: number;
}
