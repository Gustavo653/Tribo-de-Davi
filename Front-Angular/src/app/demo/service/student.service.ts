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
                return this.http.post(apiUrl, data);
            })
        );
    }

    updateStudent(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student/${id}`;
                return this.http.put(apiUrl, data);
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
