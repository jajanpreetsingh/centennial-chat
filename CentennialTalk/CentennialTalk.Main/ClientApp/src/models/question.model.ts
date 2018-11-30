export class QuestionModel {
  id: string
  content: string;
  selectMultiple: boolean;
  options: string[];
  isPollingQuestion: boolean;
  chatCode: string;
  isPublished: boolean;
  isArchived: boolean;
  publishDate: Date;
  archiveDate: Date;
}
