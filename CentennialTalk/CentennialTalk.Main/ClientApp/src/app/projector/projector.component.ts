import { Component, OnInit } from '@angular/core';
import { v4 as uuid } from 'uuid';
import { HubService } from '../services/hub.service';
import { SpeechService } from '../services/speech.service';
import { AccountService } from '../services/account.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel } from '../../models/message.model';
import { QuestionModel } from '../../models/question.model';
import { ChatService } from '../services/chat.service';

@Component({
  selector: 'app-projector',
  templateUrl: './projector.component.html',
  styleUrls: ['./projector.component.scss']
})
export class ProjectorComponent implements OnInit {
  chatData: ChatModel = new ChatModel();

  message: string;

  isListening: boolean = false;

  hubInstance: HubService;

  constructor(private speechService: SpeechService, private chatService: ChatService,
    private hubService: HubService, private accountService: AccountService) {
    this.hubInstance = this.hubService;
  }

  ngOnInit() {
    this.chatData = this.accountService.getLocalChatData();

    this.hubService.initChatHub();
  }

  publishQuestion(ques: QuestionModel) {
    this.hubService.publishQuestion(ques);
  }

  archiveQuestion(ques: QuestionModel) {
    this.hubService.archiveQuestion(ques);
  }

  startListening() {
    this.speechService.startListening();
    this.isListening = true;
  }

  stopListening() {
    this.speechService.stopListening();
    this.message = this.speechService.recordTranscript;

    this.isListening = false
  }

  playAudio(m: MessageModel) {
    let textToTranslate = m.sender + " says, " + m.content;

    window.speechSynthesis.speak(new SpeechSynthesisUtterance(textToTranslate));
  }

  sendMessage() {
    var content = this.message;

    var mid = uuid();

    let messageObj: MessageModel = new MessageModel()

    messageObj.messageId = mid;
    messageObj.content = content;
    messageObj.chatCode = this.chatData.chatCode;
    messageObj.sender = this.chatData.username;
    messageObj.replyId = mid;

    this.hubService.sendMessage(messageObj);
  }

  goToTranscript() {
    this.chatService.downloadTranscript(this.chatData.chatCode).subscribe(res => {
      console.log(res);

      let url = URL.createObjectURL(res.blob());
      var link = document.createElement("a");
      link.setAttribute("href", url);
      link.setAttribute("download", "test.docx");
      link.style.display = "none";
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    });
  }
}
