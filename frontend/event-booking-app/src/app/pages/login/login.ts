import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../models/login-request.model';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage = '';
  loading = false;
  showPassword = false;
  selectedRole: 'Buyer' | 'OwnerAgent' | null = null;

  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private fb = inject(FormBuilder);

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
  if (this.loginForm.invalid) return;

  this.loading = true;
  const payload: LoginRequest = this.loginForm.value;

  this.authService.login(payload).subscribe({
    next: () => {
      this.authService.userRole$.subscribe(role => {
        if (role === 'Admin') {
          this.router.navigate(['/dashboard']);
        } else {
          this.router.navigate(['/']);
        }
      });
    },
    error: err => {
      this.errorMessage = err.error?.message || 'Login failed';
      this.loading = false;
    }
  });
}

}
