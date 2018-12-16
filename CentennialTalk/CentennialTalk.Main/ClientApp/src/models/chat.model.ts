import { QuestionModel } from './question.model';
import { MemberModel } from './member.model';

export class ChatModel {

  chatCode: string;
  moderator: string;
  username: string;
  title: string;
  connectionId: string;
  activationDate: Date;
  expirationDate: Date;
  openQuestions: QuestionModel[];
  pollQuestions: QuestionModel[];

  members: MemberModel[];
}
