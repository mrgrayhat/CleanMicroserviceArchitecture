import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'appc-library',
  templateUrl: './library.component.html',
  styleUrls: ['./library.component.scss']
})
export class LibraryComponent implements OnInit {
  content: any[];


  constructor() { }

  ngOnInit() {
    this.content.push('1');
    this.content.push('2');
  }

  getAll() {

  }

  getInfo(id: number) {

  }

  delete(id: number) {

  }
}
