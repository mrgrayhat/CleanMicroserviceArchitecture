
export interface PostViewModel {
    id?: number;
    /** The title of the post */
    title: string;
    /** The post body */
    content: string;
    slug?: string | undefined;
    createdAt?: Date;
    lastUpdate?: Date | undefined;
    thumbnail?: string | undefined;
    isArchive?: boolean;
    isPublic?: boolean;
    visits?: number;
    author: string;
    tags?: any[];
    attachments?: any[];
    category?: string;
    // author?: AuthUser;
    // category?: CategoryViewModel;
    // tags?: TagViewModel[] | undefined;
    // attachments?: AttachmentViewModel[] | undefined;

}
