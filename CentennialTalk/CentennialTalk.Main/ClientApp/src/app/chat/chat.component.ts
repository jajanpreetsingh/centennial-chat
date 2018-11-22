import { Component, OnInit } from '@angular/core';
import { v4 as uuid } from 'uuid';
import { SpeechService } from '../services/speech.service';
import { HubService } from '../services/hub.service';
import { ChatModel } from '../../models/chat.model';
import { MessageModel } from '../../models/message.model';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  chatData: ChatModel = new ChatModel();

  message: string;

  hubInstance: HubService;

  constructor(private speechService: SpeechService,
    private hubService: HubService, private accountService: AccountService) {
    this.hubInstance = this.hubService;
  }

  ngOnInit() {
    this.chatData = this.accountService.getLocalChatData();

    this.hubService.initChatHub();
  }

  startListening() {
    this.speechService.startListening();
  }

  stopListening() {
    this.speechService.stopListening();
    this.message = this.speechService.recordTranscript;
  }

  playAudio(m: MessageModel) {
    let textToTranslate = m.sender + " says, " + m.content;

    window.speechSynthesis.speak(new SpeechSynthesisUtterance(textToTranslate));
  }

  sendMessage() {
    var content = this.message;

    var mid = uuid();

    //let messageObj: MessageModel = new MessageModel();
    
    //  messageId: mid,
    //  content: content,
    //  chatCode: this.chatData.chatCode,
    //  sender: this.chatData.username,
    //  replyId: mid,
    //};

    //this.hubService.sendMessage(messageObj);
  }
}
