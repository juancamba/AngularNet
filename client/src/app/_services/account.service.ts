import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:7015/api/'

  private currentUserSource = new ReplaySubject<User>(1);
  //observable, por convecion llevan $
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User)=>{
        const user = response;
        if(user){
          // almacena esto en formato json en el navegador ( de forma local)
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
    ;
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
