import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImportEmployeesProjectDataComponent } from './components';

const routes: Routes = [
  { path: 'import', component: ImportEmployeesProjectDataComponent },
  { path: '', pathMatch: 'full', redirectTo: 'import' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
