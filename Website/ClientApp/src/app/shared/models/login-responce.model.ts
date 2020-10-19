import { Person } from "./person.model";

export class LoginResponce {
  constructor(
    public item1?: LoginResult,
    public item2?: Person) { }
}

export enum LoginResult {
  OK = 1,
  PersonNotFound = 2,
  OtherError = 3
}
