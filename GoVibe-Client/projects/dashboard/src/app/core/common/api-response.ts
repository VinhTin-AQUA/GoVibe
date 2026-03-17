export interface ApiResponse<T = any> {
    errorMessage: string;
    status: number;
    item: T;
}
