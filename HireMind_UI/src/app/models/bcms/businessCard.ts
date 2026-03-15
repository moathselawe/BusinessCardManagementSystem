export class BusinessCard {
  id!: number;
  arabicName!: string;
  englishName!: string;
  dateOfBirth!: Date;
  email!: string;
  phone!: string;
  logo!: string;
  address!: string;
  createdDate!: Date;
  isRemoved!: boolean;


  filteredArabicNames: string[] = [];
  filteredEnglishNames: string[] = [];
  filteredEmails: string[] = [];
  filteredAddresses: string[] = [];
}

