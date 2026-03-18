import { JobQuestion } from "./JobQuestion";

export class HiringStage {
  id: number = 0;
  name: string = '';
  stageOrder: number = 0;
  viaId?: number;
  emailTemplate: string = '';
  interviewQuestions?: JobQuestion[]; 
  examQuestions?: JobQuestion[];
  isDisabled: boolean = false;
  isFinalStage: boolean = false;
}
