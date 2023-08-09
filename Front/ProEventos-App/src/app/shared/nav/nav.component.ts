import { AccountService } from './../../services/account.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  isCollapsed = true;
  constructor(private router: Router,public accountService:AccountService) { }

  ngOnInit() {
  }
  logout():void {
    this.accountService.logout();
    this.router.navigateByUrl('/user/login')
  }
  shoMenu(): boolean {
    return this.router.url !== '/user/login';
  }
}
