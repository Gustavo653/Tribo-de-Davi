import { NgModule } from '@angular/core';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppLayoutModule } from './layout/app.layout.module';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { LoginModule } from './demo/components/auth/login/login.module';
import { AuthService } from './demo/service/auth.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { BasicInterceptor } from './demo/components/auth/basic.interceptor';
import { StorageService } from './demo/service/storage.service';
import { ErrorInterceptor } from './demo/components/auth/error.interceptor';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ServiceWorkerModule } from '@angular/service-worker';
import { ToastModule } from 'primeng/toast';
import { TeacherService } from './demo/service/teacher.service';
import { StudentService } from './demo/service/student.service';
import { FieldOperationService } from './demo/service/fieldOperation.service';
import { FieldOperationTeacherService } from './demo/service/fieldOperationTeacher.service';
import { FieldOperationStudentService } from './demo/service/fieldOperationStudent.service';
import { RollCallService } from './demo/service/rollCall.service';

@NgModule({
    declarations: [AppComponent, NotfoundComponent],
    imports: [
        AppRoutingModule,
        AppLayoutModule,
        LoginModule,
        ToastModule,
        ServiceWorkerModule.register('ngsw-worker.js', {
            enabled: false,
            scope: './',
            registrationStrategy: 'registerImmediately',
        }),
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        { provide: HTTP_INTERCEPTORS, useClass: BasicInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        AuthService,
        ConfirmationService,
        MessageService,
        StudentService,
        FieldOperationService,
        FieldOperationTeacherService,
        FieldOperationStudentService,
        RollCallService,
        TeacherService,
        StorageService,
    ],
    bootstrap: [AppComponent],
    exports: [],
})
export class AppModule {}
