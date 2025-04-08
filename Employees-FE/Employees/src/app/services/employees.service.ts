import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { uploadDataUrl } from "../controllers";
import { Observable } from "rxjs";
import { ProjectEmployeesDataModel } from "../models";

@Injectable({
    providedIn: 'root'
})
export class EmployeesService {

    constructor(private http: HttpClient) { }

    uploadEmployeesData(file: File): Observable<ProjectEmployeesDataModel[]> {
        const formData = new FormData();
        formData.append('file', file, file.name);

        const headers = new HttpHeaders();
        headers.append('Content-Type', 'multipart/form-data');

        return this.http.post<ProjectEmployeesDataModel[]>(uploadDataUrl(), formData, { headers });
    }
}