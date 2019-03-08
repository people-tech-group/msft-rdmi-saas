import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Component } from '@angular/core';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'; //This is for Model driven form
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from "@angular/router";
import * as $ from 'jquery';
import { AdminMenuComponent } from './admin-menu/admin-menu.component';
import { DeploymentDashboardComponent } from './deployment-dashboard/deployment-dashboard.component';
import { AppService } from "./shared/app.service";
import { AuthGuard } from "./shared/AuthGuard.service";
import { TenantDashboardComponent } from './tenant-dashboard/tenant-dashboard.component';
import { HostpoolDashboardComponent } from './hostpool-dashboard/hostpool-dashboard.component';
import { LoadersCssModule } from 'angular2-loaders-css';
import { SimpleNotificationsModule, NotificationsService } from 'angular2-notifications';
import { SearchPipe } from "../assets/Pipes/Search.pipe";
import { BreadcrumComponent } from './breadcrum/breadcrum.component';
import { MyDatePickerModule } from 'mydatepicker';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { ClipboardModule } from 'ngx-clipboard';
import { ClickOutsideModule } from 'ng-click-outside';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdalService, AdalGuard, AdalInterceptor } from 'adal-angular4';
import { NgxPaginationModule } from 'ngx-pagination';
import { InvalidtokenmessageComponent } from './invalidtokenmessage/invalidtokenmessage.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { PostloginComponent } from './postlogin/postlogin.component';

/* getting redirectURL from Session storage */
var redirectUri = sessionStorage.getItem('redirectUri');

/* Here we are getting code from the loginurl and we are storing it in session storage */
if (window.location.href != redirectUri) {
  var sPageURL = window.location.search.substring(1);
  var codeInfo = sPageURL.split('&')[0].split('=')[1];
  if (codeInfo == "" || codeInfo == null || codeInfo == undefined) {
    var code = sessionStorage.getItem('Code');
    sessionStorage.setItem("Code", code);
  } else {
    sessionStorage.setItem("Code", codeInfo);
    sessionStorage.setItem('gotCode', 'yes');
  }
}

@NgModule({
  declarations: [
    AppComponent,
    AdminMenuComponent,
    DeploymentDashboardComponent,
    TenantDashboardComponent,
    HostpoolDashboardComponent,
    SearchPipe,
    BreadcrumComponent,
    InvalidtokenmessageComponent,
    PostloginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ClickOutsideModule,
    ReactiveFormsModule,
    HttpModule,
    HttpClientModule,
    LoadersCssModule,
    SimpleNotificationsModule,
    MyDatePickerModule,
    ClipboardModule,
    BrowserAnimationsModule,
    NgxPaginationModule,

    RouterModule.forRoot([
      {
        path: 'postlogin',
        component: PostloginComponent
      },
      {
        path: 'invalidtokenmessage',
        component: InvalidtokenmessageComponent,
        
      },
      {
        path: 'admin',
        component: AdminMenuComponent,
        canActivate: [AdalGuard],
        children: [
          {
            path: 'Tenants',
            component: DeploymentDashboardComponent,
            canActivate: [AdalGuard]
          },
          {
            path: 'tenantDashboard/:tenantName',
            component: TenantDashboardComponent,
            canActivate: [AdalGuard]
          },
          {
            path: 'hostpoolDashboard/:hostpoolName',
            component: HostpoolDashboardComponent,
            canActivate: [AdalGuard]
          },
        ]
      }
    ])
  ],
  providers: [AppService, AuthGuard, NotificationsService, BreadcrumComponent, AdalService, AdalGuard, { provide: HTTP_INTERCEPTORS, useClass: AdalInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule {

}
