import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard, AuthGuardPermission } from '@app/core';

import { PostCardComponent } from './post-card/post-card.component';
import { PostsListComponent} from './posts-list/posts-list.component';

const routes: Routes = [
  { path: 'blog', component: PostsListComponent},
  { path: 'post', component: PostCardComponent },
  {
    path: 'edit', component: PostCardComponent,
    data: {
      permission: {
        permittedRoles: ['Admin', 'Writer']
      } as AuthGuardPermission
    },
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogRoutingModule { }
