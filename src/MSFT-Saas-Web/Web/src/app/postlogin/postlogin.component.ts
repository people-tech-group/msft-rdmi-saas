import { Component, OnInit } from '@angular/core';
import { AdalService } from 'adal-angular4';
import { Router } from '@angular/router';

@Component({
  selector: 'app-postlogin',
  templateUrl: './postlogin.component.html',
  styleUrls: ['./postlogin.component.css']
})
export class PostloginComponent implements OnInit {

  constructor(private adalService: AdalService, private router: Router) { }

  ngOnInit() {
    this.adalService.handleWindowCallback();
    this.router.navigate(['/admin/Tenants']);
  }

}
