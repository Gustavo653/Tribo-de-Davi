import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { FormField, MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { GraduationService } from 'src/app/demo/service/graduation.service';
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
    graduationsListbox: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = { address: {}, legalParent: {} };
    constructor(
        protected layoutService: LayoutService,
        private studentService: StudentService,
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
        // this.selectedRegistry = {
        //     name: 'ffffffffffff',
        //     birthDate: '2020-08-30T17:40:51.853Z',
        //     weight: 2147483647,
        //     height: 2147483647,
        //     graduationId: 1,
        //     phoneNumber: '11111111111',
        //     email: 'user@example.com',
        //     password: 'string',
        //     cpf: '877.226.889-87',
        //     schoolName: 'stringdd',
        //     schoolGrade: 0,
        //     address: {
        //         streetName: 'string',
        //         streetNumber: 'string',
        //         neighborhood: 'string',
        //         city: 'string',
        //     },
        //     legalParent: {
        //         name: 'string',
        //         relationship: 'string',
        //         rg: '79.695.857-7',
        //         cpf: '140.293.030-55',
        //         phoneNumber: '11111111111',
        //     },
        // };
        this.selectedRegistry = { address: {}, legalParent: {} };
        this.modalDialog = true;
    }

    editRegistry(registry: any) {
        this.selectedRegistry = { ...registry };
        this.birthDate = new Date(this.selectedRegistry.birthDate);
        this.modalDialog = true;
    }

    hideDialog() {
        this.selectedRegistry = { address: {}, legalParent: {} };
        this.modalDialog = false;
    }

    validateData(): boolean {
        const rgPattern = /^[0-9]{2}\.[0-9]{3}\.[0-9]{3}-[0-9]{1}$/;
        const cpfPattern = /^[0-9]{3}\.[0-9]{3}\.[0-9]{3}-[0-9]{2}$/;
        const phoneNumberPattern = /^[0-9]{11}$/;
        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

        if (
            !this.selectedRegistry.name ||
            !this.selectedRegistry.legalParent.name ||
            !this.selectedRegistry.legalParent.relationship ||
            !this.selectedRegistry.legalParent.cpf ||
            !this.selectedRegistry.legalParent.rg ||
            !this.selectedRegistry.legalParent.phoneNumber ||
            !this.selectedRegistry.email ||
            !this.selectedRegistry.weight ||
            !this.selectedRegistry.height ||
            !this.selectedRegistry.birthDate ||
            !this.selectedRegistry.phoneNumber ||
            (!this.selectedRegistry.id && !this.selectedRegistry.password) ||
            !this.selectedRegistry.graduationId
        ) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        if (this.selectedRegistry.rg && !this.selectedRegistry.rg.match(rgPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'RG inválido' });
            return false;
        }

        if (this.birthDate > new Date()) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Data de Nascimento inválida' });
            return false;
        }

        if (this.selectedRegistry.cpf && !this.selectedRegistry.cpf.match(cpfPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'CPF inválido' });
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

        if (this.selectedRegistry.address && Object.keys(this.selectedRegistry.address).length > 0) {
            if (!this.selectedRegistry.address.streetName || !this.selectedRegistry.address.streetNumber || !this.selectedRegistry.address.neighborhood || !this.selectedRegistry.address.city) {
                this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Se algum campo do endereço estiver preenchido, todos os campos do endereço são obrigatórios.' });
                return false;
            }
        }

        if (!this.selectedRegistry.legalParent.rg.match(rgPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'RG do responsável legal inválido' });
            return false;
        }

        if (!this.selectedRegistry.legalParent.cpf.match(cpfPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'CPF do responsável legal inválido' });
            return false;
        }

        if (!this.selectedRegistry.legalParent.phoneNumber.match(phoneNumberPattern)) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Número de telefone do responsável legal inválido' });
            return false;
        }

        return true;
    }

    save() {
        if (this.validateData()) {
            if (Object.keys(this.selectedRegistry.address).length === 0) {
                this.selectedRegistry.address = undefined;
            }
            this.selectedRegistry.birthDate = this.birthDate;
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
    }
}
