import { HiringStage } from "./HiringStage";
import { JobQuestion } from "./JobQuestion";
export class Job {
  id!: number;
  title!: string;
  description!: string;
  locationId!: number;
  locationName?: string;
  workPlaceId!: number;
  workPlaceName?: string;
  organizationTypeId!: number;
  organizationTypeName?: string;
  contractTypeId!: number;
  contractTypeName?: string;
  industrySectorId!: number;
  industrySectorName?: string;
  jobTypeId!: number;
  jobTypeName?: string;
  companyId!: number;
  startDate: Date | null = null;
  endDate: Date | null = null;
  isActive: boolean = true;
  hiringStages: HiringStage[] = [];
  questions: JobQuestion[] = [];
  createdDate?: Date;
  lastModifiedDate?: Date;
}
