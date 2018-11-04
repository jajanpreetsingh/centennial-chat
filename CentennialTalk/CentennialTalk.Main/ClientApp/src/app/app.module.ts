import { BrowserModule } from '@angular/platform-browser';
import { NgModule, InjectionToken } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { GeneralLoginComponent } from './general-login/general-login.component';
import { ChatComponent } from './chat/chat.component';
import { NewChatComponent } from './new-chat/new-chat.component';
import { JoinChatComponent } from './join-chat/join-chat.component';
import { MenuComponent } from './menu/menu.component';
import { FooterComponent } from './footer/footer.component';
import { IconsComponent } from './icons/icons.component';
import { DiscussionComponent } from './discussion/discussion.component';
import { DiscussionPollComponent } from './discussion-poll/discussion-poll.component';
import { ProjectorComponent } from './projector/projector.component';
import { TranscriptComponent } from './transcript/transcript.component';
import { ErrorMessageComponent } from './error-message/error-message.component';

import { ChatService } from './services/chat.service';
import { MemberService } from './services/member.service';
import { MessageService } from './services/message.service';
import { DatePipe } from '@angular/common';
import { FileService } from './services/file.service';
import { SpeechService } from './services/speech.service';
import { SignUpComponent } from './sign-up/sign-up.component';
import { LoginComponent } from './login/login.component';
import { AccountService } from './services/account.service';
import { UtilityService } from './services/utility.service';
import { HubService } from './services/hub.service';
import { AlertModule } from 'ngx-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ChatComponent,
    NewChatComponent,
    JoinChatComponent,
    SignUpComponent,
    LoginComponent,
    MenuComponent,
    FooterComponent,
    IconsComponent,
    GeneralLoginComponent,
    DiscussionComponent,
    DiscussionPollComponent,
    ProjectorComponent,
    TranscriptComponent,
    ErrorMessageComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    HttpModule,
    FormsModule,
    BrowserAnimationsModule,
    AlertModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full' },
      { path: 'general-login', component: GeneralLoginComponent },
      { path: 'join', component: JoinChatComponent },
      { path: 'new', component: NewChatComponent },
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignUpComponent },
      { path: 'chat', component: ChatComponent },
      { path: 'chat/:data', component: ChatComponent },
      { path: 'chooseicon', component: IconsComponent },
      { path: 'discussionR', component: DiscussionComponent },
      { path: 'discussionpoll', component: DiscussionPollComponent },
      { path: 'projector', component: ProjectorComponent },
      { path: 'transcript', component: TranscriptComponent }
    ])
  ],
  providers: [
    ChatService,
    MemberService,
    MessageService,
    DatePipe,
    FileService,
    SpeechService,
    AccountService,
    UtilityService,
    HubService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
