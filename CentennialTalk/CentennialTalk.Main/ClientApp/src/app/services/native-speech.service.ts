import { Injectable } from '@angular/core';
import { SpeechRecognition, SpeechRecognitionTranscription } from 'nativescript-speech-recognition';

@Injectable()
export class NativeSpeechService {
  constructor() { }

  private speechRecognition = new SpeechRecognition();

  check() {
    this.speechRecognition.available().then(
      (available) => { return available; },
      (err: string) => console.log(err)
    );
  }

  requestPermission() {
    this.speechRecognition.requestPermission().then((granted) => {
      console.log('Granted? ' + granted);
      return granted;
    });
  }

  start() {

    var available = this.check();

    if (available) {
      var permit = this.requestPermission();

      if (!permit)
        return;
    }

    this.speechRecognition.startListening({
      // optional, uses the device locale by default
      locale: 'en-US',
      // set to true to get results back continuously
      returnPartialResults: false,
      // this callback will be invoked repeatedly during recognition
      onResult: (transcription) => {
        console.log('User said: ', transcription);
      },
    }).then(
      (started) => { console.log('started listening') },
      (err) => {
        console.log(err);
      });
  }

  stop() {
    this.speechRecognition.stopListening().then(
      () => { console.log('stopped listening') },
      (errorMessage: string) => { console.log('Stop error: ${errorMessage}'); }
    );
  }
}
