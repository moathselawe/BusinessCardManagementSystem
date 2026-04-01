import { Component, Input, ViewChild } from '@angular/core';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JobApplicationService } from '../../../services/hiremind/jobApplication.service';
import { GetJobApplication } from '../../../models/hiremind/GetJobApplication';
import { MenuItem } from 'primeng/api';
import { StageStatus } from '../../../enum/StageStatus';
import { HiringStageService } from '../../../services/hiremind/hiringStage.service';
import { LookupItem } from '../../../models/hiremind/LookupItem';
import { JobQuestionType } from '../../../enum/JobQuestionType';
import { Job } from '../../../models/hiremind/Job';
import { ManageJobsService } from '../../../services/hiremind/manageJobs.service';
import { JobQuestion } from '../../../models/hiremind/JobQuestion';
import { ManageLookupsService } from '../../../services/shared/managelookup.service';
import { ApplicationStageService } from '../../../services/hiremind/ApplicationStages.service';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-manage-applications.component',
  standalone: false,
  templateUrl: './manage-applications.component.html',
  styleUrl: './manage-applications.component.css',
})
export class ManageApplicationsComponent {
  @ViewChild("menu") menu: any;
  @Input() actionsModel: MenuItem[] = [];
  applications: GetJobApplication[] = [];
  hiringStages: any[] = [];
  loading: boolean = false;
  currentRowData: any;
  selectedRows: any[] = [];
  isDialogVisiable: boolean = false;
  dialogHeader!: string;
  isPreviewApplicationVisiable: boolean = false;
  activeIndex: number = 0;
  steps: any[] = [];
  activeStep: number = 1;
  JobQuestionType = JobQuestionType;
  answers: any[] = [];
  bulkAction!: StageStatus;
  bulkErrorMessage: string = '';
  countryCodes: LookupItem[] = [];
  job: Job = new Job();
  jobId!: number;
  questionsCount: number = 0;
  isFinalStageDialogVisible: boolean = false;
  pendingApplicationId!: number;
  searchInput: string = '';
  selectedStageId: number | null = null;
  isActionsVisibale: boolean = false;
  isFiltersVisibale: boolean = false;
  selectedStageStatusId: StageStatus | null = null;
  shortListCount: number | null = null;
  StageStatus = StageStatus;
  personalInfo: {
    fullName: string;
    emailAddress: string;
    mobileNumber: string;
    countryCodeId: number | null;
  } = {
      fullName: '',
      emailAddress: '',
      mobileNumber: '',
      countryCodeId: null
    };
  constructor(
    public service: JobApplicationService,
    public serviceLookups: ManageLookupsService,
    public hiringStageService: HiringStageService,
    public applicationStageService: ApplicationStageService,
    public manageJobsService: ManageJobsService,
    private route: ActivatedRoute,
    private router: Router,
    public toastService: ToastMessageService
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.jobId = id;
    this.loading = true;

    if (id) {
      this.searchApplications();          // load applications
      this.getAllHiringStagesByJobId(id); // load stages
      this.loadJob(id);                   // ⭐ load job (missing)
    }

    this.serviceLookups.getAllParentsAndChilds().subscribe({
      next: (res: any) => {
        const parents: any[] = res.response || [];
        const parent = parents.find(p => p.categoryName?.toLowerCase() === 'countrycodes');

        this.countryCodes = (parent?.children || []).map((c: any) => ({
          id: c.id,
          name: c.categoryName,
          categoryName: c.categoryName,
          parentId: c.parentId
        }));
      }
    });
  }

  onStageTagClick(status: StageStatus) {
    if (this.selectedStageStatusId === status) {
      // Deselect if clicked again
      this.selectedStageStatusId = null;
    } else {
      this.selectedStageStatusId = status;
    }
    this.searchApplications();
  }

  onStageClick(stage: any) {
    if (this.selectedStageId === stage.id) {
      // User clicked the same step again → deselect
      this.activeStep = 0; // or null if you prefer
      this.selectedStageId = null;
      this.searchApplications();
    } else {
      // Select the clicked step
      this.activeStep = stage.stageOrder;
      this.selectedStageId = stage.id;
      this.searchApplications();
    }
  }

  searchApplications() {

    const request: any = {
      jobId: this.jobId
    };

    if (this.selectedStageStatusId !== null) {
      request.stageStatusId = this.selectedStageStatusId;
    }

    if (this.selectedStageId) {
      request.stageId = this.selectedStageId;
    }

    if (this.shortListCount) {
      request.limit = this.shortListCount; // send to backend as limit
    }

    if (this.searchInput && this.searchInput.trim() !== '') {
      request.searchInput = this.searchInput.trim();
    }

    this.loading = true;

    this.applicationStageService.searchApplications(request).subscribe({
      next: (res: any) => {

        this.applications = res;
        this.loading = false;

      },
      error: () => {

        this.loading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to search applications.'
        });

      }
    });

  }

  onShortListChange(value: string) {
    const parsed = parseInt(value, 10);

    if (!isNaN(parsed) && parsed > 0) {
      this.shortListCount = parsed;
    } else {
      this.shortListCount = null; // clear if invalid
    }

    this.searchApplications();
  }

  clearSearch() {

    this.searchInput = '';
    this.selectedStageId = null;
    this.selectedStageStatusId = null;
    this.shortListCount = null;
    this.activeStep = 0;

    this.searchApplications();
  }

  loadApplicationsByJobId(id: any) {
      this.loading = true;

    this.service.getAllJobApplicationsByJobId(id).subscribe({
      next: (res: any) => {

        this.applications = res.response;
        this.loading = false;

      },
      error: () => {

        this.loading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to load Job Applications.'
        });
      }
    });

    if (id) {
      this.loadJob(id);
    } else {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Error',
        messageBody: 'Failed to load Job.'
      });
    }
  }

  getAllHiringStagesByJobId(id: any) {
    this.hiringStageService.GetAllHiringStagesByJobId(id).subscribe({
      next: (res: any) => {
        this.hiringStages = res.response;

        // ensure boolean for final stage
        this.hiringStages.forEach((s: any) => s.isFinalStage = !!s.isFinalStage);

        // sort stages
        this.hiringStages.sort((a, b) => a.stageOrder - b.stageOrder);

        // set activeStep to the last active stage
        const activeStages = this.hiringStages.filter(s => s.isActive);
        this.activeStep = activeStages.length
          ? Math.max(...activeStages.map(s => s.stageOrder))
          : 0;
      },
      error: () => {
        this.loading = false;
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to load Hiring Stages.'
        });
      }
    });
  }

  getLastHiringStageId(): number | null {
    if (!this.hiringStages || this.hiringStages.length === 0) return null;

    const last = [...this.hiringStages].sort((a, b) => b.stageOrder - a.stageOrder)[0];
    return last?.id || null;
  }

  confirmApproveLastStage() {
    this.ChangeSelectedBulkStatus([this.pendingApplicationId], StageStatus.Approved);
    this.isFinalStageDialogVisible = false;
  }

  toggleMenu(event: any, rowData: any) {
    this.currentRowData = rowData;

    const items: MenuItem[] = [];
    const currentStage = this.hiringStages.find(s => s.id === rowData.currentStageId);
    const isFinalStage = currentStage?.isFinalStage ?? false;
    const isLastStage = rowData.currentStageId === this.getLastHiringStageId();

    if (rowData.status === 'New' || rowData.status === 'NotSelected') {
      items.push({
        label: 'Mark as Selected',
        icon: 'pi pi-user-check',
        command: () => this.ChangeSelectedBulkStatus([rowData.id], StageStatus.Selected)
      });
    }

    if (rowData.status === 'New' || rowData.status === 'Selected') {
      items.push({
        label: 'Mark as Not Selected',
        icon: 'pi pi-user-minus',
        command: () => this.ChangeSelectedBulkStatus([rowData.id], StageStatus.NotSelected)
      });
    }

    if ((isFinalStage || isLastStage) && rowData.status !== 'Approved') {
      items.push({
        label: 'Mark as Approved',
        icon: 'pi pi-verified',
        command: () => {
          if (isLastStage && !isFinalStage) {
            this.pendingApplicationId = rowData.id;
            this.isFinalStageDialogVisible = true; // show confirmation dialog
          } else {
            this.ChangeSelectedBulkStatus([rowData.id], StageStatus.Approved); // approve directly
          }
        }
      });
    }

    if (!isFinalStage && rowData.status === 'Selected') {
      items.push({
        label: 'Move To Next Stage',
        icon: 'pi pi-arrow-right',
        command: () => this.ChangeSelectedBulkStatus([rowData.id], StageStatus.NextStage)
      });
    }

    items.push({
      label: 'Preview',
      icon: 'pi pi-search',
      command: () => this.previewJobApplication(rowData.id)
    });

    items.push({
      label: 'Download CV',
      icon: 'pi pi-file-arrow-down',
      command: () => this.downloadCV(rowData.id)
    });

    this.menu.model = items;
    this.menu.toggle(event);
  }

  ChangeSelectedBulkStatus(ids: number[], newStatus: StageStatus) {
    console.log("this.selectedRows", this.selectedRows);
    const request = {
      ids: ids,
      jobId: this.jobId,
      newStatus: newStatus
    };

    this.applicationStageService.updateBulkApplicationsStageStatus(request).subscribe({
      next: () => {
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Updated',
          messageBody: 'Statuses updated successfully.'
        });

        this.searchApplications();
        this.getAllHiringStagesByJobId(this.jobId);

      },
      error: () => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to update Statuses.'
        });
      }
    });

  }

  previewJobApplication(applicationId: number) {

    this.service.getJobApplicationById(applicationId).subscribe({
      next: (res: any) => {

        const response = res.response;

        // personal info
        this.personalInfo = response.personalInfo;

        // convert answers object -> array
        const userAnswers = response.userAnswers || JSON.parse(response.userAnswersJson || '{}');

        this.answers = [];

        Object.keys(userAnswers).forEach(key => {
          const index = Number(key.replace('Q_', ''));
          this.answers[index] = userAnswers[key];
        });

        this.isPreviewApplicationVisiable = true;
      },

      error: () => {
        this.loading = false;
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to load JobApplication'
        });
      }
    });
  }

  MoveBulkToNextStage() {
    console.log("this.selectedRows", this.selectedRows);

    //this.selectedRows
  }

  openBulkDialog(action: string) {

    if (!this.selectedRows || this.selectedRows.length === 0) {
      this.toastService.showMessage({
        messageType: 'warn',
        messageTitle: 'Warning',
        messageBody: 'Please select at least one application.'
      });
      return;
    }

    this.dialogHeader = action;
    this.bulkErrorMessage = '';

    if (action === 'Mark Bulk Selected') {
      this.bulkAction = StageStatus.Selected;

      const invalid = this.selectedRows.some(x =>
        x.status !== 'New' &&
        x.status !== 'Selected' &&
        x.status !== 'NotSelected'
      );

      if (invalid)
        this.bulkErrorMessage = "All selected items must have status 'New' or 'Not Selected'.";

    }

    if (action === 'Mark Bulk Not Selected') {
      this.bulkAction = StageStatus.NotSelected;

      const invalid = this.selectedRows.some(x =>
        x.status !== 'New' &&
        x.status !== 'Selected' &&
        x.status !== 'NotSelected'
      );

      if (invalid)
        this.bulkErrorMessage = "All selected items must have status 'New' or 'Selected'.";
    }

    if (action === 'Move Bulk to Next Stage') {
      this.bulkAction = StageStatus.NextStage;

      const invalid = this.selectedRows.some(x =>
        x.status !== 'Selected' &&
        x.status !== 'New'
      );

      if (invalid)
        this.bulkErrorMessage = "All selected items must have status 'New' or 'Selected'.";
    }

    this.isDialogVisiable = true;
  }

  submitBulkAction() {

    if (this.bulkErrorMessage) return;

    const ids = this.selectedRows.map(x => x.id);

    this.ChangeSelectedBulkStatus(ids, this.bulkAction);

    this.isDialogVisiable = false;
  }

  closeBulkDialog() {
    this.isDialogVisiable = false;
    this.dialogHeader = '';
    //this.bulkAction = null;
    this.bulkErrorMessage = '';
  }

  loadJob(id: any) {
    this.manageJobsService.GetById(id).subscribe({
      next: (res: any) => {
        const job = res.response;

        this.job = {
          id: job.id,
          title: job.title,
          description: job.description,
          locationId: job.locationId,
          locationName: job.locationName,
          workPlaceId: job.workPlaceId,
          workPlaceName: job.workPlaceName,               // correct
          organizationTypeId: job.organizationTypeId,
          organizationTypeName: job.organizationTypenName, // note the "n" here
          contractTypeId: job.contractTypeId,
          contractTypeName: job.contractTypeName,
          industrySectorId: job.industrySectorId,
          industrySectorName: job.industrySectorName,
          jobTypeId: job.jobTypeId,
          jobTypeName: job.jobTypeName,
          companyId: job.companyId,
          startDate: job.startDate ? new Date(job.startDate) : null,
          endDate: job.endDate ? new Date(job.endDate) : null,
          isActive: job.isActive,
          hiringStages: (job.hiringStages || [])
            .sort((a: any, b: any) => a.stageOrder - b.stageOrder),

          questions: (job.questions || []).map((q: any) => ({
            ...q,
            availableAnswers: q.availableAnswers || [],
            preferredAnswers: q.preferredAnswers || []
          }))
        };
        this.questionsCount = this.job.questions.length;

        this.loading = false;

      },

      error: () => {
        this.loading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to load Job.'
        });
      }
    });
  }

  getAnswerOptions(question: JobQuestion): { label: string; value: string }[] {
    if (!question.availableAnswers) return [];
    return question.availableAnswers.map(a => ({ label: a.text, value: a.id }));
  }

  downloadCV(applicationId: number) {
    if (!applicationId) return;

    this.service.downloadCV(applicationId).subscribe({
      next: (res: Blob) => {
        if (!res || res.size === 0) {
          this.toastService.showMessage({
            messageType: 'warn',
            messageTitle: 'No File',
            messageBody: 'No CV found for this application.'
          });
          return;
        }

        // Create blob URL and trigger download
        const url = window.URL.createObjectURL(res);
        const link = document.createElement('a');
        link.href = url;

        // Extract filename from blob if possible, otherwise fallback
        link.download = 'CV';
        link.click();

        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error(err);
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to download CV.'
        });
      }
    });
  }

  reviewCV(applicationId: number) {
    if (!applicationId) return;

    this.service.previewCV(applicationId).subscribe({
      next: (res: Blob) => {
        if (!res || res.size === 0) {
          this.toastService.showMessage({
            messageType: 'warn',
            messageTitle: 'No File',
            messageBody: 'No CV found for this application.'
          });
          return;
        }

        const url = window.URL.createObjectURL(res);
        window.open(url, '_blank');
      },
      error: () => {
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to preview CV.'
        });
      }
    });
  }
  // Export selected rows to Excel
  exportSelectedRows() {
    if (!this.selectedRows || this.selectedRows.length === 0) {
      alert('Please select at least one row to export.');
      return;
    }

    // Map selected rows to Excel-friendly format
    const exportData = this.selectedRows.map(app => ({
      Name: app.fullName,
      Email: app.email,
      Mobile: `${app.countryCode} ${app.mobileNumber}`,
      'System Score': app.systemScore,
      'Total Score': app.totalScore,
      'Stage Name': app.currentStageName,
      'Status': app.status
    }));

    // Create worksheet and workbook
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exportData);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Applications');

    // Export file
    XLSX.writeFile(wb, 'SelectedApplications.xlsx');
  }
}
