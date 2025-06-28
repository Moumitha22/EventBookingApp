import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventService } from '../../core/services/event.service';
import { CommonModule } from '@angular/common';
import { EventUpdateRequest } from '../../models/event-update-request.model';

@Component({
  selector: 'app-edit-event',
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './edit-event.html',
  styleUrl: '../post-event/post-event.css'
})
export class EditEventComponent implements OnInit {
  eventForm!: FormGroup;
  isEditMode = false;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';
  eventId!: string;

    private fb = inject(FormBuilder);
    private route =  inject(ActivatedRoute);
    private router = inject(Router);
    private eventService = inject(EventService); 

  ngOnInit(): void {
    this.eventId = this.route.snapshot.paramMap.get('id')!;
    this.buildForm();
    this.loadEvent();
  }

  buildForm(): void {
  this.eventForm = this.fb.group({
    name: [{ value: '', disabled: true }, Validators.required],
    description: [{ value: '', disabled: true }, Validators.required],
    dateTime: [{ value: '', disabled: true }, Validators.required],
    totalSeats: [{ value: 0, disabled: true }, [Validators.required, Validators.min(1)]],
    price: [{ value: 0, disabled: true }, [Validators.required, Validators.min(0)]],
    location: this.fb.group({
      name: [{ value: '', disabled: true }, Validators.required],
      locality: [{ value: '', disabled: true }, Validators.required],
      city: [{ value: '', disabled: true }, Validators.required],
      state: [{ value: '', disabled: true }, Validators.required]
    })
  });
}


  loadEvent(): void {
    this.eventService.getEventById(this.eventId).subscribe({
      next: res => {
        const e = res.data;
        const [venueName, locality, city, state] = e.locationName.split(',').map(s => s.trim());

        this.eventForm.patchValue({
          name: e.name,
          description: e.description,
          dateTime: this.formatDateTimeForInput(e.dateTime),
          totalSeats: e.totalSeats,
          price: e.price,
          location: {
            name: venueName || '',
            locality: locality || '',
            city: city || '',
            state: state || ''
          }
        });
      },
      error: () => {
        this.errorMessage = 'Failed to load event details.';
      }
    });
  }


  formatDateTimeForInput(date: Date): string {
    const d = new Date(date);
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${d.getFullYear()}-${pad(d.getMonth()+1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
  }

  toggleEdit(): void {
    this.isEditMode = !this.isEditMode;
    if (this.isEditMode) {
      this.eventForm.enable();
    } else {
      this.eventForm.disable();
      this.loadEvent(); 
      this.successMessage = '';
      this.errorMessage = '';
    }
  }

  onSubmit(): void {
    if (this.eventForm.invalid) return;
    this.isSubmitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    const updateDto: EventUpdateRequest = {
      name: this.eventForm.value.name,
      description: this.eventForm.value.description,
      dateTime: new Date(this.eventForm.value.dateTime), 
      totalSeats: this.eventForm.value.totalSeats,
      price: this.eventForm.value.price,
      location: {
        name: this.eventForm.value.location.name,
        locality: this.eventForm.value.location.locality,
        city: this.eventForm.value.location.city,
        state: this.eventForm.value.location.state
      }

    };


    this.eventService.updateEvent(this.eventId, updateDto).subscribe({
      next: () => {
        this.successMessage = 'Event updated successfully!';
        this.isEditMode = false;
        this.eventForm.disable();
        this.isSubmitting = false;
      },
      error: () => {
        this.errorMessage = 'Failed to update event.';
        this.isSubmitting = false;
      }
    });
  }
}
