import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobQuestionType } from '../../../enum/JobQuestionType';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { Job } from '../../../models/hiremind/Job';
import { JobQuestion } from '../../../models/hiremind/JobQuestion';
import { ManageJobsService } from '../../../services/hiremind/manageJobs.service';
import { ManageLookupsService } from '../../../services/shared/managelookup.service';
import { ViewChild, ElementRef } from '@angular/core';
import { LookupItem } from '../../../models/hiremind/LookupItem';

@Component({
  selector: 'app-job-form-component',
  standalone: false,
  templateUrl: './job-form-component.html',
  styleUrl: './job-form-component.css',
})

export class JobFormComponent implements OnInit {
  @ViewChild('stagesSection') stagesSection!: ElementRef;
  @ViewChild('questionsSection') questionsSection!: ElementRef;

  job: Job = new Job();
  questionTypes: { id: number, label: string }[] = [];
  examQuestionTypes: { id: number, label: string }[] = [];
  isEditMode: boolean = false;
  questionsCount: number = 1;
  today: Date = new Date();
  tomorrow: Date = new Date();
  readonly: boolean = false;
  loading: boolean = false;
  countPersonalQuestions: number = 0;
  isShowQuestions: boolean = false;
  isShowStages: boolean = false;
  stagesCount: number = 1;
  isEmailDialogVisiable: boolean = false;
  stageIndex: any;
  isExtendStages: boolean = true
  isExtendQuestions: boolean = true
  emailTemplate: any;
  isInterviewDialogVisible: boolean = false;
  interviewQuestions: JobQuestion[] = [];
  StageTitle: string = '';
  ratingValue: number = 5;
  isExamDialogVisible: boolean = false;
  examQuestions: JobQuestion[] = [];
  jobTypes: LookupItem[] = [];
  locations: LookupItem[] = [];
  workPlaces: LookupItem[] = [];
  contractTypes: LookupItem[] = [];
  organizationTypes: LookupItem[] = [];
  industrySectors: LookupItem[] = [];

  companies = [
    { name: 'Company A', id: 1 },
    { name: 'Company B', id: 2 },
  ];

  via = [
    { name: 'Initiate Application', id: 1 },
    { name: 'Teams', id: 2 },
    { name: 'Face to Face', id: 3 },
    { name: 'Hire mind exam', id: 4 },
    { name: 'External exam', id: 5 },
    { name: 'Assignment', id: 6 },
  ];
  constructor(
    public service: ManageJobsService,
    private serviceLookups: ManageLookupsService,
    private route: ActivatedRoute,
    private router: Router,
    public toastService: ToastMessageService
  ) { }

  ngOnInit(): void {
    this.questionTypes = Object.keys(JobQuestionType)
      .filter(k => isNaN(Number(k)))
      .map(k => ({
        id: JobQuestionType[k as keyof typeof JobQuestionType],
        label: k
      }))
      .filter(q => q.id !== 7)
      .filter(q => q.id !== 8)
      .filter(q => q.id !== 9);

    this.examQuestionTypes = this.questionTypes
      .filter(q => q.id !== 1)
      .filter(q => q.id !== 2);

    this.loadLookups();

    const id = this.route.snapshot.paramMap.get('id');
    const url = this.route.snapshot.url.map(u => u.path).join('/');

    if (url.includes('PreviewJob')) {
      this.readonly = true;
      this.isEditMode = true;
    }
    else if (url.includes('ModifyJob')) {
      this.isEditMode = true;
    }

    this.today.setHours(0, 0, 0, 0);

    this.tomorrow = new Date(this.today);
    this.tomorrow.setDate(this.today.getDate() + 1);

    if (id) {
      this.loading = true;
      this.loadJob(id);
    }

    this.createDefaultStage();
  }

  getViaOptions(stageIndex: number) {
    if (stageIndex === 0) {
      // المرحلة الأولى فقط
      return this.via.filter(v => v.id === 1); // First Stage
    } else {
      // بقية المراحل
      return this.via.filter(v => v.id !== 1);
    }
  }


  createDefaultStage() {
    if (!this.isEditMode && !this.readonly) {
      this.job.hiringStages = [];

      // ➤ المرحلة الأولى Initiate Application
      this.job.hiringStages.push({
        id: 0,
        name: 'Initiate Application',
        stageOrder: 1,
        viaId:1,
        emailTemplate: this.defaultEmailTemplate,
        isDisabled: true,
        isFinalStage: false
      });
    }
  }

  defaultEmailTemplate: string = `
<p>Dear Candidate,</p>
<p>Your application has been received.</p>
<p>Will come back to you soon.</p>
<p>Regards,<br/><strong>HireMind</strong></p>
`;


  isDefaultStagesAdded: boolean = false;
  createDefaultStages() {
    if (!this.isEditMode) {
      // ➤ بقية المراحل الافتراضية
      const defaultStages = [
        { name: 'CV Screening', stageOrder: 2 },
        { name: 'HR Interview', stageOrder: 3 },
        { name: 'Technical Interview', stageOrder: 4 },
        { name: 'Final Interview', stageOrder: 5 }
      ];

      defaultStages.forEach(ds => {
        this.job.hiringStages.push({
          id: 0,
          name: ds.name,
          stageOrder: ds.stageOrder,
          emailTemplate: '',
          isDisabled: false,
          isFinalStage: false
        });
      });

      // تحديث عدد المراحل
      this.stagesCount = this.job.hiringStages.length;

      this.isDefaultStagesAdded = true;
    }
  }

  addStage() {
    if (!this.job.hiringStages)
      this.job.hiringStages = [];

    this.job.hiringStages.push({
      id: 0,
      name: '',
      stageOrder: this.job.hiringStages.length + 1,
      emailTemplate: "",
      isDisabled: false,
      isFinalStage: false
    });

    this.stagesCount = this.job.hiringStages.length;
  }

  loadLookups() {
    this.loading = true;

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

        this.jobTypes = mapChildren('JobTypes');
        this.locations = mapChildren('Locations');
        this.workPlaces = mapChildren('WorkPlaces');
        this.contractTypes = mapChildren('ContractTypes');
        this.organizationTypes = mapChildren('OrganizationTypes');
        this.industrySectors = mapChildren('IndustrySectors');

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
    this.service.GetById(id).subscribe({
      next: (res: any) => {
        const job = res.response;

        this.job = {
          id: job.id,
          title: job.title,
          description: job.description,
          locationId: job.locationId,
          locationName: job.locationName,
          jobTypeId: job.jobTypeId,
          jobTypeName: job.jobTypeName,
          workPlaceId: job.workPlaceId,
          workPlaceName: job.workPlaceName,
          contractTypeId: job.contractTypeId,
          contractTypeName: job.contractTypeName,
          organizationTypeId: job.organizationTypeId,
          organizationTypeName: job.organizationTypeName,
          industrySectorId: job.industrySectorId,
          industrySectorName: job.industrySectorName,
          companyId: job.companyId,
          startDate: job.startDate ? new Date(job.startDate) : null,
          endDate: job.endDate ? new Date(job.endDate) : null,
          isActive: job.isActive,
          hiringStages: (job.hiringStages || [])
            .sort((a: any, b: any) => a.stageOrder - b.stageOrder),

          questions: (job.questions || []).map((q: any) => ({
            questionText: q.questionText,
            questionTypeId: q.questionTypeId,
            isRequired: q.isRequired,
            score: q.score,

            availableAnswers: (q.availableAnswers || []).map((a: any) => ({
              id: a.id,
              text: a.text,
              isPreferredAnswer: a.isPreferredAnswer ?? false
            }))
          }))

        };

        this.questionsCount = this.job.questions.length;

        this.stagesCount = this.job.hiringStages.length;

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

  addQuestion() {
    const newQuestion: JobQuestion = {
      questionText: '',
      questionTypeId: JobQuestionType.Text,
      isRequired: true,
      availableAnswers: [],
      score: 0
    };

    const defaultAnswer = {
      id: crypto.randomUUID(),
      text: '',
      isPreferredAnswer: true
    };

    newQuestion.availableAnswers.push(defaultAnswer);

    this.job.questions.push(newQuestion);

    this.questionsCount = this.job.questions.length;
  }

  removeQuestion(index: number) {
    this.job.questions.splice(index, 1);
    this.questionsCount = this.job.questions.length;
  }

  addAnswer(questionIndex: number) {
    const question = this.job.questions[questionIndex];

    question.availableAnswers.push({
      id: crypto.randomUUID(),
      text: '',
      isPreferredAnswer: false
    });
  }

  removeAnswer(questionIndex: number, answerIndex: number) {
    this.job.questions[questionIndex].availableAnswers.splice(answerIndex, 1);
  }

  validateForm(form: any): any {
    debugger;
    form.control.markAllAsTouched();

    if (form.invalid) {
      alert("Please fix validation errors.");
      return true;
    }

    if (this.job.startDate && this.job.endDate) {

      const start = new Date(this.job.startDate);
      const end = new Date(this.job.endDate);

      if (end <= start) {
        alert("End date must be greater than start date.");
        return true;
      }
    }

    if (!this.job.hiringStages || this.job.hiringStages.length === 0) {
      alert("At least one hiring stage is required.");
      return true;
    }

    for (let i = 0; i < this.job.hiringStages.length; i++) {

      const stage = this.job.hiringStages[i];

      if (!stage.name || stage.name.trim().length === 0) {
        alert(`Stage #${i + 1}: name is required.`);
        return true;
      }

    }

    for (let i = 0; i < this.job.hiringStages.length; i++) {
      const stage = this.job.hiringStages[i];
      if (!stage.viaId) {
        alert(`Stage #${i + 1}: Via is required.`);
        return true;
      }
    }

    for (let i = 0; i < this.job.questions.length; i++) {

      const q = this.job.questions[i];

      if (!q.questionText || q.questionText.trim().length === 0) {
        alert(`Question #${i + 1}: Question text is required.`);
        return true;
      }

      else if (q.questionText.trim().length < 5) {
        alert(`Question #${i + 1}: Minimum 5 characters required.`);
        return true;
      }

      if (q.questionText.trim().length > 500) {
        alert(`Question #${i + 1}: Maximum 500 characters allowed.`);
        return true;
      }

      // Question Type
      if (!q.questionTypeId) {
        alert(`Question #${i + 1}: Question type is required.`);
        return true;
      }

      // Score
      if (q.score === null || q.score === undefined) {
        alert(`Question #${i + 1}: Score is required.`);
        return true;
      }

      // Answers Validation
      for (let j = 0; j < q.availableAnswers.length; j++) {

        const ans = q.availableAnswers[j];

        if (!ans.text || ans.text.trim().length === 0) {
          alert(`Question #${i + 1} - Answer #${j + 1}: Answer is required.`);
          return true;
        }

        if (ans.text.trim().length < 1) {
          alert(`Question #${i + 1} - Answer #${j + 1}: Minimum 2 characters required.`);
          return true;
        }

        if (ans.text.trim().length > 100) {
          alert(`Question #${i + 1} - Answer #${j + 1}: Maximum 50 characters allowed.`);
          return true;
        }
      }

      const preferred = q.availableAnswers.filter(a => a.isPreferredAnswer);

      if (!this.isTextType(q.questionTypeId) && preferred.length === 0) {
        alert(`Question #${i + 1}: At least one preferred answer is required.`);
        return true;
      }
    }
  }

  submit(form: any) {
    form.control.markAllAsTouched();
    var notValid = this.validateForm(form);

    if (notValid)
      return;

    if (this.isEditMode) {
      this.service.updateJob(this.job).subscribe({
        next: () => {
          this.toastService.showMessage({
            messageType: 'success',
            messageTitle: 'Updated',
            messageBody: 'Job updated successfully.'
          });
          this.router.navigateByUrl('/HireMind/ManageJobs');
        },
        error: () => {
          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: 'Failed to update Job.'
          });
        }
      });
      console.log("call update")
    } else {
      this.service.createJob(this.job).subscribe({
        next: () => {
          this.toastService.showMessage({
            messageType: 'success',
            messageTitle: 'Created',
            messageBody: 'Job created successfully.'
          });
          this.router.navigateByUrl('/HireMind/ManageJobs');
        },
        error: () => {
          this.toastService.showMessage({
            messageType: 'error',
            messageTitle: 'Error',
            messageBody: 'Failed to create Job.'
          });
        }
      });
    }
  }

  reset() {
    this.job = new Job();
  }

  setPreferredAnswer(questionIndex: number, answerIndex: number) {
    const question = this.job.questions[questionIndex];
    question.availableAnswers[answerIndex].isPreferredAnswer = true;
  }

  setUnPreferredAnswer(questionIndex: number, answerIndex: number) {
    const question = this.job.questions[questionIndex];
    if (question.questionTypeId === JobQuestionType.Text || question.questionTypeId === JobQuestionType.Paragraph)
      return;

    question.availableAnswers[answerIndex].isPreferredAnswer = false;
  }

  getMaxAnswers(typeId: number | null): number {
    if (!typeId) return 0;
    switch (typeId) {
      case JobQuestionType.Text:
      case JobQuestionType.Paragraph:
        return 1;
      case JobQuestionType.YesNo:
        return 2;
      case JobQuestionType.MultipleChoice:
      case JobQuestionType.RadioButton:
      case JobQuestionType.Dropdown:
      case JobQuestionType.Rating:
      case JobQuestionType.Number:
      case JobQuestionType.Date:
        return 4;
      default:
        return 0;
    }
  }

  isDeleteDisabled(question: JobQuestion): boolean {
    if (question.questionTypeId === JobQuestionType.YesNo)
      return true;

    return question.availableAnswers.length === 1;
  }

  isAnswerInputDisabled(question: JobQuestion): boolean {
    return question.questionTypeId === JobQuestionType.YesNo;
  }

  onQuestionTypeChange(questionIndex: number) {
    const question = this.job.questions[questionIndex];
    const max = this.getMaxAnswers(question.questionTypeId);

    question.availableAnswers = [];

    for (let k = 0; k < max; k++) {
      const newAnswer = {
        id: crypto.randomUUID(),
        text: question.questionTypeId === JobQuestionType.YesNo
          ? (k === 0 ? 'Yes' : 'No')
          : '',
        isPreferredAnswer:
          question.questionTypeId === JobQuestionType.Text ||
          question.questionTypeId === JobQuestionType.Paragraph
      };

      question.availableAnswers.push(newAnswer);
    }
  }

  onQuestionsCountChange(newCount: number) {
    if (newCount > 15)
      newCount = 15;
    else if (newCount < 0)
      newCount = 0;

    if (!newCount || newCount < 1) {
      this.questionsCount = 1;
      newCount = 1;
    }

    const currentCount = this.job.questions.length;

    // 🔹 Add questions
    if (newCount > currentCount) {
      const diff = newCount - currentCount;

      for (let i = 0; i < diff; i++) {
        this.addQuestion();
      }
    }

    if (newCount < currentCount) {
      this.job.questions.splice(newCount);
    }
  }

  isTextType(typeId: number): boolean {
    return typeId === JobQuestionType.Text ||
      typeId === JobQuestionType.Paragraph;
  }

  goBack() {
    this.router.navigateByUrl('/HireMind/ManageJobs');
  }

  onViaChange(stageIndex: number) {
    const stage = this.job.hiringStages[stageIndex];
    if (!stage) return;
    stage.interviewQuestions = [];
    stage.examQuestions = [];
  }

  onStagesCountChange(newCount: number) {
    if (newCount > 5)
      newCount = 5;
    else if (newCount < 0)
      newCount = 0;

    if (!newCount || newCount < 1) {
      this.stagesCount = 1;
      newCount = 1;
    }

    const currentCount = this.job.hiringStages.length;

    // ➕ Add stages
    if (newCount > currentCount) {

      const diff = newCount - currentCount;

      for (let i = 0; i < diff; i++) {
        this.addStage();
      }
    }

    // ➖ Remove stages
    if (newCount < currentCount) {
      this.job.hiringStages.splice(newCount);
    }

    // reorder
    this.job.hiringStages.forEach((s, i) => {
      s.stageOrder = i + 1;
    });
  }

  removeStage(index: number) {
    this.job.hiringStages.splice(index, 1);

    this.job.hiringStages.forEach((s, i) => {
      s.stageOrder = i + 1;
    });

    this.stagesCount = this.job.hiringStages.length;
  }

  openEmailDialog(stageIndex: number) {
    if (this.job.hiringStages[stageIndex]) {
      this.stageIndex = stageIndex;
      this.emailTemplate = this.job.hiringStages[stageIndex].emailTemplate || '';
      this.isEmailDialogVisiable = true;
    }
  }

  saveEmailDialog() {
    if (this.stageIndex !== null && this.job.hiringStages[this.stageIndex]) {
      this.job.hiringStages[this.stageIndex].emailTemplate = this.emailTemplate;
    }
    this.isEmailDialogVisiable = false;
  }

  clodeEmailDialog() {
    this.isEmailDialogVisiable = false;
    this.stageIndex = null;
  }

  openInterviewDialog(stageIndex: number) {
    const stage = this.job.hiringStages[stageIndex];
    if (!stage) return;

    this.stageIndex = stageIndex;
    this.StageTitle = stage.name || 'Interview';

    if (!stage.interviewQuestions) {
      stage.interviewQuestions = [];
    }

    this.interviewQuestions = [...stage.interviewQuestions];
    this.isInterviewDialogVisible = true;
  }

  saveInterviewDialog() {
    if (this.stageIndex !== null) {
      const stage = this.job.hiringStages[this.stageIndex];
      stage.interviewQuestions = [...this.interviewQuestions];
    }

    this.isInterviewDialogVisible = false;
  }

  clodeInterviewDialog() {
    this.isInterviewDialogVisible = false;
    this.stageIndex = null;
  }

  addInterviewQuestion() {
    const question: JobQuestion = {
      questionText: '',
      questionTypeId: JobQuestionType.Text,
      isRequired: true,
      availableAnswers: [],
      score: 0
    };
    this.interviewQuestions.push(question);
  }

  removeInterviewQuestion(index: number) {
    this.interviewQuestions.splice(index, 1);
  }

  openExamDialog(stageIndex: number) {
    const stage = this.job.hiringStages[stageIndex];
    if (!stage) return;

    this.stageIndex = stageIndex;
    this.StageTitle = stage.name || 'Exam';

    if (!stage.examQuestions)
      stage.examQuestions = [];

    this.examQuestions = [...stage.examQuestions];
    this.isExamDialogVisible = true;
  }

  saveExamDialog() {
    if (this.stageIndex !== null) {

      const stage = this.job.hiringStages[this.stageIndex];

      stage.examQuestions = [...this.examQuestions];
    }
    this.isExamDialogVisible = false;
  }

  closeExamDialog() {
    this.isExamDialogVisible = false;
    this.stageIndex = null;
  }

  addExamQuestion() {
    const question: JobQuestion = {
      questionText: '',
      questionTypeId: JobQuestionType.Dropdown,
      isRequired: true,
      score: 0,
      availableAnswers: [
        {
          id: crypto.randomUUID(),
          text: '',
          isPreferredAnswer: true
        }
      ]
    };

    this.examQuestions.push(question);
  }

  removeExamQuestion(index: number) {
    this.examQuestions.splice(index, 1);
  }

  addExamAnswer(questionIndex: number) {
    const question = this.examQuestions[questionIndex];
    question.availableAnswers.push({
      id: crypto.randomUUID(),
      text: '',
      isPreferredAnswer: false
    });
  }

  removeExamAnswer(questionIndex: number, answerIndex: number) {
    this.examQuestions[questionIndex].availableAnswers.splice(answerIndex, 1);
  }

  setExamPreferred(questionIndex: number, answerIndex: number) {
    const q = this.examQuestions[questionIndex];
    if (q.questionTypeId !== JobQuestionType.MultipleChoice) {
      q.availableAnswers.forEach(a => a.isPreferredAnswer = false);
    }
    q.availableAnswers[answerIndex].isPreferredAnswer = true;
  }

  setExamUnPreferred(questionIndex: number, answerIndex: number) {
    const q = this.examQuestions[questionIndex];
    q.availableAnswers[answerIndex].isPreferredAnswer = false;
  }

  onExamTypeChange(questionIndex: number) {
    const question = this.examQuestions[questionIndex];

    if (!question) return;

    question.availableAnswers = [];

    if (question.questionTypeId === JobQuestionType.YesNo) {
      question.availableAnswers = [
        {
          id: crypto.randomUUID(),
          text: 'Yes',
          isPreferredAnswer: true
        },
        {
          id: crypto.randomUUID(),
          text: 'No',
          isPreferredAnswer: false
        }
      ];
    } else {
      question.availableAnswers.push({
        id: crypto.randomUUID(),
        text: '',
        isPreferredAnswer: false
      });

    }
  }
}
