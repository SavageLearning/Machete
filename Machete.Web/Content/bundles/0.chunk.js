webpackJsonp([0,9],{

/***/ 312:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__introduction_introduction_component__ = __webpack_require__(330);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__online_orders_component__ = __webpack_require__(331);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__ = __webpack_require__(329);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__ = __webpack_require__(334);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__ = __webpack_require__(333);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__ = __webpack_require__(328);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__online_orders_routing_module__ = __webpack_require__(340);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__ = __webpack_require__(122);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_9_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_forms__ = __webpack_require__(6);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "OnlineOrdersModule", function() { return OnlineOrdersModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};











var OnlineOrdersModule = (function () {
    function OnlineOrdersModule() {
    }
    return OnlineOrdersModule;
}());
OnlineOrdersModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
            __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["StepsModule"],
            __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["DropdownModule"],
            __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["CalendarModule"],
            __WEBPACK_IMPORTED_MODULE_10__angular_forms__["FormsModule"],
            __WEBPACK_IMPORTED_MODULE_10__angular_forms__["ReactiveFormsModule"],
            __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["DataTableModule"],
            __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["InputSwitchModule"],
            __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["MessagesModule"],
            __WEBPACK_IMPORTED_MODULE_8__online_orders_routing_module__["a" /* OnlineOrdersRoutingModule */]
        ],
        declarations: [
            __WEBPACK_IMPORTED_MODULE_2__introduction_introduction_component__["a" /* IntroductionComponent */],
            __WEBPACK_IMPORTED_MODULE_3__online_orders_component__["a" /* OnlineOrdersComponent */],
            __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__["a" /* IntroConfirmComponent */],
            __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__["a" /* WorkOrderComponent */],
            __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */],
            __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__["a" /* FinalConfirmComponent */]
        ]
    })
], OnlineOrdersModule);

//# sourceMappingURL=online-orders.module.js.map

/***/ }),

/***/ 314:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__reports_service__ = __webpack_require__(321);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_search_options__ = __webpack_require__(320);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_report__ = __webpack_require__(318);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_search_inputs__ = __webpack_require__(319);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return ReportsComponent; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MySelectItem; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var ReportsComponent = (function () {
    function ReportsComponent(reportsService) {
        this.reportsService = reportsService;
        this.displayDescription = false;
        this.displayDialog = false;
        this.o = new __WEBPACK_IMPORTED_MODULE_2__models_search_options__["a" /* SearchOptions */]();
        this.selectedReport = new __WEBPACK_IMPORTED_MODULE_3__models_report__["a" /* Report */]();
        this.selectedReportID = 'DispatchesByJob';
        // this.title = 'loading';
        // this.description = 'loading...';
        this.o.beginDate = '1/1/2016';
        this.o.endDate = '1/1/2017';
        this.reportsDropDown = [];
        this.reportsDropDown.push({ label: 'Select Report', value: null });
        this.inputs = new __WEBPACK_IMPORTED_MODULE_4__models_search_inputs__["a" /* SearchInputs */]();
    }
    ReportsComponent.prototype.showDescription = function () {
        this.updateDescription();
        this.displayDescription = true;
    };
    ReportsComponent.prototype.updateDescription = function () {
        var _this = this;
        this.selectedReport = this.reportsService.reportList.filter(function (x) { return x.name === _this.selectedReportID; })[0];
        // TODO catch exception if not found
        this.description = this.selectedReport.description;
        this.title = this.selectedReport.title || this.selectedReport.commonName;
        this.name = this.selectedReport.name;
        this.cols = this.selectedReport.columns.filter(function (a) { return a.visible === true; });
        this.inputs = this.selectedReport.inputs;
    };
    ReportsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.reports$ = this.reportsService.subscribeToDataService();
        this.reports$.subscribe(function (listData) {
            _this.reportsDropDown = listData.map(function (r) { return new MySelectItem(r.commonName, r.name); });
            _this.getView();
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit onCompleted'); });
    };
    ReportsComponent.prototype.getView = function () {
        var _this = this;
        this.reportsService.getReportData(this.selectedReportID.toString(), this.o)
            .subscribe(function (data) {
            _this.viewData = data;
            _this.updateDescription();
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('getView onCompleted'); });
    };
    // getList() {
    //   this.reportsService.getReportList();
    //   console.log('getList called');
    // }
    ReportsComponent.prototype.getExport = function (dt) {
        dt.exportFilename = this.name + '_' + this.o.beginDate.toString() + '_to_' + this.o.endDate.toString();
        dt.exportCSV();
    };
    return ReportsComponent;
}());
ReportsComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-reports',
        template: __webpack_require__(323),
        styles: [__webpack_require__(322)],
        providers: [__WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */]]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */]) === "function" && _a || Object])
], ReportsComponent);

var MySelectItem = (function () {
    function MySelectItem(label, value) {
        this.label = label;
        this.value = value;
    }
    return MySelectItem;
}());

var _a;
//# sourceMappingURL=reports.component.js.map

/***/ }),

/***/ 315:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var Observable_1 = __webpack_require__(5);
var catch_1 = __webpack_require__(124);
Observable_1.Observable.prototype.catch = catch_1._catch;
Observable_1.Observable.prototype._catch = catch_1._catch;
//# sourceMappingURL=catch.js.map

/***/ }),

/***/ 316:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var Observable_1 = __webpack_require__(5);
var map_1 = __webpack_require__(75);
Observable_1.Observable.prototype.map = map_1.map;
//# sourceMappingURL=map.js.map

/***/ }),

/***/ 317:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var Observable_1 = __webpack_require__(5);
var toPromise_1 = __webpack_require__(324);
Observable_1.Observable.prototype.toPromise = toPromise_1.toPromise;
//# sourceMappingURL=toPromise.js.map

/***/ }),

/***/ 318:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Report; });
/**
 * Created by jcarter on 3/9/17.
 */
var Report = (function () {
    function Report() {
    }
    return Report;
}());

//# sourceMappingURL=report.js.map

/***/ }),

/***/ 319:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SearchInputs; });
/**
 * Created by jcii on 5/22/17.
 */
var SearchInputs = (function () {
    function SearchInputs() {
    }
    return SearchInputs;
}());

//# sourceMappingURL=search-inputs.js.map

/***/ }),

/***/ 320:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SearchOptions; });
var SearchOptions = (function () {
    function SearchOptions() {
    }
    return SearchOptions;
}());

//# sourceMappingURL=search-options.js.map

/***/ }),

/***/ 321:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(36);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise__ = __webpack_require__(317);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__ = __webpack_require__(316);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch__ = __webpack_require__(315);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_BehaviorSubject__ = __webpack_require__(123);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_BehaviorSubject___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_rxjs_BehaviorSubject__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsService; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var ReportsService = (function () {
    function ReportsService(http) {
        this.http = http;
        this.reportList = new Array();
        this.initializeDataService();
    }
    // https://stackoverflow.com/questions/39627396/angular-2-observable-with-multiple-subscribers
    ReportsService.prototype.initializeDataService = function () {
        if (!this.reportList$) {
            this.reportList$ = new __WEBPACK_IMPORTED_MODULE_5_rxjs_BehaviorSubject__["BehaviorSubject"](new Array());
            this.getReportList();
        }
    };
    ReportsService.prototype.subscribeToDataService = function () {
        return this.reportList$.asObservable();
    };
    //
    ReportsService.prototype.getReportData = function (reportName, o) {
        // TODO throw exception if report is not populated
        var params = this.encodeData(o);
        var uri = '/api/reports';
        if (reportName) {
            uri = uri + '/' + reportName;
        }
        if (reportName && params) {
            uri = uri + '?' + params;
        }
        console.log('reportsService.getReportData: ' + uri);
        return this.http.get(uri)
            .map(function (res) { return res.json().data; })
            .catch(this.handleError);
    };
    ReportsService.prototype.getReportList = function () {
        var _this = this;
        var uri = '/api/reports';
        console.log('reportsService.getReportList: ' + uri);
        this.http.get(uri)
            .map(function (res) { return res.json().data; })
            .catch(this.handleError)
            .subscribe(function (data) {
            _this.reportList = data;
            _this.reportList$.next(data);
        }, function (error) { return console.log('Error subscribing to DataService: ' + error); });
    };
    ReportsService.prototype.handleError = function (error) {
        console.error('ERROR', error);
        return Promise.reject(error.message || error);
    };
    ReportsService.prototype.encodeData = function (data) {
        return Object.keys(data).map(function (key) {
            return [key, data[key]].map(encodeURIComponent).join('=');
        }).join('&');
    };
    return ReportsService;
}());
ReportsService = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["o" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["o" /* Http */]) === "function" && _a || Object])
], ReportsService);

var _a;
//# sourceMappingURL=reports.service.js.map

/***/ }),

/***/ 322:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 323:
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-g\">\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <p-dropdown [options]=\"reportsDropDown\" (onChange)=\"getView()\" [(ngModel)]=\"selectedReportID\" [filter]=\"true\" [style]=\"{'width':'20em'}\"></p-dropdown>\r\n    <button pButton type=\"button\" icon=\"ui-icon-sync\" (click)=\"getView()\" iconPos=\"left\"></button>\r\n    <button pButton type=\"button\" icon=\"ui-icon-edit\" (click)=\"displayDialog=true\" iconPos=\"left\"></button>\r\n  </div>\r\n  <div *ngIf=\"inputs.memberNumber === true\" class=\"ui-g-12 ui-md-6\">\r\n    <label for=\"memberNumber\">Membr number</label>\r\n    <input id=\"memberNumber\" type=\"text\" pInputText [(ngModel)]=\"o.memberNumber\" (onBlur)=\"getView()\" dataType=\"number\"/>\r\n  </div>\r\n  <div *ngIf=\"inputs.beginDate === true\" class=\"ui-g-12 ui-md-6 ui-lg-3\">\r\n    <p-calendar  placeholder=\"Start date\" (onSelect)=\"getView()\" (onBlur)=\"getView()\" [(ngModel)]=\"o.beginDate\" [showIcon]=\"true\" dataType=\"string\"></p-calendar>\r\n  </div>\r\n  <div *ngIf=\"inputs.endDate === true\" class=\"ui-g-12 ui-md-6 ui-lg-3\">\r\n    <p-calendar placeholder=\"End date\" (onSelect)=\"getView()\" (onBlur)=\"getView()\" [(ngModel)]=\"o.endDate\" [showIcon]=\"true\" dataType=\"string\"></p-calendar>\r\n  </div>\r\n</div>\r\n<p-dialog header=\"{{title}}\" [(visible)]=\"displayDescription\">\r\n  {{description}}\r\n</p-dialog>\r\n<div>\r\n<p-dataTable\r\n  #dt\r\n  [value]=\"viewData\"\r\n  sortField=\"value\"\r\n  sortOrder=\"-1\"\r\n  sortMode=\"single\"\r\n  [globalFilter]=\"gb\"\r\n  [responsive]=\"true\"\r\n  >\r\n  <p-header>\r\n    <div class=\"ui-helper-clearfix\">\r\n      <button type=\"button\" pButton icon=\"ui-icon-file-download\" iconPos=\"left\" label=\"CSV\" (click)=\"getExport(dt)\" style=\"float:left\"></button>\r\n      <input #gb type=\"text\" placeholder=\"Global search\" width=\"200\">\r\n\r\n      <button pButton type=\"button\" icon=\"ui-icon-help-outline\" (click)=\"showDescription()\" iconPos=\"left\" style=\"float:right\"></button>\r\n    </div>\r\n  </p-header>\r\n  <p-column *ngFor=\"let col of cols\" [field]=\"col.field\" [header]=\"col.header\" [sortable]=\"true\"></p-column>\r\n</p-dataTable>\r\n  <p-dialog\r\n    header=\"Report Details\"\r\n    [(visible)]=\"displayDialog\"\r\n    [responsive]=\"true\"\r\n    showEffect=\"fade\"\r\n    [modal]=\"false\"\r\n    resizable=\"true\"\r\n    width=\"1000\"\r\n  >\r\n    <div>\r\n      <div class=\"ui-g\" style=\"display:flex\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"name\">Name</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\"><input pInputText id=\"name\" [(ngModel)]=\"name\" style=\"width: 100%;\"/></div>\r\n      </div>\r\n      <div class=\"ui-g\" style=\"display:flex\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"commonName\">Common name</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\"><input pInputText id=\"commonName\" style=\"width: 100%;\" [(ngModel)]=\"selectedReport.commonName\" /></div>\r\n      </div>\r\n      <!--<div class=\"ui-g\">-->\r\n        <!--<div class=\"ui-sm-4 ui-md-3 ui-lg-2\"><label for=\"title\">Title</label></div>-->\r\n        <!--<div class=\"ui-sm-8 ui-md-9 ui-lg-10\"><input pInputText id=\"title\" [(ngModel)]=\"selectedReport.title\" /></div>-->\r\n      <!--</div>-->\r\n      <div class=\"ui-g\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"description\">Description</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\"><input pInputText id=\"description\" style=\"width: 100%;\" [(ngModel)]=\"selectedReport.description\" /></div>\r\n      </div>\r\n      <div class=\"ui-g\" style=\"display:flex\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"sqlquery\">SQL Query</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\">\r\n              <textarea pInputTextarea id=\"sqlquery\" rows=\"20\" style=\"width: 100%;\" [(ngModel)]=\"selectedReport.sqlquery\" autoResize=\"true\"></textarea>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <p-footer>\r\n      <div class=\"ui-dialog-buttonpane ui-widget-content ui-helper-clearfix\">\r\n        <!--<button type=\"button\" pButton icon=\"fa-close\" (click)=\"delete()\" label=\"Delete\"></button>-->\r\n        <!--<button type=\"button\" pButton icon=\"fa-check\" (click)=\"save()\" label=\"Save\"></button>-->\r\n      </div>\r\n    </p-footer>\r\n  </p-dialog>\r\n</div>\r\n"

/***/ }),

/***/ 324:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var root_1 = __webpack_require__(17);
/* tslint:enable:max-line-length */
/**
 * Converts an Observable sequence to a ES2015 compliant promise.
 *
 * @example
 * // Using normal ES2015
 * let source = Rx.Observable
 *   .of(42)
 *   .toPromise();
 *
 * source.then((value) => console.log('Value: %s', value));
 * // => Value: 42
 *
 * // Rejected Promise
 * // Using normal ES2015
 * let source = Rx.Observable
 *   .throw(new Error('woops'))
 *   .toPromise();
 *
 * source
 *   .then((value) => console.log('Value: %s', value))
 *   .catch((err) => console.log('Error: %s', err));
 * // => Error: Error: woops
 *
 * // Setting via the config
 * Rx.config.Promise = RSVP.Promise;
 *
 * let source = Rx.Observable
 *   .of(42)
 *   .toPromise();
 *
 * source.then((value) => console.log('Value: %s', value));
 * // => Value: 42
 *
 * // Setting via the method
 * let source = Rx.Observable
 *   .of(42)
 *   .toPromise(RSVP.Promise);
 *
 * source.then((value) => console.log('Value: %s', value));
 * // => Value: 42
 *
 * @param PromiseCtor promise The constructor of the promise. If not provided,
 * it will look for a constructor first in Rx.config.Promise then fall back to
 * the native Promise constructor if available.
 * @return {Promise<T>} An ES2015 compatible promise with the last value from
 * the observable sequence.
 * @method toPromise
 * @owner Observable
 */
function toPromise(PromiseCtor) {
    var _this = this;
    if (!PromiseCtor) {
        if (root_1.root.Rx && root_1.root.Rx.config && root_1.root.Rx.config.Promise) {
            PromiseCtor = root_1.root.Rx.config.Promise;
        }
        else if (root_1.root.Promise) {
            PromiseCtor = root_1.root.Promise;
        }
    }
    if (!PromiseCtor) {
        throw new Error('no Promise impl found');
    }
    return new PromiseCtor(function (resolve, reject) {
        var value;
        _this.subscribe(function (x) { return value = x; }, function (err) { return reject(err); }, function () { return resolve(value); });
    });
}
exports.toPromise = toPromise;
//# sourceMappingURL=toPromise.js.map

/***/ }),

/***/ 327:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(36);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_handle_error__ = __webpack_require__(344);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LookupsService; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var LookupsService = (function () {
    function LookupsService(http) {
        this.http = http;
        this.uriBase = '/api/lookups';
    }
    LookupsService.prototype.getLookups = function (category) {
        var uri = this.uriBase;
        if (category) {
            uri = uri + '?category=' + category;
        }
        console.log('lookupsService.getLookups: ' + uri);
        return this.http.get(uri)
            .map(function (res) { return res.json().data; })
            .catch(__WEBPACK_IMPORTED_MODULE_2__shared_handle_error__["a" /* HandleError */].error);
    };
    return LookupsService;
}());
LookupsService = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["o" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["o" /* Http */]) === "function" && _a || Object])
], LookupsService);

var _a;
//# sourceMappingURL=lookups.service.js.map

/***/ }),

/***/ 328:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FinalConfirmComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var FinalConfirmComponent = (function () {
    function FinalConfirmComponent() {
    }
    FinalConfirmComponent.prototype.ngOnInit = function () {
    };
    return FinalConfirmComponent;
}());
FinalConfirmComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-final-confirm',
        template: __webpack_require__(360),
        styles: [__webpack_require__(349)]
    }),
    __metadata("design:paramtypes", [])
], FinalConfirmComponent);

//# sourceMappingURL=final-confirm.component.js.map

/***/ }),

/***/ 329:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroConfirmComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var IntroConfirmComponent = (function () {
    function IntroConfirmComponent() {
    }
    IntroConfirmComponent.prototype.ngOnInit = function () {
    };
    return IntroConfirmComponent;
}());
IntroConfirmComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-intro-confirm',
        template: __webpack_require__(361),
        styles: [__webpack_require__(350)]
    }),
    __metadata("design:paramtypes", [])
], IntroConfirmComponent);

//# sourceMappingURL=intro-confirm.component.js.map

/***/ }),

/***/ 330:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroductionComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var IntroductionComponent = (function () {
    function IntroductionComponent() {
    }
    IntroductionComponent.prototype.ngOnInit = function () {
    };
    return IntroductionComponent;
}());
IntroductionComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-introduction',
        template: __webpack_require__(362),
        styles: [__webpack_require__(351)]
    }),
    __metadata("design:paramtypes", [])
], IntroductionComponent);

//# sourceMappingURL=introduction.component.js.map

/***/ }),

/***/ 331:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__lookups_lookups_service__ = __webpack_require__(327);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__(332);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(6);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var OnlineOrdersComponent = (function () {
    function OnlineOrdersComponent() {
        this.activeIndex = 0;
    }
    OnlineOrdersComponent.prototype.ngOnInit = function () {
        this.items = [
            { label: 'Introduction', routerLink: ['online-orders/introduction'] },
            { label: 'Confirm', routerLink: ['online-orders/intro-confirm'] },
            { label: 'work site details', routerLink: ['online-orders/work-order'] },
            { label: 'worker details', routerLink: ['online-orders/work-assignments'] },
            { label: 'finalize', routerLink: ['online-orders/final-confirm'] }
        ];
    };
    return OnlineOrdersComponent;
}());
OnlineOrdersComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-online-orders',
        template: __webpack_require__(363),
        styles: [__webpack_require__(352)],
        providers: [__WEBPACK_IMPORTED_MODULE_1__lookups_lookups_service__["a" /* LookupsService */], __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */], __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"]]
    }),
    __metadata("design:paramtypes", [])
], OnlineOrdersComponent);

//# sourceMappingURL=online-orders.component.js.map

/***/ }),

/***/ 332:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersService; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var OnlineOrdersService = (function () {
    function OnlineOrdersService() {
        this.requests = new Array();
        console.log('online-orders.service: ' + JSON.stringify(this.getRequests()));
    }
    OnlineOrdersService.prototype.getRequests = function () {
        return this.requests;
    };
    OnlineOrdersService.prototype.createRequest = function (request) {
        this.requests.push(request);
    };
    OnlineOrdersService.prototype.saveRequest = function (request) {
        this.requests[this.findSelectedRequestIndex(request)] = request;
    };
    OnlineOrdersService.prototype.deleteRequest = function () { };
    OnlineOrdersService.prototype.clearRequests = function () { };
    OnlineOrdersService.prototype.findSelectedRequestIndex = function (request) {
        return this.requests.indexOf(request);
    };
    return OnlineOrdersService;
}());
OnlineOrdersService = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
    __metadata("design:paramtypes", [])
], OnlineOrdersService);

//# sourceMappingURL=online-orders.service.js.map

/***/ }),

/***/ 333:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(6);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__ = __webpack_require__(314);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_worker_request__ = __webpack_require__(341);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__(327);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__(339);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__online_orders_service__ = __webpack_require__(332);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var WorkAssignmentsComponent = (function () {
    function WorkAssignmentsComponent(lookupsService, ordersService, fb) {
        this.lookupsService = lookupsService;
        this.ordersService = ordersService;
        this.fb = fb;
        this.selectedSkill = new __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* Lookup */]();
        this.requestList = new Array(); // list built by user in UI
        this.request = new __WEBPACK_IMPORTED_MODULE_3__models_worker_request__["a" /* WorkerRequest */](); // composed by UI to make/edit a request
        this.newRequest = true;
        this.showErrors = false;
        this.formErrors = {
            'skillId': '',
            'skill': '',
            'hours': '',
            'description': '',
            'requiresHeavyLifting': '',
            'wage': ''
        };
        this.validationMessages = {
            'skillId': { 'required': 'Please select the type of work to be performed.' },
            'skill': { 'required': 'skill is required.' },
            'hours': { 'required': 'Please enter the number of hours needed.' },
            'description': { 'required': 'description is required.' },
            'requiresHeavyLifting': { 'required': 'requiresHeavyLifting is required.' },
            'wage': { 'required': 'wage is required.' },
        };
        console.log('work-assignments.component: ' + JSON.stringify(ordersService.getRequests()));
    }
    WorkAssignmentsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.lookupsService.getLookups('skill')
            .subscribe(function (listData) {
            _this.skills = listData;
            _this.skillsDropDown = listData.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('exports.component: ngOnInit onCompleted'); });
        this.requestList = this.ordersService.getRequests();
        this.buildForm();
    };
    WorkAssignmentsComponent.prototype.buildForm = function () {
        var _this = this;
        this.requestForm = this.fb.group({
            'skillId': [this.request.skillId, __WEBPACK_IMPORTED_MODULE_1__angular_forms__["Validators"].required],
            'skill': [this.request.skill],
            'hours': [this.request.hours, __WEBPACK_IMPORTED_MODULE_1__angular_forms__["Validators"].required],
            'description': [this.request.description],
            'requiresHeavyLifting': [this.request.requiresHeavyLifting],
            'wage': [this.request.wage]
        });
        this.requestForm.valueChanges
            .subscribe(function (data) { return _this.onValueChanged(data); });
        this.onValueChanged();
    };
    WorkAssignmentsComponent.prototype.onValueChanged = function (data) {
        var form = this.requestForm;
        for (var field in this.formErrors) {
            // clear previous error message (if any)
            this.formErrors[field] = '';
            var control = form.get(field);
            if (control && !control.valid) {
                var messages = this.validationMessages[field];
                for (var key in control.errors) {
                    this.formErrors[field] += messages[key] + ' ';
                }
            }
        }
    };
    WorkAssignmentsComponent.prototype.selectSkill = function (skillId) {
        var skill = this.skills.filter(function (f) { return f.id === Number(skillId); }).shift();
        if (skill === null) {
            throw new Error('Can\'t find selected skill in component\'s list');
        }
        this.selectedSkill = skill;
        this.requestForm.controls['skill'].setValue(skill.text_EN);
        this.requestForm.controls['wage'].setValue(skill.wage);
    };
    WorkAssignmentsComponent.prototype.editRequest = function (request) {
    };
    WorkAssignmentsComponent.prototype.deleteRequest = function (request) {
    };
    WorkAssignmentsComponent.prototype.saveRequest = function () {
        this.onValueChanged();
        if (this.requestForm.status === 'INVALID') {
            this.showErrors = true;
            return;
        }
        this.showErrors = false;
        var formModel = this.requestForm.value;
        var saveRequest = {
            skillId: formModel.skillId,
            skill: formModel.skill,
            hours: formModel.hours,
            description: formModel.description,
            requiresHeavyLifting: formModel.requiresHeavyLifting,
            wage: formModel.wage
        };
        if (this.newRequest) {
            this.ordersService.createRequest(saveRequest);
        }
        else {
            this.ordersService.saveRequest(saveRequest);
        }
        this.requestList = this.ordersService.getRequests().slice();
        this.requestForm.reset();
        this.newRequest = true;
    };
    WorkAssignmentsComponent.prototype.onRowSelect = function (event) {
        this.newRequest = false;
        this.request = this.cloneRequest(event.data);
    };
    WorkAssignmentsComponent.prototype.cloneRequest = function (c) {
        var request = new __WEBPACK_IMPORTED_MODULE_3__models_worker_request__["a" /* WorkerRequest */]();
        for (var prop in c) {
            request[prop] = c[prop];
        }
        return request;
    };
    return WorkAssignmentsComponent;
}());
WorkAssignmentsComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-work-assignments',
        template: __webpack_require__(364),
        styles: [__webpack_require__(353)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"]) === "function" && _c || Object])
], WorkAssignmentsComponent);

var _a, _b, _c;
//# sourceMappingURL=work-assignments.component.js.map

/***/ }),

/***/ 334:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__(6);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_work_order__ = __webpack_require__(342);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var WorkOrderComponent = (function () {
    function WorkOrderComponent(fb) {
        this.fb = fb;
        this.order = new __WEBPACK_IMPORTED_MODULE_2__models_work_order__["a" /* WorkOrder */]();
    }
    WorkOrderComponent.prototype.ngOnInit = function () {
        this.buildForm();
    };
    WorkOrderComponent.prototype.buildForm = function () {
        this.orderForm = this.fb.group({
            'selectedTransportMethod': [this.order.transportMethodID]
        });
    };
    return WorkOrderComponent;
}());
WorkOrderComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-work-order',
        template: __webpack_require__(365),
        styles: [__webpack_require__(354)]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"]) === "function" && _a || Object])
], WorkOrderComponent);

var _a;
//# sourceMappingURL=work-order.component.js.map

/***/ }),

/***/ 339:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Lookup; });
/**
 * Created by jcii on 6/2/17.
 */
var Lookup = (function () {
    function Lookup() {
    }
    return Lookup;
}());

//# sourceMappingURL=lookup.js.map

/***/ }),

/***/ 340:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(8);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_component__ = __webpack_require__(331);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__introduction_introduction_component__ = __webpack_require__(330);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__ = __webpack_require__(329);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__ = __webpack_require__(334);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__ = __webpack_require__(333);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__ = __webpack_require__(328);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersRoutingModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};








var onlineOrderRoutes = [
    {
        path: '',
        component: __WEBPACK_IMPORTED_MODULE_2__online_orders_component__["a" /* OnlineOrdersComponent */],
        children: [
            {
                path: 'introduction',
                component: __WEBPACK_IMPORTED_MODULE_3__introduction_introduction_component__["a" /* IntroductionComponent */]
            },
            {
                path: 'intro-confirm',
                component: __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__["a" /* IntroConfirmComponent */]
            },
            {
                path: 'work-order',
                component: __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__["a" /* WorkOrderComponent */]
            },
            {
                path: 'work-assignments',
                component: __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */]
            },
            {
                path: 'final-confirm',
                component: __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__["a" /* FinalConfirmComponent */]
            }
        ]
    },
];
var OnlineOrdersRoutingModule = (function () {
    function OnlineOrdersRoutingModule() {
    }
    return OnlineOrdersRoutingModule;
}());
OnlineOrdersRoutingModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(onlineOrderRoutes)
        ],
        exports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
        ],
        providers: []
    })
], OnlineOrdersRoutingModule);

//# sourceMappingURL=online-orders-routing.module.js.map

/***/ }),

/***/ 341:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkerRequest; });
/**
 * Created by jcii on 5/31/17.
 */
var WorkerRequest = (function () {
    function WorkerRequest() {
        this.requiresHeavyLifting = false;
    }
    return WorkerRequest;
}());

//# sourceMappingURL=worker-request.js.map

/***/ }),

/***/ 342:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrder; });
/**
 * Created by jcii on 6/10/17.
 */
var WorkOrder = (function () {
    function WorkOrder() {
    }
    return WorkOrder;
}());

//# sourceMappingURL=work-order.js.map

/***/ }),

/***/ 344:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HandleError; });
/**
 * Created by jcii on 6/2/17.
 */
var HandleError = (function () {
    function HandleError() {
    }
    HandleError.error = function (error) {
        console.error('ERROR', error);
        return Promise.reject(error.message || error);
    };
    return HandleError;
}());

//# sourceMappingURL=handle-error.js.map

/***/ }),

/***/ 349:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 350:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 351:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 352:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 353:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 354:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 360:
/***/ (function(module, exports) {

module.exports = "<p>\r\n  final-confirm works!\r\n</p>\r\n"

/***/ }),

/***/ 361:
/***/ (function(module, exports) {

module.exports = "<strong>Please note:</strong>\r\n<ol>\r\n  <li>This order is not complete until you receive a confirmation email from Casa Latina.\r\n    If you do not hear from us or if you need a worker with 48 hours please call 206.956.0779 x3 during our\r\n    <a href=\"#\" id=\"businessHoursModal\">Business Hours</a>.\r\n  </li>\r\n  <li>Please allow a one hour window for worker(s) to arrive. This will account for transportation\r\n    routes with multiple stops and for traffic. There is no transportation fee to hire a Casa Latina\r\n    worker when you pick them up from our office. To have your worker(s) arrive at your door,\r\n    there is a <a href=\"#\" id=\"transportationMethodModal\">small fee</a> payable through this form.\r\n  </li>\r\n  <li>Casa Latina workers are not contractors. You will need to provide all tools, materials, and\r\n    safety equipment necessary for the job you wish to have done.\r\n  </li>\r\n</ol>\r\n"

/***/ }),

/***/ 362:
/***/ (function(module, exports) {

module.exports = "\r\n<p>\r\n  Casa Latina connects Latino immigrant workers with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! For more information about our program please read these Frequently Asked Questions\r\n</p>\r\n<p>\r\n  If you are ready to hire a worker, please fill out the following form.\r\n</p>\r\n<p>\r\n  If you still have questions about hiring a worker, please call us at 206.956.0779 x3.\r\n</p>\r\n\r\n"

/***/ }),

/***/ 363:
/***/ (function(module, exports) {

module.exports = "<h1>\r\n  Hire a Worker Online Order Form\r\n</h1>\r\n<p-steps [model]=\"items\" [readonly]=\"false\" [(activeIndex)]=\"activeIndex\" ></p-steps>\r\n<router-outlet></router-outlet>\r\n"

/***/ }),

/***/ 364:
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"requestForm\"  (ngSubmit)=\"saveRequest()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"skillsList\">Skill needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-dropdown id=\"skillsList\"\r\n                        [options]=\"skillsDropDown\"\r\n                        formControlName=\"skillId\"\r\n                        [(ngModel)]=\"request.skillId\"\r\n                        (onChange)=\"selectSkill(request.skillId)\"\r\n                        [autoWidth]=\"false\"\r\n                        placeholder=\"Select a skill\"></p-dropdown>\r\n\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['skillId'].valid && showErrors\">\r\n            {{formErrors.skillId}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"hours\">Hours needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"hours\" id=\"hours\" type=\"text\" pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['hours'].valid && showErrors\">\r\n            {{formErrors.hours}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"requiresHeavyLifting\">Requires heavy lifting?</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-inputSwitch id=\"requiresHeavyLifting\" formControlName=\"requiresHeavyLifting\"></p-inputSwitch>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Additional info about job</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <textarea rows=\"3\" class=\"ui-inputtext\" formControlName=\"description\" id=\"description\" type=\"text\" pInputText></textarea>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Skill description\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.skillDescriptionEn}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Hourly rate\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.wage}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Minimum time\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.minHour}}\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n\r\n    <p-dataTable [value]=\"requestList\" [(selection)]=\"selectedRequest\" (onRowSelect)=\"onRowSelect($event)\" [responsive]=\"true\">\r\n      <p-column field=\"skill\" header=\"Skill needed\"></p-column>\r\n      <p-column field=\"hours\" header=\"hours requested\"></p-column>\r\n      <p-column field=\"description\" header=\"notes\"></p-column>\r\n      <p-column field=\"requiresHeavyLifting\" header=\"Heavy lifting?\"></p-column>\r\n      <p-column field=\"wage\" header=\"Hourly wage\"></p-column>\r\n\r\n      <p-column styleClass=\"col-button\">\r\n        <ng-template pTemplate=\"header\">\r\n          Actions\r\n        </ng-template>\r\n        <ng-template let-request=\"rowData\" pTemplate=\"body\">\r\n          <button type=\"button\" pButton (click)=\"editRequest(request)\" icon=\"ui-icon-edit\"></button>\r\n          <button type=\"button\" pButton (click)=\"deleteRequest(request)\" icon=\"ui-icon-delete\"></button>\r\n        </ng-template>\r\n      </p-column>\r\n    </p-dataTable>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ 365:
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid\">Seattle Family Biking\r\n      <div class=\"card\">\r\n        <form [formGroup]=\"orderForm\" class=\"ui-g form-group\">\r\n          <div class=\"ui-g-12 ui-md-6\">\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n                <label for=\"dateTimeofWork\">Time needed</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n                <p-calendar id=\"dateTimeofWork\"></p-calendar>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"contactName\">Contact name</label>\r\n              </div>\r\n              <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"contactName\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"worksiteAddress1\">Address (1)</label>\r\n              </div>\r\n              <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"worksiteAddress1\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n                <label for=\"worksiteAddress2\">Address (2)</label>\r\n              </div>\r\n              <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"worksiteAddress2\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"city\">City</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"city\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"state\">State</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"state\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"zipcode\">Zipcode</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"zipcode\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"phone\">Phone</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <input class=\"ui-inputtext\" id=\"phone\" type=\"text\" pInputText/>\r\n              </div>\r\n            </div>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-6\">\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"description\">Work Description</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\" id=\"description\" type=\"text\" ></textarea>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"additionalNotes\">Additional notes to dispatcher</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\" id=\"additionalNotes\" type=\"text\" ></textarea>\r\n              </div>\r\n            </div>\r\n            <div class=\"ui-g-12\">\r\n              <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n                <label for=\"transportMethodID\">Transport method</label>\r\n              </div>\r\n              <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n                <p-dropdown id=\"transportMethodID\" [options]=\"transportMethods\" formControlName=\"selectedTransportMethod\" [autoWidth]=\"false\"></p-dropdown>\r\n              </div>\r\n            </div>\r\n          </div>\r\n        </form>\r\n      </div>\r\n</div>\r\n"

/***/ })

});
//# sourceMappingURL=0.chunk.js.map