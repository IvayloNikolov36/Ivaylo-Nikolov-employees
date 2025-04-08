import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeesService } from '../../services/employees.service';

@Component({
  selector: 'app-import-employees-project-data',
  templateUrl: './import-employees-project-data.component.html',
  standalone: false
})
export class ImportEmployeesProjectDataComponent implements OnInit {

  form!: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private employeesService: EmployeesService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  uploadFile(formValue: any): void {
    const file: File = formValue.fileData;

    this.employeesService.uploadEmployeesData(file)
      .subscribe();
  }

  onFileChange(event: Event): void {
    const files: FileList | null = (event.target as HTMLInputElement).files;
    const file: File | null = files === null ? null : files[0];
    this.form.patchValue({ fileData: file });
  }

  private initializeForm(): void {
    this.form = this.formBuilder.group({
      fileData: [null, [Validators.required]]
    });
  }
}
