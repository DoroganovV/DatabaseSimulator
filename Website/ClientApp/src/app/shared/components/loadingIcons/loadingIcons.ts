import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'loadingIcons-component',
  templateUrl: './loadingIcons.html',
  styleUrls: ['./loadingIcons.scss']
})
export class LoadingIcons implements OnInit {
  @Input()
  IsLoading: boolean = false;
  constructor() { }
  ngOnInit() { }
}
