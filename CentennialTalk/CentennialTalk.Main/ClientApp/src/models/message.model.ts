export class MessageModel {
  messageId: string;
  content: string;
  sender: string;
  chatCode: string;
  replyId: string;
  sentDate: Date;
  isMine: boolean;
}
