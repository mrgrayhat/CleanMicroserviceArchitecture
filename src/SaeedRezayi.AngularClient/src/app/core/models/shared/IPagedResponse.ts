
export interface IPagedResponse<T> {
    // totalItems: number;
    // itemsPerPage: number;
    // currentPage: number;
    // maxPagerItems: number;
    // showNumbered: boolean;
    // showFirstLast: boolean;
    TotalItems:number;
    data: T[];
}