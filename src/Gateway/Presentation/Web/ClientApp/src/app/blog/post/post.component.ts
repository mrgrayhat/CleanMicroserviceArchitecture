import { Component, OnInit } from '@angular/core';
import { Post } from '../client/blog-api-client';

@Component({
  selector: 'appc-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss']
})
export class PostComponent implements OnInit {

  constructor() { }
  post: Post;

  ngOnInit() {

  }

}
