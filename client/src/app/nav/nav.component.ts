import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any ={}
  
  // como uso public el accoutservice, puedo acceder desde la vista a el con accountService.currentUser
  //currentUser$: Observable<User>;
  
  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currenUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe(response=>{
      console.log(response)
      


    }, error=>{
      console.log(error)
    })
    
    console.log(this.model)
  }
  logout(){
    this.accountService.logout();
   
  }
 

}
