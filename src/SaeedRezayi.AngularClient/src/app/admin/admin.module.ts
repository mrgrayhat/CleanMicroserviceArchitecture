import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { SharedModule } from '@app/shared/shared.module';
import { LogsComponent } from './logs/logs.component';
import { LogModuleService } from '@app/core';


@NgModule({
  declarations: [
    AdminDashboardComponent,
    LogsComponent],
  providers: [
    LogModuleService
  ],
  imports: [
    CommonModule,
    SharedModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
