export interface EventModel {
  id: string;
  name: string;
  description: string;
  dateTime: Date;
  totalSeats: number;
  availableSeats: number;
  price: number;
  imageUrl?: string;
  isFree: boolean;
  categoryName: string;
  locationName: string;
}
