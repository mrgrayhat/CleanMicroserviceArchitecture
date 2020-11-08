
export interface Paging {
    TotalItems: number;
    ItemsPerPage: number;
    CurrentPage: number;
    MaxPagerItems: number;
    ShowNumbered: boolean;
    ShowFirstLast: boolean;

    UseReverseIncrement: boolean;
    SuppressEmptyNextPrev: boolean;
    SuppressInActiveFirstLast: boolean;
    RemoveNextPrevLinks: boolean;
}