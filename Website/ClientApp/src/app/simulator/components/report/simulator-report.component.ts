import { Component, OnInit } from '@angular/core';
import { SimulatorService } from '../../services/simulator-service';
import { map, takeUntil } from 'rxjs/operators';
import { Database } from '../../models/database.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ReplaySubject } from 'rxjs';
import { Group } from '../../../shared/models/group.model';
import { Person } from '../../models/person.model';
import { PersonExercise } from '../../models/person-exercise.model';
import { PersonAnswer } from '../../models/person-answer.model';
import { PersonAnswerBase } from '../../models/person-answer-base.model';

@Component({
  selector: 'simulator-report-component',
  templateUrl: './simulator-report.component.html',
  styleUrls: ['./simulator-report.component.scss'],
  providers: [SimulatorService]
})
export class SimulatorReportComponent implements OnInit {

  public Databases: Database[];
  public Groups: Group[];
  public Persons: Person[];
  public PersonExercises: PersonExercise[];
  public PersonAnswers: PersonAnswer[];
  public PersonAnswerBases: PersonAnswerBase[];
  public SelectedDatabase: Database;
  public SelectedPersonAnswer: PersonAnswer;
  
  public IsLogin: boolean;

  addItem(newItem: boolean) {
    this.IsLogin = newItem;
  }

  public readonly SimulatorReportForm: FormGroup = null;
  private destroyedSubj = new ReplaySubject(1);

  constructor(
    private simulatorService: SimulatorService,
    private formBuilder: FormBuilder) {

    this.SimulatorReportForm = this.formBuilder.group(
      {
        DatabaseControl: this.formBuilder.control(null),
        GroupControl: this.formBuilder.control(null),
        PersonControl: this.formBuilder.control(null),
        PersonExerciseControl: this.formBuilder.control(null),
        PersonAnswerControl: this.formBuilder.control(null),
      });

    this.SimulatorReportForm.get('DatabaseControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((databaseId: number) =>
      this.OnDatabaseControlValueChange(databaseId));
    this.SimulatorReportForm.get('GroupControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((groupId: number) =>
      this.OnGroupControlValueChange(groupId));
    this.SimulatorReportForm.get('PersonControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((personId: number) =>
      this.OnPersonControlValueChange(personId));
    this.SimulatorReportForm.get('PersonExerciseControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((personExerciseId: number) =>
      this.OnPersonExerciseControlValueChange(personExerciseId));
    this.SimulatorReportForm.get('PersonAnswerControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((personAnswerId: number) =>
      this.OnPersonAnswerControlValueChange(personAnswerId));
  }

  ngOnInit() {
    this.simulatorService.GetDataBases().pipe(
      map((data: Database[]) => {
        this.Databases = data;
      })
    ).subscribe();
  }

  private OnDatabaseControlValueChange(databaseId: number): void {
    this.Groups = null;
    this.SimulatorReportForm.get('GroupControl').setValue(null);

    if (databaseId) {
      this.SelectedDatabase = this.Databases.find(x => x.id == databaseId);
      this.simulatorService.GetGroupsByDataBase(databaseId).pipe(
        map((data: Group[]) => { this.Groups = data; })
      ).subscribe();
    } else {
      this.Groups = null;
    }
  }
  private OnGroupControlValueChange(groupId: number): void {
    this.Persons = null;
    this.SimulatorReportForm.get('PersonControl').setValue(null);

    if (groupId) {
      this.simulatorService.GetPersonsByGroup(groupId).pipe(
        map((data: Person[]) => { this.Persons = data; })
      ).subscribe();
    } else {
      this.Persons = null;
    }
  }
  private OnPersonControlValueChange(personId: number): void {
    this.PersonExercises = null;
    this.SimulatorReportForm.get('PersonExerciseControl').setValue(null);

    if (personId) {
      this.simulatorService.GetExercisesByPerson(this.SelectedDatabase.id, personId).pipe(
        map((data: PersonExercise[]) => { this.PersonExercises = data; })
      ).subscribe();
    } else {
      this.PersonExercises = null;
    }
  }
  private OnPersonExerciseControlValueChange(personExerciseId: number): void {
    this.PersonAnswers = null;
    this.SimulatorReportForm.get('PersonAnswerControl').setValue(null);

    if (personExerciseId) {
      var exerciseId = this.SimulatorReportForm.get('PersonExerciseControl').value;
      var personId = this.SimulatorReportForm.get('PersonControl').value;
      this.simulatorService.GetPersonAnswersByPerson(exerciseId, personId).pipe(
        map((data: PersonAnswerBase[]) => { this.PersonAnswerBases = data; })
      ).subscribe();
    } else {
      this.PersonAnswerBases = null;
    }
  }
  private OnPersonAnswerControlValueChange(personAnswerId: number): void {
    this.SelectedPersonAnswer = null;

    if (personAnswerId) {
      this.simulatorService.GetPersonAnswer(personAnswerId).pipe(
        map((data: PersonAnswer) => { this.SelectedPersonAnswer = data; })
      ).subscribe();
    } else {
      this.SelectedPersonAnswer = null;
    }
  }
}
