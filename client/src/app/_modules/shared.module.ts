import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import {TabsModule} from 'ngx-bootstrap/tabs'
import {NgxGalleryModule} from '@kolkov/ngx-gallery';
import {BsDatepickerModule} from  'ngx-bootstrap/datepicker'
import { PaginationModule } from 'ngx-bootstrap/pagination';
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    }),
    TabsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    NgxGalleryModule,
    PaginationModule.forRoot(),

  ],
  //agregarlos en export
  exports:[
    BsDropdownModule,
    ToastrModule,
    TabsModule,
    NgxGalleryModule,
    BsDatepickerModule,
    PaginationModule
  
    
  ]
})
export class SharedModule { }
