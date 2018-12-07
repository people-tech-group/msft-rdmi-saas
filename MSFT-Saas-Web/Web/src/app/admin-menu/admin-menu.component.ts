import { Component, AfterViewInit } from '@angular/core';
import { Http, Headers } from '@angular/http'
import * as $ from 'jquery';
import { map } from "rxjs/operator/map";
import { AppService } from '../shared/app.service';
import { BreadcrumComponent } from '../breadcrum/breadcrum.component';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { NotificationsService } from "angular2-notifications";
import { AppComponent } from "../app.component";

@Component({
  selector: 'app-admin-menu',
  templateUrl: './admin-menu.component.html',
  styleUrls: ['./admin-menu.component.css']
})

export class AdminMenuComponent {
  public selectedDeployment: any;
  public selectedTenant: any;
  public selectedHostPool: any;
  public selectedAllTenants: boolean = true;
  public loadMore: boolean = false;
  public hostpool: any;
  public tenantName: any;
  public tenantList: any=[];
  public displayTenantList: any=[];
  public hostPoolList: any = [];
  public selectedHostpoolName: any;
  public initialIndex: any = 0;
  public tenantLength: any = 10;
  public storeLength: any = 0;
  public tenantListLength: any;
 // public tenantsList: any;
 // public getTenantsUrl: any;
 // public refreshToken: any;
 //public loading: boolean = false;
 //public showMore: boolean = true;
 // data: any = [];
  //loading: boolean = false;
 // showMore: boolean = true;
  constructor(private _AppService: AppService, private http: Http, private router: Router,
    private _notificationsService: NotificationsService, private route: ActivatedRoute, ) {
    if (this.tenantListLength == 1) {
      this.loadMore = false;
    }
    else {
      this.loadMore = true;
    }
  }

  /*
   * This function is used to redirect to Tenants component and empty the hostpool and tenants list in side menu
   * ----------
   * Parameters
   * data - Accepts Tenant List
   * ----------
   */
  public RedirectToTenants(data: any) {
    this.hostPoolList = [];
    this.tenantList = data;
    this.selectedTenant = null;
    this.selectedHostPool = null;
    this.router.navigate(['/admin/Tenants']);
  }
  //public loadMore() {
  //  this.refreshToken = sessionStorage.getItem("Refresh_Token");
  // this.loading = true;
  //  this.getTenantsUrl = this._AppService.ApiUrl + '/api/Tenant/GetAllTenants?refresh_token=' + this.refreshToken;
  //  this._AppService.GetTenants(this.getTenantsUrl).subscribe(response => {
  //    let responseObject = JSON.parse(response['_body']);
  //    this.tenantsList = responseObject;
  //    this.GetAllTenants(this.tenantsList);
  //    if (this.tenantsList.length > 0) {
  //      if (this.tenantsList[0]) {
  //        if (this.tenantsList[0].code == "Invalid Token") {
  //          sessionStorage.clear();
  //          this.router.navigate(['/invalidtokenmessage']);
  //        }
  //      }
  //    }
  //  },
     
  //  );

  //}

  /**
   * This function is used  to Load all the tenants list
   * ---------
   * Parameters
   * data - Accepts Tenant List
   * ---------
   */
  public GetAllTenants(data: any) {
    this.tenantList = [];
    this.hostPoolList = [];
    if (data == undefined || data == null) {
      this.router.navigate(['/admin/Tenants']);
    }
    else {
      this.displayTenantList = [];
      this.initialIndex = 0;
      this.tenantLength = 10;
      this.storeLength = 0;
      this.tenantList = data;
      this.tenantListLength = data.length;
      this.FilterData(data);
      this.hostPoolList = [];
      this.selectedTenant = null;
      this.selectedAllTenants = true;
    }
  }

  public FilterData(data: any) {
    this.loadMore = true;
    if (this.storeLength <= this.tenantListLength) {
      var index = this.initialIndex;
      this.storeLength = this.tenantLength + this.initialIndex;
      for (index; index < this.storeLength; index++) {
        if (index < this.tenantListLength) {
          this.displayTenantList.push(this.tenantList[index]);
        }
        else {
          this.loadMore = false;
          continue;
        }
      }
      this.initialIndex = index;
    }
  }

  /*
   * This function is used  to  Hightlight the Selected Tenant
   * ----------
   * Parameters
   * index - Accepts Teant Index value
   * tenantName - Accepts Tenant Name
   * ----------
   */
  public SetSelectedTenant(index: any, tenantName: any) {
    if (index == null && tenantName == '') {
      this.router.navigate(['/admin/Tenants']);
    }
    else {
      this.selectedTenant = index;
      this.selectedHostPool = null;
      this.selectedAllTenants = false;
      sessionStorage.setItem("TenantName", tenantName);
    }
  }

  /*
   * This function is used  to  get the list of hostpools
   * ----------
   * Parameters
   * hostpoolData - Accepts Hostpool List
   * tenantName -  Accepts Tenant Name
   * ----------
   */
  public GetHostpools(hostpoolData: any, tenantName: any) {
    this.hostPoolList = [];
    this.hostPoolList = hostpoolData;
    $(".sub-sub-group-list").addClass("collapse");
    $(".sub-sub-group-list").eq(this.selectedTenant).removeClass("collapse");
    $(".sub-group-list li input[type='checkbox']").prop("checked", "");
    $("#sub-group-" + tenantName).prop("checked", true);
    let data = [{
      name: tenantName,
      type: 'Tenant',
      path: 'tenantDashboard',
    }];
    BreadcrumComponent.GetCurrentPage(data);
  }

  /*
   * This function is used to trigger  On select of Hostpool
   * ----------
   * Parameters
   * index - Accepts Hostpool List
   * tenantName -  Accepts Tenant Name
   * hostpoolName - Accepts Hostpool Name
   * ----------
   */
  public SetSelectedhostPool(index: any, tenantName: any, hostpoolName: any) {
    this.selectedHostPool = index;
    this.selectedHostpoolName = hostpoolName;
    sessionStorage.setItem('selectedhostpoolname', this.selectedHostpoolName);
    let data = [{
      name: hostpoolName,
      type: 'Hostpool',
      path: 'hostpoolDashboard',
      TenantName: tenantName,
    }];
    BreadcrumComponent.GetCurrentPage(data);
    this.selectedAllTenants = false;
  }
}
