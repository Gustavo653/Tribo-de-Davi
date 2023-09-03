import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TeacherComponent } from './teacher/teacher.component';
import { GraduationComponent } from './graduation/graduation.component';
import { LegalParentComponent } from './legalParent/legalParent.component';
import { StudentComponent } from './student/student.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: 'students', component: StudentComponent },
            { path: 'teachers', component: TeacherComponent },
            { path: 'graduations', component: GraduationComponent },
            { path: 'legal-parents', component: LegalParentComponent },
        ]),
    ],
    exports: [RouterModule],
})
export class ConfigRoutingModule {}
