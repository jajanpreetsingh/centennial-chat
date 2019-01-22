import { BrowserModule } from '@angular/platform-browser';
import { NgModule, InjectionToken } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NewChatComponent } from './new-chat/new-chat.component';
import { JoinChatComponent } from './join-chat/join-chat.component';
import { SignUpComponent } from './sign-up/sign-up.component';
import { LoginComponent } from './login/login.component';
import { MenuComponent } from './menu/menu.component';
import { DiscussionComponent } from './discussion/discussion.component';
import { DiscussionPollComponent } from './discussion-poll/discussion-poll.component';
import { FooterComponent } from './footer/footer.component';
import { GeneralLoginComponent } from './general-login/general-login.component';
import { ErrorMessageComponent } from './error-message/error-message.component';
import { IconsComponent } from './icons/icons.component';
import { ProjectorComponent } from './projector/projector.component';
import { TranscriptComponent } from './transcript/transcript.component';

import { ChatService } from './services/chat.service';
import { MemberService } from './services/member.service';
import { MessageService } from './services/message.service';
import { DatePipe } from '@angular/common';
import { FileService } from './services/file.service';
import { SpeechService } from './services/speech.service';
import { AccountService } from './services/account.service';
import { UtilityService } from './services/utility.service';
import { HubService } from './services/hub.service';
import { QuestionService } from './services/question.service';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { BrowserDomAdapter } from '@angular/platform-browser/src/browser/browser_adapter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NewChatComponent,
    JoinChatComponent,
    SignUpComponent,
    LoginComponent,
    DiscussionComponent,
    DiscussionPollComponent,
    MenuComponent,
    FooterComponent,
    GeneralLoginComponent,
    ErrorMessageComponent,
    IconsComponent,
    ProjectorComponent,
    TranscriptComponent,
    ConfirmEmailComponent,
    DashboardComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    BrowserAnimationsModule,
    HttpModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'home', component: HomeComponent, pathMatch: 'full' },
      { path: 'join', component: JoinChatComponent },
      { path: 'new', component: NewChatComponent },
      { path: 'login', component: LoginComponent },
      { path: 'signup', component: SignUpComponent },
      { path: 'discussion/:data', component: DiscussionComponent },
      { path: 'discussion', component: DiscussionComponent },
      { path: 'discussionpoll', component: DiscussionPollComponent },
      { path: 'general-login', component: GeneralLoginComponent },
      { path: 'projector', component: ProjectorComponent },
      { path: 'transcript', component: TranscriptComponent },
      { path: 'icon', component: IconsComponent },
      { path: 'verify', component: ConfirmEmailComponent },
      { path: 'forgot', component: ForgotPasswordComponent },
      { path: 'reset', component: ResetPasswordComponent },
      { path: 'dashboard', component: DashboardComponent },
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
    QuestionService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
