import { Component, OnInit } from '@angular/core';
import { HeroComponent } from '../../components/hero/hero';
import { HttpClient } from '@angular/common/http';
import { Footer } from '../../components/footer/footer';

@Component({
  selector: 'app-landing',
  imports: [HeroComponent, Footer],
  templateUrl: './landing.html',
  styleUrl: './landing.css'
})
export class Landing {

  
}

