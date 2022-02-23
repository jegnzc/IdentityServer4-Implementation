import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EnvironmentUrlService } from './environment-url.service';
import { AuthService } from '../../shared/services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {
  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService, private auth: AuthService) {


  }

  public getData = (route: string) => {
    console.log(this.auth.getAccessToken());
    var header = {
      headers: new HttpHeaders()
        .set('Authorization',  `Bearer ${this.auth.getAccessToken()}`)
    }
    console.log(header);
    console.log(this.auth.getAccessToken());
    console.log("matenme");
    return this.http.get(this.createCompleteRoute(route, this.envUrl.urlAddress), header);
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
