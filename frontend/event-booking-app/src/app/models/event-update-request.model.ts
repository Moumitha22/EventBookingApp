export interface EventUpdateRequest {
  name: string;
  description: string;
  dateTime: Date;
  totalSeats: number;
  price: number;
  location: {
    name: string,
    locality: string,
    city: string,
    state: string,
  }
}
