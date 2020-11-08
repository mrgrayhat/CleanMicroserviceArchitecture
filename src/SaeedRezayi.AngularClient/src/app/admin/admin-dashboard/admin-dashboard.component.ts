import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit, OnDestroy {

  destroy$: Subject<boolean> = new Subject<boolean>();

  isAdmin = false;
  isUser = false;
  constructor() {
  }
  ngOnInit(): void {
    // this.isAdmin = this.authService.isAuthUserInRole("Admin");

  }

  ngOnDestroy() {
    this.destroy$.next(true);
    // unsubscribe
    this.destroy$.unsubscribe();
    console.log(" admin dashboard component destroyed");
  }


}
