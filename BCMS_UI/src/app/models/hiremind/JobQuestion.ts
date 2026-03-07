export class JobQuestion {
  questionText: string = '';
  questionTypeId: number | null = null;
  isRequired: boolean = false;
  availableAnswers: AnswerOption[] = [];
  preferredAnswers: string[] = [];
  score: number = 0;
}

export class AnswerOption {
  id: string = '';
  text: string = '';
}
