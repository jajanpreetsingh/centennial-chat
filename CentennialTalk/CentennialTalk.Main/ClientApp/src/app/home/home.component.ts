import { Component, OnInit } from '@angular/core';
import { FileService } from '../services/file.service';
import { Router } from '@angular/router';
import { AccountService, StorageKeys } from '../services/account.service';

declare var MediaRecorder: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  mediaRecorder: any;

  blob: any;

  constructor(private fileService: FileService, private router: Router,
    private accountService: AccountService) {
    this.accountService.setLocalData(StorageKeys.ChatUsername, '');
  }

  ngOnInit() {
    this.accountService.setLocalData(StorageKeys.ChatUsername, '');
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

          this.fileService.saveFile(audioBlob).subscribe(res => {
          });
        });
      });
  }

  handleBlob(event) {
    var arrayBuffer = event.target.result;

    this.fileService.saveFile(btoa(arrayBuffer)).subscribe(res => {
    });
  }

  stop() {
    this.mediaRecorder.stop();
  }

  goToGeneralLogin() {
    this.router.navigate(['/general-login']);
  }
}
//const audioUrl = URL.createObjectURL(audioBlob);
//var fileReader = new FileReader();
//fileReader.onload = this.handleBlob.bind(this);
//fileReader.readAsArrayBuffer(audioBlob);
//const audio = new Audio(audioUrl);
//audio.play();
