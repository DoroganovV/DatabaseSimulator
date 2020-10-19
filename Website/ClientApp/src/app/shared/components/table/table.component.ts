import { Component, Input, OnInit } from '@angular/core';
import { SqlResult } from '../../../simulator/models/sql-result.model';

@Component({
  selector: 'simulator-table-component',
  templateUrl: './table.component.html'
})
export class TableComponent implements OnInit {

  @Input()
  public SqlTable: SqlResult;

  public get Columns(): Array<string>
  {
    if (this.SqlTable) {
      return this.SqlTable.columns;
    } else {
      return [];
    }
  }

  public get Table(): Array<string[]> {
    if (this.SqlTable) {
      return this.SqlTable.dataTable;
    } else {
      return [];
    }
  }

  constructor() { }
  ngOnInit() {
  }
}
