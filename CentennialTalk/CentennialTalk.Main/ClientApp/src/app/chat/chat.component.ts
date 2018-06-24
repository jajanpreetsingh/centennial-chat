import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';
import { ChatService } from '../services/chat.service';

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

  members: string[] = [];

  private hubConnection: HubConnection;
  message = '';
  messages: string[] = [];

  constructor(private chatService: ChatService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params: Params) => {
      console.log(this.activatedRoute.snapshot.queryParams);
    });

    this.hubConnection = new signalR.HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Trace)
      .withUrl('/discussion')
      .build();

    this.hubConnection.start();

    this.hubConnection.on('connectionStarted', data => {
      this.onConnectionStarted(data);
    });
  }

  onConnectionStarted(data) {
    console.log('connect id', data);
  }
}
