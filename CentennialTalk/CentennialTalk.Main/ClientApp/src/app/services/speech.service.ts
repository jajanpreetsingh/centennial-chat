import { Injectable } from '@angular/core';

@Injectable()
export class SpeechService {
  private _window: any;
  private recognition: any;
  public recordTranscript: string;

  constructor() {
    this._window = window;
  }

  startListening() {
    var SpeechRecognition = this._window.webkitSpeechRecognition
      || this._window.SpeechRecognition;

    console.log(SpeechRecognition);

    if (SpeechRecognition) {
      this.recognition = new SpeechRecognition();

      console.log(this.recognition);

      this.recognition.onstart = this.onStartRecognizing;
      this.recognition.onresult = ev => this.onRecognitionResult(ev);
      this.recognition.onend = this.onRecognitionEnd;
      this.recognition.continuous = true;

      this.recognition.start();
    }
    else {
      alert('sr not supported');
    }
  }

  private onRecognitionEnd() {
  }

  private onStartRecognizing() {
  }

  private onRecognitionResult(event) {
    if (event.results) {
      var result = event.results[0][0].transcript;
      console.log(result);
      this.recordTranscript = result;
    }
  }

  stopListening() {
    if (this.recognition) {
      this.recognition.stop();
      this.recognition = null;
    }
  }
}
