import { Component, OnInit } from '@angular/core';
import { Post } from '@app/blog/client/blog-api-client';
import { GridColumn, GridFieldType } from '@app/shared';
import { BlogService } from '../services/blog.service';


@Component({
    selector: 'appc-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
    constructor(private blogService: BlogService) { }

    posts: Post[];
    columns: GridColumn[] = [
        {
            field: 'id',
            type: GridFieldType.Number,
            filter: true,
            sortable: true,
            width: 100,
        },
        {
            field: 'visits',
            type: GridFieldType.Number,
            filter: true,
            sortable: true,
            width: 100,
        },
        {
            field: 'category',
            filter: true,
            sortable: true,
            width: 160,
        },
        {
            field: 'tags',
            filter: true,
            sortable: true,
            width: 160,
        },
        {
            field: 'isArchive',
            type: GridFieldType.Boolean,
            filter: true,
            sortable: true,
            width: 120,
        },
        {
            field: 'description',
            filter: true,
            sortable: true,
            width: 160,
        }
        // ,{
        //   type: GridFieldType.ActionButtons,
        //   cellRendererParams: {
        //     primaryClicked: this.editPost.bind(this),
        //     secondaryClicked: this.deletePost.bind(this),
        //     primaryLabel: 'Edit Post',
        //     secondaryLabel: 'Delete Post',
        //   },
        // },
    ];

    ngOnInit() {
        this.getData();
    }

    getData() {
        this.blogService.getAll();
    }


}
