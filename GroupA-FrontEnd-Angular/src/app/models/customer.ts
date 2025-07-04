export class Customer {
    constructor(
      public customerId: number,
      public firstName: string,
      public lastName: string,
      public addressLine1: string,
      public addressLine2: string,
      public addressLine3: string,
      public phoneNumber: string,
      public pincode: number,
      public emailAddress: string,
      public dateOfBirth: Date,
      public city: string,
      public country: string,
      public userId: string,
      public documentPhotoFile: string | null = null,
      public documentAadharFile: string | null = null,
      public documentPanCardFile: string | null = null
    ) {}
  }