import { Routes } from '@angular/router';
import { LibraryComponent } from './library/library.component';


export const routes: Routes = [
  {
    path: '', component: LibraryComponent,
    data: { displayText: 'Storage Library Home' }
  },
  {
    path: 'library',
    component: LibraryComponent
  }
];
