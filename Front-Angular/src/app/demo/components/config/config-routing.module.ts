import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TeacherComponent } from './teacher/teacher.component';
import { GraduationComponent } from './graduation/graduation.component';
import { LegalParentComponent } from './legalParent/legalParent.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: 'teachers', component: TeacherComponent },
            { path: 'graduations', component: GraduationComponent },
            { path: 'legal-parents', component: LegalParentComponent },
        ]),
    ],
    exports: [RouterModule],
})
export class ConfigRoutingModule {}
