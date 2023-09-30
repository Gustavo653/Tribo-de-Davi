import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { AddressService } from 'src/app/demo/service/address.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './address.component.html',
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
export class AddressComponent implements OnInit {
    dialog: boolean = false;
    loading: boolean = true;
    cols: TableColumn[] = [];
    data: any[] = [];
    addressesListbox: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = {};
    constructor(protected layoutService: LayoutService, private addressService: AddressService, private confirmationService: ConfirmationService, private messageService: MessageService) {}

    ngOnInit() {
        this.cols = [
            {
                field: 'streetName',
                header: 'Rua',
                type: 'text',
            },
            {
                field: 'streetNumber',
                header: 'Número',
                type: 'text',
            },
            {
                field: 'neighborhood',
                header: 'Bairro',
                type: 'text',
            },
            {
                field: 'fieldOperationsCount',
                header: 'Quantidade de Campos de Operação',
                type: 'text',
            },
            {
                field: 'studentsCount',
                header: 'Quantidade de Alunos',
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
        if (!this.selectedRegistry.streetName || !this.selectedRegistry.streetNumber || !this.selectedRegistry.neighborhood) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        return true;
    }

    save(val?: number) {
        if (val == 1) this.selectedRegistry.id = undefined;
        if (this.validateData()) {
            if (this.selectedRegistry.id) {
                this.addressService.updateAddress(this.selectedRegistry.id, this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.addressService.createAddress(this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            }
        }
    }

    deleteRegistry(registry: any) {
        this.confirmationService.confirm({
            header: 'Deletar registro',
            message: `Tem certeza que deseja apagar o registro: ${registry.streetName}`,
            acceptLabel: 'Aceitar',
            rejectLabel: 'Rejeitar',
            accept: () => {
                this.loading = true;
                this.addressService.deleteAddress(registry.id).subscribe((x) => {
                    this.messageService.add(MessageServiceSuccess);
                    this.fetchData();
                });
            },
        });
    }

    fetchData() {
        this.addressService.getAddresses().subscribe((y) => {
            this.data = y.object;
            this.loading = false;
        });
    }
}
