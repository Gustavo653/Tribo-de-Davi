import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { MessageServiceSuccess } from 'src/app/demo/api/base';
import { RollCallService } from 'src/app/demo/service/rollCall.service';
import { StudentService } from 'src/app/demo/service/student.service';

@Component({
    templateUrl: './presence.component.html',
    providers: [MessageService, ConfirmationService],
    styles: [
        `
            :host ::ng-deep .p-frozen-column {
                font-weight: bold;
            }

            :host ::ng-deep .p-datatable-frozen-tbody {
                font-weight: bold;
            }

            :host ::ng-deep .p-progressbar {
                height: 0.5rem;
            }
        `,
    ],
})
export class PresenceComponent implements OnInit {
    data: any[] = [];
    studentsListbox: any[] = [];
    studentId?: number;
    date: Date = new Date();
    loading: boolean = true;
    stateOptions: any[] = [
        { label: 'Ausente', value: false },
        { label: 'Presente', value: true },
    ];
    constructor(private rollCallService: RollCallService, private studentService: StudentService, private messageService: MessageService) {}

    ngOnInit(): void {
        this.fetchData();
    }

    fetchData() {
        this.loading = true;
        this.studentService.getStudentsForListbox().subscribe((x) => {
            this.studentsListbox = x.object;
            this.rollCallService.getRollCall(this.date, this.studentId).subscribe((x) => {
                this.data = x.object;
                this.loading = false;
            });
        });
    }

    setPresence(data: any) {
        var json: any = {
            studentId: data.studentId,
            date: data.date,
            presence: data.presence,
        };
        this.rollCallService.setPresence(json).subscribe((x) => {
            this.messageService.add(MessageServiceSuccess);
        });
    }

    generateRollCall() {
        this.rollCallService.generate().subscribe(() => {
            this.messageService.add(MessageServiceSuccess);
            this.fetchData();
        });
    }
}
