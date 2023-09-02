import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TeacherComponent } from './teacher/teacher.component';
import { GraduationComponent } from './graduation/graduation.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: 'teachers', component: TeacherComponent },
            { path: 'graduations', component: GraduationComponent },
        ]),
    ],
    exports: [RouterModule],
})
export class ConfigRoutingModule {}
