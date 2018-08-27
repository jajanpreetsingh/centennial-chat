import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class FileService {
  constructor(private http: Http) {
  }

  saveFile(audioUrl) {
    return this.http.post("/api/file/save", { 'value': audioUrl })
      .map(res => res.json());
  }
}
