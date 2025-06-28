export interface BookingResponse {
  bookingId: string;
  eventName: string;
  eventDate: Date;
  seatCount: number;
  price: number;
  bookedAt: Date;
}
