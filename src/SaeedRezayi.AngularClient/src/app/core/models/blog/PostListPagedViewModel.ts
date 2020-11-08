import { Paging } from '../shared/Paging';
import { PostViewModel } from './PostViewModel';

export interface PostListPagedViewModel {
    posts: PostViewModel[],
    paging: Paging
}