import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class AddressService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getAddresses(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/address`;
                return this.http.get(apiUrl);
            })
        );
    }

    getAddressesForListbox(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/address/listbox`;
                return this.http.get(apiUrl);
            })
        );
    }

    createAddress(data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/address`;
                return this.http.post(apiUrl, data);
            })
        );
    }

    updateAddress(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/address/${id}`;
                return this.http.put(apiUrl, data);
            })
        );
    }

    deleteAddress(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/address/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
