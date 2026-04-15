import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CategoryModel, OptionModel, CORE_API_URL } from '@govibecore';
import { ApiResponse, PaginationModel, AddCategoryModel, UpdateCategoryModel } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class CategoryService {
    private apiUrl = inject(CORE_API_URL);
    private baseUrl = `${this.apiUrl}/AdminCategories`;

    constructor(private http: HttpClient) {}

    getCategories(searchString: string, pageIndex: number, pageSize: number) {
        return this.http.get<ApiResponse<PaginationModel<CategoryModel>>>(
            `${this.baseUrl}?pageIndex=${pageIndex}&pageSize=${pageSize}&searchString=${searchString}`,
        );
    }

    getById(id: string) {
        return this.http.get<ApiResponse<CategoryModel>>(`${this.baseUrl}/${id}`);
    }

    createCategory(data: AddCategoryModel) {
        return this.http.post<ApiResponse<CategoryModel>>(this.baseUrl, data);
    }

    updateCategory(data: UpdateCategoryModel) {
        return this.http.put<ApiResponse<CategoryModel>>(this.baseUrl, data);
    }

    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.baseUrl, {
            body: { ids },
        });
    }

    deleteById(id: string) {
        return this.http.delete<ApiResponse<CategoryModel>>(`${this.baseUrl}/${id}`);
    }

    getOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.baseUrl}/options`);
    }
}
