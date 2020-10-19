import { SqlResult } from "./sql-result.model";

export class Answer {
  id?: number;
  sqlAnswer?: string;
  sqlResult?: SqlResult;
  result?: string;
  isDone?: boolean;
}
