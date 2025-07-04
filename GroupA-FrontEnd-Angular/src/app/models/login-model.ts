export class LoginModel {
    constructor(
      public userId: number,
      public token: string,
      public roleId: number,
      public lastPasswordChange: Date
    ) { }
  }