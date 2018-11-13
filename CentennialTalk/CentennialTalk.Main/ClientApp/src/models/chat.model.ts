import { QuestionModel } from './question.model';

export class ChatModel {
  chatCode: string;
  moderator: string;
  username: string;
  title: string;
  connectionId: string;
  activationDate: string;
  expirationDate: string;
  openQuestions: QuestionModel[];
  pollQuestions: QuestionModel[];
}
