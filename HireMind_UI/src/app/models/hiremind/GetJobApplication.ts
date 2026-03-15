export class GetJobApplication {
  id!: number;
  email: string = '';
  fullName: string = '';
  mobileNumber: string = '';
  countryCodeId!: number;
  countryCode?: string;
  systemScore!: number;
  totalScore!: number;
  jobTitle: string = '';
  currentStageName: string = '';
  currentStageOrder!: number;
  applicationStageId!: number;
  hiringStageId!: number;
  status: string = '';
}

