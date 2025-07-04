export class Account {
  constructor(
    public accountID: number,
    public balance: number,
    public hasCheque: boolean,
    public wd_quota: number,
    public dp_quota: number,
    public isActive: boolean,
    public customerID: number,
    public type_id: number,
    public branchID: string
  ) {}
}
