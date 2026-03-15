import { Component, OnInit } from '@angular/core';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JobQuestionType } from '../../../enum/JobQuestionType';
import { JobQuestion } from '../../../models/hiremind/JobQuestion';
import { Job } from '../../../models/hiremind/Job';
import { JobApplicationService } from '../../../services/hiremind/jobApplication.service';
import { ManageJobsService } from '../../../services/hiremind/manageJobs.service';
import { LookupItem } from '../../../models/hiremind/LookupItem';
import { ManageLookupsService } from '../../../services/shared/managelookup.service';

interface JobField {
  FieldName: string;
  DisplayName: string;
  IsRequired: boolean;
  FieldType?: string;
  Value?: any;
}

@Component({
  selector: 'app-job-application-assistant-component',
  standalone: false,
  templateUrl: './job-application-assistant-component.html',
  styleUrl: './job-application-assistant-component.css',
})

export class JobApplicationAssistantComponent implements OnInit {

  job: Job = new Job();
  questionsCount: number = 0;
  loading: boolean = false;
  isStartQuestions: boolean = false;
  JobQuestionType = JobQuestionType;
  today: Date = new Date();
  jobFields: any[] = [];
  showFields = false;
  cvUploaded: boolean = false;
  analyzeCvId: any;
  answers: any[] = [];
  progressValue: number = 0;
  isApplicantEmailFiled: boolean = false;
  isSubmited: boolean = false;
  constructor(
    private service: JobApplicationService,
    public manageJobsService: ManageJobsService,
    public serviceLookups: ManageLookupsService,
    private toastService: ToastMessageService,
    private route: ActivatedRoute,
    private router: Router,
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.loadJob(id);
    } else {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Error',
        messageBody: 'Failed to load Job.'
      });
    }

    this.JobQuestionType = JobQuestionType;

    this.serviceLookups.getAllParentsAndChilds().subscribe({
      next: (res: any) => {
        const parents: any[] = res.response || [];

        const mapChildren = (parentName: string): LookupItem[] => {
          const parent = parents.find(p => p.categoryName?.toLowerCase() === parentName.toLowerCase());
          return (parent?.children || []).map((c: any) => ({
            id: c.id,
            name: c.categoryName,
            categoryName: c.categoryName,
            parentId: c.parentId
          }));
        };

        this.countryCodes = mapChildren('CountryCodes');

        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to load lookups.'
        });
        console.error(err);
      }
    });

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

  getDaysSincePosted(startDate: string | Date | null): number {
    if (!startDate) return 0;

    const start = new Date(startDate);
    const today = new Date();

    const diffTime = today.getTime() - start.getTime();
    const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));

    return diffDays;
  }

  applyJob() {
    this.isStartQuestions = true;
  }

  cancel() {
    this.isStartQuestions = false;
    this.isApplicantEmailFiled = false;
  }

  onUpload(event: any) {

    this.loading = true;

    const file = event.files[0];

    const formData = new FormData();
    formData.append('File', file);
    formData.append('JobId', this.job.id.toString());
    formData.append('EmailAddress', this.personalInfo.emailAddress);

    this.service.analyzeCv(formData).subscribe({
      next: (res: any) => {

        console.log("analyzeCv response", res);

        this.analyzeCvId = res?.response?.analyzeCvId;
        const fields = res?.response?.analyzedCvData?.fields || {};

        Object.keys(fields).forEach(key => {

          const index = parseInt(key.replace('Q_', ''), 10);
          const value = fields[key];

          if (isNaN(index)) return;

          const question = this.job.questions[index];

          if (!question) return;

          // TEXT / PARAGRAPH / NUMBER
          if (
            question.questionTypeId === JobQuestionType.Text ||
            question.questionTypeId === JobQuestionType.Paragraph ||
            question.questionTypeId === JobQuestionType.Number
          ) {
            this.answers[index] = value;
            return;
          }

          // RADIO / YESNO / DROPDOWN
          if (
            question.questionTypeId === JobQuestionType.RadioButton ||
            question.questionTypeId === JobQuestionType.YesNo ||
            question.questionTypeId === JobQuestionType.Dropdown
          ) {

            const option = question.availableAnswers?.find(
              o => o.text?.toLowerCase().trim() === String(value).toLowerCase().trim()
            );

            if (option) {
              this.answers[index] = option.id;
            }

            return;
          }

          // MULTIPLE CHOICE
          if (question.questionTypeId === JobQuestionType.MultipleChoice) {

            if (!Array.isArray(this.answers[index])) {
              this.answers[index] = [];
            }

            const values = String(value).split(',').map((v: string) => v.trim().toLowerCase());

            const matchedOptions = question.availableAnswers?.filter(
              o => values.includes(o.text?.toLowerCase())
            );

            if (matchedOptions?.length) {
              this.answers[index] = matchedOptions.map(o => o.id);
            }

            return;
          }

        });

        this.cvUploaded = true;
        this.updateProgress();
        this.loading = false;

        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Success',
          messageBody: 'CV analyzed and answers filled.'
        });

      },

      error: (err) => {

        this.loading = false;

        const message = err?.error?.message || 'Failed to upload the CV.';

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: message
        });
      }
    });
  }

  getMultiSelectOptions(values: string[] | undefined) {
    if (!values) return [];
    return values.map(v => ({ label: v, value: v }));
  }

  getAnswerOptions(question: JobQuestion): { label: string; value: string }[] {
    if (!question.availableAnswers) return [];
    return question.availableAnswers.map(a => ({ label: a.text, value: a.id }));
  }

  updateProgress() {

    const questionsTotal = this.job.questions?.length || 0;

    // +1 step for CV
    const total = questionsTotal + 1;

    let answered = 0;

    this.answers.forEach(a => {
      if (a !== null && a !== undefined && a !== '' && !(Array.isArray(a) && a.length === 0)) {
        answered++;
      }
    });

    // ✅ add CV if uploaded
    if (this.cvUploaded) {
      answered++;
    }

    this.progressValue = Math.round((answered / total) * 100);
  }

  getJobStatus(): string {

    if (!this.job) return '';

    if (!this.job.isActive) {
      return 'closed';
    }

    if (this.job.endDate) {
      const today = new Date();
      const end = new Date(this.job.endDate);

      if (end < today) {
        return 'expired';
      }

      const diffTime = end.getTime() - today.getTime();
      const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

      if (diffDays <= 1) {
        return 'closingSoon';
      }
    }

    return 'open';
  }

  Next(form: any) {
    if (this.personalInfo.emailAddress.trim() &&
      /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(this.personalInfo.emailAddress) &&
      this.personalInfo.countryCodeId && this.personalInfo.mobileNumber.trim() &&
      this.personalInfo.fullName.trim())

      this.isApplicantEmailFiled = true;
  }

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


  countryCodes: LookupItem[] = [];


  submit() {
    if (!this.job || !this.job.id) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Error',
        messageBody: 'Job not loaded.'
      });
      return;
    }

    // Validate answers
    const missingRequired = this.job.questions.some((q, i) => q.isRequired &&
      (this.answers[i] === null || this.answers[i] === undefined || this.answers[i] === '' ||
        (Array.isArray(this.answers[i]) && this.answers[i].length === 0))
    );

    if (missingRequired) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Validation Error',
        messageBody: 'Please answer all required questions.'
      });
      return;
    }

    // Validate personal info
    if (!this.personalInfo.fullName || !this.personalInfo.emailAddress || !this.personalInfo.mobileNumber || !this.personalInfo.countryCodeId) {
      this.toastService.showMessage({
        messageType: 'error',
        messageTitle: 'Validation Error',
        messageBody: 'Please fill all personal information fields.'
      });
      return;
    }

    this.loading = true;

    const payload: any = {
      JobId: this.job.id,
      AnalyzeCvId: this.analyzeCvId || null,
      Answers: {} as any,
      PersonalInfo: this.personalInfo
    };

    this.job.questions.forEach((q, i) => {
      payload.Answers[`Q_${i}`] = this.answers[i];
    });

    this.service.submitJobApplication(payload).subscribe({
      next: (res: any) => {
        this.loading = false;
        this.isStartQuestions = false;
        this.jobFields = [];
        this.personalInfo = { fullName: '', emailAddress: '', mobileNumber: '', countryCodeId: null };
        this.isApplicantEmailFiled = false;
        this.isSubmited = true;
        this.toastService.showMessage({
          messageType: 'success',
          messageTitle: 'Success',
          messageBody: 'Job Application Submitted.'
        });
      },
      error: () => {
        this.loading = false;
        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to Submit Your Job Application.'
        });
      }
    });
  }
}
