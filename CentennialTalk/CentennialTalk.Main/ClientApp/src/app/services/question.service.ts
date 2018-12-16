import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { QuestionModel } from '../../models/question.model';

@Injectable()
export class QuestionService {
  constructor(private http: Http) {
  }

  markPublished(ques: QuestionModel) {
    return this.http.post("/api/question/published", ques)
      .map(res => res.json());
  }

  markArchived(ques: QuestionModel) {
    return this.http.post("/api/question/archived", ques)
      .map(res => res.json());
  }

  submitAnswer(ques: QuestionModel) {
    return this.http.post("/api/question/answer", ques)
      .map(res => res.json());
  }
}
