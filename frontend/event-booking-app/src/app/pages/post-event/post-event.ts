import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { EventService } from '../../core/services/event.service';
import { CategoryService } from '../../core/services/category.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-post-event',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './post-event.html',
  styleUrls: ['./post-event.css']
})
export class PostEventComponent implements OnInit {
  eventForm!: FormGroup;
  categories: { id: string; name: string }[] = [];
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';
  selectedFile?: File;

 
    private fb = inject(FormBuilder);
    private eventService= inject(EventService);
    private categoryService = inject(CategoryService);

  ngOnInit(): void {
    this.buildForm();
    this.loadCategories();
  }

  buildForm(): void {
    this.eventForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      dateTime: ['', Validators.required],
      totalSeats: [0, [Validators.required, Validators.min(1)]],
      price: [0, [Validators.required, Validators.min(0)]],
      categoryId: ['', Validators.required],
      location: this.fb.group({
        name: ['', Validators.required],
        locality: ['', Validators.required],
        city: ['', Validators.required],
        state: ['', Validators.required],
      })
    });
  }

  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe({
      next: (res) => (this.categories = res.data),
      error: () => (this.errorMessage = 'Failed to load categories')
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      this.selectedFile = input.files[0];
    }
  }

  onSubmit(): void {
    this.successMessage = '';
    this.errorMessage = '';
    if (this.eventForm.invalid) {
    this.eventForm.markAllAsTouched();
    return;
  }

    this.isSubmitting = true;

    const eventJson = JSON.stringify(this.eventForm.value);
    const formData = new FormData();
    formData.append('EventJson', eventJson);
    if (this.selectedFile) {
      formData.append('Image', this.selectedFile);
    }

    this.eventService.uploadEventWithImage(formData).subscribe({
      next: () => {
        this.successMessage = 'Event posted successfully!';
        this.eventForm.reset();
        this.selectedFile = undefined;
      },
      error: () => {
        this.errorMessage = 'Failed to post event.';
      },
      complete: () => (this.isSubmitting = false)
    });
  }
}
