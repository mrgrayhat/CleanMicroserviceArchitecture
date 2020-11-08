import { PostViewModel } from '@app/core';

export interface CategoryViewModel {
    id?: number;
    title?: string | undefined;
    createdAt?: Date;
    updatedAt?: Date | undefined;
    description?: string | undefined;
    totalPosts?: number;
    parent?: CategoryViewModel;
    posts?: PostViewModel[] | undefined;
}
