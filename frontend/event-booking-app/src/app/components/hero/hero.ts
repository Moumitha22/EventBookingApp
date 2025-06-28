import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-hero',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './hero.html',
  styleUrl: './hero.css'
})
export class HeroComponent {
  searchModel = {
    location: '',
    keyword: ''
  };

  private router = inject(Router);

  search() {
    const queryParams: any = {
      city: this.searchModel.location || '', 
      keyword: this.searchModel.keyword || '',
    };

    this.router.navigate(['/properties'], { queryParams });
  }
}