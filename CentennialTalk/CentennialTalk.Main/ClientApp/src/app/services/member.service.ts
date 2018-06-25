import { Injectable } from '@angular/core';
import { HttpModule, Http } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import 'rxjs/add/operator/map';

@Injectable()
export class MemberService {
  constructor(private http: Http) {
  }

  updateStatus(data) {
    return this.http.post("/api/member/status", data)
      .map(res => res.json());
  }
}
