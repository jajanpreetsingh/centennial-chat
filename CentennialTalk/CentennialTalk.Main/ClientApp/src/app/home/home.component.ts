import { Component, OnInit } from '@angular/core';
import { NativeSpeechService } from '../services/native-speech.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {
  constructor(private nativeService: NativeSpeechService) { }

  ngOnInit() {

  }

  start() {
    this.nativeService.start();
  }

  stop() {
    this.nativeService.stop();
  }
}
