import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginRequest } from '../../models/login-request.model';

@Injectable()
export class LoginService {

  private urlSolution = "/api/login";

  constructor(private http: HttpClient) {
  }
  GetPersonGroups() {
    return this.http.get(`${this.urlSolution}/groups`);
  }
  Login(loginRequest: LoginRequest) {
    return this.http.put(`${this.urlSolution}`, loginRequest);
  }
  TryLogin() {
    return this.http.get(`${this.urlSolution}/tryLogin`);
  }
  Logout() {
    return this.http.delete(`${this.urlSolution}`);
  }
}
