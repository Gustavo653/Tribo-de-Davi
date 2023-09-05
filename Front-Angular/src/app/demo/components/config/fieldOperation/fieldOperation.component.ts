import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { FormField, MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { GraduationService } from 'src/app/demo/service/graduation.service';
import { FieldOperationService } from 'src/app/demo/service/fieldOperation.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './fieldOperation.component.html',
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
export class FieldOperationComponent implements OnInit {
    dialog: boolean = false;
    loading: boolean = true;
    birthDate: Date = new Date();
    cols: TableColumn[] = [];
    data: any[] = [];
    graduationsListbox: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = { address: {}, legalParent: {} };
    constructor(
        protected layoutService: LayoutService,
        private fieldOperationService: FieldOperationService,
        private graduationService: GraduationService,
        private confirmationService: ConfirmationService,
        private messageService: MessageService
    ) {}

    ngOnInit() {
        this.cols = [
            {
                field: 'name',
                header: 'Nome',
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
        this.selectedRegistry = { address: {} };
        this.modalDialog = true;
    }

    editRegistry(registry: any) {
        this.selectedRegistry = { ...registry };
        this.birthDate = new Date(this.selectedRegistry.birthDate);
        this.modalDialog = true;
    }

    hideDialog() {
        this.selectedRegistry = { address: {} };
        this.modalDialog = false;
    }

    validateData(): boolean {
        if (
            !this.selectedRegistry.name ||
            !this.selectedRegistry.address.streetName ||
            !this.selectedRegistry.address.streetNumber ||
            !this.selectedRegistry.address.neighborhood ||
            !this.selectedRegistry.address.city
        ) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        return true;
    }

    save() {
        console.log(this.selectedRegistry);
        if (this.validateData()) {
            this.selectedRegistry.birthDate = this.birthDate;
            if (this.selectedRegistry.id) {
                this.fieldOperationService.updateFieldOperation(this.selectedRegistry.id, this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.fieldOperationService.createFieldOperation(this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            }
        }
    }

    deleteRegistry(registry: any) {
        this.confirmationService.confirm({
            header: 'Deletar registro',
            message: `Tem certeza que deseja apagar o registro: ${registry.name}`,
            acceptLabel: 'Aceitar',
            rejectLabel: 'Rejeitar',
            accept: () => {
                this.loading = true;
                this.fieldOperationService.deleteFieldOperation(registry.id).subscribe((x) => {
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
                this.fieldOperationService.updateFieldOperation(registry.id, registry).subscribe((x) => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            } else {
                this.fieldOperationService.createFieldOperation(registry).subscribe((x) => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            }
        }
    }

    fetchData() {
        this.fieldOperationService.getFieldOperations().subscribe((x) => {
            this.data = x.object;
            this.loading = false;
        });
    }
}
