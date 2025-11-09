import { Directive, OnInit } from '@angular/core';
import { SearchFilters } from '../models/Shared/searchFilters';

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

  ngOnInit(): void {
  //  this.loadData();
  }

  loadData(event: any = { first: 0, rows: 5 }) {
    const page = event.first / event.rows + 1;
    const request = {
      pageNumber: page,
      pageSize: event.rows
    };
    this.service.GetAll().subscribe((res: any) => {
      this.data = res.response;
      this.totalCount = res.response.length;
      console.log('First row:', this.data[0]); 

    });
  }

  getById(id: any) {
    this.service.GetById(id).subscribe((res: any) => {
      this.entity = res.response;
    });
  }

  addEntity() {
    const request = { ...this.entity };
    this.service.Add(request).subscribe({
      next: () => {
        this.clearDialog();
        this.loadData();
      },
      error: (err: any) => console.error('Add failed', err)
    });
  }

  updateEntity() {
    const request = { ...this.entity };
    this.service.Update(request).subscribe({
      next: () => {
        this.clearDialog();
        this.loadData();
      },
      error: (err: any) => console.error('Update failed', err)
    });
  }

  deleteEntity(id: any) {
    this.service.Delete(id).subscribe({
      next: () => {
        this.closeConfirmationDialog();
        this.loadData();
      },
      error: (err: any) => console.error('Delete failed', err)
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

  search(event: any = { first: 0, rows: 5 }) {
    const filter = new SearchFilters();
    filter.pageNumber = event.first / event.rows + 1;
    filter.pageSize = event.rows;
    filter.searchTerm = this.searchValue;

    if (this.dateSearch) {
      const date = new Date(this.dateSearch);
      const utcDate = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
      filter.dateSearch = utcDate;
    }

    this.service.Search(filter).subscribe((res: any) => {
      this.data = res.items;
      this.totalCount = res.totalCount || res.items.length;
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
    if (this.entity.id) this.updateEntity();
    else this.addEntity();
  }

  editEntity(item: T) {
    this.service.GetById((item as any).id).subscribe((res: any) => {
      this.entity = res;
      this.imagePreview = res?.image || null;
      this.openDetailsDialog();
    });
  }


  showImageDialog(...args: any[]) {
    const title = args.length === 1 ? args[0] : args.slice(0, -1).join(' / ');
    const image = args[args.length - 1] as string;
    this.nameImageDialog = title;
    this.imageDialog = image;
    this.visibleImage = true;
  }
}

