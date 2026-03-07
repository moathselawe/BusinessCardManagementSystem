import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { JobQuestionType } from '../../../enum/JobQuestionType';
import { ToastMessageService } from '../../../services/shared/toast-message.service';
import { Job } from '../../../models/hiremind/Job';
import { JobQuestion } from '../../../models/hiremind/JobQuestion';
import { ManageJobsService } from '../../../services/hiremind/manageJobs.service';

@Component({
  selector: 'app-job-form-component',
  standalone: false,
  templateUrl: './job-form-component.html',
  styleUrl: './job-form-component.css',
})

export class JobFormComponent implements OnInit {
  job: Job = new Job();
  questionTypes: { id: number, label: string }[] = [];
  isEditMode: boolean = false;
  questionsCount: number = 1;
  today: Date = new Date();
  tomorrow: Date = new Date();
  readonly: boolean = false;
  loading: boolean = false;

  companies = [
    { name: 'Company A', id: 1 },
    { name: 'Company B', id: 2 },
  ];

  jobTypes = [
    { name: 'Full-Time', id: 1 },
    { name: 'Part-Time', id: 2 },
    { name: 'Internship / Trainee', id: 3 },
    { name: 'Freelance / Contract', id: 4 },
  ];

  locations = [
    { name: 'Amman', id: 1 },
    { name: 'Dubai', id: 2 },
  ];

  //new
  workPlaces = [
    { name: 'On-site', id: 1 },
    { name: 'Remote', id: 2 },
    { name: 'Hybrid', id: 3 },
  ];

  contractTypes = [
    { name: 'B2B (Business to Business)', id: 1 },
    { name: 'B2C (Business to Consumer)', id: 2 },
    { name: 'C2B (Consumer to Business)', id: 3 },
    { name: 'C2C (Consumer to Consumer)', id: 4 },
    { name: 'B2G (Business to Government)', id: 5 },
  ]

  organizationTypes = [
    { name: 'Government / Public Sector', id: 1 },
    { name: 'Semi-Government', id: 2 },
    { name: 'Private', id: 3 },
    { name: 'Confidential', id: 4 }
  ];

  industrySectors = [
    { name: 'Information Technology (IT)', id: 1 },
    { name: 'Finance / Banking', id: 2 },
    { name: 'Healthcare / Medical', id: 3 },
    { name: 'Education / Training', id: 4 },
    { name: 'Manufacturing / Production', id: 5 },
    { name: 'Retail / E-commerce', id: 6 },
    { name: 'Marketing / Advertising', id: 7 },
    { name: 'Hospitality / Tourism', id: 8 },
    { name: 'Logistics / Supply Chain', id: 9 },
    { name: 'Construction / Real Estate', id: 10 },
    { name: 'Telecommunications', id: 11 },
    { name: 'Energy / Utilities', id: 12 },
    { name: 'Legal / Law Services', id: 13 },
    { name: 'Media / Entertainment', id: 14 },
    { name: 'Consulting / Professional Services', id: 15 },
  ];
  //new

  constructor(
    public service: ManageJobsService,
    private route: ActivatedRoute,
    private router: Router,
    public toastService: ToastMessageService
  ) { }

  ngOnInit(): void {

    //this.questionTypes = Object.keys(JobQuestionType)
    //  .filter(k => isNaN(Number(k)))
    //  .map(k => ({
    //    id: JobQuestionType[k as keyof typeof JobQuestionType],
    //    label: k
    //  }));

    this.questionTypes = Object.keys(JobQuestionType)
      .filter(k => isNaN(Number(k))) // get only enum names
      .map(k => ({
        id: JobQuestionType[k as keyof typeof JobQuestionType],
        label: k
      }))
      .filter(q => q.id !== 7) // hide item with id 8
      .filter(q => q.id !== 8) // hide item with id 8
      .filter(q => q.id !== 9); // hide item with id 8

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
    //else {
    //  this.addQuestion();
    //}

  }

  loadJob(id: string) {

    this.service.GetById(id).subscribe({

      next: (res: any) => {

        const job = res.response;

        this.job = {
          id: job.id,
          title: job.title,
          description: job.description,
          locationId: job.locationId,
          jobTypeId: job.jobTypeId,
          //new
          workPlaceId: job.workPlaceId,
          contractTypeId: job.contractTypeId,
          organizationTypeId: job.organizationTypeId,
          industrySectorId: job.industrySectorId,
          //new
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

        this.questionsCount = this.job.questions.length; //|| 1;

        //if (!this.job.questions.length) {
        //  this.addQuestion();
        //}    

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
      preferredAnswers: [],
      score: 0
    };

    const defaultAnswer = {
      id: crypto.randomUUID(),
      text: ''
    };

    newQuestion.availableAnswers.push(defaultAnswer);

    newQuestion.preferredAnswers = [defaultAnswer.id];

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
      text: ''
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

      // Preferred Validation (except Text / Paragraph)
      if (!this.isTextType(q.questionTypeId) && q.preferredAnswers.length === 0) {
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

    //const payload = {
    //  ...this.job,
    //  startDate: this.job.startDate ? new Date(this.job.startDate).toISOString() : null,
    //  endDate: this.job.endDate ? new Date(this.job.endDate).toISOString() : null,
    //  id: this.job.id
    //};

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
    question.preferredAnswers = question.preferredAnswers || [];
    const answer = question.availableAnswers[answerIndex];

    if (!question.preferredAnswers.includes(answer.id)) {
      question.preferredAnswers.push(answer.id);
    }
  }

  setUnPreferredAnswer(questionIndex: number, answerIndex: number) {
    const question = this.job.questions[questionIndex];
    question.preferredAnswers = question.preferredAnswers || [];

    if (
      question.questionTypeId === JobQuestionType.Text ||
      question.questionTypeId === JobQuestionType.Paragraph
    ) {
      return;
    }

    const answer = question.availableAnswers[answerIndex];
    question.preferredAnswers = question.preferredAnswers.filter((aId: string) => aId !== answer.id);
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
    question.preferredAnswers = [];

    for (let k = 0; k < max; k++) {

      const newAnswer = {
        id: crypto.randomUUID(),
        text: question.questionTypeId === JobQuestionType.YesNo
          ? (k === 0 ? 'Yes' : 'No')
          : ''
      };

      question.availableAnswers.push(newAnswer);

      if (
        question.questionTypeId === JobQuestionType.Text ||
        question.questionTypeId === JobQuestionType.Paragraph
      ) {
        question.preferredAnswers = [newAnswer.id];
      }
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
}
