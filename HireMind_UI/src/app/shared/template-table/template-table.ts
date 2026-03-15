import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { CardModule } from 'primeng/card';
import { MenuItem } from 'primeng/api';
import { MenuModule } from 'primeng/menu';
import { TooltipModule } from 'primeng/tooltip';
import { CheckboxModule } from 'primeng/checkbox';
import { ToolbarModule } from 'primeng/toolbar';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-template-table',
  standalone: true,
  templateUrl: './template-table.html',
  styleUrls: ['./template-table.css'],
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    ToggleSwitchModule,
    InputTextModule,
    IconFieldModule,
    InputIconModule,
    DatePickerModule,
    CardModule,
    MenuModule,
    TooltipModule,
    CheckboxModule,
    ToolbarModule,
    FileUploadModule,
    ToastModule
  ]
})
export class TemplateTable {
  @ViewChild("menu") menu: any;
  @Input() actionsModel: MenuItem[] = [];
  @Output() onFilePreview = new EventEmitter<File>();

  @Input() data: any[] = [];
  @Input() columns: { field: string, header: string, isImage?: boolean, isColor?: boolean }[] = []; // 👈 أضف isColor هنا
  @Input() totalRecords: number = 0;
  @Input() title!: string;
  @Input() isLoading: boolean = false;
  @Input() withoutImportAndExport: boolean = false;

  @Output() onEdit = new EventEmitter<any>();
  @Output() onApply = new EventEmitter<any>();
  @Output() onManageApplications = new EventEmitter<any>();
  @Output() onExport = new EventEmitter<any>();
  @Output() onGeneratePdf = new EventEmitter<any>();
  @Output() onImport = new EventEmitter<any>();
  @Output() onPreview = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<number>();
  @Output() onImageClick = new EventEmitter<{ item: any }>();
  @Output() onLazyLoad = new EventEmitter<any>();
  @Output() onToggleActive = new EventEmitter<any>();
  @Output() onAdd = new EventEmitter<void>();
  @Output() onSearch = new EventEmitter<{ searchValue: string; dateSearch: Date | null }>();

  selectedRows: any[] = [];
  searchValue: string = '';
  dateSearch: Date | null = null;

  currentRowData: any;

  delete(id: number) {
    this.onDelete.emit(id);
  }

  showImage(item: any) {
    this.onImageClick.emit({ item });
  }

  isDate(value: any): boolean {
    return value && !isNaN(Date.parse(value));
  }

  onDateClear(event?: Event) {
    event?.stopPropagation?.();
    this.searchValue = '';
    this.dateSearch = null;
    this.onSearch.emit({ searchValue: '', dateSearch: null });
  }

  search() {
    this.onSearch.emit({
      searchValue: this.searchValue,
      dateSearch: this.dateSearch
    });
  }

  clearSearch() {
    this.searchValue = '';
    this.dateSearch = null;
    this.search();
  }

  calculateAge(dob: string | Date): number | string {
    if (!dob) return 'No Value';
    const birthDate = new Date(dob);
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();
    const month = today.getMonth() - birthDate.getMonth();

    if (month < 0 || (month === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age;
  }

  toggleMenu(event: any, rowData: any) {
    console.log('toggleMenu called, rowData:', rowData);
    this.currentRowData = rowData;
    const menuItemsWithCommand = this.actionsModel.map(item => ({
      ...item,
      command: () => {
        if (item.label === 'Edit') {
          this.onEdit.emit(this.currentRowData.id);
        } else if (item.label === 'Preview') {
          this.onPreview.emit(this.currentRowData.id);
        } else if (item.label === 'Delete') {
          this.onDelete.emit(this.currentRowData.id);
        } else if (item.label === 'JobApplication') {   // ✅ FIX
          this.onApply.emit(this.currentRowData.id);
        }
        else if (item.label === 'ManageApplications') {   // ✅ FIX
          this.onManageApplications.emit(this.currentRowData.id);
        }
        else if (item.label === 'PrintPDF') {
          this.onGeneratePdf.emit(this.currentRowData.id);
        }
      }
    }));
    this.menu.model = menuItemsWithCommand;
    this.menu.toggle(event);
  }

  add() {
    this.onAdd.emit();
  }


  fileSelected(event: any) {
    const file: File = event.files[0];
    if (file) {
      this.onFilePreview.emit(file);
    }
  }

  export() {
    this.onExport.emit(this.selectedRows);
  }

  showToolbar: boolean = true;
  defaultUserIcon = 'data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjY2NjIiBoZWlnaHQ9IjQ4IiB3aWR0aD0iNDgiIHZpZXdCb3g9IjAgMCA0OCA0OCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48Y2lyY2xlIGN4PSIyNCIgY3k9IjE0IiByPSIxMCIvPjxwYXRoIGQ9Ik0yNCAyNmMtOC44IDAtMTYgNy4yLTE2IDE2aDMyaC0wLjAwMUM0MCAzMy4yIDMyLjggMjYgMjQgMjZ6Ii8+PC9zdmc+';

}
