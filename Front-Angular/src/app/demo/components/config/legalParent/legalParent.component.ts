import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { FormField, MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { GraduationService } from 'src/app/demo/service/graduation.service';
import { LegalParentService } from 'src/app/demo/service/legalParent.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './legalParent.component.html',
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
export class LegalParentComponent implements OnInit {
    dialog: boolean = false;
    loading: boolean = true;
    cols: TableColumn[] = [];
    data: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = {};
    constructor(
        protected layoutService: LayoutService,
        private legalParentService: LegalParentService,
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
                field: 'rg',
                header: 'RG',
                type: 'text',
            },
            {
                field: 'cpf',
                header: 'CPF',
                type: 'text',
            },
            {
                field: 'relationship',
                header: 'Parentesco',
                type: 'text',
            },
            {
                field: 'phoneNumber',
                header: 'Telefone',
                type: 'text',
            },
            {
                field: 'studentsCount',
                header: 'Quantidade de Alunos',
                type: 'number',
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
        this.selectedRegistry = {};
        this.modalDialog = true;
    }

    editRegistry(registry: any) {
        this.selectedRegistry = { ...registry };
        this.modalDialog = true;
    }

    hideDialog() {
        this.selectedRegistry = {};
        this.modalDialog = false;
    }

    validateData(): boolean {
        const phoneNumberPattern = /^[0-9]{11}$/;

        if (!this.selectedRegistry.name || !this.selectedRegistry.rg || !this.selectedRegistry.cpf || !this.selectedRegistry.relationship || !this.selectedRegistry.phoneNumber) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        if (!this.selectedRegistry.phoneNumber.match(phoneNumberPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Número de telefone inválido' });
            return false;
        }

        return true;
    }

    save(val?: number) {
        if (val == 1) this.selectedRegistry.id = undefined;
        if (this.validateData()) {
            if (this.selectedRegistry.id) {
                this.legalParentService.updateLegalParent(this.selectedRegistry.id, this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.legalParentService.createLegalParent(this.selectedRegistry).subscribe(() => {
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
                this.legalParentService.deleteLegalParent(registry.id).subscribe((x) => {
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
                this.legalParentService.updateLegalParent(registry.id, registry).subscribe((x) => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            } else {
                this.legalParentService.createLegalParent(registry).subscribe((x) => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            }
        }
    }

    fetchData() {
        this.legalParentService.getLegalParents().subscribe((z) => {
            this.data = z.object;
            this.loading = false;
        });
    }
}
