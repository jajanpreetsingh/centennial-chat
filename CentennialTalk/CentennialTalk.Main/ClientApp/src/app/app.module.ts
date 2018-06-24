import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ChatComponent } from './chat/chat.component';
import { NewChatComponent } from './new-chat/new-chat.component';
import { JoinChatComponent } from './join-chat/join-chat.component';
import { ChatService } from './services/chat.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FetchDataComponent,
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
      { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [
    ChatService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
