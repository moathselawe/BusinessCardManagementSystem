import { Component } from '@angular/core';
import { BasePageComponent } from '../../../shared/BasePageComponent';
import { MenuItem } from 'primeng/api';
import { Router } from '@angular/router';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { BusinessCardService } from '../../../services/bcms/businessCard.service';
import { BusinessCard } from '../../../models/bcms/businessCard';

@Component({
  selector: 'app-manage-businesscards-component',
  standalone: false,
  templateUrl: './manage-businesscards-component.html',
  styleUrl: './manage-businesscards-component.css'
})

export class ManageBusinesscardsComponent extends BasePageComponent<BusinessCard> {
  constructor(public override service: BusinessCardService, private router: Router, public override toastService: ToastMessageService) {
    super();
    this.entity = this.createNewEntity();
  }

  entityName = 'BusinessCard';

  columns = [
    { field: 'arabicName', header: 'Arabic Name' },
    { field: 'englishName', header: 'English Name' },
    { field: 'dateOfBirth', header: 'Age' },
    { field: 'email', header: 'Email' },
    { field: 'phone', header: 'Phone' },
    { field: 'logo', header: 'Logo', isImage: true },
    { field: 'address', header: 'Address' },
  ];

  createNewEntity(): BusinessCard {
    return new BusinessCard();
  }

  actionsModel: MenuItem[] = [
    { label: 'Preview', icon: 'pi pi-eye' },
    { label: 'PrintPDF', icon: 'pi pi-file-pdf' },
    { label: 'Edit', icon: 'pi pi-pencil' },
    { label: 'Delete', icon: 'pi pi-trash' }
  ];

  Add() {
    this.router.navigate(['/BCMS/CreateBusinesscard']);
  }

  edit(rowId: string) {
    this.router.navigate(['/BCMS/ModifyBusinesscard', rowId]);
  }

  preview(rowId: string) {
    this.router.navigate(['/BCMS/PreviewBusinesscard', rowId], { queryParams: { readonly: true } });
  }

  handleFilePreview(file: File) {
    this.service.previewFile(file).subscribe({
      next: (res: any) => {
        const cards = (res as any).cards;
        console.log("manage cards:", cards);
        this.router.navigate(['/BCMS/CreateMulipleBusinesscards'], { state: { previewCards: cards } });
      },
      error: (err) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Preview Failed',
          messageBody: 'Error previewing file.'
        });
        console.error('Error previewing file', err);
      }
    });
  }

  selectedRows!: any[];
  visibleEportFile: boolean = false;
  selectedfiletype: string = "csv";

  fileTypeOptions = [
    { label: 'Excel', value: 'csv', icon: 'pi pi-file-excel' },
    { label: 'Xml', value: 'xml', icon: 'pi pi-file' }
  ];

  export(selectedRows: any[]) {
    this.visibleEportFile = true;
    if (!selectedRows || selectedRows.length === 0)
      this.errorExportdialog = true;
    else
      this.selectedRows = selectedRows;
  }

  exportGeneratur() {
    if (!this.selectedRows || this.selectedRows.length === 0) {
      return;
    }

    const exportRequest = {
      fileType: this.selectedfiletype,
      ids: this.selectedRows.map(x => x.id)
    };

    this.service.ExportFile(exportRequest).subscribe({
      next: (response: Blob) => {
        const fileURL = window.URL.createObjectURL(response);
        const a = document.createElement('a');
        a.href = fileURL;
        a.download = `BusinessCardsExport.${this.selectedfiletype}`;
        a.click();
        window.URL.revokeObjectURL(fileURL);

        this.visibleEportFile = false;
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Exported',
          messageBody: `Business cards exported as ${this.selectedfiletype.toUpperCase()}.`
        });
      },      error: (error) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Export Failed',
          messageBody: 'Failed to export business cards.'
        });
        console.error('Export failed:', error);
      }
    });
  }
   
  errorExportdialog: boolean = false;

  onCloseEportFile() {
    this.visibleEportFile = false;
    this.errorExportdialog = false;
  }

  generatePdf(rowId: string) {
    this.service.generatePdf(rowId).subscribe((pdfBlob: Blob) => {
      const url = window.URL.createObjectURL(pdfBlob);
      window.open(url);
    });
  }
}

