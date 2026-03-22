import { Component } from '@angular/core';
import { BasePageComponent } from '../../../shared/BasePageComponent';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { Job } from '../../../models/hiremind/Job';
import { ManageJobsService } from '../../../services/hiremind/manageJobs.service';

@Component({ 
  selector: 'app-manage-jobs-component', 
  standalone: false,
  templateUrl: './manage-jobs-component.html',
  styleUrl: './manage-jobs-component.css'
}) 
export class ManageJobsComponent extends BasePageComponent<Job> {

  constructor(
    public override service: ManageJobsService,
    private router: Router,
    public override toastService: ToastMessageService
  ) {
    super();
    this.entity = this.createNewEntity();
  }

  entityName = 'Job';

  columns = [
    { field: 'title', header: 'Title' },
    { field: 'locationName', header: 'Location' },
    { field: 'jobTypeName', header: 'Job Type' },
    { field: 'companyId', header: 'Company ID' }, 
    { field: 'startDate', header: 'Start Date' },
    { field: 'endDate', header: 'End Date' },
    { field: 'isActive', header: 'Active' }
  ];

  createNewEntity(): Job {
    return new Job();
  }

  actionsModel: MenuItem[] = [
    { label: 'Preview', icon: 'pi pi-eye' },
    { label: 'Edit', icon: 'pi pi-pencil' },
    { label: 'Delete', icon: 'pi pi-trash' },
    { label: 'Job Application', icon: 'pi pi-plus' },
    { label: 'Manage Applications', icon: 'pi pi-list' },
    { label: 'Manage Stages & Questions', icon: 'pi pi-wrench' }
  ];

  Add() {
    this.router.navigate(['/HireMind/CreateJob']);
  }

  edit(rowId: any) {
    this.router.navigate(['/HireMind/ModifyJob', rowId]);
  }

  editManageStagesAndQuestions(rowId: any) {
    this.router.navigate(['/HireMind/ManageStagesAndQuestions', rowId]);
  }

  preview(rowId: any) {
    this.router.navigate(['/HireMind/PreviewJob', rowId], { queryParams: { readonly: true } });
  }

  apply(rowId: any) {
    this.router.navigate(['/HireMind/JobApplication', rowId]);
  }

  manageApplications(rowId: any) {
    this.router.navigate(['/HireMind/ManageApplications', rowId]);
  }

  selectedRows!: any[];
  visibleEportFile: boolean = false;
  selectedfiletype: string = "csv";

  fileTypeOptions = [
    { label: 'Excel', value: 'csv', icon: 'pi pi-file-excel' },
    { label: 'Xml', value: 'xml', icon: 'pi pi-file' }
  ];

  override toggleActive(rowData: any) {
    const request = {
      id: rowData.id,
      isActive: rowData.isActive
    };

    this.service.UpdateActivation(request).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Updated',
          messageBody: 'Job Activation updated successfully.'
        });
      },
      error: () => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to update Job Activation.'
        });
      }
    });
  }
}
