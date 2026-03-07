import { Component, OnInit } from '@angular/core';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JobQuestionType } from '../../../enum/JobQuestionType';
import { JobQuestion } from '../../../models/hiremind/JobQuestion';
import { Job } from '../../../models/hiremind/Job';
import { JobApplicationService } from '../../../services/hiremind/jobApplication.service';
import { ManageJobsService } from '../../../services/hiremind/manageJobs.service';

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
  constructor(
    private service: JobApplicationService,
    public manageJobsService: ManageJobsService,
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

  }

  loadJob(id: string) {
    this.manageJobsService.GetById(id).subscribe({
      next: (res: any) => {
        const job = res.response;

        this.job = {
          id: job.id,
          title: job.title,
          description: job.description,
          locationId: job.locationId,
          workPlaceId: job.workPlaceId,
          organizationTypeId: job.organizationTypeId,
          contractTypeId: job.contractTypeId,
          industrySectorId: job.industrySectorId,
          jobTypeId: job.jobTypeId,
          companyId: job.companyId,
          startDate: job.startDate ? new Date(job.startDate) : null,
          endDate: job.endDate ? new Date(job.endDate) : null,
          isActive: job.isActive,
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
  }

  onUpload(event: any) {

    this.loading = true;

    const file = event.files[0];

    const formData = new FormData();
    formData.append('File', file);
    formData.append('JobId', this.job.id);

    this.service.analyzeCv(formData).subscribe({
      next: (res: any) => {

        console.log("analyzeCv response", res);

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
      //next: (res: any) => {

      //  console.log("analyzeCv response", res);

      //  const fields = res?.response?.analyzedCvData?.fields || {};

      //  Object.keys(fields).forEach(key => {

      //    const index = parseInt(key.replace('Q_', ''), 10);
      //    const value = fields[key];

      //    if (!isNaN(index)) {
      //      this.answers[index] = value;
      //    }

      //  });

      //  this.cvUploaded = true;

      //  this.updateProgress();

      //  this.loading = false;

      //  this.toastService.showMessage({
      //    messageType: 'success',
      //    messageTitle: 'Success',
      //    messageBody: 'CV analyzed and answers filled.'
      //  });

      //},

      error: () => {

        this.loading = false;

        this.toastService.showMessage({
          messageType: 'error',
          messageTitle: 'Error',
          messageBody: 'Failed to upload the CV.'
        });

      }

    });

  }

  //onUpload(event: any) {
  //  this.loading = true; // show loader

  //  const file = event.files[0];
  //  const formData = new FormData();
  //  formData.append('File', file);
  //  formData.append('JobId', this.job.id); // ضع الـ JobId المناسب

  //  console.log("Sending formData", formData);

  //  this.service.analyzeCv(formData).subscribe({
  //    next: (res: any) => {
  //      console.log("analyzeCv response", res);

  //      // تحويل البيانات من API إلى jobFields
  //      const fields = res?.analyzedCvData?.fields || {};
  //      this.jobFields = Object.keys(fields).map(key => {
  //        const value = fields[key];
  //        let fieldType = 'text'; // افتراضياً نص

  //        // تحديد نوع الحقل
  //        if (Array.isArray(value)) {
  //          fieldType = 'multiselect';
  //        } else if (typeof value === 'string' && value.length > 100) {
  //          fieldType = 'textarea';
  //        } else {
  //          fieldType = 'text';
  //        }

  //        return {
  //          FieldName: key,
  //          DisplayName: key,
  //          FieldType: fieldType,
  //          Value: value
  //        };
  //      });

  //      //this.showFields = true;

  //      this.cvUploaded = true;
  //      this.updateProgress();

  //      this.loading = false;


  //      this.toastService.showMessage({
  //        messageType: 'success',
  //        messageTitle: 'Success',
  //        messageBody: 'CV Uploaded Successfully.'
  //      });
  //    },
  //    error: (err) => {
  //      this.loading = false;

  //      this.toastService.showMessage({
  //        messageType: 'error',
  //        messageTitle: 'Error',
  //        messageBody: 'Failed to upload the CV.'
  //      });
  //    },
  //  });
  //}

  getMultiSelectOptions(values: string[] | undefined) {
    if (!values) return [];
    return values.map(v => ({ label: v, value: v }));
  }

  answers: any[] = [];

  getAnswerOptions(question: JobQuestion): { label: string; value: string }[] {
    if (!question.availableAnswers) return [];
    return question.availableAnswers.map(a => ({ label: a.text, value: a.id }));
  }

  progressValue: number = 0;

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
}
