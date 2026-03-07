import { JobQuestion } from "./JobQuestion";

export class Job {
  id!: string;
  title!: string;
  description!: string;
  locationId!: number;
  jobTypeId!: number;
  workPlaceId!: number;
  organizationTypeId!: number;
  contractTypeId!: number;
  industrySectorId!: number;
  companyId!: number;
  startDate: Date | null = null;
  endDate: Date | null = null;
  isActive: boolean = true;
  questions: JobQuestion[] = [];
  createdDate?: Date;
  lastModifiedDate?: Date;
}
