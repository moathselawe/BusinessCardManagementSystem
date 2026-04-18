import { Gender } from "../../enum/Gender"

export class User {
  id!: string
  nameArabic!: string
  nameEnglish!: string
  mobile!: string
  address?: string
  email!: string
  gender!: Gender
  isActive!: boolean
  isLocked!: boolean
  lockedDate!: Date;
  failedLoginAttempts!: number
  roleIds!: any[];
}

