import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { MessageServiceSuccess, TableColumn, UploadEvent } from 'src/app/demo/api/base';
import { AddressService } from 'src/app/demo/service/address.service';
import { GraduationService } from 'src/app/demo/service/graduation.service';
import { LegalParentService } from 'src/app/demo/service/legalParent.service';
import { StudentService } from 'src/app/demo/service/student.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './student.component.html',
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
export class StudentComponent implements OnInit {
    dialog: boolean = false;
    loading: boolean = true;
    birthDate: Date = new Date();
    cols: TableColumn[] = [];
    data: any[] = [];
    uploadedFiles: any[] = [];
    graduationsListbox: any[] = [];
    addressesListbox: any[] = [];
    legalParentsListbox: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = {};
    constructor(
        protected layoutService: LayoutService,
        private studentService: StudentService,
        private graduationService: GraduationService,
        private confirmationService: ConfirmationService,
        private addressService: AddressService,
        private legalParentService: LegalParentService,
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
                field: 'email',
                header: 'Email',
                type: 'text',
            },
            {
                field: 'phoneNumber',
                header: 'Telefone',
                type: 'text',
            },
            {
                field: 'graduation.name',
                header: 'Graduação',
                type: 'text',
            },
            {
                field: 'legalParent.name',
                header: 'Responsável Legal',
                type: 'text',
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
        this.birthDate = new Date(this.selectedRegistry.birthDate);
        this.modalDialog = true;
    }

    hideDialog() {
        this.selectedRegistry = {};
        this.modalDialog = false;
    }

    validateData(): boolean {
        const phoneNumberPattern = /^[0-9]{11}$/;
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

        if (
            !this.selectedRegistry.name ||
            !this.selectedRegistry.email ||
            !this.selectedRegistry.weight ||
            !this.selectedRegistry.height ||
            !this.birthDate ||
            !this.selectedRegistry.phoneNumber ||
            (!this.selectedRegistry.id && !this.uploadedFiles[0]) ||
            (!this.selectedRegistry.id && !this.selectedRegistry.password) ||
            !this.selectedRegistry.graduationId
        ) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        if (this.birthDate > new Date()) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Data de Nascimento inválida' });
            return false;
        }

        if (!this.selectedRegistry.phoneNumber.match(phoneNumberPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Número de telefone inválido' });
            return false;
        }

        if (!this.selectedRegistry.email.match(emailPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Email inválido' });
            return false;
        }

        return true;
    }

    save() {
        this.selectedRegistry.file = this.uploadedFiles[0];
        if (this.validateData()) {
            this.selectedRegistry.birthDate = this.birthDate.toISOString();
            if (this.selectedRegistry.id) {
                this.studentService.updateStudent(this.selectedRegistry.id, this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.studentService.createStudent(this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            }
        }
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

    deleteRegistry(registry: any) {
        this.confirmationService.confirm({
            header: 'Deletar registro',
            message: `Tem certeza que deseja apagar o registro: ${registry.name}`,
            acceptLabel: 'Aceitar',
            rejectLabel: 'Rejeitar',
            accept: () => {
                this.loading = true;
                this.studentService.deleteStudent(registry.id).subscribe((x) => {
                    this.messageService.add(MessageServiceSuccess);
                    this.fetchData();
                });
            },
        });
    }

    fetchData() {
        this.addressService.getAddressesForListbox().subscribe((v) => {
            this.addressesListbox = v.object;
            this.legalParentService.getLegalParentsForListbox().subscribe((x) => {
                this.legalParentsListbox = x.object;
                this.graduationService.getGraduationsForListbox().subscribe((y) => {
                    this.graduationsListbox = y.object;
                    this.studentService.getStudents().subscribe((z) => {
                        this.data = z.object;
                        this.data.forEach((x) => {
                            x.address = x.address == null ? {} : x.address;
                        });
                        this.loading = false;
                    });
                });
            });
        });
    }
}
