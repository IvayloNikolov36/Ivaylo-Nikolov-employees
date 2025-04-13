import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeesService } from '../../services/employees.service';
import { ProjectEmployeesDataModel } from '../../models';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-import-employees-project-data',
  templateUrl: './import-employees-project-data.component.html',
  standalone: false
})
export class ImportEmployeesProjectDataComponent implements OnInit {

  form!: FormGroup;

  projectsData!: ProjectEmployeesDataModel[];
  dateFormats!: string[];

  constructor(
    private formBuilder: FormBuilder,
    private employeesService: EmployeesService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeDateFormats();
    this.initializeForm();
  }

  uploadFile(formValue: any): void {
    const file: File = formValue.fileData;
    const dateformat: string = formValue.dateFormat;

    this.employeesService.uploadEmployeesData(file, dateformat)
      .subscribe({
        next: (data: ProjectEmployeesDataModel[]) => {
          this.projectsData = data;
        },
        error: (err) => this.toastr.error(err.error.errors?.join(' ').trim())
      });
  }

  onFileChange(event: Event): void {
    const files: FileList | null = (event.target as HTMLInputElement).files;
    const file: File | null = files === null ? null : files[0];
    this.form.patchValue({ fileData: file });
  }

  changeDateFormat(selectedDateFormat: string): void {
    this.form.patchValue({ dateFormat: selectedDateFormat });
  }

  private initializeDateFormats(): void {
    this.dateFormats = [
      'yyyy-MM-dd',
      'MM/dd/yyyy',
      'dd.MM.yyyy',
      'd.M.yyyy',
      'd.M.yy',
      'M.d.yyyy',
      'MM.dd.yyyy',
      'MM/dd/yy',
      'dd/MM/yy',
      'yy/MM/dd',
      'M/d/yy',
      'd/M/yy',
      'yy/M/d',
    ];
  }

  private initializeForm(): void {
    this.form = this.formBuilder.group({
      fileData: [null, Validators.required],
      dateFormat: [null, Validators.required]
    });
  }
}
