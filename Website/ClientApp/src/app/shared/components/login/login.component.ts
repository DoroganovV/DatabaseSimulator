import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { map, catchError, takeUntil } from 'rxjs/operators';
import { of, ReplaySubject } from 'rxjs';
import { LoginService } from './login.service';
import { LoginResponce, LoginResult } from '../../models/login-responce.model';
import { LoginRequest } from '../../models/login-request.model';
import { Group } from '../../models/group.model';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'login-component',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [LoginService]
})
export class LoginComponent implements OnInit {
  public ResultLogin: string = '';
  public LoginRequest: LoginRequest = new LoginRequest();
  public LoginResponce: LoginResponce;
  public IsLoading: boolean = false;

  public readonly LoginForm: FormGroup = null;
  private destroyedSubj = new ReplaySubject(1);

  public get IsLogin() { return this.LoginResponce && this.LoginResponce.item1 == LoginResult.OK; }

  @Output()
  LoginEvent = new EventEmitter();

  public Groups: Group[];

  constructor(
    private loginService: LoginService,
    private formBuilder: FormBuilder) {

    this.LoginForm = this.formBuilder.group(
      {
        GroupControl: this.formBuilder.control(null),
        FirstNameControl: this.formBuilder.control(null),
        LastNameControl: this.formBuilder.control(null),
        PasswordControl: this.formBuilder.control(null)
      });

    this.LoginForm.get('GroupControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((groupId: number) => {
      this.LoginRequest.groupId = groupId;
    });
    this.LoginForm.get('FirstNameControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((firstName: string) => {
      this.LoginRequest.firstName = firstName;
    });
    this.LoginForm.get('LastNameControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((lastName: string) => {
      this.LoginRequest.lastName = lastName;
    });
    this.LoginForm.get('PasswordControl').valueChanges.pipe(takeUntil(this.destroyedSubj)).subscribe((password: string) => {
      this.LoginRequest.password = password;
    });
  }

  ngOnInit() {
    this.ResultLogin = '';
    this.loginService.GetPersonGroups().pipe(
      map((data: Group[]) => {
        this.Groups = data;
      }),
      catchError(error => {
        return of({});
      })
    ).subscribe();

    this.loginService.TryLogin().pipe(
      map((data: LoginResponce) => {
        this.LoginResponce = data;
        this.LoginEvent.emit(data.item1 == LoginResult.OK);
      }),
      catchError(error => {
        return of({});
      })
    ).subscribe();
  }

  login() {
    this.ResultLogin = '';
    if (this.LoginRequest.firstName && this.LoginRequest.lastName && this.LoginRequest.groupId && this.LoginRequest.password) {
      this.IsLoading = true;
      this.loginService.Login(this.LoginRequest).pipe(
        map((data: LoginResponce) => {
          this.IsLoading = false;
          this.LoginRequest.password = '';

          if (data.item1 == LoginResult.PersonNotFound) {
            this.ResultLogin = 'Пользователь не найден или не верный пароль';
          } else if (data.item1 == LoginResult.OK) {
            this.LoginResponce = data;
            this.LoginEvent.emit(data.item1 == LoginResult.OK);
            //appInsights.setAuthenticatedUserContext(data.item2.Id);
          } else {
            this.ResultLogin = 'Ошибка';
          }
        }),
        catchError(error => {
          this.IsLoading = false;
          this.ResultLogin = error.error.title;
          return of({});
        })
      ).subscribe();
    } else {
      this.ResultLogin = 'Введите логин/пароль';
    }
  }

  logout() {
    this.loginService.Logout().pipe(
      map(data => {
        //appInsights.clearAuthenticatedUserContext();
        window.location.assign('/');
      })
    ).subscribe();
  }
}
