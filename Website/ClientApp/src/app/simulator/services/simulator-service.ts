import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Answer } from '../models/answer.model';

@Injectable()
export class SimulatorService {

  private urlSolution = "/api/solution";

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
}
