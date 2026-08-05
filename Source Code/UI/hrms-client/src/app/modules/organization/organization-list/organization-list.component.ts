import { Component, OnInit, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgClass } from '@angular/common';
import { OrganizationService, Organization } from '../../../core/services/organization.service';

@Component({
  selector: 'app-organization-list',
  standalone: true,
  imports: [RouterModule, NgClass],
  templateUrl: './organization-list.component.html',
  styleUrl: './organization-list.component.scss'
})
export class OrganizationListComponent implements OnInit {
  private orgService = inject(OrganizationService);
  
  organizations: Organization[] = [];
  isLoading = true;

  ngOnInit() {
    this.orgService.searchOrganizations().subscribe({
      next: (data: any) => {
        this.organizations = data;
        this.isLoading = false;
      },
      error: (err: any) => {
        console.error('Error fetching organizations', err);
        this.isLoading = false;
      }
    });
  }
}
