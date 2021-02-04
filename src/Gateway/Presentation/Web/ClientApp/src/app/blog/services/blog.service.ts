import { Injectable } from '@angular/core';
import { BlogClient, Post } from '@app/blog/client/blog-api-client';

@Injectable({
    providedIn: 'root'
})
export class BlogService {

    constructor(private blogClient: BlogClient) { }
    // api version :v1, ... => /api/v1/blog
    apiVersion = '1';

    getAll(page: number = 1, pageSize: number = 10) {
        this.blogClient.index(page, pageSize, this.apiVersion).subscribe(res => {
            console.log('response: ', res);
        });
    }

    get(id: number) {
        this.blogClient.getById(id, this.apiVersion).subscribe(res => {
            console.log('response: ', res);
            console.log(res.data);
        });
    }

    delete(id: number) {
        this.blogClient.delete(id, this.apiVersion).subscribe(res => {
            console.log('response: ', res);
        });
    }

    update(post: Post) {

    }
}
