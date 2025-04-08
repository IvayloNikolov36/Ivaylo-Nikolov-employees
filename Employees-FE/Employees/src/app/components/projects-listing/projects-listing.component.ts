import { Component, Input } from '@angular/core';
import { ProjectEmployeesDataModel } from '../../models';

@Component({
  selector: 'app-projects-listing',
  templateUrl: './projects-listing.component.html',
  standalone: false
})
export class ProjectsListingComponent {

  @Input() projectsData!: ProjectEmployeesDataModel[];

}
