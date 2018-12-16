export class UserAnswer {
  questionId: string
  memberId: string
  content: string;
  selectMultiple: boolean;
  options: string[];
  isPollingQuestion: boolean;
  chatCode: string;
}
