import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';
import { v4 as uuid } from 'uuid';
import { FileService } from '../services/file.service';
import { SpeechService } from '../services/speech.service';
import { HubService } from '../services/hub.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  username: string;
  chatCode: string;
  moderator: string;
  amIModerator: boolean;
  title: string;

  message = '';

  constructor(private chatService: ChatService, private activatedRoute: ActivatedRoute,
    private fileService: FileService, private router: Router,
    private speechService: SpeechService, private hubService: HubService) {
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      console.log(this.activatedRoute.snapshot.queryParams);

      this.username = this.activatedRoute.snapshot.queryParams['username'];
      this.chatCode = this.activatedRoute.snapshot.queryParams['chatCode'];
      this.moderator = this.activatedRoute.snapshot.queryParams['moderator'];
      this.title = this.activatedRoute.snapshot.queryParams['title'];

      this.amIModerator = this.moderator == this.username;
    });

    var joinGroupReq = JSON.stringify({
      username: this.username,
      chatCode: this.chatCode,
      isModerator: this.amIModerator
    });

    this.hubService.initChatHub(joinGroupReq);
  }

  startListening() {
    this.speechService.startListening();
  }

  stopListening() {
    this.speechService.stopListening();
    this.message = this.speechService.recordTranscript;
  }

  playAudio(textToTranslate) {
    window.speechSynthesis.speak(new SpeechSynthesisUtterance(textToTranslate));
  }

  sendMessage() {
    var content = this.message;

    var mid = uuid();

    var messageObj = {
      messageId: mid,
      content: content,
      chatCode: this.chatCode,
      sender: this.username,
      replyId: mid,
    };

    this.hubService.sendMessage(messageObj);
  }
}
