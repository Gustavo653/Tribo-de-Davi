import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class StudentService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getStudents(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student`;
                return this.http.get(apiUrl);
            })
        );
    }

    getStudentsForListbox(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student/listbox`;
                return this.http.get(apiUrl);
            })
        );
    }

    createStudent(data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student`;
                const formData = new FormData();
                
                for (const key in data) {
                    if (data[key] !== undefined && data[key] !== null && data[key] !== '') {
                        if (typeof data[key] !== 'object') {
                            formData.append(key, data[key]);
                        } else {
                            for (const nestedKey in data[key]) {
                                if (data[key][nestedKey] !== undefined && data[key][nestedKey] !== null && data[key][nestedKey] !== '') {
                                    formData.append(`${key}.${nestedKey}`, data[key][nestedKey]);
                                }
                            }
                        }
                    }
                }

                return this.http.post(apiUrl, formData);
            })
        );
    }

    updateStudent(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student/${id}`;
                const formData = new FormData();

                for (const key in data) {
                    if (data[key] !== undefined && data[key] !== null && data[key] !== '') {
                        if (typeof data[key] !== 'object') {
                            formData.append(key, data[key]);
                        } else {
                            for (const nestedKey in data[key]) {
                                if (data[key][nestedKey] !== undefined && data[key][nestedKey] !== null && data[key][nestedKey] !== '') {
                                    formData.append(`${key}.${nestedKey}`, data[key][nestedKey]);
                                }
                            }
                        }
                    }
                }

                return this.http.put(apiUrl, formData);
            })
        );
    }

    deleteStudent(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
