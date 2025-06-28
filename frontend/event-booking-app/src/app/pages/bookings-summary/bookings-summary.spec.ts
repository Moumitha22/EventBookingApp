import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookingsSummary } from './bookings-summary';

describe('BookingsSummary', () => {
  let component: BookingsSummary;
  let fixture: ComponentFixture<BookingsSummary>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookingsSummary]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookingsSummary);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
