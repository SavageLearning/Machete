webpackJsonp([5,8],{

/***/ 107:
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"app/employers/employers.module": [
		310,
		2
	],
	"app/exports/exports.module": [
		311,
		0
	],
	"app/reports/reports.module": [
		312,
		1
	]
};
function webpackAsyncContext(req) {
	var ids = map[req];	if(!ids)
		return Promise.reject(new Error("Cannot find module '" + req + "'."));
	return __webpack_require__.e(ids[1]).then(function() {
		return __webpack_require__(ids[0]);
	});
};
webpackAsyncContext.keys = function webpackAsyncContextKeys() {
	return Object.keys(map);
};
module.exports = webpackAsyncContext;
webpackAsyncContext.id = 107;


/***/ }),

/***/ 108:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__(127);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__(132);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__(76);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__ = __webpack_require__(141);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__);





if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["enableProdMode"])();
}
__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 129:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__(8);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__can_deactivate_guard_service__ = __webpack_require__(135);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__selective_preloading_strategy__ = __webpack_require__(138);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppRoutingModule; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var appRoutes = [
    {
        path: 'employers',
        loadChildren: 'app/employers/employers.module#EmployersModule'
    },
    {
        path: 'reports',
        loadChildren: 'app/reports/reports.module#ReportsModule'
    },
    {
        path: 'exports',
        loadChildren: 'app/exports/exports.module#ExportsModule'
    },
    //{ path: '**', component: PageNotFoundComponent }
    { path: '**', redirectTo: '/reports' }
];
var AppRoutingModule = (function () {
    function AppRoutingModule() {
    }
    return AppRoutingModule;
}());
AppRoutingModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
        imports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forRoot(appRoutes, { preloadingStrategy: __WEBPACK_IMPORTED_MODULE_3__selective_preloading_strategy__["a" /* SelectivePreloadingStrategy */] })
        ],
        exports: [
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
        ],
        providers: [
            __WEBPACK_IMPORTED_MODULE_2__can_deactivate_guard_service__["a" /* CanDeactivateGuard */],
            __WEBPACK_IMPORTED_MODULE_3__selective_preloading_strategy__["a" /* SelectivePreloadingStrategy */]
        ]
    })
], AppRoutingModule);

//# sourceMappingURL=app-routing.module.js.map

/***/ }),

/***/ 130:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppFooter; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var AppFooter = (function () {
    function AppFooter() {
    }
    return AppFooter;
}());
AppFooter = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-footer',
        template: "\n        <div class=\"footer\">\n            <div class=\"card clearfix\">\n                <span class=\"footer-text-left\">Machete</span>\n                <span class=\"footer-text-right\"><span class=\"ui-icon ui-icon-copyright\"></span>  <span>All Rights Reserved</span></span>\n            </div>\n        </div>\n    "
    })
], AppFooter);

//# sourceMappingURL=app.footer.component.js.map

/***/ }),

/***/ 131:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_animations__ = __webpack_require__(12);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common__ = __webpack_require__(1);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__(8);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__ = __webpack_require__(122);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__app_component__ = __webpack_require__(45);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppMenuComponent; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return AppSubMenu; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};






var AppMenuComponent = (function () {
    function AppMenuComponent(app) {
        this.app = app;
    }
    AppMenuComponent.prototype.ngOnInit = function () {
        this.model = [
            { label: 'Employers', icon: 'business', url: ['/Employer'] },
            { label: 'Work Orders', icon: 'work', url: ['/Workorder'] },
            { label: 'Dispatch', icon: 'today', url: ['/dispatch'] },
            { label: 'People', icon: 'people', url: ['/people'] },
            { label: 'Activities', icon: 'local_activity', url: ['/Activity'] },
            { label: 'Sign-ins', icon: 'track_changes', url: ['/workersignin'] },
            { label: 'Emails', icon: 'email', url: ['/email'] },
            { label: 'Reports', icon: 'subtitles', routerLink: ['/reports'] },
            { label: 'Exports', icon: 'file_download', routerLink: ['/exports'] }
        ];
    };
    return AppMenuComponent;
}());
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Boolean)
], AppMenuComponent.prototype, "reset", void 0);
AppMenuComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-menu',
        template: "\n        <ul app-submenu [item]=\"model\" root=\"true\" class=\"ultima-menu ultima-main-menu clearfix\" [reset]=\"reset\" visible=\"true\"></ul>\n    "
    }),
    __param(0, __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]; }))),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]) === "function" && _a || Object])
], AppMenuComponent);

var AppSubMenu = (function () {
    function AppSubMenu(app, router, location) {
        this.app = app;
        this.router = router;
        this.location = location;
    }
    AppSubMenu.prototype.itemClick = function (event, item, index) {
        //avoid processing disabled items
        if (item.disabled) {
            event.preventDefault();
            return true;
        }
        //activate current item and deactivate active sibling if any
        this.activeIndex = (this.activeIndex === index) ? null : index;
        //execute command
        if (item.command) {
            if (!item.eventEmitter) {
                item.eventEmitter = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
                item.eventEmitter.subscribe(item.command);
            }
            item.eventEmitter.emit({
                originalEvent: event,
                item: item
            });
        }
        //prevent hash change
        if (item.items || (!item.url && !item.routerLink)) {
            event.preventDefault();
        }
        //hide menu
        if (!item.items) {
            if (this.app.isHorizontal())
                this.app.resetMenu = true;
            else
                this.app.resetMenu = false;
            this.app.overlayMenuActive = false;
            this.app.staticMenuMobileActive = false;
        }
    };
    AppSubMenu.prototype.isActive = function (index) {
        return this.activeIndex === index;
    };
    Object.defineProperty(AppSubMenu.prototype, "reset", {
        get: function () {
            return this._reset;
        },
        set: function (val) {
            this._reset = val;
            if (this._reset && this.app.isHorizontal()) {
                this.activeIndex = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    return AppSubMenu;
}());
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["MenuItem"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["MenuItem"]) === "function" && _b || Object)
], AppSubMenu.prototype, "item", void 0);
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Boolean)
], AppSubMenu.prototype, "root", void 0);
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Boolean)
], AppSubMenu.prototype, "visible", void 0);
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
    __metadata("design:type", Boolean),
    __metadata("design:paramtypes", [Boolean])
], AppSubMenu.prototype, "reset", null);
AppSubMenu = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: '[app-submenu]',
        template: "\n        <ng-template ngFor let-child let-i=\"index\" [ngForOf]=\"(root ? item : item.items)\">\n            <li [ngClass]=\"{'active-menuitem': isActive(i)}\" *ngIf=\"child.visible === false ? false : true\">\n                <a [href]=\"child.url||'#'\" (click)=\"itemClick($event,child,i)\" class=\"ripplelink\" *ngIf=\"!child.routerLink\" [attr.tabindex]=\"!visible ? '-1' : null\" [attr.target]=\"child.target\">\n                    <i class=\"material-icons\">{{child.icon}}</i>\n                    <span>{{child.label}}</span>\n                    <i class=\"material-icons\" *ngIf=\"child.items\">keyboard_arrow_down</i>\n                </a>\n\n                <a (click)=\"itemClick($event,child,i)\" class=\"ripplelink\" *ngIf=\"child.routerLink\"\n                    [routerLink]=\"child.routerLink\" routerLinkActive=\"active-menuitem-routerlink\" [routerLinkActiveOptions]=\"{exact: true}\" [attr.tabindex]=\"!visible ? '-1' : null\" [attr.target]=\"child.target\">\n                    <i class=\"material-icons\">{{child.icon}}</i>\n                    <span>{{child.label}}</span>\n                    <i class=\"material-icons\" *ngIf=\"child.items\">keyboard_arrow_down</i>\n                </a>\n                <ul app-submenu [item]=\"child\" *ngIf=\"child.items\" [@children]=\"isActive(i) ? 'visible' : 'hidden'\" [visible]=\"isActive(i)\" [reset]=\"reset\"></ul>\n            </li>\n        </ng-template>\n    ",
        animations: [
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["trigger"])('children', [
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["state"])('hidden', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["style"])({
                    height: '0px'
                })),
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["state"])('visible', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["style"])({
                    height: '*'
                })),
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["transition"])('visible => hidden', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["transition"])('hidden => visible', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
            ])
        ]
    }),
    __param(0, __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]; }))),
    __metadata("design:paramtypes", [typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_2__angular_common__["Location"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_common__["Location"]) === "function" && _e || Object])
], AppSubMenu);

var _a, _b, _c, _d, _e;
//# sourceMappingURL=app.menu.component.js.map

/***/ }),

/***/ 132:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__(21);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_animations__ = __webpack_require__(128);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_routing_module__ = __webpack_require__(129);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__(6);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_http__ = __webpack_require__(36);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__app_component__ = __webpack_require__(45);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__app_menu_component__ = __webpack_require__(131);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__app_topbar_component__ = __webpack_require__(134);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__app_footer_component__ = __webpack_require__(130);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__app_profile_component__ = __webpack_require__(133);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__not_found_component__ = __webpack_require__(137);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__angular_router__ = __webpack_require__(8);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13_angular_in_memory_web_api__ = __webpack_require__(140);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__in_memory_data_service__ = __webpack_require__(136);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__environments_environment__ = __webpack_require__(76);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* unused harmony export getBackend */
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
















var AppModule = (function () {
    // Diagnostic only: inspect router configuration
    function AppModule(router) {
        console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
    }
    return AppModule;
}());
AppModule = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_3__angular_core__["NgModule"])({
        declarations: [
            __WEBPACK_IMPORTED_MODULE_6__app_component__["a" /* AppComponent */],
            __WEBPACK_IMPORTED_MODULE_7__app_menu_component__["a" /* AppMenuComponent */],
            __WEBPACK_IMPORTED_MODULE_7__app_menu_component__["b" /* AppSubMenu */],
            __WEBPACK_IMPORTED_MODULE_8__app_topbar_component__["a" /* AppTopBar */],
            __WEBPACK_IMPORTED_MODULE_9__app_footer_component__["a" /* AppFooter */],
            __WEBPACK_IMPORTED_MODULE_10__app_profile_component__["a" /* InlineProfileComponent */],
            __WEBPACK_IMPORTED_MODULE_11__not_found_component__["a" /* PageNotFoundComponent */]
        ],
        imports: [
            __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["BrowserModule"],
            __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_animations__["a" /* BrowserAnimationsModule */],
            __WEBPACK_IMPORTED_MODULE_2__app_routing_module__["a" /* AppRoutingModule */],
            __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormsModule"],
            __WEBPACK_IMPORTED_MODULE_5__angular_http__["a" /* HttpModule */],
            __WEBPACK_IMPORTED_MODULE_13_angular_in_memory_web_api__["a" /* InMemoryWebApiModule */].forRoot(__WEBPACK_IMPORTED_MODULE_14__in_memory_data_service__["a" /* InMemoryDataService */])
        ],
        providers: [
            {
                provide: __WEBPACK_IMPORTED_MODULE_5__angular_http__["b" /* XHRBackend */],
                useFactory: getBackend,
                deps: [__WEBPACK_IMPORTED_MODULE_3__angular_core__["Injector"], __WEBPACK_IMPORTED_MODULE_5__angular_http__["c" /* BrowserXhr */], __WEBPACK_IMPORTED_MODULE_5__angular_http__["d" /* XSRFStrategy */], __WEBPACK_IMPORTED_MODULE_5__angular_http__["e" /* ResponseOptions */]]
            }
        ],
        bootstrap: [__WEBPACK_IMPORTED_MODULE_6__app_component__["a" /* AppComponent */]]
    }),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_12__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_12__angular_router__["Router"]) === "function" && _a || Object])
], AppModule);

function getBackend(injector, browser, xsrf, options) {
    {
        if (__WEBPACK_IMPORTED_MODULE_15__environments_environment__["a" /* environment */].production) {
            return new __WEBPACK_IMPORTED_MODULE_5__angular_http__["b" /* XHRBackend */](browser, options, xsrf);
        }
        else {
            return __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_13_angular_in_memory_web_api__["b" /* inMemoryBackendServiceFactory */])(injector, new __WEBPACK_IMPORTED_MODULE_14__in_memory_data_service__["a" /* InMemoryDataService */](), {});
        }
    }
}
var _a;
//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ 133:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return InlineProfileComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var InlineProfileComponent = (function () {
    function InlineProfileComponent() {
    }
    InlineProfileComponent.prototype.onClick = function (event) {
        this.active = !this.active;
        event.preventDefault();
    };
    return InlineProfileComponent;
}());
InlineProfileComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'inline-profile',
        template: "\n        <div class=\"profile\" [ngClass]=\"{'profile-expanded':active}\">\n            <div class=\"profile-image\"></div>\n            <a href=\"#\" (click)=\"onClick($event)\">\n                <span class=\"profile-name\">Jimmy Carter</span>\n                <i class=\"material-icons\">keyboard_arrow_down</i>\n            </a>\n        </div>\n\n        <ul class=\"ultima-menu profile-menu\" [@menu]=\"active ? 'visible' : 'hidden'\">\n            <li role=\"menuitem\">\n                <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\n                    <i class=\"material-icons\">person</i>\n                    <span>Profile</span>\n                </a>\n            </li>\n            <li role=\"menuitem\">\n                <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\n                    <i class=\"material-icons\">power_settings_new</i>\n                    <span>Logout</span>\n                </a>\n            </li>\n        </ul>\n    ",
        animations: [
            __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["trigger"])('menu', [
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["state"])('hidden', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["style"])({
                    height: '0px'
                })),
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["state"])('visible', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["style"])({
                    height: '*'
                })),
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["transition"])('visible => hidden', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
                __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["transition"])('hidden => visible', __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
            ])
        ]
    })
], InlineProfileComponent);

//# sourceMappingURL=app.profile.component.js.map

/***/ }),

/***/ 134:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__app_component__ = __webpack_require__(45);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppTopBar; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};


var AppTopBar = (function () {
    function AppTopBar(app) {
        this.app = app;
    }
    return AppTopBar;
}());
AppTopBar = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-topbar',
        template: "\n        <div class=\"topbar clearfix\">\n            <div class=\"topbar-left\">            \n                <div class=\"logo\"></div>\n            </div>\n\n            <div class=\"topbar-right\">\n                <a id=\"menu-button\" href=\"#\" (click)=\"app.onMenuButtonClick($event)\">\n                    <i></i>\n                </a>\n            </div>\n        </div>\n    "
    }),
    __param(0, __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(__webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */]; }))),
    __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */]) === "function" && _a || Object])
], AppTopBar);

var _a;
//# sourceMappingURL=app.topbar.component.js.map

/***/ }),

/***/ 135:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return CanDeactivateGuard; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var CanDeactivateGuard = (function () {
    function CanDeactivateGuard() {
    }
    CanDeactivateGuard.prototype.canDeactivate = function (component) {
        return component.canDeactivate ? component.canDeactivate() : true;
    };
    return CanDeactivateGuard;
}());
CanDeactivateGuard = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])()
], CanDeactivateGuard);

//# sourceMappingURL=can-deactivate-guard.service.js.map

/***/ }),

/***/ 136:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_http__ = __webpack_require__(36);
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
        var SeattleCityReport = [
            {
                'id': '20160101-20170101-CityReport-ESL-2016',
                'label': 'Counts of members who accessed at least 12 hours of ESL classes',
                'year': 2016,
                'Jan': 10,
                'Feb': 10,
                'Mar': 8,
                'Apr': 19,
                'May': 14,
                'Jun': 10,
                'Jul': 10,
                'Aug': 8,
                'Sep': 2,
                'Oct': 10,
                'Nov': 14,
                'Dec': 5
            },
            {
                'id': '20160101-20170101-CityReport-FinEd-2016',
                'label': 'Counts of members who accessed finanacial literacy',
                'year': 2016,
                'Jan': 0,
                'Feb': 23,
                'Mar': 0,
                'Apr': 0,
                'May': 0,
                'Jun': 0,
                'Jul': 0,
                'Aug': 0,
                'Sep': 0,
                'Oct': 0,
                'Nov': 25,
                'Dec': 0
            },
            {
                'id': '20160101-20170101-CityReport-NewEnrolls-2016',
                'label': 'Newly enrolled in program (within time range)',
                'year': 2016,
                'Jan': 178,
                'Feb': 47,
                'Mar': 50,
                'Apr': 75,
                'May': 56,
                'Jun': 51,
                'Jul': 26,
                'Aug': 22,
                'Sep': 25,
                'Oct': 30,
                'Nov': 21,
                'Dec': 23
            },
            {
                'id': '20160101-20170101-CityReport-Training-2016',
                'label': 'Counts of members who participated in job skill training or workshops',
                'year': 2016,
                'Jan': 13,
                'Feb': 33,
                'Mar': 15,
                'Apr': 25,
                'May': 12,
                'Jun': 13,
                'Jul': 2,
                'Aug': 16,
                'Sep': 10,
                'Oct': 11,
                'Nov': 22,
                'Dec': 3
            },
            {
                'id': '20160101-20170101-CityReport-UndupCount-2016',
                'label': 'A2H1-0 count of unduplicated individuals securing day labor employment this month',
                'year': 2016,
                'Jan': 92,
                'Feb': 114,
                'Mar': 138,
                'Apr': 157,
                'May': 155,
                'Jun': 157,
                'Jul': 161,
                'Aug': 146,
                'Sep': 136,
                'Oct': 125,
                'Nov': 121,
                'Dec': 103
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
                'inputs': {
                    'beginDate': true,
                    'endDate': true,
                    'memberNumber': true
                },
                'columns': [
                    {
                        'field': 'id',
                        'header': 'id',
                        'visible': false
                    },
                    {
                        'field': 'label',
                        'header': 'label',
                        'visible': true
                    },
                    {
                        'field': 'value',
                        'header': 'value',
                        'visible': true
                    }
                ]
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
                'inputs': {
                    'beginDate': false,
                    'endDate': false,
                    'memberNumber': true
                },
                'columns': [
                    {
                        'field': 'id',
                        'header': 'id',
                        'visible': false
                    },
                    {
                        'field': 'label',
                        'header': 'label',
                        'visible': true
                    },
                    {
                        'field': 'value',
                        'header': 'value',
                        'visible': true
                    }
                ]
            },
            {
                'name': 'SeattleCityReport',
                'id': 21,
                'title': null,
                'commonName': 'Seattle City report',
                'description': 'Casa Latina\'s monthly numbers for the City of Seattle',
                'sqlquery': 'select\r\nconvert(varchar(8), @startDate, 112) + \'-\' + convert(varchar(8), @endDate, 112) + \'-CityReport-NewEnrolls-\' + convert(varchar(4), [year]) as id, \r\n    \'Newly enrolled in program (within time range)\' as label, \r\n    cast(year as int) as year, \r\n\tcast([1] as int) as \'Jan\', \r\n\tcast([2] as int) as \'Feb\', \r\n\tcast([3] as int) as \'Mar\', \r\n\tcast([4] as int) as \'Apr\',\r\n\tcast([5] as int) as \'May\', \r\n\tcast([6] as int) as \'Jun\', \r\n\tcast([7] as int) as \'Jul\', \r\n\tcast([8] as int) as \'Aug\',\r\n\tcast([9] as int) as \'Sep\', \r\n\tcast([10] as int) as \'Oct\', \r\n\tcast([11] as int) as \'Nov\', \r\n\tcast([12] as int) as \'Dec\'\r\nfrom\r\n(\r\n\tselect min(year(signindate)) as year, min(month(signindate)) as month, cardnum\r\n\tfrom \r\n\t(\r\n\t\tSELECT MIN(dateforsignin) AS signindate, dwccardnum as cardnum\r\n\t\tFROM dbo.WorkerSignins\r\n\t\tWHERE dateforsignin >= @startdate AND\r\n\t\tdateforsignin < @EnDdate\r\n\t\tGROUP BY dwccardnum\r\n\r\n\t\tunion \r\n\r\n\t\tselect min(dateforsignin) as singindate, dwccardnum as cardnum\r\n\t\tfrom activitysignins asi\r\n\t\twhere dateforsignin >= @startdate\r\n\t\tand dateforsignin < @enddate\r\n\t\tgroup by dwccardnum\r\n\t) \r\n\tas result_set\r\n\tgroup by  cardnum\r\n) as foo\r\nPIVOT  \r\n(  \r\ncount (cardnum)  \r\nFOR month IN  \r\n( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  \r\n) AS pvt \r\n\r\nunion \r\n\r\nselect\r\nconvert(varchar(8), @startDate, 112) + \'-\' + convert(varchar(8), @endDate, 112) + \'-CityReport-FinEd-\' + convert(varchar(4), [year]) as id, \r\n    \'Counts of members who accessed finanacial literacy\' as label, \r\n    cast(year as int) as year, \r\n\tcast([1] as int) as \'Jan\', \r\n\tcast([2] as int) as \'Feb\', \r\n\tcast([3] as int) as \'Mar\', \r\n\tcast([4] as int) as \'Apr\',\r\n\tcast([5] as int) as \'May\', \r\n\tcast([6] as int) as \'Jun\', \r\n\tcast([7] as int) as \'Jul\', \r\n\tcast([8] as int) as \'Aug\',\r\n\tcast([9] as int) as \'Sep\', \r\n\tcast([10] as int) as \'Oct\', \r\n\tcast([11] as int) as \'Nov\', \r\n\tcast([12] as int) as \'Dec\'\r\nfrom\r\n(\r\n\tselect min(year(signindate)) as year, min(month(signindate)) as month, cardnum\r\n\tfrom \r\n\t(\r\n\t\tSELECT ASIs.dwccardnum as cardnum, MIN(dateStart) as signindate\r\n\t\tFROM dbo.Activities Acts\r\n\t\tJOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID\r\n\t\tJOIN dbo.Lookups Ls ON Acts.name = Ls.ID\r\n\t\tWHERE Ls.ID = 179 AND dateStart >= @startDate AND dateStart <= @endDate\r\n\r\n\t\tGROUP BY ASIs.dwccardnum\r\n\t) \r\n\tas result_set\r\n\tgroup by  cardnum\r\n) as foo\r\nPIVOT  \r\n(  \r\ncount (cardnum)  \r\nFOR month IN  \r\n( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  \r\n) AS pvt \r\n\r\nunion \r\n\r\nselect\r\nconvert(varchar(8), @startDate, 112) + \'-\' + convert(varchar(8), @endDate, 112) + \'-CityReport-Training-\' + convert(varchar(4), [year]) as id, \r\n    \'Counts of members who participated in job skill training or workshops\' as label, \r\n    cast(year as int) as year, \r\n\tcast([1] as int) as \'Jan\', \r\n\tcast([2] as int) as \'Feb\', \r\n\tcast([3] as int) as \'Mar\', \r\n\tcast([4] as int) as \'Apr\',\r\n\tcast([5] as int) as \'May\', \r\n\tcast([6] as int) as \'Jun\', \r\n\tcast([7] as int) as \'Jul\', \r\n\tcast([8] as int) as \'Aug\',\r\n\tcast([9] as int) as \'Sep\', \r\n\tcast([10] as int) as \'Oct\', \r\n\tcast([11] as int) as \'Nov\', \r\n\tcast([12] as int) as \'Dec\'\r\nfrom\r\n(\r\n\tselect min(year(signindate)) as year, min(month(signindate)) as month, cardnum\r\n\tfrom \r\n\t(\r\n\t\tSELECT ASIs.dwccardnum as cardnum, MIN(dateStart) as signindate\r\n\t\tFROM dbo.Activities Acts\r\n\t\tJOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID\r\n\t\tJOIN dbo.Lookups Ls ON Acts.name = Ls.ID\r\n\t\tWHERE dateStart >= @startdate AND dateStart <= @enddate AND\r\n\t\t(Ls.ID = 182 OR Ls.ID = 181 OR Ls.ID = 180\r\n\t\tOR Ls.ID = 179 OR Ls.ID = 178 OR Ls.ID = 134\r\n\t\tOR Ls.ID = 168 OR Ls.ID = 156 OR Ls.ID = 152\r\n\t\tOR Ls.ID = 145 OR Ls.ID = 135 OR Ls.ID = 104)\r\n\r\nGROUP BY dwccardnum\r\n\t) \r\n\tas result_set\r\n\tgroup by  cardnum\r\n) as foo\r\nPIVOT  \r\n(  \r\ncount (cardnum)  \r\nFOR month IN  \r\n( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  \r\n) AS pvt \r\n\r\nunion \r\n\r\nselect\r\nconvert(varchar(8), @startDate, 112) + \'-\' + convert(varchar(8), @endDate, 112) + \'-CityReport-ESL-\' + convert(varchar(4), [year]) as id, \r\n    \'Counts of members who accessed at least 12 hours of ESL classes\' as label, \r\n    cast(year as int) as year, \r\n\tcast([1] as int) as \'Jan\', \r\n\tcast([2] as int) as \'Feb\', \r\n\tcast([3] as int) as \'Mar\', \r\n\tcast([4] as int) as \'Apr\',\r\n\tcast([5] as int) as \'May\', \r\n\tcast([6] as int) as \'Jun\', \r\n\tcast([7] as int) as \'Jul\', \r\n\tcast([8] as int) as \'Aug\',\r\n\tcast([9] as int) as \'Sep\', \r\n\tcast([10] as int) as \'Oct\', \r\n\tcast([11] as int) as \'Nov\', \r\n\tcast([12] as int) as \'Dec\'\r\nfrom\r\n(\r\n\tselect year, month, cardnum\r\n\tfrom \r\n\t(\r\n\t\t\tSELECT  year(dateStart) as year, month(datestart) as month, dwccardnum as cardnum,\r\n\t\t\tsum(DATEDIFF( minute, dateStart, dateEnd )) as Minutes\r\n\t\t\tfrom dbo.Activities Acts\r\n\r\n\t\t\tJOIN dbo.Lookups Ls ON Acts.name = Ls.ID\r\n\t\t\tJOIN dbo.ActivitySignins ASIs ON Acts.ID = ASIs.ActivityID\r\n\t\t\tWHERE text_en LIKE \'%English%\'\r\n\t\t\tAND dateStart >= @startdate AND dateend <= @EnDdate\r\n\t\t\tgroup by year(datestart), month(datestart), dwccardnum\r\n\t) as foo\r\n\twhere foo.minutes >= 720\r\n\r\n) as foo\r\nPIVOT  \r\n(  \r\ncount (cardnum)  \r\nFOR month IN  \r\n( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  \r\n) AS pvt\r\n\r\nunion \r\n\r\nselect\r\nconvert(varchar(8), @startDate, 112) + \'-\' + convert(varchar(8), @endDate, 112) + \'-CityReport-UndupCount-\' + convert(varchar(4), [year]) as id, \r\n\r\n    \'A2H1-0 count of unduplicated individuals securing day labor employment this month\' as label, \r\n    cast(year as int) as year, \r\n\tcast([1] as int) as \'Jan\', \r\n\tcast([2] as int) as \'Feb\', \r\n\tcast([3] as int) as \'Mar\', \r\n\tcast([4] as int) as \'Apr\',\r\n\tcast([5] as int) as \'May\', \r\n\tcast([6] as int) as \'Jun\', \r\n\tcast([7] as int) as \'Jul\', \r\n\tcast([8] as int) as \'Aug\',\r\n\tcast([9] as int) as \'Sep\', \r\n\tcast([10] as int) as \'Oct\', \r\n\tcast([11] as int) as \'Nov\', \r\n\tcast([12] as int) as \'Dec\'\r\nfrom\r\n(\r\n\tSELECT count(distinct(dwccardnum)) AS cardnum, year(dateTimeofWork) as year, month(dateTimeofWork) AS month\r\n\tfrom dbo.WorkAssignments WAs\r\n\tJOIN dbo.WorkOrders WOs ON WAs.workOrderID = WOs.ID\r\n\tJOIN dbo.Workers Ws on WAs.workerAssignedID = Ws.ID\r\n\tjoin dbo.lookups l on l.id = wos.status\r\n\tWHERE dateTimeofWork >= @startdate \r\n\tand dateTimeofWork <= @EnDdate\r\n\tand l.text_EN = \'Completed\'\r\n\tgroup by year(dateTimeofWork), month(dateTimeofWork)\r\n\r\n) as foo\r\nPIVOT  \r\n(  \r\nsum (cardnum)  \r\nFOR month IN  \r\n( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )  \r\n) AS pvt',
                'category': 'site-specific',
                'subcategory': null,
                'data': SeattleCityReport,
                'inputs': {
                    'beginDate': true,
                    'endDate': true,
                    'memberNumber': false
                },
                'columns': [
                    {
                        'field': 'id',
                        'header': 'id',
                        'visible': false
                    },
                    {
                        'field': 'label',
                        'header': 'label',
                        'visible': true
                    },
                    {
                        'field': 'year',
                        'header': 'year',
                        'visible': true
                    },
                    {
                        'field': 'Jan',
                        'header': 'Jan',
                        'visible': true
                    },
                    {
                        'field': 'Feb',
                        'header': 'Feb',
                        'visible': true
                    },
                    {
                        'field': 'Mar',
                        'header': 'Mar',
                        'visible': true
                    },
                    {
                        'field': 'Apr',
                        'header': 'Apr',
                        'visible': true
                    },
                    {
                        'field': 'May',
                        'header': 'May',
                        'visible': true
                    },
                    {
                        'field': 'Jun',
                        'header': 'Jun',
                        'visible': true
                    },
                    {
                        'field': 'Jul',
                        'header': 'Jul',
                        'visible': true
                    },
                    {
                        'field': 'Aug',
                        'header': 'Aug',
                        'visible': true
                    },
                    {
                        'field': 'Sep',
                        'header': 'Sep',
                        'visible': true
                    },
                    {
                        'field': 'Oct',
                        'header': 'Oct',
                        'visible': true
                    },
                    {
                        'field': 'Nov',
                        'header': 'Nov',
                        'visible': true
                    },
                    {
                        'field': 'Dec',
                        'header': 'Dec',
                        'visible': true
                    }
                ]
            }
        ];
        var Activities = [
            {
                'name': 'ID',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            },
            {
                'name': 'name',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            },
            {
                'name': 'type',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            },
            {
                'name': 'dateStart',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            },
            {
                'name': 'dateEnd',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            },
            {
                'name': 'teacher',
                'is_nullable': false,
                'system_type_name': 'nvarchar(max)',
                'include': true
            },
            {
                'name': 'notes',
                'is_nullable': true,
                'system_type_name': 'nvarchar(4000)',
                'include': true
            },
            {
                'name': 'datecreated',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            },
            {
                'name': 'dateupdated',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            },
            {
                'name': 'Createdby',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            },
            {
                'name': 'Updatedby',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            },
            {
                'name': 'recurring',
                'is_nullable': false,
                'system_type_name': 'bit',
                'include': true
            },
            {
                'name': 'firstID',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            },
            {
                'name': 'nameEN',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            },
            {
                'name': 'nameES',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            },
            {
                'name': 'typeEN',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            },
            {
                'name': 'typeES',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            }
        ];
        var ActivitySignins = [
            {
                'name': 'ID',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'ActivityID',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'dwccardnum',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'memberStatus',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'dateforsignin',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'datecreated',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'dateupdated',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'Createdby',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            }, {
                'name': 'Updatedby',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            }, {
                'name': 'personID',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'timeZoneOffset',
                'is_nullable': false,
                'system_type_name': 'float',
                'include': true
            }
        ];
        var Workers = [
            {
                'name': 'ID', 'is_nullable': false, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'typeOfWorkID',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'dateOfMembership', 'is_nullable': false, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'dateOfBirth',
                'is_nullable': true,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'memberStatus', 'is_nullable': false, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'memberReactivateDate',
                'is_nullable': true,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'active', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'RaceID',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'raceother', 'is_nullable': true, 'system_type_name': 'nvarchar(20)', 'include': true
            }, {
                'name': 'height',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            }, {
                'name': 'weight', 'is_nullable': true, 'system_type_name': 'nvarchar(10)', 'include': true
            }, {
                'name': 'englishlevelID',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'recentarrival', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'dateinUSA',
                'is_nullable': true,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'dateinseattle', 'is_nullable': true, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'disabled',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'disabilitydesc', 'is_nullable': true, 'system_type_name': 'nvarchar(50)', 'include': true
            }, {
                'name': 'maritalstatus',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'livewithchildren', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'numofchildren',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'incomeID', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'livealone',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'emcontUSAname', 'is_nullable': true, 'system_type_name': 'nvarchar(50)', 'include': true
            }, {
                'name': 'emcontUSArelation',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            }, {
                'name': 'emcontUSAphone', 'is_nullable': true, 'system_type_name': 'nvarchar(14)', 'include': true
            }, {
                'name': 'dwccardnum',
                'is_nullable': false,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'neighborhoodID', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'immigrantrefugee',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'countryoforiginID', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'emcontoriginname',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            }, {
                'name': 'emcontoriginrelation',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            }, {
                'name': 'emcontoriginphone',
                'is_nullable': true,
                'system_type_name': 'nvarchar(14)',
                'include': true
            }, {
                'name': 'memberexpirationdate', 'is_nullable': false, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'driverslicense',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'licenseexpirationdate', 'is_nullable': true, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'carinsurance',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'insuranceexpiration', 'is_nullable': true, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'ImageID',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'skill1', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'skill2',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'skill3', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'datecreated',
                'is_nullable': false,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'dateupdated', 'is_nullable': false, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'Createdby',
                'is_nullable': true,
                'system_type_name': 'nvarchar(30)',
                'include': true
            }, {
                'name': 'Updatedby', 'is_nullable': true, 'system_type_name': 'nvarchar(30)', 'include': true
            }, {
                'name': 'homeless',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'workerRating', 'is_nullable': true, 'system_type_name': 'real', 'include': true
            }, {
                'name': 'housingType',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'liveWithSpouse', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'liveWithDescription',
                'is_nullable': true,
                'system_type_name': 'nvarchar(1000)',
                'include': true
            }, {
                'name': 'americanBornChildren', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'numChildrenUnder18',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'educationLevel', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'farmLaborCharacteristics',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'wageTheftVictim', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'wageTheftRecoveryAmount',
                'is_nullable': true,
                'system_type_name': 'float',
                'include': true
            }, {
                'name': 'lastPaymentDate', 'is_nullable': true, 'system_type_name': 'datetime', 'include': true
            }, {
                'name': 'lastPaymentAmount',
                'is_nullable': true,
                'system_type_name': 'float',
                'include': true
            }, {
                'name': 'ownTools', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'healthInsurance',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'usVeteran', 'is_nullable': true, 'system_type_name': 'bit', 'include': true
            }, {
                'name': 'healthInsuranceDate',
                'is_nullable': true,
                'system_type_name': 'datetime',
                'include': true
            }, {
                'name': 'vehicleTypeID', 'is_nullable': true, 'system_type_name': 'int', 'include': true
            }, {
                'name': 'incomeSourceID',
                'is_nullable': true,
                'system_type_name': 'int',
                'include': true
            }, {
                'name': 'introToCenter', 'is_nullable': true, 'system_type_name': 'nvarchar(1000)', 'include': true
            }, {
                'name': 'lgbtq',
                'is_nullable': true,
                'system_type_name': 'bit',
                'include': true
            }, {
                'name': 'typeOfWork', 'is_nullable': true, 'system_type_name': 'nvarchar(max)', 'include': true
            }, {
                'name': 'memberStatusEN',
                'is_nullable': true,
                'system_type_name': 'nvarchar(50)',
                'include': true
            }, {
                'name': 'memberStatusES', 'is_nullable': true, 'system_type_name': 'nvarchar(50)', 'include': true
            }, {
                'name': 'fullNameAndID',
                'is_nullable': true,
                'system_type_name': 'nvarchar(100)',
                'include': true
            }, {
                'name': 'skillCodes', 'is_nullable': true, 'system_type_name': 'nvarchar(max)', 'include': true
            }
        ];
        var exports = [
            {
                'id': 'activities',
                'name': 'Activities',
                'data': Activities
            },
            {
                'id': 'activitysignins',
                'name': 'ActivitySignins',
                'data': ActivitySignins
            },
            {
                id: 'workers',
                name: 'Workers',
                data: Workers
            }
        ];
        return { exports: exports, reports: reports };
    };
    // intercept response from the default HTTP method handlers
    InMemoryDataService.prototype.responseInterceptor = function (response, reqInfo) {
        response.body = response.body; // matches web api controller
        var method = __WEBPACK_IMPORTED_MODULE_0__angular_http__["j" /* RequestMethod */][reqInfo.req.method].toUpperCase();
        var body = JSON.stringify(response.body);
        // console.log(`responseInterceptor: ${method} ${reqInfo.req.url}: \n${body}`);
        console.log("responseInterceptor: " + method + " " + reqInfo.req.url);
        if (typeof reqInfo.query === 'object') {
            // if query parameters present, replace object w/ data key's value.
            // useful for testing; matches API behavior
            response.body = response.body.data;
        }
        if (reqInfo.req.url === '/api/exports/activities' ||
            reqInfo.req.url === '/api/exports/activitysignins' ||
            reqInfo.req.url === '/api/exports/workers') {
            response.body = response.body.data;
        }
        return response;
    };
    InMemoryDataService.prototype.parseUrl = function (url) {
        try {
            var loc = this.getLocation(url);
            var drop = 0;
            var urlRoot = '';
            if (loc.host !== undefined) {
                // url for a server on a different host!
                // assume it's collection is actually here too.
                drop = 1; // the leading slash
                urlRoot = loc.protocol + '//' + loc.host + '/';
            }
            var path = loc.pathname.substring(drop);
            var _a = path.split('/'), base = _a[0], collectionName = _a[1], id = _a[2];
            var resourceUrl = urlRoot + base + '/' + collectionName + '/';
            collectionName = collectionName.split('.')[0]; // ignore anything after the '.', e.g., '.json'
            var query = loc.search && new __WEBPACK_IMPORTED_MODULE_0__angular_http__["k" /* URLSearchParams */](loc.search.substr(1));
            var result = { base: base, collectionName: collectionName, id: id, query: query, resourceUrl: resourceUrl };
            console.log('parsedUrl: ' + JSON.stringify(result));
            return result;
        }
        catch (err) {
            var msg = "unable to parse url '" + url + "'; original error: " + err.message;
            throw new Error(msg);
        }
    };
    InMemoryDataService.prototype.getLocation = function (href) {
        var l = document.createElement('a');
        l.href = href;
        return l;
    };
    ;
    return InMemoryDataService;
}());

//# sourceMappingURL=in-memory-data.service.js.map

/***/ }),

/***/ 137:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__(0);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PageNotFoundComponent; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var PageNotFoundComponent = (function () {
    function PageNotFoundComponent() {
    }
    return PageNotFoundComponent;
}());
PageNotFoundComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        template: '<h2>Page not found</h2>'
    })
], PageNotFoundComponent);

//# sourceMappingURL=not-found.component.js.map

/***/ }),

/***/ 138:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of__ = __webpack_require__(273);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__(0);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__(5);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SelectivePreloadingStrategy; });
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var SelectivePreloadingStrategy = (function () {
    function SelectivePreloadingStrategy() {
        this.preloadedModules = [];
    }
    SelectivePreloadingStrategy.prototype.preload = function (route, load) {
        if (route.data && route.data['preload']) {
            // add the route path to the preloaded module array
            this.preloadedModules.push(route.path);
            // log the route path to the console
            console.log('Preloaded: ' + route.path);
            return load();
        }
        else {
            return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["Observable"].of(null);
        }
    };
    return SelectivePreloadingStrategy;
}());
SelectivePreloadingStrategy = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])()
], SelectivePreloadingStrategy);

//# sourceMappingURL=selective-preloading-strategy.js.map

/***/ }),

/***/ 199:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(20)(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ 269:
/***/ (function(module, exports) {

module.exports = "<div class=\"layout-wrapper\" [ngClass]=\"{'layout-compact':layoutCompact}\">\r\n\r\n    <div #layoutContainer class=\"layout-container\" \r\n            [ngClass]=\"{'menu-layout-static': !isOverlay(),\r\n            'menu-layout-overlay': isOverlay(),\r\n            'layout-menu-overlay-active': overlayMenuActive,\r\n            'menu-layout-horizontal': isHorizontal(),\r\n            'layout-menu-static-inactive': staticMenuDesktopInactive,\r\n            'layout-menu-static-active': staticMenuMobileActive}\">\r\n\r\n        <app-topbar></app-topbar>\r\n\r\n        <div class=\"layout-menu\" [ngClass]=\"{'layout-menu-dark':darkMenu}\" (click)=\"onMenuClick($event)\">\r\n            <div #layoutMenuScroller class=\"nano\">\r\n                <div class=\"nano-content menu-scroll-content\">\r\n                    <inline-profile *ngIf=\"profileMode=='inline'&&!isHorizontal()\"></inline-profile>\r\n                    <app-menu [reset]=\"resetMenu\"></app-menu>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        \r\n        <div class=\"layout-main\">\r\n            <router-outlet></router-outlet>\r\n            \r\n            <app-footer></app-footer>\r\n        </div>\r\n        \r\n        <div class=\"layout-mask\"></div>\r\n    </div>\r\n\r\n</div>"

/***/ }),

/***/ 307:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(108);


/***/ }),

/***/ 45:
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
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var MenuOrientation;
(function (MenuOrientation) {
    MenuOrientation[MenuOrientation["STATIC"] = 0] = "STATIC";
    MenuOrientation[MenuOrientation["OVERLAY"] = 1] = "OVERLAY";
    MenuOrientation[MenuOrientation["HORIZONTAL"] = 2] = "HORIZONTAL";
})(MenuOrientation || (MenuOrientation = {}));
;
var AppComponent = (function () {
    function AppComponent(renderer) {
        this.renderer = renderer;
        this.layoutCompact = false;
        this.layoutMode = MenuOrientation.STATIC;
        this.darkMenu = true;
        this.profileMode = 'inline';
    }
    AppComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.layoutContainer = this.layourContainerViewChild.nativeElement;
        this.layoutMenuScroller = this.layoutMenuScrollerViewChild.nativeElement;
        //hides the horizontal submenus or top menu if outside is clicked
        this.documentClickListener = this.renderer.listenGlobal('body', 'click', function (event) {
            if (!_this.topbarItemClick) {
                _this.activeTopbarItem = null;
                _this.topbarMenuActive = false;
            }
            if (!_this.menuClick && _this.isHorizontal()) {
                _this.resetMenu = true;
            }
            _this.topbarItemClick = false;
            _this.menuClick = false;
        });
        setTimeout(function () {
            jQuery(_this.layoutMenuScroller).nanoScroller({ flash: true });
        }, 10);
    };
    AppComponent.prototype.onMenuButtonClick = function (event) {
        this.rotateMenuButton = !this.rotateMenuButton;
        this.topbarMenuActive = false;
        if (this.layoutMode === MenuOrientation.OVERLAY) {
            this.overlayMenuActive = !this.overlayMenuActive;
        }
        else {
            if (this.isDesktop())
                this.staticMenuDesktopInactive = !this.staticMenuDesktopInactive;
            else
                this.staticMenuMobileActive = !this.staticMenuMobileActive;
        }
        event.preventDefault();
    };
    AppComponent.prototype.onMenuClick = function ($event) {
        var _this = this;
        this.menuClick = true;
        this.resetMenu = false;
        if (!this.isHorizontal()) {
            setTimeout(function () {
                jQuery(_this.layoutMenuScroller).nanoScroller();
            }, 500);
        }
    };
    AppComponent.prototype.onTopbarMenuButtonClick = function (event) {
        this.topbarItemClick = true;
        this.topbarMenuActive = !this.topbarMenuActive;
        if (this.overlayMenuActive || this.staticMenuMobileActive) {
            this.rotateMenuButton = false;
            this.overlayMenuActive = false;
            this.staticMenuMobileActive = false;
        }
        event.preventDefault();
    };
    AppComponent.prototype.onTopbarItemClick = function (event, item) {
        this.topbarItemClick = true;
        if (this.activeTopbarItem === item)
            this.activeTopbarItem = null;
        else
            this.activeTopbarItem = item;
        event.preventDefault();
    };
    AppComponent.prototype.isTablet = function () {
        var width = window.innerWidth;
        return width <= 1024 && width > 640;
    };
    AppComponent.prototype.isDesktop = function () {
        return window.innerWidth > 1024;
    };
    AppComponent.prototype.isMobile = function () {
        return window.innerWidth <= 640;
    };
    AppComponent.prototype.isOverlay = function () {
        return this.layoutMode === MenuOrientation.OVERLAY;
    };
    AppComponent.prototype.isHorizontal = function () {
        return this.layoutMode === MenuOrientation.HORIZONTAL;
    };
    AppComponent.prototype.changeToStaticMenu = function () {
        this.layoutMode = MenuOrientation.STATIC;
    };
    AppComponent.prototype.changeToOverlayMenu = function () {
        this.layoutMode = MenuOrientation.OVERLAY;
    };
    AppComponent.prototype.changeToHorizontalMenu = function () {
        this.layoutMode = MenuOrientation.HORIZONTAL;
    };
    AppComponent.prototype.ngOnDestroy = function () {
        if (this.documentClickListener) {
            this.documentClickListener();
        }
        jQuery(this.layoutMenuScroller).nanoScroller({ flash: true });
    };
    return AppComponent;
}());
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('layoutContainer'),
    __metadata("design:type", typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"]) === "function" && _a || Object)
], AppComponent.prototype, "layourContainerViewChild", void 0);
__decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('layoutMenuScroller'),
    __metadata("design:type", typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"]) === "function" && _b || Object)
], AppComponent.prototype, "layoutMenuScrollerViewChild", void 0);
AppComponent = __decorate([
    __webpack_require__.i(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
        selector: 'app-root',
        template: __webpack_require__(269),
        styles: [__webpack_require__(199)]
    }),
    __metadata("design:paramtypes", [typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_0__angular_core__["Renderer"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_core__["Renderer"]) === "function" && _c || Object])
], AppComponent);

var _a, _b, _c;
//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ 76:
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
var environment = {
    production: true
};
//# sourceMappingURL=environment.js.map

/***/ })

},[307]);
//# sourceMappingURL=main.bundle.js.map