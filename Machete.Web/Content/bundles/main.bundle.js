webpackJsonp([2,4],{

/***/ 106:
/***/ (function(module, exports) {

function webpackEmptyContext(req) {
	throw new Error("Cannot find module '" + req + "'.");
}
webpackEmptyContext.keys = function() { return []; };
webpackEmptyContext.resolve = webpackEmptyContext;
module.exports = webpackEmptyContext;
webpackEmptyContext.id = 106;


/***/ }),

/***/ 107:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__(112);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__(115);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__(73);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__ = __webpack_require__(123);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__);





if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["enableProdMode"])();
}
__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 114:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var AppComponent = (function () {
    function AppComponent() {
        this.title = 'Reports 2.0';
    }
    return AppComponent;
}());
AppComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-root',
        template: __webpack_require__(249),
        styles: [__webpack_require__(180)]
    })
], AppComponent);

//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 115:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__(20);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__(5);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_http__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__reports_reports_module__ = __webpack_require__(119);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__app_component__ = __webpack_require__(114);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_angular_in_memory_web_api__ = __webpack_require__(122);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__in_memory_data_service__ = __webpack_require__(116);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__environments_environment__ = __webpack_require__(73);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* unused harmony export getBackend */
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};









var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["NgModule"])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["BrowserModule"],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormsModule"],
            __WEBPACK_IMPORTED_MODULE_3__angular_http__["a" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_4__reports_reports_module__["a" /* ReportsModule */],
            __WEBPACK_IMPORTED_MODULE_6_angular_in_memory_web_api__["a" /* InMemoryWebApiModule */].forRoot(__WEBPACK_IMPORTED_MODULE_7__in_memory_data_service__["a" /* InMemoryDataService */])
        ],
        providers: [
            {
                provide: __WEBPACK_IMPORTED_MODULE_3__angular_http__["b" /* XHRBackend */],
                useFactory: getBackend,
                deps: [__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injector"], __WEBPACK_IMPORTED_MODULE_3__angular_http__["c" /* BrowserXhr */], __WEBPACK_IMPORTED_MODULE_3__angular_http__["d" /* XSRFStrategy */], __WEBPACK_IMPORTED_MODULE_3__angular_http__["e" /* ResponseOptions */]]
            }
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]]
    })
], AppModule);

function getBackend(injector, browser, xsrf, options) {
    {
        if (__WEBPACK_IMPORTED_MODULE_8__environments_environment__["a" /* environment */].production) {
            return new __WEBPACK_IMPORTED_MODULE_3__angular_http__["b" /* XHRBackend */](browser, options, xsrf);
        }
        else {
            return __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_6_angular_in_memory_web_api__["b" /* inMemoryBackendServiceFactory */])(injector, new __WEBPACK_IMPORTED_MODULE_7__in_memory_data_service__["a" /* InMemoryDataService */](), {});
        }
    }
}
//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 116:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return InMemoryDataService; });
var InMemoryDataService = (function () {
    function InMemoryDataService() {
    }
    InMemoryDataService.prototype.createDb = function () {
        var DispatchesByMonth = [
            {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-1',
                'label': '2016-01',
                'value': 250
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-2',
                'label': '2016-02',
                'value': 355
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-3',
                'label': '2016-03',
                'value': 579
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-4',
                'label': '2016-04',
                'value': 992
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-5',
                'label': '2016-05',
                'value': 837
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-6',
                'label': '2016-06',
                'value': 833
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-7',
                'label': '2016-07',
                'value': 803
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-8',
                'label': '2016-08',
                'value': 799
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-9',
                'label': '2016-09',
                'value': 667
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-10',
                'label': '2016-10',
                'value': 483
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-11',
                'label': '2016-11',
                'value': 422
            }, {
                'id': '2016-01-01T00:00:00-2017-01-01T00:00:00-12',
                'label': '2016-12',
                'value': 296
            }
        ];
        var DispatchesByJob = [
            {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-83',
                'label': 'Advanced Gardening',
                'value': 41
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-74',
                'label': 'Basic House Cleaning',
                'value': 54
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-64',
                'label': 'Build retaining wall- Landscaping',
                'value': 1
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-65',
                'label': 'Carpentry  Framing and Cabinetry',
                'value': 15
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-67',
                'label': 'Deep and/or Move in/out Cleaning',
                'value': 59
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-77',
                'label': 'Demolition',
                'value': 29
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-122',
                'label': 'Digging',
                'value': 48
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-63',
                'label': 'Drywall - Hanging Sheetrock',
                'value': 8
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-133',
                'label': 'Drywall - Taping and Sanding',
                'value': 11
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-60',
                'label': 'General Labor',
                'value': 10
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-128',
                'label': 'Hauling',
                'value': 55
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-131',
                'label': 'Insulation',
                'value': 6
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-88',
                'label': 'Landscaping',
                'value': 12
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-68',
                'label': 'Moving Furniture and Boxes',
                'value': 100
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-61',
                'label': 'Painting (roller brush)',
                'value': 42
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-120',
                'label': 'Pressure Washing',
                'value': 1
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-132',
                'label': 'Tile Installation',
                'value': 3
            }, {
                'id': '2017-01-01T00:00:00-2017-03-01T00:00:00-69',
                'label': 'Yardwork',
                'value': 35
            }
        ];
        var reports = [
            {
                'name': 'DispatchesByJob',
                'commonName': 'Dispatches by job, with some other text',
                'description': 'The number of completed dispatches, grouped by job (skill ID)',
                'sqlquery': 'SELECT\r\nconvert(varchar(24), @startDate, 126) + "-" + convert(varchar(23), @endDate, 126) + "-" + convert(varchar(5), min(wa.skillid)) as id,\r\nlskill.text_en  AS label,\r\ncount(lskill.text_en) value\r\nFROM [dbo].WorkAssignments as WA \r\njoin [dbo].lookups as lskill on (wa.skillid = lskill.id)\r\njoin [dbo].WorkOrders as WO ON (WO.ID = WA.workorderID)\r\njoin [dbo].lookups as lstatus on (WO.status = lstatus.id) \r\nWHERE wo.dateTimeOfWork < (@endDate) \r\nand wo.dateTimeOfWork > (@startDate)\r\nand lstatus.text_en = "Completed"\r\ngroup by lskill.text_en',
                'category': 'Dispatches',
                'subcategory': null,
                'idString': 'reportdef',
                'id': 1,
                'data': DispatchesByJob,
                'datecreated': '2017-05-05T10:21:16.957',
                'dateupdated': '2017-05-05T10:21:16.957',
                'createdby': 'Init T. Script',
                'updatedby': 'Init T. Script',
                'idPrefix': 'reportdef1-',
                'columnLabelsJson': '{ "label": "Job types", "value": "Count of jobs"}'
            },
            {
                'name': 'DispatchesByMonth',
                'title': 'A different title for Dispatches by Month',
                'commonName': 'Dispatches by Month, (weee!)',
                'description': 'The number of completed dispatches, grouped by month',
                'sqlquery': 'SELECT\r\nconvert(varchar(23), @startDate, 126) + "-" + convert(varchar(23), @endDate, 126) + "-" + convert(varchar(5), month(min(wo.datetimeofwork))) as id,\r\nconvert(varchar(7), min(wo.datetimeofwork), 126)  AS label,\r\ncount(*) value\r\nfrom workassignments wa\r\njoin workorders wo on wo.id = wa.workorderid\r\njoin lookups l on wo.status = l.id\r\nwhere  datetimeofwork >= @startDate\r\nand datetimeofwork < @endDate\r\nand l.text_en = "Completed"\r\nand wa.workerassignedid is not null\r\ngroup by month(wo.datetimeofwork)',
                'category': 'Dispatches',
                'subcategory': null,
                'idString': 'reportdef',
                'id': 2,
                'data': DispatchesByMonth,
                'datecreated': '2017-05-05T10:21:16.997',
                'dateupdated': '2017-05-05T10:21:16.997',
                'createdby': 'Init T. Script',
                'updatedby': 'Init T. Script',
                'idPrefix': 'reportdef2-',
                'columnLabelsJson': '{ "label": "Month", "value": "Count of jobs"}'
            }
        ];
        return { reports: reports };
    };
    // intercept response from the default HTTP method handlers
    InMemoryDataService.prototype.responseInterceptor = function (response, reqInfo) {
        // response.body = (<SimpleAggregateRow[]>response.body); // matches web api controller
        // const method = RequestMethod[reqInfo.req.method].toUpperCase();
        // const body = JSON.stringify(response.body);
        // console.log(`responseInterceptor: ${method} ${reqInfo.req.url}: \n${body}`);
        if (typeof reqInfo.query === 'object') {
            // if query parameters present, replace object w/ data key's value.
            // useful for testing; matches API behavior
            response.body = response.body.data;
        }
        return response;
    };
    return InMemoryDataService;
}());

//# sourceMappingURL=in-memory-data.service.js.map

/***/ }),

/***/ 117:
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

/***/ 118:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__reports_service__ = __webpack_require__(120);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_search_options__ = __webpack_require__(117);
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
        this.display = false;
        this.o = new __WEBPACK_IMPORTED_MODULE_2__models_search_options__["a" /* SearchOptions */]();
        this.selectedReportID = '1';
        this.title = 'loading';
        this.description = 'loading...';
        this.headerLabel = 'loading...';
        this.headerValue = 'loading...';
        this.o.beginDate = '1/1/2016';
        this.o.endDate = '1/1/2017';
        this.reportsDropDown = [];
        this.reportsDropDown.push({ label: 'Select Report', value: null });
    }
    ReportsComponent.prototype.showDialog = function () {
        this.updateDialog();
        this.display = true;
    };
    ReportsComponent.prototype.updateDialog = function () {
        var _this = this;
        this.selectedReport = this.reportsService.listData.filter(function (x) { return x.id === Number(_this.selectedReportID); });
        // TODO catch exception if not found
        this.description = this.selectedReport[0].description;
        this.title = this.selectedReport[0].title || this.selectedReport[0].commonName;
        var foo = JSON.parse(this.selectedReport[0].columnLabelsJson);
        this.headerLabel = foo.label;
        this.headerValue = foo.value;
        this.name = this.selectedReport[0].name;
    };
    ReportsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.reports$ = this.reportsService.subscribeToDataService();
        this.reports$.subscribe(function (listData) { return _this.reportsDropDown = listData.map(function (r) { return new MySelectItem(r.commonName, r.id.toString()); }); }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit onCompleted'); });
        this.getView();
    };
    ReportsComponent.prototype.getView = function () {
        var _this = this;
        this.reportsService.getReport(this.selectedReportID.toString(), this.o)
            .subscribe(function (data) {
            _this.viewData = data;
            _this.updateDialog();
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('getView onCompleted'); });
    };
    ReportsComponent.prototype.getList = function () {
        this.reportsService.getList();
        console.log('getList called');
        // .subscribe(
        //   data => this.reports = data,
        //   // data => this.reportsDropDown = data.map(r => new MySelectItem(r.name, r.id.toString()) as SelectItem),
        //   error => this.errorMessage = <any>error,
        //   () => console.log('getList onCompleted'));
        // this.reportsService.getList()
        //   .subscribe(
        //     data => this.reports = data,
        //     error => this.errorMessage = <any>error,
        //     () => console.log('getList onCompleted')
        //   );
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
        template: __webpack_require__(250),
        styles: [__webpack_require__(181)],
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

/***/ 119:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_component__ = __webpack_require__(118);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__(5);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_platform_browser_animations__ = __webpack_require__(113);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__ = __webpack_require__(247);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_primeng_primeng__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};







var ReportsModule = (function () {
    function ReportsModule() {
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
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["TabViewModule"],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["ChartModule"],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["DataTableModule"],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["SharedModule"],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["CalendarModule"],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormsModule"],
            __WEBPACK_IMPORTED_MODULE_4__angular_http__["a" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_4__angular_http__["l" /* JsonpModule */],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["ButtonModule"],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["DropdownModule"],
            __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["DialogModule"],
            __WEBPACK_IMPORTED_MODULE_5__angular_platform_browser_animations__["a" /* BrowserAnimationsModule */]
        ],
        exports: [
            __WEBPACK_IMPORTED_MODULE_2__reports_component__["a" /* ReportsComponent */]
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_2__reports_component__["a" /* ReportsComponent */]]
    })
], ReportsModule);

//# sourceMappingURL=reports.module.js.map

/***/ }),

/***/ 120:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__(26);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise__ = __webpack_require__(257);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__ = __webpack_require__(256);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch__ = __webpack_require__(254);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_add_operator_catch__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_BehaviorSubject__ = __webpack_require__(90);
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
        this.listData = new Array();
        this.initializeDataService();
    }
    ReportsService.prototype.initializeDataService = function () {
        if (!this.listData$) {
            this.listData$ = new __WEBPACK_IMPORTED_MODULE_5_rxjs_BehaviorSubject__["BehaviorSubject"](new Array());
            this.getList();
        }
    };
    ReportsService.prototype.subscribeToDataService = function () {
        return this.listData$.asObservable();
    };
    ReportsService.prototype.getReport = function (report, o) {
        // TODO throw exception if report is not populated
        var params = this.encodeData(o);
        var uri = '/api/reports';
        if (report) {
            uri = uri + '/' + report;
        }
        if (report && params) {
            uri = uri + '?' + params;
        }
        console.log('reportsService.getReport: ' + uri);
        return this.http.get(uri)
            .map(function (res) { return res.json().data; })
            .catch(this.handleError);
    };
    ReportsService.prototype.getList = function () {
        var _this = this;
        var uri = '/api/reports';
        console.log('reportsService.getList: ' + uri);
        this.http.get(uri)
            .map(function (res) { return res.json().data; })
            .catch(this.handleError)
            .subscribe(function (data) {
            _this.listData = data;
            _this.listData$.next(data);
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

/***/ 180:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(23)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 181:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(23)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 249:
/***/ (function(module, exports) {

module.exports = "<h1>\r\n  {{title}}\r\n</h1>\r\n<app-reports>loading reports...</app-reports>\r\n"

/***/ }),

/***/ 250:
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-g\">\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <p-dropdown [options]=\"reportsDropDown\" (onChange)=\"getView()\" [(ngModel)]=\"selectedReportID\" [filter]=\"true\" [style]=\"{'width':'20em'}\"></p-dropdown>\r\n    <!--<button pButton type=\"button\" icon=\"fa-refresh\" (click)=\"getList()\" iconPos=\"left\"></button>-->\r\n  </div>\r\n\r\n  <div class=\"ui-g-12 ui-md-6 ui-lg-3\">\r\n    <p-calendar  placeholder=\"Start date\" (onSelect)=\"getView()\" (onBlur)=\"getView()\" [(ngModel)]=\"o.beginDate\" [showIcon]=\"true\" dataType=\"string\"></p-calendar>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-6 ui-lg-3\">\r\n    <p-calendar placeholder=\"End date\" (onSelect)=\"getView()\" (onBlur)=\"getView()\" [(ngModel)]=\"o.endDate\" [showIcon]=\"true\" dataType=\"string\"></p-calendar>\r\n  </div>\r\n\r\n</div>\r\n<p-dialog header=\"{{title}}\" [(visible)]=\"display\">\r\n  {{description}}\r\n</p-dialog>\r\n<div>\r\n<p-dataTable\r\n  #dt\r\n  [value]=\"viewData\"\r\n  sortField=\"value\"\r\n  sortOrder=\"-1\"\r\n  sortMode=\"single\">\r\n  <p-header>\r\n    <div class=\"ui-helper-clearfix\">\r\n      <button type=\"button\" pButton icon=\"fa-download\" iconPos=\"left\" label=\"CSV\" (click)=\"getExport(dt)\" style=\"float:left\"></button>\r\n      <button pButton type=\"button\" icon=\"fa-question\" (click)=\"showDialog()\" iconPos=\"left\" style=\"float:right\"></button>\r\n    </div>\r\n  </p-header>\r\n  <p-column field=\"label\" header=\"{{headerLabel}}\" [sortable]=\"true\" class=\"removeBgImage\"></p-column>\r\n  <p-column field=\"value\" header=\"{{headerValue}}\" [sortable]=\"true\" class=\"removeBgImage\"></p-column>\r\n</p-dataTable>\r\n</div>\r\n"

/***/ }),

/***/ 299:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(107);


/***/ }),

/***/ 73:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
var environment = {
    production: true
};
//# sourceMappingURL=environment.js.map

/***/ })

},[299]);
//# sourceMappingURL=main.bundle.js.map