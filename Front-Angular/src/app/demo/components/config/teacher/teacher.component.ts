import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { FormField, MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { GraduationService } from 'src/app/demo/service/graduation.service';
import { TeacherService } from 'src/app/demo/service/teacher.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './teacher.component.html',
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
export class TeacherComponent implements OnInit {
    dialog: boolean = false;
    loading: boolean = true;
    cols: TableColumn[] = [];
    data: any[] = [];
    graduationsListbox: any[] = [];
    teachersListbox: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = {};
    constructor(
        protected layoutService: LayoutService,
        private teacherService: TeacherService,
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
                field: 'mainTeacherName',
                header: 'Nome Professor Principal',
                type: 'text',
            },
            {
                field: 'graduationName',
                header: 'Graduação',
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
        //     name: 'diogo',
        //     rg: '11.111.111-1',
        //     cpf: '111.111.111-11',
        //     email: 'teste@teste.com',
        //     phoneNumber: '47999999999',
        //     graduationId: 1,
        //     password: 'ad12345',
        // };
        this.selectedRegistry = {};
        this.modalDialog = true;
    }

    editRegistry(registry: any) {
        this.selectedRegistry = { ...registry };
        this.selectedRegistry.mainTeacherId = this.selectedRegistry.mainTeacherId == 0 ? undefined : this.selectedRegistry.mainTeacherId;
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
            !this.selectedRegistry.rg ||
            !this.selectedRegistry.cpf ||
            !this.selectedRegistry.email ||
            !this.selectedRegistry.phoneNumber ||
            (!this.selectedRegistry.id && !this.selectedRegistry.password) ||
            !this.selectedRegistry.graduationId
        ) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
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
        if (this.validateData()) {
            if (this.selectedRegistry.id) {
                this.teacherService.updateTeacher(this.selectedRegistry.id, this.selectedRegistry).subscribe((x) => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.teacherService.createTeacher(this.selectedRegistry).subscribe((x) => {
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
                this.teacherService.deleteTeacher(registry.id).subscribe((x) => {
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
                this.teacherService.updateTeacher(registry.id, registry).subscribe(() => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            } else {
                this.teacherService.createTeacher(registry).subscribe(() => {
                    this.fetchData();
                    this.modalDialog = false;
                });
            }
        }
    }

    fetchData() {
        this.teacherService.getTeacherForListbox().subscribe((x) => {
            this.teachersListbox = x.object;
            this.graduationService.getGraduationsForListbox().subscribe((y) => {
                this.graduationsListbox = y.object;
                this.teacherService.getTeachers().subscribe((z) => {
                    this.data = z.object;
                    this.loading = false;
                });
            });
        });
    }
}
