import { Component, Input, OnInit } from '@angular/core';

import { take } from 'rxjs/operators';
import { Photo } from 'src/app/_models/Photo';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: Member;


  user: User;

  constructor(private accountService: AccountService, private memberService: MembersService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user=user);

  }

  ngOnInit(): void {
   
  }

 
  setMainPhoto(photo: Photo)
  {
    this.memberService.setMainPhoto(photo.id).subscribe(()=>{

      this.user.photoUrl = photo.url;
      this.accountService.setCurrentUser(this.user);
      this.member.photoUrl = photo.url;
      this.member.photos.forEach(p=>{
        if(p.isMain){
          p.isMain = false;
        }
        if(p.id === photo.id){
          p.isMain = true;
        }
          
      })
    })
  }
  
  deletePhoto(photoId: number){
    this.memberService.deletePhoto(photoId).subscribe(()=>{
      this.member.photos = this.member.photos.filter(x=>x.id!= photoId);
    })
  }



}
