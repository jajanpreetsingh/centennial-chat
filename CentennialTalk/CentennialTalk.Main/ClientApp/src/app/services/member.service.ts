import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
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
