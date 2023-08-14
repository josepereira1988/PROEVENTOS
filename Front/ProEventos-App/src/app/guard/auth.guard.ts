import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router:Router,private toaster:ToastrService){

  }
  canActivate()  : boolean{
      if(localStorage.getItem('user') != null){
        return true;
      }else{
        this.toaster.info('Usuario não autenticado!');
        this.router.navigate(['/user/login']);
        return false;
      }

  }

}
