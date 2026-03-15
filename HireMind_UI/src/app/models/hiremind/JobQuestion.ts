export class JobQuestion {
  questionText: string = '';
  questionTypeId: number | null = null;
  isRequired: boolean = false;
  availableAnswers: AnswerOption[] = [];
  score: number = 0;
}

export class AnswerOption {
  id: string = '';
  text: string = '';
  isPreferredAnswer: boolean = false;
}
