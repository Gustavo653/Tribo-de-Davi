import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class LegalParentService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getLegalParents(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/legalParent`;
                return this.http.get(apiUrl);
            })
        );
    }

    createLegalParent(device: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/legalParent`;
                return this.http.post(apiUrl, device);
            })
        );
    }

    updateLegalParent(id: string, device: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/legalParent/${id}`;
                return this.http.put(apiUrl, device);
            })
        );
    }

    deleteLegalParent(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/legalParent/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
