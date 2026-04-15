import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CategoryModel, OptionModel, CORE_API_URL } from '@govibecore';
import { ApiResponse, PaginationModel, AddCategoryModel, UpdateCategoryModel } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class CategoryService {
    private apiUrl = inject(CORE_API_URL);
    private adminBaseUrl = `${this.apiUrl}/AdminCategories`;
    private userBaseUrl = `${this.apiUrl}/UserCategories`;
    private baseUrl = `${this.apiUrl}/Categories`;

    constructor(private http: HttpClient) {}

    /* ===== common ===== */
    getOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.baseUrl}/options`);
    }

    /* ===== admin ===== */
    getCategories(searchString: string, pageIndex: number, pageSize: number) {
        return this.http.get<ApiResponse<PaginationModel<CategoryModel>>>(
            `${this.adminBaseUrl}?pageIndex=${pageIndex}&pageSize=${pageSize}&searchString=${searchString}`,
        );
    }

    getById(id: string) {
        return this.http.get<ApiResponse<CategoryModel>>(`${this.adminBaseUrl}/${id}`);
    }

    createCategory(data: AddCategoryModel) {
        return this.http.post<ApiResponse<CategoryModel>>(this.adminBaseUrl, data);
    }

    updateCategory(data: UpdateCategoryModel) {
        return this.http.put<ApiResponse<CategoryModel>>(this.adminBaseUrl, data);
    }

    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.adminBaseUrl, {
            body: { ids },
        });
    }

    deleteById(id: string) {
        return this.http.delete<ApiResponse<CategoryModel>>(`${this.adminBaseUrl}/${id}`);
    }

    /* ===== user ===== */
}
