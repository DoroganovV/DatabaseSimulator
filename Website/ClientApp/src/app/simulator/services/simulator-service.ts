import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Answer } from '../models/answer.model';

@Injectable()
export class SimulatorService {

  private urlSolution = "/api/solution";
  private urlSolutionReport = "/api/solution-report";

  constructor(private http: HttpClient) {
  }
  GetDataBases() {
    return this.http.get(`${this.urlSolution}/data-bases`);
  }
  GetExercisesByDataBase(dataBaseId: number) {
    return this.http.get(`${this.urlSolution}/data-bases/${dataBaseId}/exercises`);
  }
  GetExerciseById(exerciseId: number) {
    return this.http.get(`${this.urlSolution}/exercise/${exerciseId}`);
  }
  GetCorrectSolution(exerciseId: number) {
    return this.http.get(`${this.urlSolution}/exercise/${exerciseId}/correct-solution`);
  }
  GetMyLastAnswer(exerciseId: number) {
    return this.http.get(`${this.urlSolution}/exercise/${exerciseId}/my-solution`);
  }
  TryMySolution(exerciseId: number, answer: Answer) {
    return this.http.post(`${this.urlSolution}/exercise/${exerciseId}/my-solution`, answer);
  }

  GetGroupsByDataBase(dataBaseId: number) {
    return this.http.get(`${this.urlSolutionReport}/data-bases/${dataBaseId}/groups`);
  }
  GetPersonsByGroup(groupId: number) {
    return this.http.get(`${this.urlSolutionReport}/group/${groupId}/persons`);
  }
  GetExercisesByPerson(dataBaseId: number, personId: number) {
    return this.http.get(`${this.urlSolutionReport}/data-bases/${dataBaseId}/exercise/${personId}`);
  }
  GetPersonAnswersByPerson(exerciseId: number, personId: number) {
    return this.http.get(`${this.urlSolutionReport}/exercise/${exerciseId}/person/${personId}`);
  }
  GetPersonAnswer(personAnswerId: number) {
    return this.http.get(`${this.urlSolutionReport}/person-answer/${personAnswerId}`);
  }
}
