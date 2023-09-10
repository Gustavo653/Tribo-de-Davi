import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class FieldOperationService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getFieldOperations(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperation`;
                return this.http.get(apiUrl);
            })
        );
    }

    getFieldOperationForListbox(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperation/listbox`;
                return this.http.get(apiUrl);
            })
        );
    }

    createFieldOperation(data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperation`;
                return this.http.post(apiUrl, data);
            })
        );
    }

    updateFieldOperation(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperation/${id}`;
                return this.http.put(apiUrl, data);
            })
        );
    }

    deleteFieldOperation(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/fieldOperation/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
