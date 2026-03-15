//import { Component } from '@angular/core';
//import { BasePageComponent } from '../../../shared/BasePageComponent';
//import { Router } from '@angular/router';
//import { ToastMessageService } from '../../../services/shared/toast-message.service';
//import { MenuItem } from 'primeng/api';
//import { ManageLookupsService } from '../../../services/shared/managelookup.service';
//import { Lookup } from '../../../models/Shared/Lookup';
//import { SearchFilters } from '../../../models/Shared/searchFilters';

//@Component({
//  selector: 'app-manage-lookups-component',
//  standalone: false,
//  templateUrl: './manage-lookups-component.html',
//  styleUrls: ['./manage-lookups-component.css'],
//})
//export class ManageLookupsComponent extends BasePageComponent<Lookup> {

//  visible: boolean = false;
//  lookupOptions: Lookup[] = [];

//  entityName = 'Lookup';

//  columns = [
//    { field: 'categoryName', header: 'Category' }, 
//    { field: 'type', header: 'Type' },
//  ];

//  actionsModel: MenuItem[] = [
//    { label: 'Preview', icon: 'pi pi-eye' },
//    { label: 'Edit', icon: 'pi pi-pencil' },
//    { label: 'Delete', icon: 'pi pi-trash' }
//  ];

//  typeOptions = [
//    { label: 'Parent', value: 'Parent' },
//    { label: 'Child', value: 'Child' }
//  ];

//  dialogMode: 'Add' | 'Edit' | 'Preview' = 'Add';

//  constructor(
//    public override service: ManageLookupsService,
//    private router: Router,
//    public override toastService: ToastMessageService
//  ) {
//    super();
//    this.entity = this.createNewEntity();
//    this.loadParentOptions();
//  }

//  Add() {
//    console.log("Add ");

//    this.entity = this.createNewEntity();
//    this.dialogMode = 'Add';
//    this.visible = true;
//  }

//  edit(id: string) {
//    console.log("edit id: ", id);
//    this.service.getById(id).subscribe({
//      next: (res: any) => {
//        this.entity = res.response;

//        // ✅ Determine if entity is Child or Parent
//        this.entity.type = this.entity.parentId ? 'Child' : 'Parent';

//        this.dialogMode = 'Edit';
//        this.visible = true;
//      },
//      error: (err) => console.error(err)
//    });
//  }

//  preview(id: string) {
//    console.log("preview id: ", id);
//    this.service.getById(id).subscribe({
//      next: (res: any) => {
//        this.entity = res.response;

//        // ✅ Determine if entity is Child or Parent
//        this.entity.type = this.entity.parentId ? 'Child' : 'Parent';

//        this.dialogMode = 'Preview';
//        this.visible = true;
//      },
//      error: (err) => console.error(err)
//    });
//  }

//  override search(event: any = { first: 0, rows: 5 }) {

//    this.isLoading = true;

//    const filter = new SearchFilters();
//    filter.pageNumber = event.first / event.rows + 1;
//    filter.pageSize = event.rows;
//    filter.searchTerm = this.searchValue;

//    this.service.Search(filter).subscribe({
//      next: (res: any) => {

//        this.data = (res.items ?? []).map((item: any) => ({
//          ...item,
//          type: item.parentId ? 'Child' : 'Parent'
//        }));

//        this.totalCount = res.totalCount ?? this.data.length;

//        this.isLoading = false;
//      },
//      error: (err: any) => {
//        this.isLoading = false;
//        console.error(err);
//      }
//    });
//  }

//  createNewEntity(): Lookup {
//    return new Lookup();
//  }

//  override submit() {
//    if (this.entity.id) {
//      this.service.updateLookup(this.entity).subscribe({
//        next: () => {
//          this.toastService.showMessage({
//            messageType: 'success',
//            messageTitle: 'Updated',
//            messageBody: `${this.entityName} updated successfully.`
//          });
//          this.visible = false;
//          this.search();
//        },
//        error: (err) => {
//          this.toastService.showMessage({
//            messageType: 'error',
//            messageTitle: 'Update Failed',
//            messageBody: `Failed to update ${this.entityName}. ${err?.message || ''}`
//          });
//          console.error(err);
//        }
//      });
//    } else {
//      this.service.createLookup(this.entity).subscribe({
//        next: () => {
//          this.toastService.showMessage({
//            messageType: 'success',
//            messageTitle: 'Created',
//            messageBody: `${this.entityName} created successfully.`
//          });
//          this.visible = false;
//          this.search();
//        },
//        error: (err) => {
//          this.toastService.showMessage({
//            messageType: 'error',
//            messageTitle: 'Creation Failed',
//            messageBody: `Failed to create ${this.entityName}. ${err?.message || ''}`
//          });
//          console.error(err);
//        }
//      });
//    }
//  }

//  override openConfirmationDialog(id: any) {
//    console.log("Delete requested for id:", id);
//    this.deleteId = id;
//    this.visibleConfirmation = true;
//  }

//  onCloseDialog() {
//    this.visible = false;
//    this.entity = this.createNewEntity();
//  }

//  loadParentOptions() {
//    this.service.getAllParents().subscribe({
//      next: (res: any) => this.lookupOptions = res.response,
//      error: (err) => console.error(err)
//    });
//  }

//  confirmDelete() {
//    if (!this.deleteId) return;

//    this.service.delete(this.deleteId).subscribe({
//      next: () => {
//        this.toastService.showMessage({
//          messageType: 'success',
//          messageTitle: 'Deleted',
//          messageBody: `${this.entityName} deleted successfully.`
//        });
//        this.visibleConfirmation = false;
//        this.deleteId = null;
//        this.search({ first: 0, rows: 5 }); // إعادة تحميل البيانات بعد الحذف
//      },
//      error: (err) => {
//        this.toastService.showMessage({
//          messageType: 'error',
//          messageTitle: 'Delete Failed',
//          messageBody: `Failed to delete ${this.entityName}.`
//        });
//        console.error('Delete failed', err);
//      }
//    });
//  }
//}


import { Component } from '@angular/core';
import { BasePageComponent } from '../../../shared/BasePageComponent';
import { Router } from '@angular/router';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { MenuItem } from 'primeng/api';
import { ManageLookupsService } from '../../../services/shared/managelookup.service';
import { Lookup } from '../../../models/Shared/Lookup';
import { SearchFilters } from '../../../models/Shared/searchFilters';

@Component({
  selector: 'app-manage-lookups-component',
  standalone: false,
  templateUrl: './manage-lookups-component.html',
  styleUrls: ['./manage-lookups-component.css'],
})
export class ManageLookupsComponent extends BasePageComponent<Lookup> {

  visible: boolean = false;
  lookupOptions: Lookup[] = [];

  entityName = 'Lookup';

  columns = [
    { field: 'categoryName', header: 'Category' },
    { field: 'parentName', header: 'Parent' } // ✅ تحديث ليعرض الاسم مباشرة
  ];

  actionsModel: MenuItem[] = [
    { label: 'Preview', icon: 'pi pi-eye' },
    { label: 'Edit', icon: 'pi pi-pencil' },
    { label: 'Delete', icon: 'pi pi-trash' }
  ];

  typeOptions = [
    { label: 'Parent', value: 'Parent' },
    { label: 'Child', value: 'Child' }
  ];

  dialogMode: 'Add' | 'Edit' | 'Preview' = 'Add';

  constructor(
    public override service: ManageLookupsService,
    private router: Router,
    public override toastService: ToastMessageService
  ) {
    super();
    this.entity = this.createNewEntity();
    this.loadParentOptions();
  }

  Add() {
    this.entity = this.createNewEntity();
    this.dialogMode = 'Add';
    this.visible = true;
  }

  edit(id: string) {
    this.service.getById(id).subscribe({
      next: (res: any) => {
        this.entity = res.response;

        // ✅ تحديد النوع بناءً على وجود parentId
        this.entity.type = this.entity.parentId ? 'Child' : 'Parent';

        this.dialogMode = 'Edit';
        this.visible = true;
      },
      error: (err) => console.error(err)
    });
  }

  preview(id: string) {
    this.service.getById(id).subscribe({
      next: (res: any) => {
        this.entity = res.response;

        // ✅ تحديد النوع بناءً على وجود parentId
        this.entity.type = this.entity.parentId ? 'Child' : 'Parent';

        this.dialogMode = 'Preview';
        this.visible = true;
      },
      error: (err) => console.error(err)
    });
  }

  override search(event: any = { first: 0, rows: 5 }) {
    this.isLoading = true;

    const filter = new SearchFilters();
    filter.pageNumber = event.first / event.rows + 1;
    filter.pageSize = event.rows;
    filter.searchTerm = this.searchValue;

    this.service.Search(filter).subscribe({
      next: (res: any) => {
        this.data = (res.items ?? []).map((item: any) => ({
          ...item,
          type: item.parentId ? 'Child' : 'Parent',
          parentName: item.parentName // ✅ اعتماد الاسم من backend
        }));

        this.totalCount = res.totalCount ?? this.data.length;
        this.isLoading = false;
      },
      error: (err: any) => {
        this.isLoading = false;
        console.error(err);
      }
    });
  }

  createNewEntity(): Lookup {
    return new Lookup();
  }

  override submit() {
    if (this.entity.id) {
      this.service.updateLookup(this.entity).subscribe({
        next: () => {
          this.toastService.showMessage({
            messageType: 'success',
            messageTitle: 'Updated',
            messageBody: `${this.entityName} updated successfully.`
          });
          this.visible = false;
          this.search();
        },
        error: (err) => {
          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Update Failed',
            messageBody: `Failed to update ${this.entityName}. ${err?.message || ''}`
          });
          console.error(err);
        }
      });
    } else {
      this.service.createLookup(this.entity).subscribe({
        next: () => {
          this.toastService.showMessage({
            messageType: 'success',
            messageTitle: 'Created',
            messageBody: `${this.entityName} created successfully.`
          });
          this.visible = false;
          this.search();
        },
        error: (err) => {
          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Creation Failed',
            messageBody: `Failed to create ${this.entityName}. ${err?.message || ''}`
          });
          console.error(err);
        }
      });
    }
  }

  override openConfirmationDialog(id: any) {
    this.deleteId = id;
    this.visibleConfirmation = true;
  }

  onCloseDialog() {
    this.visible = false;
    this.entity = this.createNewEntity();
  }

  loadParentOptions() {
    this.service.getAllParents().subscribe({
      next: (res: any) => {
        // ✅ استخدام parentName من backend
        this.lookupOptions = res.response;
      },
      error: (err) => console.error(err)
    });
  }

  confirmDelete() {
    if (!this.deleteId) return;

    this.service.delete(this.deleteId).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Deleted',
          messageBody: `${this.entityName} deleted successfully.`
        });
        this.visibleConfirmation = false;
        this.deleteId = null;
        this.search({ first: 0, rows: 5 });
      },
      error: (err) => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Delete Failed',
          messageBody: `Failed to delete ${this.entityName}.`
        });
        console.error('Delete failed', err);
      }
    });
  }
}
