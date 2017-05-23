webpackJsonp([0,7],{

/***/ 311:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_component__ = __webpack_require__(313);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(6);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__(44);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__ = __webpack_require__(122);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__reports_routing_module__ = __webpack_require__(318);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ReportsModule", function() { return ReportsModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var ReportsModule = (function () {
    function ReportsModule() {
        console.log('reports');
    }
    return ReportsModule;
}());
ReportsModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_2__reports_component__["a" /* ReportsComponent */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["TabViewModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["ChartModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["DataTableModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["SharedModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["CalendarModule"],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormsModule"],
            __WEBPACK_IMPORTED_MODULE_4__angular_http__["l" /* JsonpModule */],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["ButtonModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["DropdownModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["DialogModule"],
            __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["InputTextareaModule"],
            __WEBPACK_IMPORTED_MODULE_6__reports_routing_module__["a" /* ReportsRoutingModule */]
        ],
        bootstrap: []
    }),
    __metadata("design:paramtypes", [])
], ReportsModule);

//# sourceMappingURL=reports.module.js.map

/***/ }),

/***/ 313:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__reports_service__ = __webpack_require__(319);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_search_options__ = __webpack_require__(317);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_report__ = __webpack_require__(315);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_search_inputs__ = __webpack_require__(316);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsComponent; });
/* unused harmony export MySelectItem */
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
        this.selectedReportID = '1';
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
        this.selectedReport = this.reportsService.reportList.filter(function (x) { return x.id === Number(_this.selectedReportID); })[0];
        // TODO catch exception if not found
        this.description = this.selectedReport.description;
        this.title = this.selectedReport.title || this.selectedReport.commonName;
        this.name = this.selectedReport.name;
        this.cols = this.selectedReport.columns.filter(function (a) { return a.visible === true; });
        this.inputs = this.selectedReport.inputs;
        console.log('updateDescription: ' + JSON.stringify(this.inputs, undefined, 2));
    };
    ReportsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.reports$ = this.reportsService.subscribeToDataService();
        this.reports$.subscribe(function (listData) {
            _this.reportsDropDown = listData.map(function (r) { return new MySelectItem(r.commonName, r.id.toString()); });
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
    ReportsComponent.prototype.getList = function () {
        this.reportsService.getReportList();
        console.log('getList called');
    };
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
        styles: [__webpack_require__(321)],
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

/***/ 316:
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

/***/ 317:
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

/***/ 318:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(8);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_component__ = __webpack_require__(313);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsRoutingModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var reportsRoutes = [
    {
        path: '',
        component: __WEBPACK_IMPORTED_MODULE_2__reports_component__["a" /* ReportsComponent */]
    }
];
var ReportsRoutingModule = (function () {
    function ReportsRoutingModule() {
    }
    return ReportsRoutingModule;
}());
ReportsRoutingModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(reportsRoutes)
        ],
        exports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
        ],
        providers: []
    })
], ReportsRoutingModule);

//# sourceMappingURL=reports-routing.module.js.map

/***/ }),

/***/ 319:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(44);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise__ = __webpack_require__(326);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__ = __webpack_require__(325);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch__ = __webpack_require__(324);
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
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["m" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["m" /* Http */]) === "function" && _a || Object])
], ReportsService);

var _a;
//# sourceMappingURL=reports.service.js.map

/***/ }),

/***/ 321:
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

var Observable_1 = __webpack_require__(5);
var catch_1 = __webpack_require__(124);
Observable_1.Observable.prototype.catch = catch_1._catch;
Observable_1.Observable.prototype._catch = catch_1._catch;
//# sourceMappingURL=catch.js.map

/***/ }),

/***/ 325:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var Observable_1 = __webpack_require__(5);
var map_1 = __webpack_require__(75);
Observable_1.Observable.prototype.map = map_1.map;
//# sourceMappingURL=map.js.map

/***/ }),

/***/ 326:
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var Observable_1 = __webpack_require__(5);
var toPromise_1 = __webpack_require__(327);
Observable_1.Observable.prototype.toPromise = toPromise_1.toPromise;
//# sourceMappingURL=toPromise.js.map

/***/ }),

/***/ 327:
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

/***/ })

});
//# sourceMappingURL=0.chunk.js.map