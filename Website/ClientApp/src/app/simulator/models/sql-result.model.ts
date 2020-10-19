export class SqlResult {
  id: number;
  columns: Array<string>;
  dataTable: Array<string[]>;
  hasException: boolean;
  countRows: number;
  countColumns: number;
}
