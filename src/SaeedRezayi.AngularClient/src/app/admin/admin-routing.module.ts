import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard, AuthGuardPermission } from '@app/core';

import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';

const routes: Routes = [
  {
    path: "admin", component: AdminDashboardComponent,
    data: {
      permission: {
        permittedRoles: ["Admin"]
      } as AuthGuardPermission
    },
    canActivate: [AuthGuard]
  },
  { path: '', component: AdminDashboardComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
