import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TeacherComponent } from './teacher/teacher.component';
import { GraduationComponent } from './graduation/graduation.component';
import { LegalParentComponent } from './legalParent/legalParent.component';
import { StudentComponent } from './student/student.component';
import { FieldOperationComponent } from './fieldOperation/fieldOperation.component';
import { FieldOperationTeacherComponent } from './fieldOperationTeacher/fieldOperationTeacher.component';
import { FieldOperationStudentComponent } from './fieldOperationStudent/fieldOperationStudent.component';
import { AddressComponent } from './address/address.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: 'students', component: StudentComponent },
            { path: 'addresses', component: AddressComponent },
            { path: 'teachers', component: TeacherComponent },
            { path: 'graduations', component: GraduationComponent },
            { path: 'legal-parents', component: LegalParentComponent },
            { path: 'field-operations', component: FieldOperationComponent },
            { path: 'field-operation-teachers', component: FieldOperationTeacherComponent },
            { path: 'field-operation-students', component: FieldOperationStudentComponent },
        ]),
    ],
    exports: [RouterModule],
})
export class ConfigRoutingModule {}
