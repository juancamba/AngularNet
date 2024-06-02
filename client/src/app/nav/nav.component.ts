import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any ={}
  
  // como uso public el accoutservice, puedo acceder desde la vista a el con accountService.currentUser
  //currentUser$: Observable<User>;
  
  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }
  

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currenUser$;
  }

  login(){
    this.accountService.login(this.model).subscribe(response=>{
      // redireccionar
      this.router.navigateByUrl('/members')
      console.log(response)
      


    }, error=>{
      console.log(error)
      //this.toastr.error(error.error);
    })
    
    console.log(this.model)
  }
  logout(){

    this.accountService.logout();
    this.router.navigateByUrl('/')
  }
 

}
