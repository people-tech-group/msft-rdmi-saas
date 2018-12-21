import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms'; //This is for Model driven form
import { Router, ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { AppService } from '../shared/app.service';
import { NotificationsService } from "angular2-notifications";
import { SearchPipe } from "../../assets/Pipes/Search.pipe";
import { AppComponent } from "../app.component";
import { BreadcrumComponent } from "../breadcrum/breadcrum.component";
import { AdminMenuComponent } from "../admin-menu/admin-menu.component";


@Component({
  selector: 'app-deployment-dashboard',
  templateUrl: './deployment-dashboard.component.html',
  styleUrls: ['./deployment-dashboard.component.css'],
  animations: []
})

export class DeploymentDashboardComponent implements OnInit {
  public previousPageNo: any = 1;
  public currentPageNo: any = 1;
  public nextPageNo: any = 1;
  public pageSize: any = 10;
  public tenantsCount: any = 0;
  public initialSkip: any = 0;
  public curentIndex: any;
  public currentNoOfPagesCount: any = 1;
  public ListOfPages: any = [];
  public lastEntry: any = '';
  public listItems: any = 10;
  public scopeArray: any;
  public arcount: any = [];
  public isPrevious: boolean = false;
  public isNext: boolean = false;
  public hasError: boolean = false;
  public isDescending: boolean = false;
  public createTenantUniqueName: boolean = false;
  public createTenantId: boolean = false;
  public tenantNextButtonDisable: boolean = true;
  public tenantDoneButtonDisable: boolean = true;
  public refreshTenantLoading: any = false;
  public searchTenants: any = [];
  private sub: any;
  public editedBody: boolean = false;
  public tenantsList: any;
  public tenants: any;
  public tenantDeleteUrl: any;
  public tenantUrl: any;
  public getTenantsUrl: any;
  public updateTenantUrl: any;
  public aadTenantId: any;
  public checked: any = [];
  public checkedMain: boolean;
  public checkedTrue: any = [];
  public checkedAllTrue: any = [];
  public selectedRows: any = [];
  public selectedTenantName: any;
  public showCreateTenant: any;
  public showTenantTab2: boolean;
  public showTenantDialog: boolean = false;
  public isEditDisabled: boolean = true;
  public isDeleteDisabled: boolean = true;
  public tenantlistErrorFound: boolean = false;
  public tenantGroupName: any;
  public deleteCount: any;
  public refreshToken: any;
  public options: any = {
    timeOut: 2000,
    position: ["top", "right"]
  };
  tenantForm;
  tenantFormEdit;
  deploymentID = 0;
  @ViewChild('editclose') editclose: ElementRef;


  /*This  is used to close the edit modal popup*/
  public tenantUpdateClose(): void {
    this.editclose.nativeElement.click();
  }

  constructor(private _AppService: AppService, private _notificationsService: NotificationsService, private router: Router,
    private adminMenuComponent: AdminMenuComponent) {
  }

  /* This function is  called directly on page load */
  public ngOnInit() {
    this.hasError = true;
    this.adminMenuComponent.SetSelectedTenant(null, '');
    let data = [{
      name: 'Tenants',
      type: 'Tenants',
      path: 'Tenants',
    }];
    BreadcrumComponent.GetCurrentPage(data);
    this.tenantForm = new FormGroup({
      id: new FormControl("00000000-0000-0000-0000-000000000000"),
      aadTenantId: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$/)])),
      tenantName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\-\_\.])+$/)])),
      friendlyName: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\.\-\_])+$/)])),
      description: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[\dA-Za-z]+[\dA-Za-z\s\.\-\_\!\@\#\$\%\^\&\*\(\)\{\}\[\]\:\'\"\?\>\<\,\;\/\\\+\=\|]{0,1600}$/)])),
    });
    this.tenantFormEdit = new FormGroup({
      id: new FormControl("00000000-0000-0000-0000-000000000000"),
      aadTenantId: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$/)])),
      tenantName: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\.\-\_])+$/)])),
      friendlyName: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\.\-\_])+$/)])),
      description: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[\dA-Za-z]+[\dA-Za-z\s\.\-\_\!\@\#\$\%\^\&\*\(\)\{\}\[\]\:\'\"\?\>\<\,\;\/\\\+\=\|]{0,1600}$/)])),
    });
    this.CheckTenantAccess();
  }

  /*
   * This Function is called on Component Load and it is used to check the Access level of Tenant 
   */
  public CheckTenantAccess() {
    this.tenantGroupName = sessionStorage.getItem("TenantGroupName");
    this.scopeArray = localStorage.getItem("Scope").split(",");
    if (this.scopeArray != null && this.scopeArray.length > 2) {
      this.tenants = [{
        "id": "",
        "tenantGroupName": "",
        "aadTenantId": "",
        "tenantName": this.scopeArray[1],
        "description": "",
        "friendlyName": "",
        "ssoAdfsAuthority": "",
        "ssoClientId": "",
        "ssoClientSecret": "",
        "noOfHostpool": "",
        "noOfActivehosts": "",
        "noOfAppgroups": "",
        "noOfUsers": "",
        "noOfSessions": "",
        "code": null,
        "refresh_token": null
      }];
      this.searchTenants = this.tenants;
    }
    else {
      this.GetTenants();
      this.GetAllTenantsList();
    }
  }

  /* This function is used to show validation error messages  on change of Azure AD Tenant ID
   * --------------------
   * paremeters-
   * value : Accepts the textbox value.
   * --------------------
  */
  public TenantChange(value) {
    this.aadTenantId = value;
    if (value != "") {
      this.createTenantId = false;
      this.tenantNextButtonDisable = false;
    } else {
      this.createTenantId = true;
      this.tenantNextButtonDisable = true;
    }
  }

  /* This function is used to show validation error messages  on change of Tenant unique name
   * -----------
   * paremeters-
   * value : Accepts the textbox value.
   * ----------
  */
  public TenantNameChange(value) {
    if (value != "") {
      this.createTenantUniqueName = false;
      this.tenantDoneButtonDisable = false;
    } else {
      this.createTenantUniqueName = true;
      this.tenantDoneButtonDisable = true;
    }
  }

  /* This function is called on Click of Create Tenant and it clears the input fields
  * --------------
   * paremeters-
   * event - Accepts Event.
   * -------------
  */
  public OpenCreateTenant(event: any) {
    this.tenantForm = new FormGroup({
      id: new FormControl("00000000-0000-0000-0000-000000000000"),
      aadTenantId: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[a-z0-9]{8}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{4}-[a-z0-9]{12}$/)])),
      tenantName: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\-\_\.])+$/)])),
      friendlyName: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\.\-\_])+$/)])),
      description: new FormControl("", Validators.compose([Validators.required, Validators.pattern(/^[\dA-Za-z]+[\dA-Za-z\s\.\-\_\!\@\#\$\%\^\&\*\(\)\{\}\[\]\:\'\"\?\>\<\,\;\/\\\+\=\|]{0,1600}$/)])),
    });
    this.createTenantId = false;
    this.tenantNextButtonDisable = true;
    this.showTenantDialog = true;
  }

  /* This function is called  to close the create tenant modal slide dialog
   * --------------
   * paremeters-
   * event - Accepts event.
   * -------------
  */
  public CreateTenantSlideClose(event: any) {
    event.preventDefault();
    this.CloseCreateTenant(event);
    this.ShowPreviousTab();
  }

  /* This function is used to close the create tenat modal dialog for cancel button
   * --------------
   * paremeters-
   * event - Accepts event.
   * -------------
  */
  public CloseCreateTenant(event: any) {
    this.showTenantDialog = false;
  }

  /* This function is used to show the next slide in create tenant dialog
   * --------------
   * paremeters-
   * deploymentUrlData - Accepts event.
   * -------------
  */
  public ShowNextTab(deploymentUrlData: any) {
    this.createTenantUniqueName = false;
    this.showTenantTab2 = true;
  }

  /* This function is used to show the previous slide in create tenant dialog */
  public ShowPreviousTab() {
    this.showTenantTab2 = false;
  }

  /*This function is used select all the Tenants using checkbox
   * --------------
   * paremeters-
   * event - Accepts event.
   * -------------
   */
  public CheckAll(event: any) {
    this.checkedMain = !this.checkedMain;
    this.checkedTrue = [];
    /* If we check the checkbox then this block of code executes*/
    for (let i = 0; i < this.searchTenants.length; i++) {
      if (event.target.checked) {
        this.checked[i] = true;
      }
      /* If we uncheck the checkbox then this block of code executes*/
      else {
        this.checked[i] = false;
      }
    }
    /* If we check the multiple checkboxes then this block of code executes*/
    this.checkedAllTrue = [];
    for (let j = 0; j < this.checked.length; j++) {
      if (this.checked[j] == true) {
        this.checkedAllTrue.push(this.checked[j]);
        this.selectedRows.push(j);
        var index = j;
      }
    }
    /*If the selected checkbox length=1 then this block of code executes to show the selected tenant name */
    if (this.checkedAllTrue.length == 1) {
      this.isEditDisabled = false;
      this.isDeleteDisabled = false;
      this.tenantFormEdit = new FormGroup({
        id: new FormControl("00000000-0000-0000-0000-000000000000"),
        aadTenantId: new FormControl(this.searchTenants[index].aadTenantId),
        tenantName: new FormControl(this.searchTenants[index].tenantName, Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\-\_\.])+$/)])),
        friendlyName: new FormControl(this.searchTenants[index].friendlyName),
        description: new FormControl(this.searchTenants[index].description),
      });
      this.deleteCount = this.searchTenants[index].tenantName;
    }
    /*If the selected checkbox length>1 then this block of code executes to show the no of selected tenants(i.e; if we select multiple checkboxes) */
    else if (this.checkedAllTrue.length > 1) {
      this.isEditDisabled = true;
      this.isDeleteDisabled = false;
      this.deleteCount = this.checkedAllTrue.length;
    }
    else {
      this.isEditDisabled = true;
      this.isDeleteDisabled = true;
    }
  }

  /* This function is used to  select single/Multiple the Tenants using checkbox
    * --------------
   * Parameters
   * ind - Accepts index number of selected checkbox .
   * --------------
  */
  public IsChecked(ind: any) {
    this.checked[ind] = !this.checked[ind];
    this.checkedTrue = [];
    for (let i = 0; i < this.checked.length; i++) {
      if (this.checked[i] == true) {
        this.checkedTrue.push(this.checked[i]);
      }
      if (this.checked[i] == false) {
        this.checkedMain = false;
        break;
      }
      else {
        if (this.searchTenants.length == this.checkedTrue.length) {
          this.checkedMain = true;
          break;
        }
      }
    }
  }

  /* This function that triggers on click of  Tenants table row click
   * --------------
   * Parameters
   * tenantAadId - Accepts selected tenantId
   * tenantName - Accepts selected tenant name
   * ind - Accepts User Index 
   * --------------
 */
  public TenantRowClicked(tenantAadId: any, tenantName: any, ind: any) {
    this.selectedTenantName = tenantName;
    this.IsChecked(ind);
    this.aadTenantId = tenantAadId;
    this.checkedTrue = [];
    this.selectedRows = [];
    var index;
    for (var i = 0; i < this.checked.length; i++) {
      if (this.checked[i] == true) {
        this.checkedTrue.push(this.checked[i]);
        this.selectedRows.push(i);
        index = i;
      }
    }
    if (this.checkedTrue.length >= 1) {
      if (this.checkedTrue.length == 1) {
        this.deleteCount = this.searchTenants[index].tenantName;
        this.isEditDisabled = false;
        this.isDeleteDisabled = false;
        this.tenantFormEdit = new FormGroup({
          id: new FormControl("00000000-0000-0000-0000-000000000000"),
          aadTenantId: new FormControl(this.searchTenants[index].aadTenantId),
          tenantName: new FormControl(this.searchTenants[index].tenantName, Validators.compose([Validators.required, Validators.maxLength(36), Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\-\_\.])+$/)])),
          friendlyName: new FormControl(this.searchTenants[index].friendlyName, Validators.compose([Validators.required, Validators.pattern(/^[^\s\W\_]([A-Za-z0-9\s\.\-\_])+$/)])),
          description: new FormControl(this.searchTenants[index].description, Validators.compose([Validators.required, Validators.pattern(/^[\dA-Za-z]+[\dA-Za-z\s\.\-\_\!\@\#\$\%\^\&\*\(\)\{\}\[\]\:\'\"\?\>\<\,\;\/\\\+\=\|]{0,1600}$/)])),
        });
      }
      else if (this.checkedTrue.length > 1) {
        this.isDeleteDisabled = false;
        this.isEditDisabled = true;
        this.deleteCount = this.checkedTrue.length;
      }
      else {
        this.isDeleteDisabled = true;
        this.isEditDisabled = true;
      }
    }
    else if (this.checkedTrue.length == this.searchTenants.length) {
      this.checkedMain = true;
    }
    else if (this.checkedTrue.length < 1) {
      this.checkedTrue = [];
      this.selectedRows = [];
      this.deleteCount = this.checkedTrue.length;
      this.checkedMain = false;
      this.isDeleteDisabled = true;
      this.isEditDisabled = true;
    }
  }

  /* This function gets all the selected tenant data
   * --------------
   * Parameters
   * index - Accepts User Index of selected tenant .
   * TenantName- Accepts tenantname .
   * --------------
  */
  public SetSelectedTenant(index: any, TenantName: any) {
    sessionStorage.setItem("TenantName", TenantName);
    sessionStorage.setItem("TenantNameIndex", index);
    this.adminMenuComponent.SetSelectedTenant(index, TenantName);
    this.router.navigate(['/admin/tenantDashboard/', TenantName]);
    let data = [{
      name: TenantName,
      type: 'Tenant',
      path: 'tenantDashboard',
    }];
    BreadcrumComponent.GetCurrentPage(data);
  }

  /* This function is used Search functonality from the tenant table
   * --------------
   * Parameters
   * query - Accepts the searchbox text value .
   * --------------
   */
  public getSearchByTenantName(query: any) {
    let _SearchPipe = new SearchPipe();
    this.searchTenants = _SearchPipe.transform(query, 'tenantName', 'friendlyName', 'description', this.tenants);
  }

  /* This function is used to create an  array of current page numbers */
  public counter(i: number) {
    return new Array(i);
  }

  /* This function is used to  divide the number of pages based on Tenants Count */
  public GetcurrentNoOfPagesCount() {
    let currentNoOfPagesCountCount = Math.floor(this.tenantsCount / this.pageSize);
    let remaingCount = this.tenantsCount % this.pageSize;
    if (remaingCount > 0) {
      this.currentNoOfPagesCount = currentNoOfPagesCountCount + 1;
    }
    else {
      this.currentNoOfPagesCount = currentNoOfPagesCountCount;
    }
    this.curentIndex = 0;
  }

  public GetAllTenantsList() {
    this.tenantGroupName = sessionStorage.getItem("TenantGroupName");
    this.refreshToken = sessionStorage.getItem("Refresh_Token");
    this.refreshTenantLoading = true;
    this.getTenantsUrl = this._AppService.ApiUrl + '/api/Tenant/GetAllTenants?tenantGroupName=' + this.tenantGroupName +'&refresh_token=' + this.refreshToken;
    this._AppService.GetTenants(this.getTenantsUrl).subscribe(response => {
      let responseObject = JSON.parse(response['_body']);
      this.tenantsList = responseObject;
      this.adminMenuComponent.GetAllTenants(this.tenantsList);
      if (this.tenantsList.length > 0) {
        if (this.tenantsList[0]) {
          if (this.tenantsList[0].code == "Invalid Token") {
            sessionStorage.clear();
            this.router.navigate(['/invalidtokenmessage']);
          }
        }
      }
    },
      error => {
        this.refreshTenantLoading = false;
      }
    );
  }

  /* This function is used to  loads all the tenants into table on page load */
  public GetTenants() {
    this.tenantGroupName = sessionStorage.getItem("TenantGroupName");
    this.refreshToken = sessionStorage.getItem("Refresh_Token");
    this.refreshTenantLoading = true;
    this.tenantlistErrorFound = false;
    this.getTenantsUrl = this._AppService.ApiUrl + '/api/Tenant/GetTenantList?tenantGroupName=' + this.tenantGroupName +'&refresh_token=' + this.refreshToken + '&pageSize=' + this.pageSize + '&sortField=TenantName&isDescending=false&initialSkip=' + this.initialSkip + '&lastEntry=' + this.lastEntry;
    this._AppService.GetTenants(this.getTenantsUrl).subscribe(response => {
      let responseObject = JSON.parse(response['_body']);
      this.tenants = responseObject.rdMgmtTenants;
      this.tenantsCount = responseObject.count;
      this.refreshTenantLoading = false;
      //this.lastEntry = responseObject.lastEntry;
      if (this.tenants[0]) {
        if (this.tenants[0].code == "Invalid Token") {
          sessionStorage.clear();
          this.router.navigate(['/invalidtokenmessage']);
        }
      }
      this.GetcurrentNoOfPagesCount();
      this.searchTenants = this.tenants;
      //this.adminMenuComponent.GetAllTenants(this.tenants);
      if (this.searchTenants.length == 0) {
        this.editedBody = true;
        this.showCreateTenant = true;
        this.tenantlistErrorFound = false;
      }
      else {
        if (this.searchTenants[0].Message == null) {
          this.editedBody = false;
          this.showCreateTenant = false;
          this.tenantlistErrorFound = false;
        }
      }

    },
      /*
       * If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Execute
       */
      error => {
        this.editedBody = false;
        this.tenantlistErrorFound = true;
        this.refreshTenantLoading = false;
      }
    );
    this.isEditDisabled = true;
    this.isDeleteDisabled = true;
    for (let i = 0; i < this.searchTenants.length; i++) {
      this.checked[i] = false;
    }
    this.checkedMain = false;

  };

  /* This function is used to  loads all the tenants into table on click of Previous button in the table */
  public previousPage() {
    this.refreshToken = sessionStorage.getItem("Refresh_Token");
    this.refreshTenantLoading = true;
    this.tenantlistErrorFound = false;
    this.lastEntry = this.searchTenants[0].tenantName;
    this.curentIndex = this.curentIndex - 1;
    this.getTenantsUrl = this._AppService.ApiUrl + '/api/Tenant/GetTenantList?tenantGroupName=' + this.tenantGroupName +'&refresh_token=' + this.refreshToken + '&pageSize=' + this.pageSize + '&sortField=TenantName&isDescending=true&initialSkip=' + this.initialSkip + '&lastEntry=' + this.searchTenants[0].tenantName;
    this._AppService.GetTenants(this.getTenantsUrl).subscribe(response => {
      let responseObject = JSON.parse(response['_body']);
      this.tenants = responseObject.rdMgmtTenants.reverse();
      this.tenantsCount = responseObject.count;
      this.previousPageNo = this.currentPageNo;
      this.currentPageNo = this.currentPageNo - 1;
      if (this.tenants[0]) {
        if (this.tenants[0].code == "Invalid Token") {
          sessionStorage.clear();
          this.router.navigate(['/invalidtokenmessage']);
        }
      }
      this.searchTenants = this.tenants;
      if (this.searchTenants.length == 0) {
        this.editedBody = true;
        this.showCreateTenant = true;
        this.tenantlistErrorFound = false;
      }
      else {
        if (this.searchTenants[0].Message == null) {
          this.editedBody = false;
          this.showCreateTenant = false;
          this.tenantlistErrorFound = false;
        }
      }
      this.refreshTenantLoading = false;
    },
      /*
       * If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Execute
       */
      error => {
        this.editedBody = false;
        this.tenantlistErrorFound = true;
        this.refreshTenantLoading = false;
      }
    );
    this.isEditDisabled = true;
    this.isDeleteDisabled = true;
    for (let i = 0; i < this.searchTenants.length; i++) {
      this.checked[i] = false;
    }
    this.checkedMain = false;
  }

  /* This function is used to  loads all the tenants into table on click of Current page number values  in the table
    * ---------
   * Parameters
   * index - Accepts current index  count 
   * ---------
   */
  public CurrentPage(index) {
    this.previousPageNo = this.currentPageNo;
    this.currentPageNo = index + 1;
    this.curentIndex = index;
    this.refreshToken = sessionStorage.getItem("Refresh_Token");
    this.refreshTenantLoading = true;
    this.tenantlistErrorFound = false;
    let diff = this.currentPageNo - this.previousPageNo;
    // to get intialskip
    if (this.currentPageNo >= this.previousPageNo) {
      this.isDescending = false;
      this.pageSize = diff * this.pageSize;
      this.lastEntry = this.searchTenants[this.searchTenants.length - 1].tenantName;
    } else {
      this.isDescending = true;
      this.lastEntry = this.searchTenants[0].tenantName;
    }
    this.getTenantsUrl = this._AppService.ApiUrl + '/api/Tenant/GetTenantList?tenantGroupName=' + this.tenantGroupName +'&refresh_token=' + this.refreshToken + '&pageSize=' + this.pageSize + '&sortField=TenantName&isDescending=' + this.isDescending + '&initialSkip=' + this.initialSkip + '&lastEntry=' + this.lastEntry;
    console.log(this.getTenantsUrl);
    this._AppService.GetTenants(this.getTenantsUrl).subscribe(response => {
      let responseObject = JSON.parse(response['_body']);
      this.tenants = responseObject.rdMgmtTenants; //.splice(0, 3)
      this.tenantsCount = responseObject.count;
      if (this.tenants[0]) {
        if (this.tenants[0].code == "Invalid Token") {
          sessionStorage.clear();
          this.router.navigate(['/invalidtokenmessage']);
        }
      }
      this.searchTenants = this.tenants;
      if (this.searchTenants.length == 0) {
        this.editedBody = true;
        this.showCreateTenant = true;
        this.tenantlistErrorFound = false;
      }
      else {
        if (this.searchTenants[0].Message == null) {
          this.editedBody = false;
          this.showCreateTenant = false;
          this.tenantlistErrorFound = false;
        }
      }
      this.refreshTenantLoading = false;
    },
      /*
       * If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Execute
       */
      error => {
        this.editedBody = false;
        this.tenantlistErrorFound = true;
        this.refreshTenantLoading = false;
      }
    );
    this.isEditDisabled = true;
    this.isDeleteDisabled = true;
    for (let i = 0; i < this.searchTenants.length; i++) {
      this.checked[i] = false;
    }
    this.checkedMain = false;
  }

  /* This function is used to  loads all the tenants into table on click of Next button in the table */
  public NextPage() {
    this.refreshToken = sessionStorage.getItem("Refresh_Token");
    this.refreshTenantLoading = true;
    this.tenantlistErrorFound = false;
    this.lastEntry = this.searchTenants[this.searchTenants.length - 1].tenantName;
    this.curentIndex = this.curentIndex + 1;
    this.getTenantsUrl = this._AppService.ApiUrl + '/api/Tenant/GetTenantList?tenantGroupName=' + this.tenantGroupName +'&refresh_token=' + this.refreshToken + '&pageSize=' + this.pageSize + '&sortField=TenantName&isDescending=false&initialSkip=' + this.initialSkip + '&lastEntry=' + this.lastEntry;
    this._AppService.GetTenants(this.getTenantsUrl).subscribe(response => {
      let responseObject = JSON.parse(response['_body']);
      this.tenants = responseObject.rdMgmtTenants;
      this.tenantsCount = responseObject.count;
      this.previousPageNo = this.currentPageNo;
      this.currentPageNo = this.currentPageNo + 1;
      if (this.tenants[0]) {
        if (this.tenants[0].code == "Invalid Token") {
          sessionStorage.clear();
          this.router.navigate(['/invalidtokenmessage']);
        }
      }
      this.searchTenants = this.tenants;
      if (this.searchTenants.length == 0) {
        this.editedBody = true;
        this.showCreateTenant = true;
        this.tenantlistErrorFound = false;
      }
      else {
        if (this.searchTenants[0].Message == null) {
          this.editedBody = false;
          this.showCreateTenant = false;
          this.tenantlistErrorFound = false;
        }
      }
      this.refreshTenantLoading = false;
    },
      /*
       * If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Execute
       */
      error => {
        this.editedBody = false;
        this.tenantlistErrorFound = true;
        this.refreshTenantLoading = false;
      }
    );
    this.isEditDisabled = true;
    this.isDeleteDisabled = true;
    for (let i = 0; i < this.searchTenants.length; i++) {
      this.checked[i] = false;
    }
    this.checkedMain = false;
  };

  /* This function is used to check Tenant Access and refresh the tenants list */
  public RefreshTenant() {
    this.CheckTenantAccess();
  }

  /* This function is used to  Create Tenant using the required details
   * --------------
   * Parameters
   * tenantData - Accepts details of tenant Form.
   * --------------
  */
  public CreateTenant(tenantData) {
    this.refreshTenantLoading = true;
    var newTenantData = {
      refresh_token: sessionStorage.getItem("Refresh_Token"),
      tenantGroupName: sessionStorage.getItem("TenantGroupName"),
      aadTenantId: this.aadTenantId,
      id: tenantData.id,
      tenantName: tenantData.tenantName.trim(),
      friendlyName: tenantData.friendlyName.trim(),
      description: tenantData.description.trim(),
    }
    this.tenantUrl = this._AppService.ApiUrl + '/api/Tenant/Post';
    this._AppService.CreateTenant(this.tenantUrl, newTenantData).subscribe(response => {
      this.refreshTenantLoading = false;
      this.ShowPreviousTab();
      var responseData = JSON.parse(response['_body']);
      if (responseData.message == "Invalid Token") {
        sessionStorage.clear();
        this.router.navigate(['/invalidtokenmessage']);
      }
      /* If response data is success then it enters into if and this block of code will execute to show the 'Tenant Created Successfully' notification */
      if (responseData.isSuccess === true) {
        this._notificationsService.html(
          '<i class="icon icon-check angular-Notify col-xs-1 no-pad"></i>' +
          '<label class="notify-label col-xs-10 no-pad">Tenant Created Successfully</label>' +
          '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
          '<p class="notify-text col-xs-12 no-pad">' + responseData.message + '</p>',
          'content optional one',
          {
            position: ["top", "right"],
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: false,
            clickToClose: true,
            maxLength: 10
          }
        )
        AppComponent.GetNotification('icon icon-check angular-Notify', 'Tenant Created Successfully', responseData.message, new Date());
        this.CloseCreateTenant(event);
        this.RefreshTenant();
      }
      /* If response data is success then it enters into else and this block of code will execute to show the 'Failed To Create Tenant' notification */
      else {
        this.CloseCreateTenant(event);
        this.ShowPreviousTab();
        this._notificationsService.html(
          '<i class="icon icon-fail angular-NotifyFail col-xs-1 no-pad"></i>' +
          '<label class="notify-label col-xs-10 no-pad">Failed To Create Tenant</label>' +
          '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
          '<p class="notify-text col-xs-12 no-pad">' + responseData.message + '</p>',
          'content optional one',
          {
            position: ["top", "right"],
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: false,
            clickToClose: true,
            maxLength: 10
          }
        )
        AppComponent.GetNotification('icon icon-fail angular-NotifyFail', 'Failed To Create Tenant', responseData.message, new Date());
        this.RefreshTenant();
      }
    },
      /* If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Exequte */
      error => {
        this.refreshTenantLoading = false;
        this._notificationsService.html(
          '<i class="icon icon-close angular-NotifyFail"></i>' +
          '<label class="notify-label padleftright">Failed To Create Tenant</label>' +
          '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
          '<p class="notify-text">Problem with the service. Please try later</p>',
          'content optional one',
          {
            position: ["top", "right"],
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: false,
            clickToClose: true,
            maxLength: 10
          }
        )
        AppComponent.GetNotification('fa fa-times-circle checkstyle', 'Failed To Create Tenant', 'Problem with the service. Please try later', new Date());
      }
    );

    this.tenantForm = new FormGroup({
      id: new FormControl("00000000-0000-0000-0000-000000000000"),
      aadTenantId: new FormControl(""),
      tenantName: new FormControl(""),
      friendlyName: new FormControl(""),
      description: new FormControl(""),
    });
  }

  /* This function is used to  Update/Edit single selected Tenant
   * --------------
   * Parameters
   * tenantData - Accepts details of tenant Form Edit.
   * --------------
   */
  public UpdateTenant(tenantData: any) {
    var updateArray = {
      "refresh_token": sessionStorage.getItem("Refresh_Token"),
      "tenantGroupName": sessionStorage.getItem("TenantGroupName"),
      "tenantName": tenantData.tenantName,
      "friendlyName": tenantData.friendlyName,
      "description": tenantData.description,
      "aadTenantId": this.aadTenantId,
      "id": "00000000-0000-0000-0000-000000000000",
    };
    this.refreshTenantLoading = true;
    this.updateTenantUrl = this._AppService.ApiUrl + '/api/Tenant/Put';
    this._AppService.UpdateTenant(this.updateTenantUrl, updateArray).subscribe(response => {
      this.refreshTenantLoading = false;
      var responseData = JSON.parse(response['_body']);
      if (responseData.message == "Invalid Token") {
        sessionStorage.clear();
        this.router.navigate(['/invalidtokenmessage']);
      }
      /* If response data is success then it enters into if and this block of code will execute to show the 'Tenant Updated Successfully' notification */
      if (responseData.isSuccess === true) {
        this._notificationsService.html(
          '<i class="icon icon-check angular-Notify col-xs-1 no-pad"></i>' +
          '<label class="notify-label col-xs-10 no-pad">Tenant Updated Successfully</label>' +
          '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
          '<p class="notify-text col-xs-12 no-pad">' + responseData.message + '</p>',
          'content optional one',
          {
            position: ["top", "right"],
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: false,
            clickToClose: true,
            maxLength: 10
          }
        )
        AppComponent.GetNotification('icon icon-check angular-Notify', 'Tenant Updated Successfully', responseData.message, new Date());
        this.tenantUpdateClose();
        this.RefreshTenant();
      }
      /* If response data is success then it enters into else and this block of code will execute to show the 'Failed To Update Tenant' notification */
      else {
        this._notificationsService.html(
          '<i class="icon icon-fail angular-NotifyFail col-xs-1 no-pad"></i>' +
          '<label class="notify-label col-xs-10 no-pad">Failed To Update Tenant</label>' +
          '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
          '<p class="notify-text col-xs-12 no-pad">' + responseData.message + '</p>',
          'content optional one',
          {
            position: ["top", "right"],
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: false,
            clickToClose: true,
            maxLength: 10
          }
        )
        AppComponent.GetNotification('icon icon-fail angular-NotifyFail', 'Failed To Update Tenant', responseData.message, new Date());
        this.tenantUpdateClose();
        this.RefreshTenant();
      }
    },
      /* If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Exequte */
      error => {
        this.refreshTenantLoading = false;
        this._notificationsService.html(
          '<i class="icon icon-close angular-NotifyFail"></i>' +
          '<label class="notify-label padleftright">Failed To Update Tenant</label>' +
          '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
          '<p class="notify-text">Problem with the service. Please try later</p>',
          'content optional one',
          {
            position: ["top", "right"],
            timeOut: 3000,
            showProgressBar: false,
            pauseOnHover: false,
            clickToClose: true,
            maxLength: 10
          }
        )
        AppComponent.GetNotification('fa fa-times-circle checkstyle', 'Failed To Update Tenant', 'Problem with the service. Please try later', new Date());
      }
    );

  }

  /* This function is used to  delete the selected Tenant */
  public DeleteTenant() {
    this.refreshTenantLoading = true;
    for (let i = 0; i < this.selectedRows.length; i++) {
      let index = this.selectedRows[i];
      this.tenantDeleteUrl = this._AppService.ApiUrl + '/api/Tenant/Delete?tenantGroupName=' + this.tenantGroupName +'&tenantName=' + this.searchTenants[index].tenantName + '&refresh_token=' + sessionStorage.getItem("Refresh_Token");
      this._AppService.DeleteTenantService(this.tenantDeleteUrl).subscribe(response => {
        this.refreshTenantLoading = false;
        var responseData = JSON.parse(response['_body']);
        if (responseData.message == "Invalid Token") {
          sessionStorage.clear();
          this.router.navigate(['/invalidtokenmessage']);
        }
        /* If response data is success then it enters into if and this block of code will execute to show the 'Tenant Deleted Successfully' notification */
        if (responseData.isSuccess === true) {
          this._notificationsService.html(
            '<i class="icon icon-check angular-Notify col-xs-1 no-pad"></i>' +
            '<label class="notify-label col-xs-10 no-pad">Tenant Deleted Successfully</label>' +
            '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
            '<p class="notify-text col-xs-12 no-pad">' + responseData.message + '</p>',
            'content optional one',
            {
              position: ["top", "right"],
              timeOut: 3000,
              showProgressBar: false,
              pauseOnHover: false,
              clickToClose: true,
              maxLength: 10
            }
          )
          AppComponent.GetNotification('icon icon-check angular-Notify', 'Tenant Deleted Successfully', responseData.message, new Date());
          this.RefreshTenant();
        }
        /* If response data is Failure then it enters into else and this block of code will execute to show the 'Failed To Delete Tenant' notification */
        else {
          this._notificationsService.html(
            '<i class="icon icon-fail angular-NotifyFail col-xs-1 no-pad"></i>' +
            '<label class="notify-label col-xs-10 no-pad">Failed To Delete Tenant</label>' +
            '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
            '<p class="notify-text col-xs-12 no-pad">' + responseData.message + '</p>',
            'content optional one',
            {
              position: ["top", "right"],
              timeOut: 3000,
              showProgressBar: false,
              pauseOnHover: false,
              clickToClose: true,
              maxLength: 10
            }
          )
          AppComponent.GetNotification('icon icon-fail angular-NotifyFail', 'Failed To Delete Tenant', responseData.message, new Date());
          this.RefreshTenant();
        }
      },
        /* If Any Error (or) Problem With Services (or) Problem in internet this Error Block Will Exequte */
        error => {
          this.refreshTenantLoading = false;
          this._notificationsService.html(
            '<i class="icon icon-close angular-NotifyFail"></i>' +
            '<label class="notify-label padleftright">Failed To Delete Tenant</label>' +
            '<a class="close"><i class="icon icon-close notify-close" aria-hidden="true"></i></a>' +
            '<p class="notify-text col-xs-12 no-pad">' + error.message + '</p>',
            'content optional one',
            {
              position: ["top", "right"],
              timeOut: 3000,
              showProgressBar: false,
              pauseOnHover: false,
              clickToClose: true,
              maxLength: 10
            }
          )
          AppComponent.GetNotification('fa fa-times-circle checkstyle', 'Failed To Delete Tenant', 'Problem with the service. Please try later', new Date());
        }

      );

    }
  }
}
