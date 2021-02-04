import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { PostComponent } from './post/post.component';
import { SharedModule } from '@app/shared';
import { routes } from './blog.routes';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [HomeComponent, PostComponent],
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class BlogModule { }
