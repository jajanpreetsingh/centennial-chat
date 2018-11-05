import { Component, OnInit } from '@angular/core';
import { FileService } from '../services/file.service';
const speech = require('@google-cloud/speech');

declare var MediaRecorder: any;
const fs = require('fs');

const record = require('node-record-lpcm16');

const client = new speech.SpeechClient();

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  mediaRecorder: any;

  blob: any;

  constructor(private fileService: FileService) {
  }

  ngOnInit() {
  }

  start() {
    navigator.mediaDevices.getUserMedia({ audio: { sampleRate: 16000 } })
      .then(stream => {
        this.mediaRecorder = new MediaRecorder(stream);
        this.mediaRecorder.start();

        const audioChunks = [];

        this.mediaRecorder.addEventListener("dataavailable", event => {
          audioChunks.push(event.data);
        });

        this.mediaRecorder.addEventListener("stop", () => {
          const audioBlob = new Blob(audioChunks, { type: "audio/wav" });

          this.fileService.saveFile(audioBlob).subscribe(res => { console.log(res); });
        });
      });
  }

  startTwo() {
    const encoding = 'Encoding of the audio file, e.g. LINEAR16';
    const sampleRateHertz = 16000;
    const languageCode = 'BCP-47 language code, e.g. en-US';

    const request = {
      config: {
        encoding: encoding,
        sampleRateHertz: sampleRateHertz,
        languageCode: languageCode,
      },
      interimResults: false, // If you want interim results, set this to true
    };

    // Create a recognize stream
    const recognizeStream = client
      .streamingRecognize(request)
      .on('error', console.error)
      .on('data', data =>
        process.stdout.write(
          data.results[0] && data.results[0].alternatives[0]
            ? `Transcription: ${data.results[0].alternatives[0].transcript}\n`
            : `\n\nReached transcription time limit, press Ctrl+C\n`
        )
      );

    record
      .start({
        sampleRateHertz: sampleRateHertz,
        threshold: 0,
        // Other options, see https://www.npmjs.com/package/node-record-lpcm16#options
        verbose: false,
        recordProgram: 'rec', // Try also "arecord" or "sox"
        silence: '10.0',
      })
      .on('error', console.error)
      .pipe(recognizeStream);
  }

  handleBlob(event) {
    var arrayBuffer = event.target.result;

    console.log(arrayBuffer);

    this.fileService.saveFile(btoa(arrayBuffer)).subscribe(res => { console.log(res); });
  }

  stop() {
    this.mediaRecorder.stop();
  }
}
//const audioUrl = URL.createObjectURL(audioBlob);
//var fileReader = new FileReader();
//fileReader.onload = this.handleBlob.bind(this);
//fileReader.readAsArrayBuffer(audioBlob);
//const audio = new Audio(audioUrl);
//audio.play();
