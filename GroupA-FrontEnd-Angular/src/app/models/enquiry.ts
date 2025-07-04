export class Enquiry {
    constructor(
      public firstName: string,
      public lastName: string,
      public addressLine1: string,
      public addressLine2: string,
      public addressLine3: string,
      public phoneNumber: string,
      public emailAddress: string,
      public dateOfBirth: Date,
      public city: string,
      public country: string,
      public status: string,
      public pinCode: string,
      public wantsCheque: boolean,
      public feedback: string,
      public accountType: string,
      public balance: number,
      public photo: string,
      public aadhaar: string,
      public panCard: string
    ) { }
  }