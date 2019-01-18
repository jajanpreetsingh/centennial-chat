export class MessageModel {
  messageId: string;
  content: string;
  sender: string;
  chatCode: string;

  replyId: string;
  oldMessage: string;
  oldSender: string;

  sentDate: string;
  isMine: boolean;
  reactions: MemberReaction[];
  likeCount: number;
  dislikeCount: number;

  constructor() {
    this.reactions = [];
  }
}

export class MemberReaction {
  messageId: string;
  member: string;
  reaction: number;
  chatCode: string;
}
