import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewChatLoginComponent } from './new-chat-login.component';

describe('NewChatLoginComponent', () => {
  let component: NewChatLoginComponent;
  let fixture: ComponentFixture<NewChatLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewChatLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewChatLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
