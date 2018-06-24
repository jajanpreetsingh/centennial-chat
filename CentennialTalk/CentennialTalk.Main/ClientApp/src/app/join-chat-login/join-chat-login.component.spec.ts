import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JoinChatLoginComponent } from './join-chat-login.component';

describe('JoinChatLoginComponent', () => {
  let component: JoinChatLoginComponent;
  let fixture: ComponentFixture<JoinChatLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JoinChatLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JoinChatLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
