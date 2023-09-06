import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class FieldOperationStudentService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getFieldOperationStudents(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperationStudent`;
                return this.http.get(apiUrl);
            })
        );
    }

    createFieldOperationStudent(device: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperationStudent`;
                return this.http.post(apiUrl, device);
            })
        );
    }

    updateFieldOperationStudent(id: string, device: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperationStudent/${id}`;
                return this.http.put(apiUrl, device);
            })
        );
    }

    deleteFieldOperationStudent(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperationStudent/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
