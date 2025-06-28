import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';
import { RegisterRequest } from '../../models/register-request.model';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-signup',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './signup.html',
  styleUrl: './signup.css'
})
export class SignupComponent {
  registerForm: FormGroup;
  errorMessage = '';
  successMessage = '';

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['User', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.registerForm.invalid) 
      return;

    const payload: RegisterRequest = this.registerForm.value;

    this.authService.register(payload).subscribe({
      next: () => {
        this.successMessage = 'Registration successful. You can now login.';
        this.registerForm.reset({ role: 'User' });
      },
      error: err => {
        this.errorMessage = err.error?.message || 'Registration failed';
      }
    });
  }
}
