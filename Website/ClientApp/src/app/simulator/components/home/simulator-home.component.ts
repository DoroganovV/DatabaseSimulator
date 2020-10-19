import { Component, OnInit } from '@angular/core';
import { SimulatorService } from '../../services/simulator-service';
import { map, takeUntil } from 'rxjs/operators';
import { Database } from '../../models/database.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Exercise } from '../../models/exercise.model';
import { Answer } from '../../models/answer.model';
import { ReplaySubject } from 'rxjs';

@Component({
  selector: 'simulator-home-component',
  templateUrl: './simulator-home.component.html',
  styleUrls: ['./simulator-home.component.scss'],
  providers: [SimulatorService]
})
export class SimulatorHomeComponent implements OnInit {

  public Databases: Database[];
  public Exercises: Exercise[];
  public SelectedDatabase: Database;
  public SelectedExercise: Exercise;
  public CorrectAnswer: Answer;
  public MyAnswer: Answer;
  public TextSql: string = '';

  public IsLogin: boolean;

  addItem(newItem: boolean) {
    this.IsLogin = newItem;
  }

  public readonly SimulatorHomeForm: FormGroup = null;
  private destroyedSubj = new ReplaySubject(1);

  public get TaskLevel(): number {
    if (this.SelectedExercise)
      return this.SelectedExercise.taskLevel;
    else
      return;
  }
  public get TaskText(): string {
    if (this.SelectedExercise)
      return this.SelectedExercise.text;
    else
      return '';
  }

  constructor(
    private simulatorService: SimulatorService,
    private formBuilder: FormBuilder) {

    this.SimulatorHomeForm = this.formBuilder.group(
      {
        DatabaseControl: this.formBuilder.control(null),
        ExerciseControl: this.formBuilder.control(null),
        TextSqlControl: this.formBuilder.control(null),
      });

    this.SimulatorHomeForm.get('DatabaseControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((databaseId: number) => {
      this.OnDatabaseControlValueChange(databaseId);
    });
    this.SimulatorHomeForm.get('ExerciseControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((exerciseId: number) => {
      this.OnExerciseControlValueChange(exerciseId);
    });
    this.SimulatorHomeForm.get('TextSqlControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((sql: string) => {
      this.TextSql = sql;
    });
  }

  ngOnInit() {
    this.simulatorService.GetDataBases().pipe(
      map((data: Database[]) => {
        this.Databases = data;
      })
    ).subscribe();
  }

  private OnDatabaseControlValueChange(databaseId: number): void {
    this.Exercises = null;
    this.SelectedExercise = null;
    if (databaseId) {
      this.SelectedDatabase = this.Databases.find(x => x.id == databaseId);
      this.simulatorService.GetExercisesByDataBase(databaseId).pipe(
        map((data: Exercise[]) => {
          this.Exercises = data;
        })
      ).subscribe();
    } else {
      this.Exercises = null;
    }
  }

  private OnExerciseControlValueChange(exerciseId: number): void {
    if (exerciseId) {
      this.SelectedExercise = this.Exercises.find(x => x.id == exerciseId);
      this.simulatorService.GetCorrectSolution(exerciseId).pipe(
        map((data: Answer) => this.CorrectAnswer = data)
      ).subscribe();
      this.simulatorService.GetMyLastAnswer(exerciseId).pipe(
        map((data: Answer) => {
          this.MyAnswer = data;
          this.SimulatorHomeForm.get('TextSqlControl').setValue(data.sqlAnswer);
        })
      ).subscribe();
    } else {
      this.SelectedExercise = null;
      this.CorrectAnswer = null;
      this.MyAnswer = null;
    }
  }

  public ExuteSql(): void {
    if (this.TextSql) {
      this.MyAnswer = { sqlAnswer: this.TextSql };
      this.simulatorService.TryMySolution(this.SelectedExercise.id, this.MyAnswer).pipe(
        map((data: Answer) => {
          this.MyAnswer = data;
          this.SimulatorHomeForm.get('TextSqlControl').setValue(data.sqlAnswer);
        })
      ).subscribe();
    }
  }

  public SchemeWin: boolean = false;
  public ChangeModeScheme() {
    this.SchemeWin = !this.SchemeWin;
  }
}
