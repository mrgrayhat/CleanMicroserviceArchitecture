import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PostComponent } from './post/post.component';


export const routes: Routes = [
    {
        path: '', component: HomeComponent,
        data: { displayText: 'Blog Home' }
    },
    {
        path: 'post',
        component: PostComponent
    }
    // {
    //     path: 'post',
    //     loadChildren: () => import('./blog/post/post.module').then(m => m.ShopModule),
    //     data: { displayText: 'Post' },
    // },
];
