import { Component, HostListener, OnInit, inject } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthService } from '../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit {
  private router = inject(Router);
  private authService = inject(AuthService);

  isLoggedIn$ = this.authService.isLoggedIn$;
  userRole$ = this.authService.userRole$;

  isLandingPage = true;
  isScrolled = false;
  showNavbar = true;

  ngOnInit(): void {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      const url = this.router.url;

      // Control navbar visibility
      // this.showNavbar = !['/login', '/signup'].includes(url);

      // Detect landing page
      this.isLandingPage = url === '/' || url.startsWith('/landing') || url.startsWith('/login') || url.startsWith('/signup');
      this.checkNavbarState();
    });

    this.checkNavbarState();
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    this.checkNavbarState();
  }

  private checkNavbarState() {
    const scrollTop = window.scrollY || document.documentElement.scrollTop;
    this.isScrolled = scrollTop > 300;
  }

  get navbarClasses() {
    return {
      'fixed-top': this.isLandingPage,
      'sticky-top': !this.isLandingPage,
      'scrolled': !this.isLandingPage || this.isScrolled,
      'navbar': true,
      'navbar-expand-lg': true,
      'shadow': true
    };
  }

  logout(): void {
    this.authService.setLoggingOut(true);
    this.authService.logout().subscribe({
      next: () => {
        this.authService.clearLocalState();
        this.router.navigate(['/login']);
      },
      error: err => console.error('Logout failed', err),
      complete: () => this.authService.setLoggingOut(false)
    });
  }
}

