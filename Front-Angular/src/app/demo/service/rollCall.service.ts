import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class RollCallService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getRollCall(date: Date, studentId: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/rollCall`;
                let params = new HttpParams();
                if (date !== null) {
                    params = params.set('date', date.toDateString());
                }
                if (studentId !== undefined && studentId !== null) {
                    params = params.set('studentId', studentId);
                }
                return this.http.get(apiUrl, { params: params });
            })
        );
    }

    setPresence(data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/rollCall/presence`;
                return this.http.post(apiUrl, data);
            })
        );
    }

    generate(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/rollCall/generate`;
                return this.http.post(apiUrl, {});
            })
        );
    }
}
