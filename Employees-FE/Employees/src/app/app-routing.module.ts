import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ImportEmployeesProjectDataComponent } from './components';

const routes: Routes = [
  { path: 'import', component: ImportEmployeesProjectDataComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
