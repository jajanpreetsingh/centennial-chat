import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { QuestionModel } from '../../models/question.model';
import { UserAnswer } from '../../models/useranswer.model';

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

  submitAnswer(answer: UserAnswer) {
    return this.http.post("/api/question/answer", answer)
      .map(res => res.json());
  }

  getPollingQuestions(chatCode: string) {
    return this.http.post("/api/question/getpolls", { 'value': chatCode })
      .map(res => res.json());
  }

  getOpenQuestions(chatCode: string) {
    return this.http.post("/api/question/getopen", { 'value': chatCode })
      .map(res => res.json());
  }
}
