import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { CategoryModel } from 'govibe-core';
import { AddCategoryModel, UpdateCategoryModel } from '../models/category.model';
import { ApiResponse } from '../common/api-response';
import { PaginationModel } from '../common/pagination.model';

@Injectable({
    providedIn: 'root',
})
export class CategoryService {
    private baseUrl = `${environment.API_URL}/Categories`;

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
}
