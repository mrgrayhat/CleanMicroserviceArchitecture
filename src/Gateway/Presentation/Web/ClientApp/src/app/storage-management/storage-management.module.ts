import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LibraryComponent } from './library/library.component';
import { RouterModule } from '@angular/router';
import { routes } from './storage-management.routes';
import { ListComponent } from '@app/shared';


@NgModule({
  declarations: [LibraryComponent, ListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class StorageManagementModule { }
