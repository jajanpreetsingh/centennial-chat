import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class FileService {
  constructor(private http: Http) {
  }

  saveFile(blob) {
    const formData = new FormData();
    formData.append('speech', blob);
    return this.http.post("/api/file/save", formData)
      .map(res => res.json());
  }
}
