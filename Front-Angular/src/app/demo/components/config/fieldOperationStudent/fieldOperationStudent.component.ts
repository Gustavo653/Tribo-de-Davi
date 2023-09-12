import { Component, OnInit } from '@angular/core';
import { MessageService, ConfirmationService } from 'primeng/api';
import { MessageServiceSuccess, TableColumn } from 'src/app/demo/api/base';
import { FieldOperationService } from 'src/app/demo/service/fieldOperation.service';
import { FieldOperationStudentService } from 'src/app/demo/service/fieldOperationStudent.service';
import { FieldOperationTeacherService } from 'src/app/demo/service/fieldOperationTeacher.service';
import { StudentService } from 'src/app/demo/service/student.service';
import { LayoutService } from 'src/app/layout/service/app.layout.service';

@Component({
    templateUrl: './fieldOperationStudent.component.html',
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
export class FieldOperationStudentComponent implements OnInit {
    dialog: boolean = false;
    loading: boolean = true;
    cols: TableColumn[] = [];
    data: any[] = [];
    studentsListbox: any[] = [];
    fieldOperationTeachersListbox: any[] = [];
    modalDialog: boolean = false;
    selectedRegistry: any = {};
    stateOptions: any[] = [
        { name: 'Não', code: false },
        { name: 'Sim', code: true },
    ];
    constructor(
        private fieldOperationStudentService: FieldOperationStudentService,
        private fieldOperationTeacherService: FieldOperationTeacherService,
        private studentService: StudentService,
        private confirmationService: ConfirmationService,
        private messageService: MessageService
    ) {}

    ngOnInit() {
        this.cols = [
            {
                field: 'fieldOperationTeacherName',
                header: 'Campo de Operação',
                type: 'text',
            },
            {
                field: 'studentName',
                header: 'Aluno',
                type: 'text',
            },
            {
                field: 'enabled',
                header: 'Ativo?',
                type: 'boolean',
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
        if (!this.selectedRegistry.studentId || !this.selectedRegistry.fieldOperationTeacherId || this.selectedRegistry.enabled == undefined) {
            this.messageService.add({ severity: 'error', summary: 'Erro', detail: 'Preencha todos os campos obrigatórios.' });
            return false;
        }

        return true;
    }

    save() {
        if (this.validateData()) {
            if (this.selectedRegistry.id) {
                this.fieldOperationStudentService.updateFieldOperationStudent(this.selectedRegistry.id, this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            } else {
                this.fieldOperationStudentService.createFieldOperationStudent(this.selectedRegistry).subscribe(() => {
                    this.hideDialog();
                    this.fetchData();
                });
            }
        }
    }

    deleteRegistry(registry: any) {
        this.confirmationService.confirm({
            header: 'Deletar registro',
            message: `Tem certeza que deseja apagar o registro?`,
            acceptLabel: 'Aceitar',
            rejectLabel: 'Rejeitar',
            accept: () => {
                this.loading = true;
                this.fieldOperationStudentService.deleteFieldOperationStudent(registry.id).subscribe((x) => {
                    this.messageService.add(MessageServiceSuccess);
                    this.fetchData();
                });
            },
        });
    }

    fetchData() {
        this.studentService.getStudentsForListbox().subscribe((x) => {
            this.studentsListbox = x.object;
            this.fieldOperationTeacherService.getFieldOperationTeachersForListbox().subscribe((x) => {
                this.fieldOperationTeachersListbox = x.object;
                this.fieldOperationStudentService.getFieldOperationStudents().subscribe((z) => {
                    this.data = z.object;
                    this.loading = false;
                });
            });
        });
    }
}
