import { Directive, OnInit } from '@angular/core';
import { SearchFilters } from '../models/Shared/searchFilters';
import { ToastMessageService } from '../services/shared/toast-message.service';

@Directive()
export abstract class BasePageComponent<T> implements OnInit {

  data: T[] = [];
  entity: any;
  totalCount: number = 0;

  visibleDetails: boolean = false;
  visibleImage: boolean = false;
  visibleConfirmation: boolean = false;

  imagePreview: string | null = null;
  imageDialog: any;
  deleteId: any = null;
  nameImageDialog: string | null = null;

  searchValue: string = '';
  dateSearch: Date | null = null;

  abstract service: any;
  abstract columns: any[];
  abstract entityName: string;
  abstract createNewEntity(): T;
  abstract toastService: ToastMessageService;


  ngOnInit(): void {
    this.search({ first: 0, rows: 5 });
  }

  loadData(event: any = { first: 0, rows: 5 }) {
    const page = event.first / event.rows + 1;
    const request = {
      pageNumber: page,
      pageSize: event.rows
    };
    this.service.search().subscribe({
      next: (res: any) => {
        this.data = res.response;
        this.totalCount = res.response.length;
      },
      error: (err: any) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Load Failed',
          messageBody: `Failed to load ${this.entityName} data.`
        });
        console.error('Load data failed', err);
      }
    });
  }

  getById(id: any) {

    this.isDialogLoading = true;

    this.service.GetById(id).subscribe({
      next: (res: any) => {
        this.entity = res.response;
        this.isDialogLoading = false;
      },
      error: (err: any) => {
        this.isDialogLoading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Fetch Failed',
          messageBody: `Failed to fetch ${this.entityName}.`
        });

        console.error('Get by id failed', err);
      }
    });
  }

  addEntity() {
    const request = { ...this.entity };
    this.service.Add(request).subscribe({
      next: () => {
        this.isLoading = false;
        this.isSaving = false;

        this.clearDialog();
        this.search({ first: 0, rows: 5 });
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Added',
          messageBody: `${this.entityName} added successfully.`
        });
      },
      error: (err: any) => {
        this.isLoading = false;
        this.isSaving = false;

        this.toastService.showMessage({ 
          messageType: 'error',
          messageTitle: 'Add Failed',
          messageBody: `Failed to add ${this.entityName}.`
        });
        console.error('Add failed', err);
      }
    });
  }

  updateEntity() {    
    const request = { ...this.entity };
    this.service.Update(request).subscribe({
      next: () => {
        this.isLoading = false;
        this.isSaving = false;

        this.clearDialog();
        this.search({ first: 0, rows: 5 });
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Updated',
          messageBody: `${this.entityName} updated successfully.`
        });
      },
      error: (err: any) => {
        this.isSaving = false;
        this.isLoading = false;
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Update Failed',
          messageBody: `Failed to update ${this.entityName}.`
        });
        console.error('Update failed', err);
      }
    });
  }

  deleteEntity(id: any) {
    this.service.Delete(id).subscribe({
      next: () => {
        this.closeConfirmationDialog();
        //this.loadData();
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Deleted',
          messageBody: `${this.entityName} deleted successfully.`
        });
        this.search({ first: 0, rows: 5 });
      },
      error: (err: any) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Delete Failed',
          messageBody: `Failed to delete ${this.entityName}.`
        });
        console.error('Delete failed', err);
      }
    });
  }

  openConfirmationDialog(id: any) {
    console.log("id base", id)
    this.deleteId = id;
    this.visibleConfirmation = true;
  }

  closeConfirmationDialog() {
    this.visibleConfirmation = false;
    this.deleteId = null;
  }

  onFileSelected(event: Event) {
    const fileInput = event.target as HTMLInputElement;
    if (fileInput.files && fileInput.files.length > 0) {
      const file = fileInput.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.entity.image = reader.result as string;
        this.imagePreview = this.entity.image;
      };
      reader.readAsDataURL(file);
    }
  }

  clearDialog() {
    this.entity = this.createNewEntity();
    this.imagePreview = null;
    this.visibleDetails = false;
  }

  openDetailsDialog() {
    this.visibleDetails = true;
  }


  closeImageDialog() {
    this.visibleImage = false;
    this.imageDialog = null;
    this.nameImageDialog = null;
  }

  isLoading: boolean = false;
  isDialogLoading: boolean = false; // loading getById
  isSaving: boolean = false;        // loading save

  search(event: any = { first: 0, rows: 5 }) {

    this.isLoading = true;

    const filter = new SearchFilters();
    filter.pageNumber = event.first / event.rows + 1;
    filter.pageSize = event.rows;
    filter.searchTerm = this.searchValue;

    this.service.Search(filter).subscribe({
      next: (res: any) => {

        console.log("API RESULT", res);

        this.data = res.items ?? [];
        this.totalCount = res.totalCount ?? this.data.length;

        this.isLoading = false;
      },
      error: (err: any) => {
        this.isLoading = false;
        console.error(err);
      }
    });

  }
  clearSearch() {
    this.searchValue = '';
    this.dateSearch = null;
    this.search();
  }

  searchFromTable(event: { searchValue: string; dateSearch: Date | null }) {
    this.searchValue = event.searchValue;
    this.dateSearch = event.dateSearch;
    this.search();
  }

  toggleActive(item: any) {
    const request = { ...item };
    this.service.Update(request).subscribe({
      next: () => this.search(),
      error: (err: any) => console.error('Toggle active failed', err)
    });
  }

  submit(updatedModel: any) {

    if (this.isSaving) return;

    this.isSaving = true;

    if (this.entity.id)
      this.updateEntity();
    else
      this.addEntity();
  }

  editEntity(item: T) {
    this.service.GetById((item as any).id).subscribe({
      next: (res: any) => {
        this.entity = res;
        this.imagePreview = res?.image || null;
        this.openDetailsDialog();
      },
      error: (err: any) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Fetch Failed',
          messageBody: `Failed to fetch ${this.entityName}.`
        });
        console.error('Edit entity failed', err);
      }
    });
  }


  showImageDialog(...args: any[]) {
    const title = args.length === 1 ? args[0] : args.slice(0, -1).join(' / ');
    const image = args[args.length - 1] as string;
    this.nameImageDialog = title;
    this.imageDialog = image;
    this.visibleImage = true;
  }

  resetForm(form: any) {
    form.resetForm();
    this.entity = this.createNewEntity();
    this.imagePreview = null;
  }
}

