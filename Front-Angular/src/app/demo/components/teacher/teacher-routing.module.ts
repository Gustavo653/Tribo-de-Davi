import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { PresenceComponent } from './presence/presence.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            { path: 'presence', component: PresenceComponent },
        ]),
    ],
    exports: [RouterModule],
})
export class TeacherRoutingModule {}
