export interface AttachmentViewModel {
    id?: string;
    name?: string | undefined;
    size?: number;
    totalDownloads?: number;
    isValid?: boolean;
    url?: string | undefined;
    createdAt?: Date;
    updatedAt?: Date | undefined;
}