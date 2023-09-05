import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { FormField, MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { GraduationService } from 'src/app/demo/service/graduation.service';
import { TeacherService } from 'src/app/demo/service/teacher.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './graduation.component.html',
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
export class GraduationComponent implements OnInit {
    loading: boolean = true;
    cols: TableColumn[] = [];
    data: any[] = [];
    fields: FormField[] = [
        { id: 'name', type: 'text', label: 'Nome', required: true },
        { id: 'url', type: 'text', label: 'URL', required: true },
        { id: 'position', type: 'number', label: 'Posição', required: true },
    ];
    modalDialog: boolean = false;
    selectedRegistry: any;
    constructor(protected layoutService: LayoutService, private graduationService: GraduationService, private confirmationService: ConfirmationService, private messageService: MessageService) {}

    ngOnInit() {
        this.cols = [
            {
                field: 'name',
                header: 'Nome',
                type: 'text',
            },
            {
                field: 'position',
                header: 'Posição',
                type: 'number',
            },
            {
                field: 'url',
                header: 'URL',
                type: 'text',
            },
            {
                field: 'createdAt',
                header: 'Criado em',
                type: 'date',
                format: 'dd/MM/yy HH:mm:ss',
            },
            {
                field: 'updatedAt',
                header: 'Atualizado em',
                type: 'date',
                format: 'dd/MM/yy HH:mm:ss',
            },
            {
                field: '',
                header: 'Editar',
                type: 'edit',
            },
            {
                field: '',
                header: 'Apagar',
                type: 'delete',
            },
        ];
        this.fetchData();
    }

    event(selectedItems: any) {
        if (selectedItems.type == 0) {
        } else if (selectedItems.type == 1) {
            this.editRegistry(selectedItems.data);
        } else if (selectedItems.type == 2) {
            this.deleteRegistry(selectedItems.data);
        } else {
            console.error(selectedItems);
        }
    }

    create() {
        this.selectedRegistry = { name: undefined };
        this.modalDialog = true;
    }

    editRegistry(registry: any) {
        this.selectedRegistry = { ...registry };
        this.modalDialog = true;
    }

    deleteRegistry(registry: any) {
        this.confirmationService.confirm({
            header: 'Deletar registro',
            message: `Tem certeza que deseja apagar o registro: ${registry.name}`,
            acceptLabel: 'Aceitar',
            rejectLabel: 'Rejeitar',
            accept: () => {
                this.loading = true;
                this.graduationService.deleteGraduation(registry.id).subscribe((x) => {
                    this.messageService.add(MessageServiceSuccess);
                    this.fetchData();
                });
            },
        });
    }

    getFormData(registry: any) {
        this.loading = true;
        if (!registry) {
            this.loading = false;
            this.modalDialog = false;
        } else {
            if (registry.id) {
                this.graduationService.updateGraduation(registry.id, registry).subscribe((x) => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            } else {
                this.graduationService.createGraduation(registry).subscribe((x) => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            }
        }
    }

    fetchData() {
        this.graduationService.getGraduations().subscribe((x) => {
            this.data = x.object;
            this.loading = false;
        });
    }
}
