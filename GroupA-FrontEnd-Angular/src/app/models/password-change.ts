export class PasswordChangeModel {
    constructor(
      public username: string,
      public oldPassword: string,
      public newPassword: string
    ) {}
}