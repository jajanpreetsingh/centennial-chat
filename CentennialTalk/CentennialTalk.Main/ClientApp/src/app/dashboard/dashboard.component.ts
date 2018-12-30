import { Component, OnInit } from '@angular/core';
import { ChatModel } from '../../models/chat.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  chats: ChatModel[] = [];

  constructor() { }

  ngOnInit() {
  }

}
