import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { CategoryModel, OptionModel, MAIN_API_URL, SEARCHING_API_URL } from '@govibecore';
import { ApiResponse, PaginationModel, AddCategoryModel, UpdateCategoryModel } from '@shared';

@Injectable({
    providedIn: 'root',
})
export class CategoryService {
    private mainApiUrl = inject(MAIN_API_URL);

    private commonMainApi = `${this.mainApiUrl}/Categories`;
    private mainAdminApi = `${this.mainApiUrl}/AdminCategories`;

    constructor(private http: HttpClient) {}

    /* ===== common ===== */
    getOptions() {
        return this.http.get<ApiResponse<OptionModel<string>[]>>(`${this.commonMainApi}/options`);
    }

    /* ===== admin ===== */
    getCategories(searchString: string, pageIndex: number, pageSize: number) {
        return this.http.get<ApiResponse<PaginationModel<CategoryModel>>>(
            `${this.mainAdminApi}?pageIndex=${pageIndex}&pageSize=${pageSize}&searchString=${searchString}`,
        );
    }

    getById(id: string) {
        return this.http.get<ApiResponse<CategoryModel>>(`${this.mainAdminApi}/${id}`);
    }

    createCategory(data: AddCategoryModel) {
        return this.http.post<ApiResponse<CategoryModel>>(this.mainAdminApi, data);
    }

    updateCategory(data: UpdateCategoryModel) {
        return this.http.put<ApiResponse<CategoryModel>>(this.mainAdminApi, data);
    }

    deleteMultiple(ids: string[]) {
        return this.http.request<ApiResponse<any>>('delete', this.mainAdminApi, {
            body: { ids },
        });
    }

    deleteById(id: string) {
        return this.http.delete<ApiResponse<CategoryModel>>(`${this.mainAdminApi}/${id}`);
    }

    /* ===== user ===== */
}
