export interface PaginationModel<T = any> {
    items: T[];
    pageIndex: number;
    pageSize: number;
    totalCount: number;
    totalPage: number;
}
