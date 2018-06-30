import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { ChatComponent } from './chat/chat.component';
import { NewChatComponent } from './new-chat/new-chat.component';
import { JoinChatComponent } from './join-chat/join-chat.component';

import { ChatService } from './services/chat.service';
import { MemberService } from './services/member.service';
import { MessageService } from './services/message.service';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ChatComponent,
    NewChatComponent,
    JoinChatComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'join', component: JoinChatComponent },
      { path: 'new', component: NewChatComponent },
      { path: 'chat', component: ChatComponent },
      { path: 'chat/:data', component: ChatComponent },
    ])
  ],
  providers: [
    ChatService,
    MemberService,
    MessageService,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
