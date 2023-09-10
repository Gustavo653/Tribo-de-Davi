import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class TeacherService {
    constructor(private http: HttpClient, private storageService: StorageService) {}

    private getAPIURL(): Observable<string> {
        return this.storageService.getAPIURL();
    }

    getTeachers(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/teacher`;
                return this.http.get(apiUrl);
            })
        );
    }

    getTeacherForListbox(): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/teacher/listbox`;
                return this.http.get(apiUrl);
            })
        );
    }

    createTeacher(data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/teacher`;
                return this.http.post(apiUrl, data);
            })
        );
    }

    updateTeacher(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/teacher/${id}`;
                return this.http.put(apiUrl, data);
            })
        );
    }

    deleteTeacher(id: string): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/teacher/${id}`;
                return this.http.delete(apiUrl);
            })
        );
    }
}
