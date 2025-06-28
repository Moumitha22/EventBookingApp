import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../core/services/admin.service';
import { AdminDashboardSummaryModel } from '../../models/admin-dashboard-summary.model';
import { BookingsSummary } from '../bookings-summary/bookings-summary';

@Component({
  selector: 'app-dashboard',
  imports: [BookingsSummary],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  dashboard!: AdminDashboardSummaryModel;
  isLoading = true;
  errorMessage = '';

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.adminService.getDashboardSummary().subscribe({
      next: (res:any) => {
        console.log(res);
        this.dashboard = res.data as AdminDashboardSummaryModel;
        this.isLoading = false;
      },
      error: err => {
        this.errorMessage = 'Failed to load dashboard data.';
        this.isLoading = false;
      }
    });
  }
}
