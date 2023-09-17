import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ButtonModule } from 'primeng/button';
import { AtomicModule } from '../atomic/atomic.module';
import { DataViewModule } from 'primeng/dataview';
import { ProgressBarModule } from 'primeng/progressbar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogModule } from 'primeng/dialog';
import { ConfigRoutingModule } from './config-routing.module';
import { TeacherComponent } from './teacher/teacher.component';
import { GraduationComponent } from './graduation/graduation.component';
import { ListboxModule } from 'primeng/listbox';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { ToastModule } from 'primeng/toast';
import { LegalParentComponent } from './legalParent/legalParent.component';
import { StudentComponent } from './student/student.component';
import { CalendarModule } from 'primeng/calendar';
import { InputNumberModule } from 'primeng/inputnumber';
import { FieldOperationComponent } from './fieldOperation/fieldOperation.component';
import { FieldOperationTeacherComponent } from './fieldOperationTeacher/fieldOperationTeacher.component';
import { SelectButtonModule } from 'primeng/selectbutton';
import { FileUploadModule } from 'primeng/fileupload';
import { FieldOperationStudentComponent } from './fieldOperationStudent/fieldOperationStudent.component';
import { AddressComponent } from './address/address.component';

@NgModule({
    declarations: [
        TeacherComponent,
        GraduationComponent,
        AddressComponent,
        LegalParentComponent,
        StudentComponent,
        FieldOperationComponent,
        FieldOperationTeacherComponent,
        FieldOperationStudentComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        DialogModule,
        ConfirmDialogModule,
        ProgressBarModule,
        DataViewModule,
        SelectButtonModule,
        FormsModule,
        AtomicModule,
        ButtonModule,
        CalendarModule,
        InputTextareaModule,
        ListboxModule,
        InputNumberModule,
        FieldsetModule,
        DropdownModule,
        ToastModule,
        FileUploadModule,
        InputTextModule,
        ConfigRoutingModule,
    ],
})
export class ConfigModule {}
