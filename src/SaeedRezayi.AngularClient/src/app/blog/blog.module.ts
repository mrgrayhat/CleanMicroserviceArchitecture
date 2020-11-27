import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PostCardComponent } from './post-card/post-card.component';
import { BlogRoutingModule } from './blog-routing.module';
import { PostsListComponent } from './posts-list/posts-list.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    PostCardComponent,
    PostsListComponent
  ],
  // providers: [BlogService],
  imports: [
    CommonModule,
    SharedModule,
    BlogRoutingModule
  ]
})
export class BlogModule { }
