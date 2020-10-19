import { SqlResult } from "./sql-result.model";

export class PersonAnswer {
  id: number;
  result: boolean;
  sqlAnswer: string;
  sqlCorrectAnswer: string;
  sqlResult?: SqlResult;
  sqlCorrectResult: SqlResult;
}
