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
                formData.append('name', data.name);
                formData.append('birthDate', data.birthDate);
                formData.append('file', data.file);
                formData.append('weight', data.weight);
                formData.append('height', data.height);
                formData.append('graduationId', data.graduationId);
                formData.append('phoneNumber', data.phoneNumber);
                formData.append('email', data.email);
                formData.append('password', data.password);
                formData.append('rg', data.rg);
                formData.append('cpf', data.cpf);
                formData.append('schoolName', data.schoolName);
                formData.append('schoolGrade', data.schoolGrade);
                formData.append('address.streetName', data.address.streetName);
                formData.append('address.streetNumber', data.address.streetNumber);
                formData.append('address.neighborhood', data.address.neighborhood);
                formData.append('address.city', data.address.city);
                formData.append('legalParent.name', data.legalParent.name);
                formData.append('legalParent.relationship', data.legalParent.relationship);
                formData.append('legalParent.rg', data.legalParent.rg);
                formData.append('legalParent.cpf', data.legalParent.cpf);
                formData.append('legalParent.phoneNumber', data.legalParent.phoneNumber);
                return this.http.post(apiUrl, formData);
            })
        );
    }

    updateStudent(id: string, data: any): Observable<any> {
        return this.getAPIURL().pipe(
            switchMap((url) => {
                const apiUrl = `${url}/student/${id}`;
                const formData = new FormData();
                formData.append('name', data.name);
                formData.append('birthDate', data.birthDate);
                formData.append('file', data.file);
                formData.append('weight', data.weight);
                formData.append('height', data.height);
                formData.append('graduationId', data.graduationId);
                formData.append('phoneNumber', data.phoneNumber);
                formData.append('email', data.email);
                formData.append('password', data.password);
                formData.append('rg', data.rg);
                formData.append('cpf', data.cpf);
                formData.append('schoolName', data.schoolName);
                formData.append('schoolGrade', data.schoolGrade);
                formData.append('address.streetName', data.address.streetName);
                formData.append('address.streetNumber', data.address.streetNumber);
                formData.append('address.neighborhood', data.address.neighborhood);
                formData.append('address.city', data.address.city);
                formData.append('legalParent.name', data.legalParent.name);
                formData.append('legalParent.relationship', data.legalParent.relationship);
                formData.append('legalParent.rg', data.legalParent.rg);
                formData.append('legalParent.cpf', data.legalParent.cpf);
                formData.append('legalParent.phoneNumber', data.legalParent.phoneNumber);
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
