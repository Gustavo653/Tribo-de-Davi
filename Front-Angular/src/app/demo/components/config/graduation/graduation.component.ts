import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { MessageServiceSuccess, TableColumn, UploadEvent } from 'src/app/demo/api/base';
import { GraduationService } from 'src/app/demo/service/graduation.service';
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
    dialog: boolean = false;
    loading: boolean = true;
    cols: TableColumn[] = [];
    data: any[] = [];
    uploadedFiles: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = {};
    stateOptions: any[] = [
        { label: 'Criança', value: 0 },
        { label: 'Adulto', value: 1 },
    ];
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
        if (!this.selectedRegistry.name || !this.selectedRegistry.position || this.selectedRegistry.graduationType == undefined || (!this.selectedRegistry.id && !this.uploadedFiles[0])) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        return true;
    }

    save() {
        this.selectedRegistry.file = this.uploadedFiles[0];
        if (this.validateData()) {
            if (this.selectedRegistry.id) {
                this.graduationService.updateGraduation(this.selectedRegistry.id, this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.graduationService.createGraduation(this.selectedRegistry).subscribe(() => {
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
                this.graduationService.deleteGraduation(registry.id).subscribe((x) => {
                    this.messageService.add(MessageServiceSuccess);
                    this.fetchData();
                });
            },
        });
    }

    onUpload(event: UploadEvent) {
        for (let file of event.files) {
            this.uploadedFiles.push(file);
        }
    }

    removeFile(event: any): void {
        const file: File = event;
        const index = this.uploadedFiles.findIndex((uploadedFile: File) => uploadedFile.name === file.name);
        if (index !== -1) {
            this.uploadedFiles.splice(index, 1);
        }
    }

    fetchData() {
        this.graduationService.getGraduations().subscribe((x) => {
            this.data = x.object;
            this.loading = false;
        });
    }
}
