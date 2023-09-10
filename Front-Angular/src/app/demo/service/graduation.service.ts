import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class GraduationService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getGraduations(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/graduation`;
                return this.http.get(apiUrl);
            })
        );
    }
    
    getGraduationsForListbox(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/graduation/listbox`;
                return this.http.get(apiUrl);
            })
        );
    }

    createGraduation(data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/graduation`;
                return this.http.post(apiUrl, data);
            })
        );
    }

    updateGraduation(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/graduation/${id}`;
                return this.http.put(apiUrl, data);
            })
        );
    }

    deleteGraduation(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/graduation/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
