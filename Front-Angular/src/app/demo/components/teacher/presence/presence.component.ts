import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { RollCallService } from 'src/app/demo/service/rollCall.service';

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
    stateOptions: any[] = [
        { label: 'Ausente', value: false },
        { label: 'Presente', value: true },
    ];
    value: boolean = false;
    constructor(private rollCallService: RollCallService) {}

    ngOnInit(): void {
        this.fetchData();
        console.log(this.data);
    }

    fetchData() {
        this.rollCallService.getRollCall().subscribe((x) => {
            this.data = x.object;
        });
    }
}
