import { Injectable } from '@angular/core';
import { HubService } from './hub.service';
import { Observable } from 'rxjs/Observable';
import { UtilityService } from './utility.service';
import { Level } from '../../models/popup.model';

@Injectable()
export class SpeechService {
  private _window: any;
  private recognition: any;
  public recordTranscript: string;

  callback: Function;

  constructor(private util: UtilityService) {
    this._window = window;
  }

  startListening() {

    var SpeechRecognition = this._window.webkitSpeechRecognition
      || this._window.SpeechRecognition;

    if (SpeechRecognition) {
      this.recognition = new SpeechRecognition();

      this.recognition.onstart = this.onStartRecognizing;
      this.recognition.onresult = ev => this.onRecognitionResult(ev);
      this.recognition.onend = this.onRecognitionEnd.bind(this);
      this.recognition.continuous = true;

      this.recognition.start();
    }
    else {
      this.util.addPageError("Not supported", "HTML 5 Speech recognition not supported on this browser", Level[Level.danger]);
    }
  }

  private onRecognitionEnd() {

    this.callback();
  }

  private onStartRecognizing() {
  }

  private onRecognitionResult(event) {
    if (event.results) {
      let m = event.results[0][0].transcript;
      this.recordTranscript = m;
    }
  }

  stopListening(callback) {

    this.callback = callback;

    this.recognition.stop();
    this.recognition = null;
  }
}
