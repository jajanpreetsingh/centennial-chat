import { Component, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate, keyframes } from '@angular/animations';
import { v4 as uuid } from 'uuid';
import { SpeechService } from '../services/speech.service';
import { HubService } from '../services/hub.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel, MemberReaction } from '../../models/message.model';
import { AccountService, StorageKeys } from '../services/account.service';
import { QuestionService } from '../services/question.service';
import { UserAnswer } from '../../models/useranswer.model';
import { MessageService } from '../services/message.service';

@Component({
  selector: 'app-discussion',
  templateUrl: './discussion.component.html',
  styleUrls: ['./discussion.component.scss'],
  animations: [
    trigger('fiftyAnimation', [
      transition('small <=> large', animate('1500ms ease-in', keyframes([
        style({ color: '#744DA8', offset: 0.05 }),
        style({ color: '#1FB3E0', offset: 0.20 }),
        style({ color: '#49C219', offset: 0.35 }),
        style({ color: '#F4DC2A', offset: 0.5 }),
        style({ color: '#EEB417', offset: 0.65 }),
        style({ color: '#D65129', offset: 0.8 }),
        style({ color: '#D6CDCA', offset: 0.95 })
      ]))),
    ]),
    trigger('seventyFiveAnimation', [
      transition('fixed <=> shaked', animate('1000ms ease-in', keyframes([
        style({ transform: 'scale(1.2)', offset: 0.2 }),
        style({ transform: 'scale(1.4)', offset: 0.5 }),
        style({ transform: 'scale(1.6)', offset: 0.8 }),
        style({ transform: 'scale(1)', offset: 1.0 }),
        style({ opacity: 0, transform: 'translateX(-50%)', offset: 0.0 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #FFF, #744DA8)', transform: 'translateY(11%)', offset: 0.1 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #744DA8, #1FB3E0)', transform: 'translateX(30%)', offset: 0.2 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #1FB3E0, #49C219)', transform: 'translateY(-29%)', offset: 0.3 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #49C219, #F4DC2A)', transform: 'translateX(-25%)', offset: 0.4 }),
        style({ opacity: 0.6, background: 'linear-gradient(to right, #F4DC2A, #EEB417)', transform: 'translateY(44%)', offset: 0.5 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #EEB417, #D65129)', transform: 'scale(1.5)', offset: 0.7 }),
        style({ opacity: 0.2, background: 'linear-gradient(to right, #D65129, #D6CDCA)', transform: 'scale(1.2)', offset: 0.8 }),
        style({ opacity: 1, background: 'linear-gradient(to right, #D6CDCA, #FFF)', transform: 'translateX(0)', offset: 0.9 }),
        style({ opacity: 1, transform: 'scale(1)', offset: 0.95 }),
        style({ opacity: 1, transform: 'translateY(0)', offset: 1.0 })
      ])))
    ]),
  ]
})
export class DiscussionComponent implements OnInit {
  chatData: ChatModel = new ChatModel();

  message: string;

  hubInstance: HubService;

  replyMessage: MessageModel = null;

  isListening: boolean = false;

  selectedOptions: string[] = [];

  stateFifty: string = 'small';
  stateSeventyFive: string = 'fixed';

  constructor(private speechService: SpeechService, private questionService: QuestionService,
    private hubService: HubService, private accountService: AccountService, private messageService: MessageService) {
  }

  ngOnInit() {
    this.hubInstance = this.hubService;

    this.chatData = this.accountService.getLocalChatData();
    this.hubService.initChatHub();
  }

  startListening() {
    this.speechService.startListening();
    this.isListening = true;
  }

  stopListening() {
    this.speechService.stopListening(this.setMessage);
  }

  public setMessage() {
    this.message = this.speechService.recordTranscript;

    this.isListening = false;
  }

  playAudio(m: MessageModel) {
    if (this.accountService.isValNull(m.content)) {
      let textToTranslate = m.sender + " says, " + m.content;
      window.speechSynthesis.speak(new SpeechSynthesisUtterance(textToTranslate));
    }
    else {
      window.speechSynthesis.speak(new SpeechSynthesisUtterance("Sometimes silence is the best answer.."));
    }
  }

  onOptionSelect(option: string) {
    let index: number = this.selectedOptions.indexOf(option);

    if (this.hubInstance.publishedQuestion.selectMultiple) {
      if (index >= 0)
        this.selectedOptions.splice(index, 1);
      else
        this.selectedOptions.push(option);
    }
    else {
      this.selectedOptions = [];
      this.selectedOptions.push(option);
    }
  }

  submitAnswer() {
    let answer: UserAnswer = new UserAnswer();

    answer.chatCode = this.chatData.chatCode;
    answer.isPollingQuestion = this.hubInstance.publishedQuestion.isPollingQuestion;
    let usrnm = this.accountService.getLocalData(StorageKeys.ChatUsername);

    let member = this.hubInstance.members.find(x => x.username == usrnm);

    if (member == null)
      return;

    answer.memberId = member.memberId;

    answer.content = this.message;
    answer.options = this.selectedOptions;

    answer.questionId = this.hubInstance.publishedQuestion.id;
    answer.selectMultiple = this.hubInstance.publishedQuestion.selectMultiple;

    this.questionService.submitAnswer(answer).subscribe(res => {
      if (res.code != 500) {
        this.hubInstance.answeredQuestions.push(this.hubInstance.publishedQuestion.id);

        this.hubInstance.publishedQuestion = null;
      }
    });
  }

  likeMessage(messageModel: MessageModel) {
    let react = new MemberReaction();
    react.member = this.chatData.username;
    react.messageId = messageModel.messageId;
    react.reaction = 1;
    react.chatCode = this.chatData.chatCode;

    this.hubInstance.sendReact(react);
  }

  dislikeMessage(messageModel: MessageModel) {
    let react = new MemberReaction();
    react.member = this.chatData.username;
    react.messageId = messageModel.messageId;
    react.reaction = -1;
    react.chatCode = this.chatData.chatCode;

    this.hubInstance.sendReact(react);
  }

  setReplyMessage(m: MessageModel) {
    this.replyMessage = m;
  }

  clearReplyMessage() {
    this.replyMessage = null;
  }

  sendMessage() {
    var content = this.message;

    var mid = uuid();

    let messageObj: MessageModel = new MessageModel()

    messageObj.messageId = mid;
    messageObj.content = content;
    messageObj.chatCode = this.chatData.chatCode;
    messageObj.sender = this.accountService.getLocalData(StorageKeys.ChatUsername);

    if (!this.accountService.isValNull(this.replyMessage))
      messageObj.replyId = this.replyMessage.messageId;

    this.hubService.sendMessage(messageObj);
    this.message = "";
    this.replyMessage = null;
  }

  animateMeFifty() {
    this.stateFifty = (this.stateFifty === 'small' ? 'large' : 'small');
  }

  animateMeSeventyFive() {
    this.stateSeventyFive = (this.stateSeventyFive === 'fixed' ? 'shaked' : 'fixed');
  }
}
