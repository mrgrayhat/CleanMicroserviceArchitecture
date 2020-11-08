import { Component, OnDestroy, OnInit } from '@angular/core';
import { PostListPagedViewModel } from '@app/core/models/blog/PostListPagedViewModel';
import { Subject } from 'rxjs';

@Component({
  // selector: 'app-posts-list',
  templateUrl: './posts-list.component.html',
  styleUrls: ['./posts-list.component.css']
})
export class PostsListComponent implements OnInit, OnDestroy {
  destroy$: Subject<boolean> = new Subject<boolean>();
  pagedPosts: PostListPagedViewModel;

  constructor() {

  }

  ngOnInit(): void {
    console.log("waiting to recieve data");
    // console.log("logs items recieved: ", this.pagedLogs)
    // console.log("paging setting recieved: ", this.pagedLogs.Paging)
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    // unsubscribe
    this.destroy$.unsubscribe();
    console.log(" post list component destroyed");
  }

  getPosts() {
    this.pagedPosts?.posts.push();
  }

}
