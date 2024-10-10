import { Injectable } from '@angular/core';
import Swal, { SweetAlertIcon } from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SwalService {

  constructor() { }

  callToast(title: string, icon: SweetAlertIcon){
    const Toast = Swal.mixin({
      toast: true,
      position: 'top-end',
      timer: 3000,
      timerProgressBar: true,
      showConfirmButton: false
    })
    Toast.fire(title, '', icon)
  }
  callSwal(title: string, cancelButtonName: string, confirmButtonName: string, callBack: ()=> void){
    Swal.fire({
      title: title,
      icon: 'question',
      showCancelButton: true,
      cancelButtonText: cancelButtonName,
      showConfirmButton: true,
      confirmButtonText: confirmButtonName
    }).then(res => {
      if(res.isConfirmed){
        callBack();
      }
    })
  }

  callSwal2(title: string, confirmButtonName: string, callBack: ()=> void){
    Swal.fire({
      title: title,
      icon: 'warning',
      showConfirmButton: true,
      confirmButtonText: confirmButtonName
    }).then(res => {
      if(res.isConfirmed){
        callBack();
      }
    })
  }

  callSwalErr(title: string) {
    Swal.fire({
      title: title,
      icon: 'error',
      timer: 3000,
      showConfirmButton: false
    });
  }

  callSwal3(title: string, confirmButtonName: string, callBack: ()=> void){
    Swal.fire({
      title: title,
      icon: 'error',
      timer: 3000,
      showConfirmButton: false,
    
      confirmButtonText: confirmButtonName
    }).then(res => {
      if(res.isDenied){
        callBack();
      }
    })
  }


}
