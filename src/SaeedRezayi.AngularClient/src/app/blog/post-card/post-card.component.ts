import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { PostViewModel } from '@app/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.css']
})

export class PostCardComponent implements OnInit, OnDestroy {
  @Input()
  model: PostViewModel;
  error = "";
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(

  ) { }

  ngOnInit(): void {
    this.Seed();
  }
  Seed() {
    this.model.title = "test title";
    this.model.content = "test content";
    this.model.slug = "test-post"
    this.model.author = "saeed"
    this.model.createdAt = new Date();
    this.model.visits = 100;
  }

  ngOnDestroy() {
    this.destroy$.next(true);
    // unsubscribe
    this.destroy$.unsubscribe();
    console.log(" post card destroyed");
  }

}
