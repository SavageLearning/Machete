webpackJsonp(["main"],{

/***/ "../../../../../src/$$_gendir lazy recursive":
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	return new Promise(function(resolve, reject) { reject(new Error("Cannot find module '" + req + "'.")); });
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "../../../../../src/$$_gendir lazy recursive";

/***/ }),

/***/ "../../../../../src/app/app-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__selective_preloading_strategy__ = __webpack_require__("../../../../../src/app/selective-preloading-strategy.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__auth___ = __webpack_require__("../../../../../src/app/auth/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__auth_register_register_component__ = __webpack_require__("../../../../../src/app/auth/register/register.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var appRoutes = [
    {
        path: '',
        redirectTo: '/welcome',
        pathMatch: 'full'
    },
    {
        path: 'welcome',
        component: __WEBPACK_IMPORTED_MODULE_5__auth___["d" /* WelcomeComponent */],
    },
    {
        path: 'register',
        component: __WEBPACK_IMPORTED_MODULE_6__auth_register_register_component__["a" /* RegisterComponent */],
    },
    {
        path: 'dashboard',
        component: __WEBPACK_IMPORTED_MODULE_5__auth___["b" /* DashboardComponent */],
        canActivate: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
    },
    {
        path: 'unauthorized',
        component: __WEBPACK_IMPORTED_MODULE_5__auth___["c" /* UnauthorizedComponent */]
    },
    // Used to receive redirect from Identity server
    {
        path: 'authorize',
        component: __WEBPACK_IMPORTED_MODULE_5__auth___["a" /* AuthorizeComponent */]
    },
    //{ path: '**', component: PageNotFoundComponent }
    { path: '**', redirectTo: '/welcome' }
];
var AppRoutingModule = (function () {
    function AppRoutingModule() {
        console.log('.ctor');
    }
    AppRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                //AppComponent,
                __WEBPACK_IMPORTED_MODULE_5__auth___["c" /* UnauthorizedComponent */],
                __WEBPACK_IMPORTED_MODULE_5__auth___["b" /* DashboardComponent */],
                __WEBPACK_IMPORTED_MODULE_5__auth___["d" /* WelcomeComponent */],
                __WEBPACK_IMPORTED_MODULE_6__auth_register_register_component__["a" /* RegisterComponent */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_2__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forRoot(appRoutes, { preloadingStrategy: __WEBPACK_IMPORTED_MODULE_4__selective_preloading_strategy__["a" /* SelectivePreloadingStrategy */] })
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */],
                __WEBPACK_IMPORTED_MODULE_4__selective_preloading_strategy__["a" /* SelectivePreloadingStrategy */]
            ]
        }),
        __metadata("design:paramtypes", [])
    ], AppRoutingModule);
    return AppRoutingModule;
}());

//# sourceMappingURL=app-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/app.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"layout-wrapper\" [ngClass]=\"{'layout-compact':layoutCompact}\">\r\n\r\n    <div #layoutContainer class=\"layout-container\" \r\n            [ngClass]=\"{'menu-layout-static': !isOverlay(),\r\n            'menu-layout-overlay': isOverlay(),\r\n            'layout-menu-overlay-active': overlayMenuActive,\r\n            'menu-layout-horizontal': isHorizontal(),\r\n            'layout-menu-static-inactive': staticMenuDesktopInactive,\r\n            'layout-menu-static-active': staticMenuMobileActive}\">\r\n\r\n        <app-topbar></app-topbar>\r\n\r\n        <div class=\"layout-menu\" [ngClass]=\"{'layout-menu-dark':darkMenu}\" (click)=\"onMenuClick($event)\">\r\n            <div #layoutMenuScroller class=\"nano\">\r\n                <div class=\"nano-content menu-scroll-content\">\r\n                    <inline-profile *ngIf=\"profileMode=='inline'&&!isHorizontal()\"></inline-profile>\r\n                    <app-menu [reset]=\"resetMenu\"></app-menu>\r\n                </div>\r\n            </div>\r\n        </div>\r\n        \r\n        <div class=\"layout-main\">\r\n            <router-outlet></router-outlet>\r\n            \r\n            <app-footer></app-footer>\r\n        </div>\r\n        \r\n        <div class=\"layout-mask\"></div>\r\n    </div>\r\n\r\n</div>"

/***/ }),

/***/ "../../../../../src/app/app.component.scss":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/app.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




console.log('environment.name:', __WEBPACK_IMPORTED_MODULE_1__environments_environment__["a" /* environment */].name);
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
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('layoutContainer'),
        __metadata("design:type", typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"]) === "function" && _a || Object)
    ], AppComponent.prototype, "layourContainerViewChild", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('layoutMenuScroller'),
        __metadata("design:type", typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"]) === "function" && _b || Object)
    ], AppComponent.prototype, "layoutMenuScrollerViewChild", void 0);
    AppComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-root',
            template: __webpack_require__("../../../../../src/app/app.component.html"),
            styles: [__webpack_require__("../../../../../src/app/app.component.scss")],
            providers: [__WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__["a" /* LookupsService */], __WEBPACK_IMPORTED_MODULE_2__configs_configs_service__["a" /* ConfigsService */]]
        }),
        __metadata("design:paramtypes", [typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_0__angular_core__["Renderer"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_0__angular_core__["Renderer"]) === "function" && _c || Object])
    ], AppComponent);
    return AppComponent;
    var _a, _b, _c;
}());

//# sourceMappingURL=app.component.js.map

/***/ }),

/***/ "../../../../../src/app/app.footer.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppFooter; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var AppFooter = (function () {
    function AppFooter() {
    }
    AppFooter = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-footer',
            template: "\n        <div class=\"footer\">\n            <div class=\"card clearfix\">\n                <span class=\"footer-text-left\">Machete</span>\n                <span class=\"footer-text-right\"><span class=\"ui-icon ui-icon-copyright\"></span>  <span>All Rights Reserved</span></span>\n            </div>\n        </div>\n    "
        })
    ], AppFooter);
    return AppFooter;
}());

//# sourceMappingURL=app.footer.component.js.map

/***/ }),

/***/ "../../../../../src/app/app.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__("../../../platform-browser/@angular/platform-browser.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_animations__ = __webpack_require__("../../../platform-browser/@angular/platform-browser/animations.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_routing_module__ = __webpack_require__("../../../../../src/app/app-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__menu_app_menu_component__ = __webpack_require__("../../../../../src/app/menu/app.menu.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__app_topbar_component__ = __webpack_require__("../../../../../src/app/app.topbar.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__app_footer_component__ = __webpack_require__("../../../../../src/app/app.footer.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__menu_app_profile_component__ = __webpack_require__("../../../../../src/app/menu/app.profile.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__not_found_component__ = __webpack_require__("../../../../../src/app/not-found.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__auth_authorize_authorize_component__ = __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__shared_services_token_interceptor__ = __webpack_require__("../../../../../src/app/shared/services/token.interceptor.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__online_orders_online_orders_module__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__reports_reports_module__ = __webpack_require__("../../../../../src/app/reports/reports.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__exports_exports_module__ = __webpack_require__("../../../../../src/app/exports/exports.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__work_orders_work_orders_module__ = __webpack_require__("../../../../../src/app/work-orders/work-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_22__employers_employers_module__ = __webpack_require__("../../../../../src/app/employers/employers.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_23_oidc_client__);
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
        if (!__WEBPACK_IMPORTED_MODULE_14__environments_environment__["a" /* environment */].production) {
            __WEBPACK_IMPORTED_MODULE_23_oidc_client__["Log"].level = __WEBPACK_IMPORTED_MODULE_23_oidc_client__["Log"].INFO;
            __WEBPACK_IMPORTED_MODULE_23_oidc_client__["Log"].logger = console;
        }
        console.log('.ctor');
    }
    AppModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_7__app_component__["a" /* AppComponent */],
                __WEBPACK_IMPORTED_MODULE_8__menu_app_menu_component__["a" /* AppMenuComponent */],
                __WEBPACK_IMPORTED_MODULE_8__menu_app_menu_component__["b" /* AppSubMenu */],
                __WEBPACK_IMPORTED_MODULE_9__app_topbar_component__["a" /* AppTopBar */],
                __WEBPACK_IMPORTED_MODULE_10__app_footer_component__["a" /* AppFooter */],
                __WEBPACK_IMPORTED_MODULE_11__menu_app_profile_component__["a" /* InlineProfileComponent */],
                __WEBPACK_IMPORTED_MODULE_12__not_found_component__["a" /* PageNotFoundComponent */],
                __WEBPACK_IMPORTED_MODULE_16__auth_authorize_authorize_component__["a" /* AuthorizeComponent */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["BrowserModule"],
                __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_animations__["a" /* BrowserAnimationsModule */],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormsModule"],
                __WEBPACK_IMPORTED_MODULE_6__angular_http__["c" /* HttpModule */],
                __WEBPACK_IMPORTED_MODULE_5__angular_common_http__["c" /* HttpClientModule */],
                __WEBPACK_IMPORTED_MODULE_19__reports_reports_module__["a" /* ReportsModule */],
                __WEBPACK_IMPORTED_MODULE_18__online_orders_online_orders_module__["a" /* OnlineOrdersModule */],
                __WEBPACK_IMPORTED_MODULE_20__exports_exports_module__["a" /* ExportsModule */],
                __WEBPACK_IMPORTED_MODULE_21__work_orders_work_orders_module__["a" /* WorkOrdersModule */],
                __WEBPACK_IMPORTED_MODULE_22__employers_employers_module__["a" /* EmployersModule */],
                __WEBPACK_IMPORTED_MODULE_2__app_routing_module__["a" /* AppRoutingModule */]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_15__shared_services_auth_service__["a" /* AuthService */],
                {
                    provide: __WEBPACK_IMPORTED_MODULE_5__angular_common_http__["a" /* HTTP_INTERCEPTORS */],
                    useClass: __WEBPACK_IMPORTED_MODULE_17__shared_services_token_interceptor__["a" /* TokenInterceptor */],
                    multi: true
                }
            ],
            bootstrap: [__WEBPACK_IMPORTED_MODULE_7__app_component__["a" /* AppComponent */]]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_13__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_13__angular_router__["Router"]) === "function" && _a || Object])
    ], AppModule);
    return AppModule;
    var _a;
}());

//# sourceMappingURL=app.module.js.map

/***/ }),

/***/ "../../../../../src/app/app.topbar.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppTopBar; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
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
    AppTopBar = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-topbar',
            template: "\n        <div class=\"topbar clearfix\">\n            <div class=\"topbar-left\">\n                <div class=\"logo\"></div>\n            </div>\n\n            <div class=\"topbar-right\">\n                <a id=\"menu-button\" href=\"#\" (click)=\"app.onMenuButtonClick($event)\">\n                    <i></i>\n                </a>\n            </div>\n        </div>\n    "
        }),
        __param(0, Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */]; }))),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */]) === "function" && _a || Object])
    ], AppTopBar);
    return AppTopBar;
    var _a;
}());

//# sourceMappingURL=app.topbar.component.js.map

/***/ }),

/***/ "../../../../../src/app/auth/authorize/authorize.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/auth/authorize/authorize.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\r\n  authorize works!\r\n</p>\r\n"

/***/ }),

/***/ "../../../../../src/app/auth/authorize/authorize.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthorizeComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var AuthorizeComponent = (function () {
    function AuthorizeComponent(auth, router) {
        this.auth = auth;
        this.router = router;
    }
    AuthorizeComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.auth.endSigninMainWindow()
            .subscribe(function (user) {
            // not sure why i have to copy to local variable.
            // https://github.com/Microsoft/TypeScript/wiki/'this'-in-TypeScript
            var rtr = _this.router;
            console.log('endSigninMainWindow.user: ', user);
            _this.auth.getUserEmitter().emit(user);
            if (user.state) {
                rtr.navigate([user.state]);
            }
        }, function (err) {
            console.error('endSigninMainWindow returned: ', err);
        });
    };
    AuthorizeComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-authorize',
            template: __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/authorize/authorize.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]) === "function" && _b || Object])
    ], AuthorizeComponent);
    return AuthorizeComponent;
    var _a, _b;
}());

//# sourceMappingURL=authorize.component.js.map

/***/ }),

/***/ "../../../../../src/app/auth/dashboard/dashboard.component.html":
/***/ (function(module, exports) {

module.exports = "<div>\r\n  <button (click)=\"clearState()\" id='clearState'>clear stale state</button>\r\n  <button (click)=\"getUser()\" id='getUser'>get user</button>\r\n  <button (click)=\"removeUser()\" id='removeUser'>remove user</button>\r\n</div>\r\n<div>\r\n  <button (click)=\"startSigninMainWindow()\" id='startSigninMainWindow'>start signin main window</button>\r\n  <button (click)=\"endSigninMainWindow()\" id='endSigninMainWindow'>end signin main window</button>\r\n</div>\r\n<div>\r\n  <button (click)=\"startSignoutMainWindow()\" id='startSignoutMainWindow'>start signout main window</button>\r\n  <button (click)=\"endSignoutMainWindow()\" id='endSignoutMainWindow'>end signout main window</button>\r\n</div>\r\n<div *ngIf=\"_user\">\r\n  <pre>{{_user | json}}</pre>\r\n</div>"

/***/ }),

/***/ "../../../../../src/app/auth/dashboard/dashboard.component.scss":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "div {\n  padding: 10px;\n  margin: 10px; }\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/auth/dashboard/dashboard.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return DashboardComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var DashboardComponent = (function () {
    function DashboardComponent(authService) {
        this.authService = authService;
    }
    DashboardComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.loadedUserSub = this.authService.getUserEmitter()
            .subscribe(function (user) {
            _this._user = user;
        });
    };
    DashboardComponent.prototype.clearState = function () {
        this.authService.clearState();
    };
    DashboardComponent.prototype.getUser = function () {
        this.authService.getUser();
    };
    DashboardComponent.prototype.removeUser = function () {
        this.authService.removeUser();
    };
    DashboardComponent.prototype.startSigninMainWindow = function () {
        this.authService.startSigninMainWindow();
    };
    DashboardComponent.prototype.endSigninMainWindow = function () {
        this.authService.endSigninMainWindow();
    };
    DashboardComponent.prototype.startSignoutMainWindow = function () {
        this.authService.startSignoutMainWindow();
    };
    DashboardComponent.prototype.endSignoutMainWindow = function () {
        this.authService.endSigninMainWindow();
    };
    DashboardComponent.prototype.ngOnDestroy = function () {
        if (this.loadedUserSub.unsubscribe()) {
            this.loadedUserSub.unsubscribe();
        }
    };
    DashboardComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-dashboard',
            template: __webpack_require__("../../../../../src/app/auth/dashboard/dashboard.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/dashboard/dashboard.component.scss")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */]) === "function" && _a || Object])
    ], DashboardComponent);
    return DashboardComponent;
    var _a;
}());

//# sourceMappingURL=dashboard.component.js.map

/***/ }),

/***/ "../../../../../src/app/auth/index.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__unauthorized_unauthorized_component__ = __webpack_require__("../../../../../src/app/auth/unauthorized/unauthorized.component.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "c", function() { return __WEBPACK_IMPORTED_MODULE_0__unauthorized_unauthorized_component__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__welcome_welcome_component__ = __webpack_require__("../../../../../src/app/auth/welcome/welcome.component.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "d", function() { return __WEBPACK_IMPORTED_MODULE_1__welcome_welcome_component__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__authorize_authorize_component__ = __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_2__authorize_authorize_component__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__dashboard_dashboard_component__ = __webpack_require__("../../../../../src/app/auth/dashboard/dashboard.component.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_3__dashboard_dashboard_component__["a"]; });




//# sourceMappingURL=index.js.map

/***/ }),

/***/ "../../../../../src/app/auth/register/register.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/auth/register/register.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\r\n  register works!\r\n</p>\r\n"

/***/ }),

/***/ "../../../../../src/app/auth/register/register.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return RegisterComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var RegisterComponent = (function () {
    function RegisterComponent() {
    }
    RegisterComponent.prototype.ngOnInit = function () {
    };
    RegisterComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-register',
            template: __webpack_require__("../../../../../src/app/auth/register/register.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/register/register.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], RegisterComponent);
    return RegisterComponent;
}());

//# sourceMappingURL=register.component.js.map

/***/ }),

/***/ "../../../../../src/app/auth/unauthorized/unauthorized.component.html":
/***/ (function(module, exports) {

module.exports = "<div>\r\n  Unauthorized Request. Please login to the system.<p> \r\n  <button pButton type=\"button\" (click)=\"login()\" label=\"Click\">Login to Machete</button> \r\n\r\n</div>\r\n<br>\r\n"

/***/ }),

/***/ "../../../../../src/app/auth/unauthorized/unauthorized.component.scss":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "a {\n  text-decoration: underline;\n  color: blue;\n  cursor: pointer; }\n", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/auth/unauthorized/unauthorized.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return UnauthorizedComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var UnauthorizedComponent = (function () {
    //, private location:Location
    function UnauthorizedComponent(location, service) {
        this.location = location;
        this.service = service;
    }
    UnauthorizedComponent.prototype.ngOnInit = function () {
    };
    UnauthorizedComponent.prototype.login = function () {
        this.service.startSigninMainWindow();
    };
    UnauthorizedComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-unauthorized',
            template: __webpack_require__("../../../../../src/app/auth/unauthorized/unauthorized.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/unauthorized/unauthorized.component.scss")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_common__["Location"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_common__["Location"]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__shared_services_auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__shared_services_auth_service__["a" /* AuthService */]) === "function" && _b || Object])
    ], UnauthorizedComponent);
    return UnauthorizedComponent;
    var _a, _b;
}());

//# sourceMappingURL=unauthorized.component.js.map

/***/ }),

/***/ "../../../../../src/app/auth/welcome/welcome.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/auth/welcome/welcome.component.html":
/***/ (function(module, exports) {

module.exports = "<div [innerHTML]=\"welcome\">\r\n</div>\r\n<button pButton type=\"button\" (click)=\"register()\" label=\"Register\">New employer</button>\r\n<button pButton type=\"button\" (click)=\"login()\" label=\"Login\">Returning employer</button>"

/***/ }),

/***/ "../../../../../src/app/auth/welcome/welcome.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WelcomeComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var WelcomeComponent = (function () {
    function WelcomeComponent(cfgService, authService, router) {
        this.cfgService = cfgService;
        this.authService = authService;
        this.router = router;
    }
    WelcomeComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.cfgService.getConfig('WorkCenterDescription_EN')
            .subscribe(function (data) { return _this.welcome = data.value; }, function (error) { return console.error('welcome.component.OnInit:' + error); });
    };
    WelcomeComponent.prototype.login = function () {
        this.authService.startSigninMainWindow();
    };
    WelcomeComponent.prototype.register = function () {
        this.router.navigate(['/register']);
    };
    WelcomeComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-welcome',
            template: __webpack_require__("../../../../../src/app/auth/welcome/welcome.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/welcome/welcome.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__configs_configs_service__["a" /* ConfigsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__configs_configs_service__["a" /* ConfigsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__shared_index__["b" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__shared_index__["b" /* AuthService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"]) === "function" && _c || Object])
    ], WelcomeComponent);
    return WelcomeComponent;
    var _a, _b, _c;
}());

//# sourceMappingURL=welcome.component.js.map

/***/ }),

/***/ "../../../../../src/app/configs/configs.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ConfigsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var ConfigsService = (function () {
    function ConfigsService(http) {
        this.http = http;
        this.uriBase = __WEBPACK_IMPORTED_MODULE_1__environments_environment__["a" /* environment */].dataUrl + '/api/configs';
        this.configs = new Array();
        this.configsAge = 0;
    }
    ConfigsService.prototype.isStale = function () {
        if (this.configsAge > Date.now() - 36000) {
            return false;
        }
        return true;
    };
    ConfigsService.prototype.isNotStale = function () {
        return !this.isStale();
    };
    ConfigsService.prototype.getAllConfigs = function () {
        var _this = this;
        if (this.isNotStale()) {
            return __WEBPACK_IMPORTED_MODULE_3_rxjs_Observable__["Observable"].of(this.configs);
        }
        console.log('getAllConfigs: ' + this.uriBase);
        return this.http.get(this.uriBase)
            .map(function (res) {
            _this.configs = res['data'];
            _this.configsAge = Date.now();
            return res['data'];
        })
            .catch(__WEBPACK_IMPORTED_MODULE_4__shared_handle_error__["a" /* HandleError */].error);
    };
    ConfigsService.prototype.getConfigs = function (category) {
        return this.getAllConfigs()
            .map(function (res) { return res.filter(function (l) { return l.category == category; }); })
            .catch(__WEBPACK_IMPORTED_MODULE_4__shared_handle_error__["a" /* HandleError */].error);
    };
    ConfigsService.prototype.getConfig = function (key) {
        return this.getAllConfigs()
            .mergeMap(function (a) { return a.filter(function (ll) { return ll.key == key; }); })
            .first()
            .catch(__WEBPACK_IMPORTED_MODULE_4__shared_handle_error__["a" /* HandleError */].error);
    };
    ConfigsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object])
    ], ConfigsService);
    return ConfigsService;
    var _a;
}());

//# sourceMappingURL=configs.service.js.map

/***/ }),

/***/ "../../../../../src/app/employers/employers-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__employers_component__ = __webpack_require__("../../../../../src/app/employers/employers.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var employerRoutes = [
    {
        path: 'employers',
        component: __WEBPACK_IMPORTED_MODULE_2__employers_component__["a" /* EmployersComponent */],
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_index__["a" /* AuthGuardService */]],
        canActivate: [__WEBPACK_IMPORTED_MODULE_3__shared_index__["a" /* AuthGuardService */]]
    }
];
var EmployersRoutingModule = (function () {
    function EmployersRoutingModule() {
    }
    EmployersRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(employerRoutes)
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
            ],
            providers: []
        })
    ], EmployersRoutingModule);
    return EmployersRoutingModule;
}());

//# sourceMappingURL=employers-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/employers/employers.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/employers/employers.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\r\n  employers works!\r\n</p>\r\n<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"employerForm\" (ngSubmit)=\"saveEmployer()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"name\">Name</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"name\" \r\n                      id=\"name\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.name}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"phone\">Phone number</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"phone\" \r\n                      id=\"phone\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.phone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"cellphone\">Cell phone</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"cellphone\" \r\n                      id=\"cellphone\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.cellphone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"email\">Email</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"email\" \r\n                      id=\"email\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.email}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"referredBy\">Referred by?</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"referredBy\" \r\n                      id=\"referredBy\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.referredBy}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"referredByOther\">Referred by notes</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"referredByOther\" \r\n                      id=\"referredByOther\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.referredByOther}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n        \r\n      </div>\r\n      <!-- -------------------------vertical divide---------------------------- -->\r\n      <div class=\"ui-g-12  ui-md-6\">\r\n        <div class=\"ui-g-12\"> \r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"business\">Is a business?</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <p-dropdown id=\"business\" [options]=\"yesNoDropDown\" formControlName=\"business\"\r\n                [autoWidth]=\"false\"></p-dropdown>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.business}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"address1\">Address (1)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"address1\" \r\n                      id=\"address1\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.address1}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"address2\">Address (2)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"address2\" \r\n                      id=\"address2\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.address2}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"city\">City</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"city\" \r\n                      id=\"city\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.city}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"state\">State</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"state\" \r\n                      id=\"state\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"zipcode\">Zipcode</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"zipcode\" \r\n                      id=\"zipcode\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.zipcode}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        \r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>        "

/***/ }),

/***/ "../../../../../src/app/employers/employers.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_models_employer__ = __webpack_require__("../../../../../src/app/shared/models/employer.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var EmployersComponent = (function () {
    function EmployersComponent(employersService, lookupsService, fb) {
        this.employersService = employersService;
        this.lookupsService = lookupsService;
        this.fb = fb;
        this.employer = new __WEBPACK_IMPORTED_MODULE_2__shared_models_employer__["a" /* Employer */]();
        this.showErrors = false;
        this.yesNoDropDown = [new __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__["a" /* MySelectItem */]('no', 'false'),
            new __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__["a" /* MySelectItem */]('yes', 'true')];
        this.formErrors = {
            'address1': '',
            'address2': '',
            'blogparticipate?': '',
            'business': '',
            'businessname': '',
            'cellphone': '',
            'city': '',
            'email': '',
            'fax': '',
            'name': '',
            'phone': '',
            'referredBy': '',
            'referredByOther': '',
            'state': '',
            'zipcode': ''
        };
        this.validationMessages = {
            'address1': { 'required': 'Address is required' },
            'address2': {},
            'blogparticipate': {},
            'business': {},
            'businessname': {},
            'cellphone': {},
            'city': { 'required': 'City is required' },
            'email': { 'required': 'Email is required' },
            'fax': { 'required': '' },
            'name': { 'required': 'Name is required' },
            'phone': { 'required': 'Phonr is required' },
            'referredBy': {},
            'referredByOther': {},
            'state': { 'required': 'State is required' },
            'zipcode': { 'required': 'zipcode is required' }
        };
    }
    EmployersComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.employersService.getEmployerBySubject()
            .subscribe(function (data) {
            _this.employer = data || new __WEBPACK_IMPORTED_MODULE_2__shared_models_employer__["a" /* Employer */]();
            _this.buildForm();
        });
        this.buildForm();
    };
    EmployersComponent.prototype.buildForm = function () {
        var _this = this;
        this.employerForm = this.fb.group({
            'id': [this.employer.id],
            'address1': [this.employer.address1, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
            'address2': [this.employer.address2],
            'blogparticipate': [this.employer.blogparticipate],
            'business': [this.employer.business],
            'businessname': [this.employer.businessname],
            'cellphone': [this.employer.cellphone],
            'city': [this.employer.city, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
            'email': [this.employer.email, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
            'fax': [this.employer.fax],
            'name': [this.employer.name, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
            'phone': [this.employer.phone, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
            'referredBy': [this.employer.referredBy],
            'referredByOther': [this.employer.referredByOther],
            'state': [this.employer.state, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
            'zipcode': [this.employer.zipcode, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required]
        });
        this.employerForm.valueChanges
            .subscribe(function (data) { return _this.onValueChanged(data); });
        this.onValueChanged();
    };
    EmployersComponent.prototype.onValueChanged = function (data) {
        var form = this.employerForm;
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
    EmployersComponent.prototype.saveEmployer = function () {
        console.log('saveEmployer: called');
        this.onValueChanged();
        if (this.employerForm.status === 'INVALID') {
            this.showErrors = true;
            return;
        }
        console.log('saveEmployer: valid');
        this.showErrors = false;
        var formModel = this.employerForm.value;
        this.employersService.save(formModel)
            .subscribe(function (data) { }, 
        //   this.employer = data;
        //   this.buildForm();
        // },
        function (error) {
            console.error(error);
        });
    };
    EmployersComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-employers',
            template: __webpack_require__("../../../../../src/app/employers/employers.component.html"),
            styles: [__webpack_require__("../../../../../src/app/employers/employers.component.css")],
            providers: [__WEBPACK_IMPORTED_MODULE_1__employers_service__["a" /* EmployersService */], __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__employers_service__["a" /* EmployersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__employers_service__["a" /* EmployersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"]) === "function" && _c || Object])
    ], EmployersComponent);
    return EmployersComponent;
    var _a, _b, _c;
}());

//# sourceMappingURL=employers.component.js.map

/***/ }),

/***/ "../../../../../src/app/employers/employers.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__employers_component__ = __webpack_require__("../../../../../src/app/employers/employers.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__employers_routing_module__ = __webpack_require__("../../../../../src/app/employers/employers-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_primeng_primeng__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var EmployersModule = (function () {
    // Diagnostic only: inspect router configuration
    function EmployersModule() {
        console.log('.ctor');
    }
    EmployersModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormsModule"],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["ReactiveFormsModule"],
                __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["InputTextModule"],
                __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["ButtonModule"],
                __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["DropdownModule"],
                __WEBPACK_IMPORTED_MODULE_3__employers_routing_module__["a" /* EmployersRoutingModule */]
            ],
            declarations: [__WEBPACK_IMPORTED_MODULE_2__employers_component__["a" /* EmployersComponent */]]
        }),
        __metadata("design:paramtypes", [])
    ], EmployersModule);
    return EmployersModule;
}());

//# sourceMappingURL=employers.module.js.map

/***/ }),

/***/ "../../../../../src/app/employers/employers.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__shared_models_employer__ = __webpack_require__("../../../../../src/app/shared/models/employer.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var EmployersService = (function () {
    function EmployersService(http, auth) {
        this.http = http;
        this.auth = auth;
        console.log('.ctor');
    }
    EmployersService.prototype.getEmployerBySubject = function () {
        var _this = this;
        return this.auth.getUser$()
            .mergeMap(function (user) {
            var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/employer/profile';
            //uri = uri + '?sub=' + user.profile['sub'];
            return _this.http.get(uri)
                .map(function (o) {
                console.log(uri, o);
                if (o['data'] == null) {
                    return new __WEBPACK_IMPORTED_MODULE_4__shared_models_employer__["a" /* Employer */]();
                }
                return o['data'];
            })
                .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
        });
    };
    EmployersService.prototype.save = function (employer) {
        var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/employer/profile';
        var method;
        //uri = uri + '/' + employer.id;
        console.log('save:', uri, employer);
        if (employer.id === null)
            return this.http.post(uri, JSON.stringify(employer), {
                headers: new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json'),
            })
                .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
        else
            return this.http.put(uri, JSON.stringify(employer), {
                headers: new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json'),
            })
                .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
    };
    EmployersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__shared_index__["b" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__shared_index__["b" /* AuthService */]) === "function" && _b || Object])
    ], EmployersService);
    return EmployersService;
    var _a, _b;
}());

//# sourceMappingURL=employers.service.js.map

/***/ }),

/***/ "../../../../../src/app/exports/exports-options.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/exports/exports-options.component.html":
/***/ (function(module, exports) {

module.exports = "<div [formGroup]=\"form\">\r\n  <p-dataTable  [value]=\"columns\" [responsive]=\"true\">\r\n    <p-column field=\"name\" header=\"Column name\"></p-column>\r\n    <p-column field=\"is_nullable\" header=\"Contains nulls?\"></p-column>\r\n    <p-column field=\"system_type_name\" header=\"Data type\"></p-column>\r\n    <p-column header=\"Include in export\">\r\n      <ng-template let-foo=\"rowData\" pTemplate=\"body\">\r\n        <p-inputSwitch onLabel=\"Yes\" offLabel=\"No\" [(ngModel)]=\"foo.include\" [formControlName]=\"foo.name\"></p-inputSwitch>\r\n      </ng-template>\r\n    </p-column>\r\n  </p-dataTable>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/exports/exports-options.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsOptionsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ExportsOptionsComponent = (function () {
    function ExportsOptionsComponent() {
        this.columns = [];
    }
    ExportsOptionsComponent.prototype.ngOnInit = function () {
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Array)
    ], ExportsOptionsComponent.prototype, "columns", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormGroup"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormGroup"]) === "function" && _a || Object)
    ], ExportsOptionsComponent.prototype, "form", void 0);
    ExportsOptionsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'exports-options',
            template: __webpack_require__("../../../../../src/app/exports/exports-options.component.html"),
            styles: [__webpack_require__("../../../../../src/app/exports/exports-options.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], ExportsOptionsComponent);
    return ExportsOptionsComponent;
    var _a;
}());

//# sourceMappingURL=exports-options.component.js.map

/***/ }),

/***/ "../../../../../src/app/exports/exports-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__exports_component__ = __webpack_require__("../../../../../src/app/exports/exports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var exportsRoutes = [
    {
        path: 'exports',
        component: __WEBPACK_IMPORTED_MODULE_2__exports_component__["a" /* ExportsComponent */],
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
        canActivate: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
    }
];
var ExportsRoutingModule = (function () {
    function ExportsRoutingModule() {
    }
    ExportsRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(exportsRoutes)
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
            ],
            providers: []
        })
    ], ExportsRoutingModule);
    return ExportsRoutingModule;
}());

//# sourceMappingURL=exports-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/exports/exports.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/exports/exports.component.html":
/***/ (function(module, exports) {

module.exports = "<h1>Exports</h1>\r\n<div class=\"ui-g\" >\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <p-dropdown\r\n      id=\"exportsDD\"\r\n      placeholder=\"Select a table\"\r\n      [options]=\"exportsDropDown\"\r\n      (onChange)=\"getColumns()\"\r\n      [(ngModel)]=\"selectedExportName\"\r\n      [filter]=\"true\"\r\n      [style]=\"{'width':'10em'}\"></p-dropdown>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <p-dropdown\r\n      placeholder=\"date field for filter\"\r\n      [options]=\"dateFilterDropDown\"\r\n      [(ngModel)]=\"selectedDateFilter\"\r\n      [style]=\"{'width':'10em'}\"></p-dropdown>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-4 ui-lg-3\">\r\n    <p-calendar\r\n      placeholder=\"Start date\"\r\n      [showIcon]=\"true\"\r\n      [(ngModel)]=\"selectedStartDate\"\r\n      dataType=\"string\"></p-calendar>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-4 ui-lg-3\">\r\n    <p-calendar\r\n      placeholder=\"End date\"\r\n      [showIcon]=\"true\"\r\n      [(ngModel)]=\"selectedEndDate\"\r\n      dataType=\"string\"></p-calendar>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-4 ui-lg-3\">\r\n    <button pButton type=\"submit\" label=\"Export\" (click)=\"onSubmit()\"></button>\r\n  </div>\r\n</div>\r\n<div>\r\n    <exports-options [columns]=\"selectedColumns\" [form]=\"form\"></exports-options>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/exports/exports.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__exports_service__ = __webpack_require__("../../../../../src/app/exports/exports.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_file_saver__ = __webpack_require__("../../../../file-saver/FileSaver.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_file_saver___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_file_saver__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_content_disposition__ = __webpack_require__("../../../../content-disposition/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_content_disposition___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_content_disposition__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var ExportsComponent = (function () {
    function ExportsComponent(exportsService, _fb) {
        this.exportsService = exportsService;
        this._fb = _fb;
        this.form = new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormGroup"]({});
    }
    ExportsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.exportsService.getExportsList()
            .subscribe(function (listData) {
            _this.exports = listData;
            _this.exportsDropDown = listData.map(function (r) {
                return new __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__["a" /* MySelectItem */](r.name, r.name);
            });
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit onCompleted'); });
    };
    ExportsComponent.prototype.getColumns = function () {
        var _this = this;
        this.exportsService.getColumns(this.selectedExportName)
            .subscribe(function (data) {
            _this.selectedColumns = data;
            _this.dateFilterDropDown = data.filter(function (f) { return f.system_type_name === 'datetime'; })
                .map(function (r) {
                return new __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__["a" /* MySelectItem */](r.name, r.name);
            });
            var group = {};
            data.forEach(function (col) {
                group[col.name] = new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormControl"](true);
            });
            _this.form = new __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormGroup"](group);
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('getColumns completed'); });
    };
    ExportsComponent.prototype.onSubmit = function () {
        var _this = this;
        var data = Object.assign({
            beginDate: this.selectedStartDate,
            endDate: this.selectedEndDate,
            filterField: this.selectedDateFilter
        }, this.form.value);
        console.log(this.form.value);
        this.exportsService.getExport(this.selectedExportName, data)
            .subscribe(function (res) {
            _this.downloadFile(res['_body'], _this.getFilename(res.headers.get('content-disposition')), res['_body'].type);
        }, function (error) {
            _this.errorMessage = error;
        }, function () { return console.log('onSubmit.getExport completed'); });
    };
    ExportsComponent.prototype.downloadFile = function (data, fileName, ttype) {
        var blob = new Blob([data], { type: ttype });
        Object(__WEBPACK_IMPORTED_MODULE_2_file_saver__["saveAs"])(blob, fileName);
    };
    ExportsComponent.prototype.getFilename = function (content) {
        return __WEBPACK_IMPORTED_MODULE_4_content_disposition__["parse"](content).parameters['filename'];
    };
    ExportsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-exports',
            template: __webpack_require__("../../../../../src/app/exports/exports.component.html"),
            styles: [__webpack_require__("../../../../../src/app/exports/exports.component.css")],
            providers: [__WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */]]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"]) === "function" && _b || Object])
    ], ExportsComponent);
    return ExportsComponent;
    var _a, _b;
}());

//# sourceMappingURL=exports.component.js.map

/***/ }),

/***/ "../../../../../src/app/exports/exports.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__exports_component__ = __webpack_require__("../../../../../src/app/exports/exports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__exports_routing_module__ = __webpack_require__("../../../../../src/app/exports/exports-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__exports_options_component__ = __webpack_require__("../../../../../src/app/exports/exports-options.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};








var ExportsModule = (function () {
    function ExportsModule() {
        console.log('.ctor');
    }
    ExportsModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormsModule"],
                __WEBPACK_IMPORTED_MODULE_3__angular_forms__["ReactiveFormsModule"],
                __WEBPACK_IMPORTED_MODULE_4__angular_http__["d" /* JsonpModule */],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["TabViewModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["ChartModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["DataTableModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["SharedModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["CalendarModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["ButtonModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["DropdownModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["DialogModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["InputSwitchModule"],
                __WEBPACK_IMPORTED_MODULE_6_primeng_primeng__["InputTextareaModule"],
                __WEBPACK_IMPORTED_MODULE_5__exports_routing_module__["a" /* ExportsRoutingModule */]
            ],
            declarations: [__WEBPACK_IMPORTED_MODULE_2__exports_component__["a" /* ExportsComponent */], __WEBPACK_IMPORTED_MODULE_7__exports_options_component__["a" /* ExportsOptionsComponent */]]
        }),
        __metadata("design:paramtypes", [])
    ], ExportsModule);
    return ExportsModule;
}());

//# sourceMappingURL=exports.module.js.map

/***/ }),

/***/ "../../../../../src/app/exports/exports.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__ = __webpack_require__("../../../../rxjs/add/operator/toPromise.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map__ = __webpack_require__("../../../../rxjs/add/operator/map.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch__ = __webpack_require__("../../../../rxjs/add/operator/catch.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};








var ExportsService = (function () {
    function ExportsService(http) {
        this.http = http;
        this.uriBase = __WEBPACK_IMPORTED_MODULE_6__environments_environment__["a" /* environment */].dataUrl + '/api/exports';
    }
    ExportsService.prototype.getExportsList = function () {
        console.log('getExportList: ', this.uriBase);
        return this.http.get(this.uriBase)
            .map(function (res) { return res['data']; })
            .catch(this.handleError);
    };
    ExportsService.prototype.getColumns = function (tableName) {
        var uri = this.uriBase + '/' + tableName.toLowerCase();
        console.log('getColumns ', uri);
        return this.http.get(uri)
            .map(function (res) { return res['data']; })
            .catch(this.handleError);
    };
    ExportsService.prototype.getExport = function (tableName, o) {
        var headers = new __WEBPACK_IMPORTED_MODULE_4__angular_http__["a" /* Headers */]({ 'Content-Type': 'application/text' });
        var options = new __WEBPACK_IMPORTED_MODULE_4__angular_http__["e" /* RequestOptions */]({
            headers: headers,
            responseType: __WEBPACK_IMPORTED_MODULE_4__angular_http__["f" /* ResponseContentType */].Blob
        });
        var params = this.encodeData(o);
        console.log('getExport: ', params);
        //const uri = this.uriBase + '/' + tableName.toLowerCase();
        var uri = this.uriBase + '/' + tableName + '/execute?' + params;
        return this.http.get(uri)
            .map(function (res) {
            return res;
        });
    };
    ExportsService.prototype.handleError = function (error) {
        console.error('exports.service.handleError:', JSON.stringify(error));
        return __WEBPACK_IMPORTED_MODULE_5_rxjs_Observable__["Observable"].of(error);
    };
    ExportsService.prototype.encodeData = function (data) {
        return Object.keys(data).map(function (key) {
            return [key, data[key]].map(encodeURIComponent).join('=');
        }).join('&');
    };
    ExportsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_7__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_7__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object])
    ], ExportsService);
    return ExportsService;
    var _a;
}());

//# sourceMappingURL=exports.service.js.map

/***/ }),

/***/ "../../../../../src/app/lookups/lookups.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LookupsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_rxjs__);
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
        this.uriBase = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/lookups';
        this.lookups = new Array();
        this.lookupsSource = new __WEBPACK_IMPORTED_MODULE_5_rxjs__["ReplaySubject"](1);
        this.lookups$ = this.lookupsSource.asObservable();
        this.lookupsAge = 0;
        this.storageKey = 'machete.lookups';
        console.log('.ctor');
        var data = sessionStorage.getItem(this.storageKey);
        this.lookupsAge = Number(sessionStorage.getItem(this.storageKey + '.age'));
        if (data && this.isNotStale) {
            console.log('.ctor using sessionStorage');
            this.lookups = JSON.parse(data);
            this.lookupsSource.next(this.lookups);
        }
        else {
            this.getAllLookups();
        }
    }
    LookupsService.prototype.isStale = function () {
        if (this.lookupsAge > Date.now() - 1800 * 1000) {
            return false;
        }
        return true;
    };
    LookupsService.prototype.isNotStale = function () {
        return !this.isStale();
    };
    LookupsService.prototype.getAllLookups = function () {
        var _this = this;
        // if (this.lookups != null && this.lookups.length > 0 && this.isNotStale()) {
        //   console.log('cache hit');
        //   return Observable.of(this.lookups);
        // }
        // TODO: set timer for refresh
        console.log('getLookups: ', this.uriBase);
        this.http.get(this.uriBase)
            .subscribe(function (res) {
            _this.lookups = res['data'];
            _this.lookupsAge = Date.now();
            _this.lookupsSource.next(_this.lookups);
            _this.storeLookups();
            return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].of(res['data']);
        });
    };
    LookupsService.prototype.storeLookups = function () {
        sessionStorage.setItem(this.storageKey, JSON.stringify(this.lookups));
        sessionStorage.setItem(this.storageKey + '.age', JSON.stringify(this.lookupsAge));
    };
    LookupsService.prototype.getLookups = function (category) {
        return this.lookups$
            .map(function (res) { return res.filter(function (l) { return l.category == category; }); })
            .catch(__WEBPACK_IMPORTED_MODULE_2__shared_handle_error__["a" /* HandleError */].error);
    };
    LookupsService.prototype.getLookup = function (id) {
        return this.lookups$
            .mergeMap(function (a) { return a.filter(function (ll) { return ll.id == id; }); })
            .first()
            .catch(__WEBPACK_IMPORTED_MODULE_2__shared_handle_error__["a" /* HandleError */].error);
    };
    LookupsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object])
    ], LookupsService);
    return LookupsService;
    var _a;
}());

//# sourceMappingURL=lookups.service.js.map

/***/ }),

/***/ "../../../../../src/app/lookups/models/lookup.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony export Record */
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return Lookup; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LCategory; });
/**
 * Created by jcii on 6/2/17.
 */
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Record = (function () {
    function Record(init) {
        Object.assign(this, init);
    }
    return Record;
}());

var Lookup = (function (_super) {
    __extends(Lookup, _super);
    function Lookup() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.idString = 'Lookup';
        return _this;
    }
    return Lookup;
}(Record));

var LCategory;
(function (LCategory) {
    LCategory["SKILL"] = "skill";
    LCategory["TRANSPORT"] = "transportmethod";
})(LCategory || (LCategory = {}));
//# sourceMappingURL=lookup.js.map

/***/ }),

/***/ "../../../../../src/app/menu/app.menu.component.html":
/***/ (function(module, exports) {

module.exports = "<ng-template ngFor let-child let-i=\"index\" [ngForOf]=\"(root ? item : item.items)\">\r\n    <li [ngClass]=\"{'active-menuitem': isActive(i)}\" *ngIf=\"child.visible === false ? false : true\">\r\n        <a [href]=\"child.url||'#'\" \r\n            (click)=\"itemClick($event,child,i)\" \r\n            class=\"ripplelink\" \r\n            *ngIf=\"!child.routerLink\" \r\n            [attr.tabindex]=\"!visible ? '-1' : null\" \r\n            [attr.target]=\"child.target\">\r\n            <i class=\"material-icons\">{{child.icon}}</i>\r\n            <span>{{child.label}}</span>\r\n            <i class=\"material-icons\" *ngIf=\"child.items\">keyboard_arrow_down</i>\r\n        </a>\r\n\r\n        <a (click)=\"itemClick($event,child,i)\" \r\n            class=\"ripplelink\" \r\n            *ngIf=\"child.routerLink\"\r\n            [routerLink]=\"child.routerLink\" \r\n            routerLinkActive=\"active-menuitem-routerlink\" \r\n            [routerLinkActiveOptions]=\"{exact: true}\" \r\n            [attr.tabindex]=\"!visible ? '-1' : null\" \r\n            [attr.target]=\"child.target\">\r\n            <i class=\"material-icons\">{{child.icon}}</i>\r\n            <span>{{child.label}}</span>\r\n            <i class=\"material-icons\" *ngIf=\"child.items\">keyboard_arrow_down</i>\r\n        </a>\r\n        <ul app-submenu [item]=\"child\" *ngIf=\"child.items\" [@children]=\"isActive(i) ? 'visible' : 'hidden'\" \r\n            [visible]=\"isActive(i)\" [reset]=\"reset\"></ul>\r\n    </li>\r\n</ng-template>"

/***/ }),

/***/ "../../../../../src/app/menu/app.menu.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppMenuComponent; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return AppSubMenu; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_animations__ = __webpack_require__("../../../animations/@angular/animations.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__load_menu_rules__ = __webpack_require__("../../../../../src/app/menu/load-menu-rules.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
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
    function AppMenuComponent(app, auth) {
        this.app = app;
        this.auth = auth;
    }
    AppMenuComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        this.auth.getUserEmitter()
            .subscribe(function (user) {
            if (user == null) {
                return new Array();
            }
            var roles = user.profile['role'];
            if (typeof roles === 'string') {
                roles = [roles];
            }
            _this.model = Object(__WEBPACK_IMPORTED_MODULE_6__load_menu_rules__["a" /* loadMenuRules */])(roles);
            console.log(_this.model);
        });
        this.auth.getUser();
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Boolean)
    ], AppMenuComponent.prototype, "reset", void 0);
    AppMenuComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-menu',
            template: "\n        <ul app-submenu [item]=\"model\"\n            root=\"true\"\n            class=\"ultima-menu ultima-main-menu clearfix\"\n            [reset]=\"reset\"\n            visible=\"true\"></ul>\n    "
        }),
        __param(0, Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]; }))),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_7__shared_index__["b" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_7__shared_index__["b" /* AuthService */]) === "function" && _b || Object])
    ], AppMenuComponent);
    return AppMenuComponent;
    var _a, _b;
}());

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
            item.command({
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
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["MenuItem"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["MenuItem"]) === "function" && _a || Object)
    ], AppSubMenu.prototype, "item", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Boolean)
    ], AppSubMenu.prototype, "root", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Boolean)
    ], AppSubMenu.prototype, "visible", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Boolean),
        __metadata("design:paramtypes", [Boolean])
    ], AppSubMenu.prototype, "reset", null);
    AppSubMenu = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: '[app-submenu]',
            template: __webpack_require__("../../../../../src/app/menu/app.menu.component.html"),
            animations: [
                Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["trigger"])('children', [
                    Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["state"])('hidden', Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["style"])({
                        height: '0px'
                    })),
                    Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["state"])('visible', Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["style"])({
                        height: '*'
                    })),
                    Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["transition"])('visible => hidden', Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
                    Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["transition"])('hidden => visible', Object(__WEBPACK_IMPORTED_MODULE_1__angular_animations__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
                ])
            ]
        }),
        __param(0, Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]; }))),
        __metadata("design:paramtypes", [typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_2__angular_common__["Location"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_common__["Location"]) === "function" && _d || Object])
    ], AppSubMenu);
    return AppSubMenu;
    var _a, _b, _c, _d;
}());

//# sourceMappingURL=app.menu.component.js.map

/***/ }),

/***/ "../../../../../src/app/menu/app.profile.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"profile\" [ngClass]=\"{'profile-expanded':active}\">\r\n    <div class=\"profile-image\"></div>\r\n    <a href=\"#\" (click)=\"onClick($event)\">\r\n        <span class=\"profile-name\">{{this.user}}</span>\r\n        <i class=\"material-icons\">keyboard_arrow_down</i>\r\n    </a>\r\n</div>\r\n\r\n<ul class=\"ultima-menu profile-menu\" [@menu]=\"active ? 'visible' : 'hidden'\">\r\n    <li role=\"menuitem\">\r\n        <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\r\n            <i class=\"material-icons\">person</i>\r\n            <span>Profile</span>\r\n        </a>\r\n    </li>\r\n    <li role=\"menuitem\">\r\n        <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\r\n            <i class=\"material-icons\">power_settings_new</i>\r\n            <span>Logout</span>\r\n        </a>\r\n    </li>\r\n</ul>"

/***/ }),

/***/ "../../../../../src/app/menu/app.profile.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return InlineProfileComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var InlineProfileComponent = (function () {
    function InlineProfileComponent(auth) {
        this.auth = auth;
    }
    InlineProfileComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.auth.getUserEmitter()
            .subscribe(function (user) {
            if (user === null || user === undefined) {
                _this.user = '<logged out>';
                return;
            }
            if (user.profile == null || user.profile.preferred_username == null) {
                _this.user = 'profile missing';
            }
            else {
                _this.user = user.profile.preferred_username;
            }
        });
    };
    InlineProfileComponent.prototype.onClick = function (event) {
        this.active = !this.active;
        event.preventDefault();
    };
    InlineProfileComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'inline-profile',
            template: __webpack_require__("../../../../../src/app/menu/app.profile.component.html"),
            animations: [
                Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["trigger"])('menu', [
                    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["state"])('hidden', Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["style"])({
                        height: '0px'
                    })),
                    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["state"])('visible', Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["style"])({
                        height: '*'
                    })),
                    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["transition"])('visible => hidden', Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)')),
                    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["transition"])('hidden => visible', Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["animate"])('400ms cubic-bezier(0.86, 0, 0.07, 1)'))
                ])
            ]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__shared_index__["b" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__shared_index__["b" /* AuthService */]) === "function" && _a || Object])
    ], InlineProfileComponent);
    return InlineProfileComponent;
    var _a;
}());

//# sourceMappingURL=app.profile.component.js.map

/***/ }),

/***/ "../../../../../src/app/menu/load-menu-rules.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = loadMenuRules;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__menu_rule__ = __webpack_require__("../../../../../src/app/menu/menu-rule.ts");

function loadMenuRules(authList) {
    var rules = [
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({
            id: 1,
            label: 'Place an order',
            icon: 'business',
            routerLink: ['/online-orders/introduction'],
            authorizedRoles: [
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].ADMIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].HIRER
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({
            id: 2,
            label: 'My profile',
            icon: 'business',
            routerLink: ['/employers'],
            authorizedRoles: [
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].ADMIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].HIRER
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({
            id: 3,
            label: 'My work orders',
            icon: 'work',
            routerLink: ['/work-orders'],
            authorizedRoles: [
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].ADMIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].HIRER
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({ id: 4, label: 'Dispatch', icon: 'today', url: ['/workassignment'] }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({ id: 5, label: 'People', icon: 'people', url: ['/person'] }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({ id: 6, label: 'Activities', icon: 'local_activity', url: ['/Activity'] }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({ id: 7, label: 'Sign-ins', icon: 'track_changes', url: ['/workersignin'] }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({ id: 8, label: 'Emails', icon: 'email', url: ['/email'] }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({
            id: 9,
            label: 'Reports',
            icon: 'subtitles',
            routerLink: ['/reports'],
            authorizedRoles: [
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].ADMIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].MANAGER
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({
            id: 10,
            label: 'Exports',
            icon: 'file_download',
            routerLink: ['/exports'],
            authorizedRoles: [
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].ADMIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].MANAGER
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__menu_rule__["b" /* MenuRule */]({
            id: 11,
            label: 'Dashboard',
            icon: 'file_download',
            routerLink: ['/dashboard'],
            authorizedRoles: [
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].ADMIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].CHECKIN,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].HIRER,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].MANAGER,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].PHONEDESK,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].TEACHER,
                __WEBPACK_IMPORTED_MODULE_0__menu_rule__["a" /* LRole */].USER
            ]
        }),
    ];
    // lambda-fu
    if (authList === null || authList === undefined) {
        return new Array();
    }
    return rules.filter(function (rule) {
        return rule.authorizedRoles.findIndex(function (role) {
            return authList.findIndex(function (auth) { return auth == role; }) > -1;
        }) > -1;
    });
}
//# sourceMappingURL=load-menu-rules.js.map

/***/ }),

/***/ "../../../../../src/app/menu/menu-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return MenuRule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LRole; });
var MenuRule = (function () {
    function MenuRule(init) {
        this.authorizedRoles = new Array();
        Object.assign(this, init);
    }
    return MenuRule;
}());

var LRole;
(function (LRole) {
    LRole["HIRER"] = "Hirer";
    LRole["ADMIN"] = "Administrator";
    LRole["CHECKIN"] = "Check-in";
    LRole["MANAGER"] = "Manager";
    LRole["PHONEDESK"] = "PhoneDesk";
    LRole["TEACHER"] = "Teacher";
    LRole["USER"] = "User";
})(LRole || (LRole = {}));
//# sourceMappingURL=menu-rule.js.map

/***/ }),

/***/ "../../../../../src/app/not-found.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PageNotFoundComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var PageNotFoundComponent = (function () {
    function PageNotFoundComponent() {
    }
    PageNotFoundComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            template: '<h2>Page not found</h2>'
        })
    ], PageNotFoundComponent);
    return PageNotFoundComponent;
}());

//# sourceMappingURL=not-found.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/final-confirm/final-confirm.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "div.ui-g-12.ui-md-8.ui-g-nopad { font-weight: bold; }", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/final-confirm/final-confirm.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid card ui-g\">\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n        <label for=\"dateTimeofWork\">Time needed</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n        {{order.dateTimeofWork |date:'short'}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"contactName\">Contact name</label>\r\n      </div>\r\n      <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n        {{order.contactName}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"worksiteAddress1\">Address (1)</label>\r\n      </div>\r\n      <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n        {{order.worksiteAddress1}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\" *ngIf=\"order.worksiteAddress2\">\r\n      <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n        <label for=\"worksiteAddress2\">Address (2)</label>\r\n      </div>\r\n      <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n        {{order.worksiteAddress2}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"city\">City</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.city}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"state\">State</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.state}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"zipcode\">Zipcode</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.zipcode}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"phone\">Phone</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.phone}}\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"description\">Work Description</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.description}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\" *ngIf=\"order.additionalNotes\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"additionalNotes\">Additional notes to dispatcher</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.additionalNotes}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"transportLabel\">Transport method</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{transportLabel}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"workerCount\">Worker count</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{workerCount}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"transportCost\">transport fees</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{transportCost | currency:'USD':true}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"laborCost\">labor cost</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{laborCost | currency:'USD':true}}\r\n      </div>\r\n    </div>      \r\n  </div>\r\n  <div>\r\n      <p-dataTable [value]=\"order.workAssignments\" \r\n        [responsive]=\"true\">\r\n          <p-column field=\"skill\" header=\"Skill needed\"></p-column>\r\n          <p-column field=\"requiresHeavyLifting\" header=\"Heavy lifting?\"></p-column>\r\n          <p-column field=\"transportCost\" header=\"Transport cost\">\r\n            <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n              <span>{{wa[col.field]| currency:'USD':true}}</span>\r\n            </ng-template>\r\n          </p-column>\r\n          <p-column field=\"hours\" header=\"hours requested\"></p-column>\r\n          <p-column field=\"wage\" header=\"Hourly wage\">\r\n            <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n              <span>{{wa[col.field]| currency:'USD':true}}</span>\r\n          </ng-template>\r\n          </p-column>\r\n        </p-dataTable>\r\n  </div>\r\n  <div *ngIf=\"transportCost > 0\">\r\n  You have chosen a transport method that has fees associated with it. You will need to\r\n  pay the fees before the workers will be dispatched. You can pay using our PayPal form, \r\n  or call 206.956.0779 x3 to make arrangements. When you finalize this order below, you \r\n  will be taken to the PayPal transaction page. \r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <button pButton type=\"button\" (click)=\"submit()\" label=\"Finalize and submit\"></button>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/final-confirm/final-confirm.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FinalConfirmComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__work_order_models_work_order__ = __webpack_require__("../../../../../src/app/online-orders/work-order/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_7_rxjs_Observable__);
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
    function FinalConfirmComponent(ordersService, onlineService, lookups, assignmentService) {
        this.ordersService = ordersService;
        this.onlineService = onlineService;
        this.lookups = lookups;
        this.assignmentService = assignmentService;
        this.order = new __WEBPACK_IMPORTED_MODULE_2__work_order_models_work_order__["a" /* WorkOrder */]();
    }
    FinalConfirmComponent.prototype.ngOnInit = function () {
        var _this = this;
        __WEBPACK_IMPORTED_MODULE_7_rxjs_Observable__["Observable"].combineLatest(this.lookups.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT), this.ordersService.getStream(), this.assignmentService.getStream()).subscribe(function (_a) {
            var l = _a[0], o = _a[1], wa = _a[2];
            console.log('ngOnInit', l, o, wa);
            _this.order = o;
            _this.transportLabel = l.find(function (ll) { return ll.id == o.transportMethodID; }).text_EN;
            if (wa != null && wa.length > 0) {
                _this.transportCost =
                    wa.map(function (wa) { return wa.transportCost; })
                        .reduce(function (a, b) { return a + b; });
                _this.workerCount = wa.length;
                _this.laborCost =
                    wa.map(function (wa) { return wa.wage * wa.hours; })
                        .reduce(function (a, b) { return a + b; });
            }
            else {
                _this.workerCount = 0;
                _this.transportCost = 0;
                _this.laborCost = 0;
            }
            _this.order.workAssignments = wa;
        }, function (error) { return console.error('error', error); });
    };
    FinalConfirmComponent.prototype.submit = function () {
        this.onlineService.postToApi(this.order);
    };
    FinalConfirmComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-final-confirm',
            template: __webpack_require__("../../../../../src/app/online-orders/final-confirm/final-confirm.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/final-confirm/final-confirm.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */]) === "function" && _d || Object])
    ], FinalConfirmComponent);
    return FinalConfirmComponent;
    var _a, _b, _c, _d;
}());

//# sourceMappingURL=final-confirm.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/guards/final-confirm.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FinalConfirmGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var FinalConfirmGuard = (function () {
    function FinalConfirmGuard(onlineService, router) {
        var _this = this;
        this.onlineService = onlineService;
        this.router = router;
        this.isConfirmed = false;
        console.log('.ctor');
        onlineService.getWorkAssignmentConfirmedStream().subscribe(function (confirm) {
            console.log('.ctor->finalConfirmed:', confirm);
            _this.isConfirmed = confirm;
        });
    }
    FinalConfirmGuard.prototype.canActivate = function () {
        return this.isConfirmed;
    };
    FinalConfirmGuard = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]) === "function" && _b || Object])
    ], FinalConfirmGuard);
    return FinalConfirmGuard;
    var _a, _b;
}());

//# sourceMappingURL=final-confirm.guard.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/guards/work-assignments.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var WorkAssignmentsGuard = (function () {
    function WorkAssignmentsGuard(onlineService, router) {
        var _this = this;
        this.onlineService = onlineService;
        this.router = router;
        this.isConfirmed = false;
        console.log('.ctor');
        onlineService.getWorkOrderConfirmedStream()
            .subscribe(function (confirm) {
            console.log('.ctor->workAssignmentsConfirmed:', confirm);
            _this.isConfirmed = confirm;
        });
    }
    WorkAssignmentsGuard.prototype.canActivate = function () {
        return this.isConfirmed;
    };
    WorkAssignmentsGuard = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]) === "function" && _b || Object])
    ], WorkAssignmentsGuard);
    return WorkAssignmentsGuard;
    var _a, _b;
}());

//# sourceMappingURL=work-assignments.guard.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/guards/work-order.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var WorkOrderGuard = (function () {
    function WorkOrderGuard(onlineService, router) {
        var _this = this;
        this.onlineService = onlineService;
        this.router = router;
        this.isConfirmed = false;
        console.log('.ctor');
        onlineService.getInitialConfirmedStream().subscribe(function (confirm) {
            console.log('.ctor->initialConfirmed:', confirm);
            _this.isConfirmed = confirm.map(function (a) { return a.confirmed; }).reduce(function (a, b) { return a && b; });
        });
    }
    WorkOrderGuard.prototype.canActivate = function () {
        return this.isConfirmed;
    };
    WorkOrderGuard = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]) === "function" && _b || Object])
    ], WorkOrderGuard);
    return WorkOrderGuard;
    var _a, _b;
}());

//# sourceMappingURL=work-order.guard.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.html":
/***/ (function(module, exports) {

module.exports = "<strong>You must accept the following terms to continue:</strong>\r\n<table>\r\n  <tr *ngFor=\"let item of confirmChoices\">\r\n    <td>\r\n      <input type=\"checkbox\" [(ngModel)]=\"item.confirmed\" (ngModelChange)=\"checkConfirm()\">         \r\n    </td>\r\n    <td>\r\n      {{item.description}}\r\n    </td>\r\n  </tr>\r\n</table>\r\n<button [disabled]=\"!confirmStatus\" (click)=\"nextStep()\">Confirm and proceed</button>\r\n\r\n<!-- <p-checkbox type=\"checkbox\" name=\"confirmBox\" [(ngModel)]=\"status\" (onChange)=\"checkConfirm($event)\"> -->\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroConfirmComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
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
    // TODO: Refactor as a service that polls from API
    function IntroConfirmComponent(onlineService, router) {
        this.onlineService = onlineService;
        this.router = router;
        this.confirmChoices = new Array();
        this.confirmStatus = false;
        console.log('.ctor');
    }
    IntroConfirmComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.onlineService.getInitialConfirmedStream()
            .subscribe(function (confirmed) {
            _this.confirmChoices = confirmed;
            _this.confirmStatus = _this.confirmChoices
                .map(function (a) { return a.confirmed; })
                .reduce(function (a, b) { return a && b; });
            console.log('ngOnInit:getInitialConfirmedStream', confirmed, _this.confirmStatus);
        });
    };
    IntroConfirmComponent.prototype.checkConfirm = function (event) {
        var result = this.confirmChoices
            .map(function (a) { return a.confirmed; })
            .reduce(function (a, b) { return a && b; });
        this.confirmStatus = result;
        this.onlineService.setInitialConfirm(this.confirmChoices);
        console.log('checkConfirm', this.confirmChoices, result);
    };
    IntroConfirmComponent.prototype.nextStep = function () {
        this.router.navigate(['/online-orders/work-order']);
    };
    IntroConfirmComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-intro-confirm',
            template: __webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]) === "function" && _b || Object])
    ], IntroConfirmComponent);
    return IntroConfirmComponent;
    var _a, _b;
}());

//# sourceMappingURL=intro-confirm.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/introduction/introduction.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/introduction/introduction.component.html":
/***/ (function(module, exports) {

module.exports = "\r\n<p>\r\n  Casa Latina connects Latino immigrant workers with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! For more information about our program please read these Frequently Asked Questions\r\n</p>\r\n<p>\r\n  If you are ready to hire a worker, please fill out the following form.\r\n</p>\r\n<p>\r\n  If you still have questions about hiring a worker, please call us at 206.956.0779 x3.\r\n</p>\r\n\r\n<button pButton type=\"button\" (click)=\"onClick()\" label=\"Next\"></button>"

/***/ }),

/***/ "../../../../../src/app/online-orders/introduction/introduction.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroductionComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
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
    function IntroductionComponent(router, onlineService) {
        this.router = router;
        this.onlineService = onlineService;
    }
    IntroductionComponent.prototype.ngOnInit = function () {
    };
    IntroductionComponent.prototype.onClick = function () {
        this.router.navigate(['/online-orders/intro-confirm']);
    };
    IntroductionComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-introduction',
            template: __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _b || Object])
    ], IntroductionComponent);
    return IntroductionComponent;
    var _a, _b;
}());

//# sourceMappingURL=introduction.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_component__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__introduction_introduction_component__ = __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/final-confirm/final-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__guards_work_order_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/work-order.guard.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__guards_work_assignments_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/work-assignments.guard.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__guards_final_confirm_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/final-confirm.guard.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};















var onlineOrderRoutes = [
    {
        path: 'online-orders',
        component: __WEBPACK_IMPORTED_MODULE_2__online_orders_component__["a" /* OnlineOrdersComponent */],
        canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
        canActivate: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
        children: [
            {
                path: 'introduction',
                component: __WEBPACK_IMPORTED_MODULE_3__introduction_introduction_component__["a" /* IntroductionComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
            },
            {
                path: 'intro-confirm',
                component: __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__["a" /* IntroConfirmComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
            },
            {
                path: 'work-order',
                component: __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__["a" /* WorkOrderComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
                canActivate: [__WEBPACK_IMPORTED_MODULE_9__guards_work_order_guard__["a" /* WorkOrderGuard */]]
            },
            {
                path: 'work-assignments',
                component: __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
                canActivate: [__WEBPACK_IMPORTED_MODULE_13__guards_work_assignments_guard__["a" /* WorkAssignmentsGuard */]]
            },
            {
                path: 'final-confirm',
                component: __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__["a" /* FinalConfirmComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
                canActivate: [__WEBPACK_IMPORTED_MODULE_14__guards_final_confirm_guard__["a" /* FinalConfirmGuard */]]
            }
        ]
    },
];
var OnlineOrdersRoutingModule = (function () {
    function OnlineOrdersRoutingModule() {
    }
    OnlineOrdersRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(onlineOrderRoutes)
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
            ],
            providers: [
                [
                    __WEBPACK_IMPORTED_MODULE_9__guards_work_order_guard__["a" /* WorkOrderGuard */],
                    __WEBPACK_IMPORTED_MODULE_13__guards_work_assignments_guard__["a" /* WorkAssignmentsGuard */],
                    __WEBPACK_IMPORTED_MODULE_14__guards_final_confirm_guard__["a" /* FinalConfirmGuard */],
                    __WEBPACK_IMPORTED_MODULE_2__online_orders_component__["a" /* OnlineOrdersComponent */],
                    __WEBPACK_IMPORTED_MODULE_10__work_order_work_order_service__["a" /* WorkOrderService */],
                    __WEBPACK_IMPORTED_MODULE_11__employers_employers_service__["a" /* EmployersService */],
                    __WEBPACK_IMPORTED_MODULE_12__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */]
                ]
            ]
        })
    ], OnlineOrdersRoutingModule);
    return OnlineOrdersRoutingModule;
}());

//# sourceMappingURL=online-orders-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.component.html":
/***/ (function(module, exports) {

module.exports = "<h1>\r\n  Hire a Worker Online Order Form\r\n</h1>\r\n<p-steps [model]=\"items\" [(activeIndex)]=\"activeIndex\" ></p-steps>\r\n<router-outlet></router-outlet>\r\n\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
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
    function OnlineOrdersComponent(onlineService, router) {
        var _this = this;
        this.onlineService = onlineService;
        this.router = router;
        this.activeIndex = 0;
        this.confirmation = false;
        router.events.subscribe(function (event) {
            // NavigationEnd event occurs after route succeeds
            if (event instanceof __WEBPACK_IMPORTED_MODULE_6__angular_router__["NavigationEnd"]) {
                switch (event.urlAfterRedirects) {
                    case '/online-orders/introduction': {
                        _this.activeIndex = 0;
                        break;
                    }
                    case '/online-orders/intro-confirm': {
                        _this.activeIndex = 1;
                        break;
                    }
                    case '/online-orders/work-order': {
                        _this.activeIndex = 2;
                        break;
                    }
                    case '/online-orders/work-assignments': {
                        _this.activeIndex = 3;
                        break;
                    }
                    case '/online-orders/final-confirm': {
                        _this.activeIndex = 4;
                        break;
                    }
                }
            }
        });
    }
    OnlineOrdersComponent.prototype.ngOnInit = function () {
        this.items = [
            { label: 'Introduction', routerLink: ['introduction'] },
            { label: 'Confirm', routerLink: ['intro-confirm'] },
            { label: 'work site details', routerLink: ['work-order'] },
            { label: 'worker details', routerLink: ['work-assignments'] },
            { label: 'finalize', routerLink: ['final-confirm'] }
        ];
    };
    OnlineOrdersComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-online-orders',
            template: __webpack_require__("../../../../../src/app/online-orders/online-orders.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/online-orders.component.css")],
            providers: [
                __WEBPACK_IMPORTED_MODULE_5__employers_employers_service__["a" /* EmployersService */],
                __WEBPACK_IMPORTED_MODULE_4__work_order_work_order_service__["a" /* WorkOrderService */],
                __WEBPACK_IMPORTED_MODULE_3__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */],
                __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"]
            ]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_6__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__angular_router__["Router"]) === "function" && _b || Object])
    ], OnlineOrdersComponent);
    return OnlineOrdersComponent;
    var _a, _b;
}());

//# sourceMappingURL=online-orders.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__introduction_introduction_component__ = __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__online_orders_component__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/final-confirm/final-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__online_orders_routing_module__ = __webpack_require__("../../../../../src/app/online-orders/online-orders-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_9_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};












var OnlineOrdersModule = (function () {
    function OnlineOrdersModule() {
    }
    OnlineOrdersModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
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
                __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["DialogModule"],
                __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["CheckboxModule"],
                __WEBPACK_IMPORTED_MODULE_8__online_orders_routing_module__["a" /* OnlineOrdersRoutingModule */]
            ],
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__introduction_introduction_component__["a" /* IntroductionComponent */],
                __WEBPACK_IMPORTED_MODULE_3__online_orders_component__["a" /* OnlineOrdersComponent */],
                __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__["a" /* IntroConfirmComponent */],
                __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__["a" /* WorkOrderComponent */],
                __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */],
                __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__["a" /* FinalConfirmComponent */]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_11__online_orders_service__["a" /* OnlineOrdersService */]
            ]
        })
    ], OnlineOrdersModule);
    return OnlineOrdersModule;
}());

//# sourceMappingURL=online-orders.module.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_rxjs__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_rxjs__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared_rules_load_confirms__ = __webpack_require__("../../../../../src/app/online-orders/shared/rules/load-confirms.ts");
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
    function OnlineOrdersService(http, orderService) {
        this.http = http;
        this.orderService = orderService;
        this.scheduleRules = new Array();
        this.transportRules = new Array();
        this.workOrderConfirm = false;
        this.workOrderConfirmSource = new __WEBPACK_IMPORTED_MODULE_6_rxjs__["BehaviorSubject"](false);
        this.workAssignmentsConfirm = false;
        this.workAssignmentsConfirmSource = new __WEBPACK_IMPORTED_MODULE_6_rxjs__["BehaviorSubject"](false);
        this.storageKey = 'machete.online-orders-service';
        this.initialConfirmKey = this.storageKey + '.initialconfirm';
        this.workOrderConfirmKey = this.storageKey + '.workorderconfirm';
        this.workAssignmentConfirmKey = this.storageKey + '.workassignmentsconfirm';
        console.log('.ctor');
        // this loads static data from a file. will replace later.
        this.loadConfirmState();
        this.scheduleRules = Object(__WEBPACK_IMPORTED_MODULE_5__shared__["b" /* loadScheduleRules */])();
        this.transportRules = Object(__WEBPACK_IMPORTED_MODULE_5__shared__["c" /* loadTransportRules */])();
    }
    OnlineOrdersService.prototype.getInitialConfirmedStream = function () {
        return this.initialConfirmSource.asObservable();
    };
    OnlineOrdersService.prototype.getWorkOrderConfirmedStream = function () {
        return this.workOrderConfirmSource.asObservable();
    };
    OnlineOrdersService.prototype.getWorkAssignmentConfirmedStream = function () {
        return this.workAssignmentsConfirmSource.asObservable();
    };
    OnlineOrdersService.prototype.loadConfirmState = function () {
        var loaded = JSON.parse(sessionStorage.getItem(this.initialConfirmKey));
        if (loaded != null && loaded.length > 0) {
            this.initialConfirm = loaded;
            this.initialConfirmSource = new __WEBPACK_IMPORTED_MODULE_6_rxjs__["BehaviorSubject"](loaded);
        }
        else {
            this.initialConfirm = Object(__WEBPACK_IMPORTED_MODULE_7__shared_rules_load_confirms__["a" /* loadConfirms */])();
            this.initialConfirmSource = new __WEBPACK_IMPORTED_MODULE_6_rxjs__["BehaviorSubject"](Object(__WEBPACK_IMPORTED_MODULE_7__shared_rules_load_confirms__["a" /* loadConfirms */])());
        }
        this.workOrderConfirm = (sessionStorage.getItem(this.workOrderConfirmKey) == 'true');
        this.workAssignmentsConfirm = (sessionStorage.getItem(this.workAssignmentConfirmKey) == 'true');
        // notify the subscribers
        //this.initialConfirmSource.next(this.initialConfirm);
        this.workOrderConfirmSource.next(this.workOrderConfirm);
        this.workAssignmentsConfirmSource.next(this.workAssignmentsConfirm);
    };
    OnlineOrdersService.prototype.getInitialConfirmValue = function () {
        return this.initialConfirm;
    };
    OnlineOrdersService.prototype.setInitialConfirm = function (choice) {
        console.log('setInitialConfirm:', choice);
        this.initialConfirm = choice;
        sessionStorage.setItem(this.initialConfirmKey, JSON.stringify(choice));
        this.initialConfirmSource.next(choice);
    };
    OnlineOrdersService.prototype.setWorkorderConfirm = function (choice) {
        console.log('setWorkOrderConfirm:', choice);
        this.workOrderConfirm = choice;
        sessionStorage.setItem(this.storageKey + '.workorderconfirm', JSON.stringify(choice));
        this.workOrderConfirmSource.next(choice);
    };
    OnlineOrdersService.prototype.setWorkAssignmentsConfirm = function (choice) {
        console.log('setWorkAssignmentsConfirm:', choice);
        this.workAssignmentsConfirm = choice;
        sessionStorage.setItem(this.storageKey + '.workassignmentsconfirm', JSON.stringify(choice));
        this.workAssignmentsConfirmSource.next(choice);
    };
    OnlineOrdersService.prototype.postToApi = function (order) {
        var _this = this;
        var url = __WEBPACK_IMPORTED_MODULE_4__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders';
        var postHeaders = new __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json');
        this.http.post(url, JSON.stringify(order), {
            headers: postHeaders
        }).subscribe(function (data) { _this.submitResult = data; console.log(_this.submitResult); }, function (err) {
            if (err.error instanceof Error) {
                console.error('Client-side error occured.');
            }
            else {
                console.error('online-orders.service.POST: ' + err.message);
            }
        });
    };
    OnlineOrdersService.prototype.getScheduleRules = function () {
        return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].of(this.scheduleRules);
    };
    OnlineOrdersService.prototype.getTransportRules = function () {
        return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].of(this.transportRules);
    };
    OnlineOrdersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object])
    ], OnlineOrdersService);
    return OnlineOrdersService;
    var _a, _b;
}());

//# sourceMappingURL=online-orders.service.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/index.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__validators_required__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/required.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "d", function() { return __WEBPACK_IMPORTED_MODULE_0__validators_required__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__validators_scheduling__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/scheduling.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "e", function() { return __WEBPACK_IMPORTED_MODULE_1__validators_scheduling__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__validators_transport__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/transport.ts");
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_schedule_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/schedule-rule.ts");
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_transport_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/transport-rule.ts");
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__models_record__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/record.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_5__models_record__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__rules_load_schedule_rules__ = __webpack_require__("../../../../../src/app/online-orders/shared/rules/load-schedule-rules.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_6__rules_load_schedule_rules__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__rules_load_transport_rules__ = __webpack_require__("../../../../../src/app/online-orders/shared/rules/load-transport-rules.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "c", function() { return __WEBPACK_IMPORTED_MODULE_7__rules_load_transport_rules__["a"]; });








//# sourceMappingURL=index.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/confirm.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Confirm; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/record.ts");
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

var Confirm = (function (_super) {
    __extends(Confirm, _super);
    function Confirm() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return Confirm;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));

//# sourceMappingURL=confirm.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/record.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Record; });
var Record = (function () {
    function Record(init) {
        Object.assign(this, init);
    }
    return Record;
}());

//# sourceMappingURL=record.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/schedule-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ScheduleRule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/record.ts");
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

var ScheduleRule = (function (_super) {
    __extends(ScheduleRule, _super);
    function ScheduleRule() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ScheduleRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));

//# sourceMappingURL=schedule-rule.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/skill-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SkillRule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/record.ts");
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

var SkillRule = (function (_super) {
    __extends(SkillRule, _super);
    function SkillRule() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SkillRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));

//# sourceMappingURL=skill-rule.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/transport-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return CostRule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return TransportRule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "c", function() { return TransportType; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/record.ts");
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

var CostRule = (function (_super) {
    __extends(CostRule, _super);
    function CostRule() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CostRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));

var TransportRule = (function (_super) {
    __extends(TransportRule, _super);
    function TransportRule() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return TransportRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));

var TransportType;
(function (TransportType) {
    TransportType[TransportType["transport_van"] = 0] = "transport_van";
    TransportType[TransportType["transport_bus"] = 1] = "transport_bus";
    TransportType[TransportType["transport_pickup"] = 2] = "transport_pickup";
})(TransportType || (TransportType = {}));
//# sourceMappingURL=transport-rule.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/rules/load-confirms.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = loadConfirms;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__models_confirm__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/confirm.ts");

function loadConfirms() {
    return [
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'completion',
            description: "This order is not complete until you receive a confirmation email from Casa Latina.\n      If you do not hear from us or if you need a worker with 48 hours please call 206.956.0779 x3 during our business hours",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'arrival_time',
            description: "Please allow a one hour window for worker(s) to arrive. This will account for transportation\n      routes with multiple stops and for traffic. There is no transportation fee to hire a Casa Latina\n      worker when you pick them up from our office. To have your worker(s) arrive at your door,\n      there is a small fee payable through this form.",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'employer_agency',
            description: "Casa Latina workers are not contractors. You will need to provide all tools, materials, and\n      safety equipment necessary for the job you wish to have done.",
            confirmed: false
        })
    ];
}
//# sourceMappingURL=load-confirms.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/rules/load-schedule-rules.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = loadScheduleRules;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/schedule-rule.ts");

function loadScheduleRules() {
    return [
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 0, leadHours: 48, minStartMin: 420, maxEndMin: 1020 }),
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 1, leadHours: 48, minStartMin: 420, maxEndMin: 1020 }),
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 2, leadHours: 48, minStartMin: 420, maxEndMin: 1020 }),
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 3, leadHours: 48, minStartMin: 420, maxEndMin: 1020 }),
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 4, leadHours: 48, minStartMin: 420, maxEndMin: 1020 }),
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 5, leadHours: 48, minStartMin: 420, maxEndMin: 1020 }),
        new __WEBPACK_IMPORTED_MODULE_0__models_schedule_rule__["a" /* ScheduleRule */]({ id: 6, leadHours: 48, minStartMin: 420, maxEndMin: 1020 })
    ];
}
//# sourceMappingURL=load-schedule-rules.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/rules/load-skill-rules.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = loadSkillRules;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/skill-rule.ts");

function loadSkillRules() {
    return [
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 1,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            speciality: false,
            key: 'default',
            descriptionEn: 'General Labor',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 2,
            subcategory: 'paint',
            level: 1,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            ltrCode: 'P',
            key: 'skill_painting_rollerbrush',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 3,
            subcategory: 'drywall',
            level: 2,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            ltrCode: 'S',
            key: 'skill_drywall',
            descriptionEn: 'Sheetrock hanging',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 4,
            subcategory: 'fence',
            level: 1,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            ltrCode: 'F',
            key: 'skill_landscaping_fence',
            descriptionEn: 'Build retaining wall / wood fence',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 5,
            subcategory: 'carpentry',
            level: 1,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            ltrCode: 'C',
            key: 'skill_carpentry',
            descriptionEn: 'Carpentry, repairing fences, decks, etc.',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 6,
            subcategory: 'build',
            level: 2,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            ltrCode: 'M',
            key: 'skill_masonry',
            descriptionEn: 'finishing and stone work',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 7,
            wage: 23,
            minHour: 4,
            maxHour: 8,
            speciality: false,
            key: 'skill_deep_cleaning',
            descriptionEn: 'kitchen, bedrooms, living room, bathroom, etc.',
            'minimumCost': 92,
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 8,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            key: 'skill_moving',
            descriptionEn: 'Moving Furniture and Boxes',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 9,
            subcategory: 'garden',
            level: 0,
            wage: 19,
            minHour: 5,
            maxHour: 8,
            fixedJob: false,
            speciality: false,
            ltrCode: 'Y',
            key: 'skill_yardwork',
            descriptionEn: 'only: yard cleaning, mowing, weeding and planting',
            descriptionEs: 'only: yard cleaning, mowing, weeding and planting',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 10,
            wage: 20,
            minHour: 4,
            maxHour: 8,
            speciality: false,
            key: 'skill_basic_cleaning',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 11,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            fixedJob: false,
            speciality: false,
            key: 'skill_demolition',
            descriptionEn: 'walls, ceilings, floors, siding, fences, etc.',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 12,
            subcategory: 'garden',
            level: 1,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            fixedJob: false,
            speciality: true,
            ltrCode: 'G',
            key: 'skill_adv_gardening',
            descriptionEn: 'includes pruning, trimming, transplanting, and basic yard work',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 13,
            subcategory: 'garden',
            level: 2,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            fixedJob: false,
            speciality: true,
            ltrCode: 'L',
            key: 'skill_landscaping',
            descriptionEn: 'patio pavers, retaining walls and walkways',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 14,
            subcategory: 'roof',
            level: 1,
            wage: 18,
            minHour: 5,
            maxHour: 8,
            fixedJob: false,
            speciality: true,
            ltrCode: 'R',
            key: 'skill_roofing',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 15,
            level: 3,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            fixedJob: false,
            speciality: true,
            key: 'skill_event_help',
            descriptionEn: 'Party and Events Staffing',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 16,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            speciality: false,
            key: 'skill_pressure_washing',
            descriptionEn: 'Pressure washing',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 17,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            speciality: false,
            key: 'skill_digging',
            descriptionEn: 'renches, drains, removing sod, etc',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 18,
            wage: 20,
            minHour: 5,
            maxHour: 8,
            speciality: false,
            key: 'skill_hauling',
            descriptionEn: 'heavy material: lumber, dirt, rocks, bricks, bricks, mulch compost, heavy branches, etc.',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 19,
            level: 1,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            ltrCode: 'I',
            key: 'skill_insulation',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 20,
            subcategory: 'drywall',
            level: 1,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: false,
            ltrCode: 'S',
            key: 'skill_drywall',
            active: true
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_skill_rule__["a" /* SkillRule */]({
            id: 21,
            wage: 22,
            minHour: 5,
            maxHour: 8,
            speciality: true,
            key: 'skill_flooring',
            active: true
        })
    ];
}
//# sourceMappingURL=load-skill-rules.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/rules/load-transport-rules.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = loadTransportRules;
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/transport-rule.ts");

function loadTransportRules() {
    return [
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 1,
            key: 'bus_inside_zone',
            lookupKey: 'transport_bus',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_bus,
            zoneLabel: 'inside',
            zipcodes: ['98101', '98102', '98103', '98104', '98105', '98106',
                '98107', '98109', '98112', '98115', '98116', '98117', '98118',
                '98119', '98121', '98122', '98125', '98126', '98133', '98134',
                '98136', '98144', '98154', '98164', '98174', '98177', '98195',
                '98199'],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 1, minWorker: 0, maxWorker: 10000, cost: 5 })
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 2,
            key: 'bus_outside_zone',
            lookupKey: 'transport_bus',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_bus,
            zoneLabel: 'outside',
            zipcodes: ['98005', '98006', '98007', '98008', '98033', '98039',
                '98052', '98040', '98004', '98074', '98075', '98029', '98027',
                '98028', '98155', '98166', '98146', '98168', '98057', '98056',
                '98059', '98037', '98020', '98026', '98043', '98021', '98011'],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 2, minWorker: 0, maxWorker: 10000, cost: 10 })
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 3,
            key: 'van_inside_zone',
            lookupKey: 'transport_van',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_van,
            zoneLabel: 'inside',
            zipcodes: ['98101', '98102', '98103', '98104', '98105', '98106',
                '98107', '98109', '98112', '98115', '98116', '98117', '98118',
                '98119', '98121', '98122', '98125', '98126', '98133', '98134',
                '98136', '98144', '98154', '98164', '98174', '98177', '98195',
                '98199'],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 3, minWorker: 0, maxWorker: 1, cost: 15 }),
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 4, minWorker: 1, maxWorker: 2, cost: 5 }),
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 5, minWorker: 2, maxWorker: 10, cost: 0 }),
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 4,
            key: 'van_outside_zone',
            lookupKey: 'transport_van',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_van,
            zoneLabel: 'outside',
            zipcodes: ['98005', '98006', '98007', '98008', '98033', '98039',
                '98052', '98040', '98004', '98074', '98075', '98029', '98027',
                '98028', '98155', '98166', '98146', '98168', '98057', '98056',
                '98059', '98037', '98020', '98026', '98043', '98021', '98011'],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 6, minWorker: 0, maxWorker: 1, cost: 25 }),
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 7, minWorker: 1, maxWorker: 10, cost: 0 }),
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 5,
            key: 'pickup',
            lookupKey: 'transport_pickup',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_pickup,
            zoneLabel: 'all',
            zipcodes: ['98101', '98102', '98103', '98104', '98105', '98106',
                '98107', '98109', '98112', '98115', '98116', '98117', '98118',
                '98119', '98121', '98122', '98125', '98126', '98133', '98134',
                '98136', '98144', '98154', '98164', '98174', '98177', '98195',
                '98199', '98005', '98006', '98007', '98008', '98033', '98039',
                '98052', '98040', '98004', '98074', '98075', '98029', '98027',
                '98028', '98155', '98166', '98146', '98168', '98057', '98056',
                '98059', '98037', '98020', '98026', '98043', '98021', '98011'],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ id: 8, minWorker: 0, maxWorker: 100, cost: 0 }),
            ]
        }),
    ];
}
//# sourceMappingURL=load-transport-rules.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/validators/hours.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = hoursValidator;
function hoursValidator(rules, lookups, skillIdKey, hoursKey) {
    return function (control) {
        if (!control.parent) {
            return null;
        }
        var skillIdControl = control.parent.get(skillIdKey);
        var hoursControl = control;
        if (!hoursControl.value) {
            return null;
        }
        if (!skillIdControl.value) {
            return null;
        }
        // https://gist.github.com/slavafomin/17ded0e723a7d3216fb3d8bf845c2f30
        var hours = Number(hoursControl.value);
        var skill = Number(skillIdControl.value);
        if (!Number.isInteger(hours)) {
            return { 'hours': 'Value must be a whole number.' };
        }
        var lookup = lookups.find(function (l) { return l.id == skill; });
        if (lookup == null) {
            throw new Error('skillId control didn\'t match a lookup record.');
        }
        var rule = rules.find(function (f) { return f.key == lookup.key; });
        if (hoursControl.value < rule.minHour) {
            return { 'hours': lookup.text_EN + " requires a minimum of " + rule.minHour + " hours" };
        }
        if (hoursControl.value > rule.maxHour) {
            return { 'hours': lookup.text_EN + " cannot exceed " + rule.maxHour + " hours" };
        }
    };
}
//# sourceMappingURL=hours.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/validators/required.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = requiredValidator;
function isEmptyInputValue(value) {
    // we don't check for string here so it also works with arrays
    return value == null || value.length === 0;
}
function requiredValidator(message) {
    return function (control) {
        return isEmptyInputValue(control.value) ? { 'required': message } : null;
    };
}
//# sourceMappingURL=required.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/validators/scheduling.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = schedulingValidator;
function schedulingValidator(rules) {
    return function (control) {
        if (control.value == null) {
            return null;
        }
        var date = control.value;
        var diffdate = date.valueOf() - Date.now();
        var rule = rules.find(function (s) { return s.id === date.getDay(); });
        if (diffdate < 0) {
            return { 'scheduling': 'Date cannot be in the past.' };
        }
        if (diffdate < (rule.leadHours * 3600)) {
            return { 'scheduling': 'Lead time less than ' + String(rule.leadHours) + ' hours.' };
        }
        if (date.getHours() < (rule.minStartMin / 60)) {
            return { 'scheduling': 'Start time is before minimum time of ' + String(rule.minStartMin / 60) + 'hours' };
        }
        if (date.getHours() > (rule.maxEndMin / 60)) {
            return { 'scheduling': 'End time is after maximum time of ' + String(rule.minStartMin / 60) + 'hours' };
        }
        return null;
    };
}
// @Directive({
//   selector: '[scheduling]',
//   providers: [{provide: NG_VALIDATORS, useExisting: ForbiddenValidatorDirective, multi: true}]
// })
// export class ForbiddenValidatorDirective implements Validator {
//   @Input() rules: ScheduleRule[];
//   validate(control: AbstractControl): {[key: string]: any} {
//     return schedulingValidator(this.rules)(control);
//   }
// }
//# sourceMappingURL=scheduling.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/validators/transport.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony export transportValidator */
function transportValidator(rules) {
    return function (control) {
        return null;
    };
}
//# sourceMappingURL=transport.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/models/work-assignment.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignment; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/**
 * Created by jcii on 5/31/17.
 */

var WorkAssignment = (function (_super) {
    __extends(WorkAssignment, _super);
    function WorkAssignment() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.requiresHeavyLifting = false;
        return _this;
    }
    WorkAssignment.sort = function (a, b) {
        if (a.id < b.id) {
            return -1;
        }
        if (a.id > b.id) {
            return 1;
        }
        return 0;
    };
    return WorkAssignment;
}(__WEBPACK_IMPORTED_MODULE_0__shared__["a" /* Record */]));

//# sourceMappingURL=work-assignment.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"requestForm\" (ngSubmit)=\"saveRequest()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"skillsList\">Skill needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-dropdown id=\"skillsList\"\r\n                        [options]=\"skillsDropDown\"\r\n                        formControlName=\"skillId\"\r\n                        [(ngModel)]=\"request.skillId\"\r\n                        (onChange)=\"selectSkill(request.skillId)\"\r\n                        [autoWidth]=\"false\"\r\n                        placeholder=\"Select a skill\"></p-dropdown>\r\n\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['skillId'].valid && showErrors\">\r\n            {{formErrors.skillId}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"hours\">Hours needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"hours\" id=\"hours\" type=\"text\" pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['hours'].valid && showErrors\">\r\n            {{formErrors.hours}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"requiresHeavyLifting\">Requires heavy lifting?</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-inputSwitch id=\"requiresHeavyLifting\" formControlName=\"requiresHeavyLifting\"></p-inputSwitch>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Additional info about job</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <textarea rows=\"3\" class=\"ui-inputtext\" formControlName=\"description\" id=\"description\" type=\"text\" pInputText></textarea>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Skill description\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.skillDescriptionEn}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Hourly rate\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.wage | currency:'USD':true}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Minimum time\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.minHour}}\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  <div>\r\n    <p-dataTable [value]=\"requestList\" [(selection)]=\"selectedRequest\" (onRowSelect)=\"onRowSelect($event)\" [responsive]=\"true\">\r\n      <p-column field=\"id\" header=\"#\"></p-column>\r\n      <p-column field=\"skill\" header=\"Skill needed\"></p-column>\r\n      <p-column field=\"transportCost\" header=\"Transport cost\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field]| currency:'USD':true}}</span>\r\n        </ng-template>\r\n      </p-column>\r\n      <p-column field=\"hours\" header=\"hours requested\"></p-column>\r\n      <p-column field=\"description\" header=\"notes\"></p-column>\r\n      <p-column field=\"requiresHeavyLifting\" header=\"Heavy lifting?\"></p-column>\r\n      <p-column field=\"wage\" header=\"Hourly wage\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field]| currency:'USD':true}}</span>\r\n        </ng-template>\r\n      </p-column>\r\n\r\n      <p-column styleClass=\"col-button\">\r\n        <ng-template pTemplate=\"header\">\r\n          Actions\r\n        </ng-template>\r\n        <ng-template let-request=\"rowData\" pTemplate=\"body\">\r\n          <button type=\"button\" pButton (click)=\"editRequest(request)\" icon=\"ui-icon-edit\"></button>\r\n          <button type=\"button\" pButton (click)=\"deleteRequest(request)\" icon=\"ui-icon-delete\"></button>\r\n        </ng-template>\r\n      </p-column>\r\n    </p-dataTable>\r\n      <div class=\"ui-g\">\r\n        <button pButton type=\"button\"  (click)=\"finalize()\" [disabled]=\"!hasRequests\" label=\"Finalize\"></button>\r\n      </div>\r\n  </div>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_work_assignment__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/models/work-assignment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__shared_validators_hours__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/hours.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__shared_rules_load_skill_rules__ = __webpack_require__("../../../../../src/app/online-orders/shared/rules/load-skill-rules.ts");
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
    function WorkAssignmentsComponent(lookupsService, orderService, waService, onlineService, router, fb) {
        this.lookupsService = lookupsService;
        this.orderService = orderService;
        this.waService = waService;
        this.onlineService = onlineService;
        this.router = router;
        this.fb = fb;
        this.selectedSkill = new __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["b" /* Lookup */]();
        this.requestList = new Array(); // list built by user in UI
        this.request = new __WEBPACK_IMPORTED_MODULE_3__models_work_assignment__["a" /* WorkAssignment */](); // composed by UI to make/edit a request
        this.newRequest = true;
        this.showErrors = false;
        this.hasRequests = false;
        this.formErrors = {
            'skillId': '',
            'skill': '',
            'hours': '',
            'description': '',
            'requiresHeavyLifting': '',
            'wage': ''
        };
        console.log('.ctor');
    }
    WorkAssignmentsComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        // waService.transportRules could fail under race conditions
        this.waService.getTransportRulesStream()
            .subscribe(function (data) { return _this.transportRules = data; }, 
        // When this leads to a REST call, compactRequests will depend on it
        function (error) { return console.error('ngOnInit.getTransportRules.error' + error); }, function () { return console.log('ngOnInit:getTransportRules onCompleted'); });
        this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].SKILL)
            .subscribe(function (listData) {
            _this.skills = listData;
            _this.skillsDropDown = listData.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_10__shared_models_my_select_item__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit:skills onCompleted'); });
        this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT)
            .subscribe(function (listData) {
            _this.transports = listData;
            _this.waService.compactRequests();
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit:transports onCompleted'); });
        this.requestList = this.waService.getAll();
        this.setHasRequests();
        this.buildForm();
    };
    WorkAssignmentsComponent.prototype.buildForm = function () {
        var _this = this;
        this.requestForm = this.fb.group({
            'id': '',
            'skillId': ['', Object(__WEBPACK_IMPORTED_MODULE_9__shared__["d" /* requiredValidator */])('Please select the type of work to be performed.')],
            'skill': [''],
            'hours': ['', Object(__WEBPACK_IMPORTED_MODULE_11__shared_validators_hours__["a" /* hoursValidator */])(Object(__WEBPACK_IMPORTED_MODULE_12__shared_rules_load_skill_rules__["a" /* loadSkillRules */])(), this.skills, 'skillId', 'hours')],
            'description': [''],
            'requiresHeavyLifting': [false],
            'wage': ['']
        });
        this.requestForm.valueChanges
            .subscribe(function (data) { return _this.onValueChanged(data); });
        this.onValueChanged();
    };
    WorkAssignmentsComponent.prototype.setHasRequests = function () {
        if (this.requestList.length > 0) {
            this.hasRequests = true;
        }
        else {
            this.hasRequests = false;
        }
    };
    WorkAssignmentsComponent.prototype.onValueChanged = function (data) {
        var form = this.requestForm;
        for (var field in this.formErrors) {
            // clear previous error message (if any)
            this.formErrors[field] = '';
            var control = form.get(field);
            if (control && !control.valid) {
                for (var key in control.errors) {
                    console.log('onValueChanged.error:' + field + ': ' + control.errors[key]);
                    this.formErrors[field] += control.errors[key] + ' ';
                }
            }
        }
    };
    WorkAssignmentsComponent.prototype.selectSkill = function (skillId) {
        console.log('selectSkill.skillId:' + String(skillId));
        var skill = this.skills.filter(function (f) { return f.id === Number(skillId); }).shift();
        if (skill === null || skill === undefined) {
            throw new Error('Can\'t find selected skill in component\'s list');
        }
        this.selectedSkill = skill;
        this.requestForm.controls['skill'].setValue(skill.text_EN);
        this.requestForm.controls['wage'].setValue(skill.wage);
    };
    // loads an existing item into the form fields
    WorkAssignmentsComponent.prototype.editRequest = function (request) {
        this.requestForm.controls['id'].setValue(request.id);
        this.requestForm.controls['skillId'].setValue(request.skillId);
        this.requestForm.controls['skill'].setValue(request.skill);
        this.requestForm.controls['hours'].setValue(request.hours);
        this.requestForm.controls['description'].setValue(request.description);
        this.requestForm.controls['requiresHeavyLifting'].setValue(request.requiresHeavyLifting);
        this.requestForm.controls['wage'].setValue(request.wage);
        this.newRequest = false;
    };
    WorkAssignmentsComponent.prototype.deleteRequest = function (request) {
        this.waService.delete(request);
        this.requestList = this.waService.getAll().slice();
        this.requestForm.reset();
        this.newRequest = true;
        if (this.requestList == null || this.requestList.length == 0) {
            this.onlineService.setWorkAssignmentsConfirm(false);
        }
        this.setHasRequests();
    };
    WorkAssignmentsComponent.prototype.saveRequest = function () {
        this.onValueChanged();
        if (this.requestForm.status === 'INVALID') {
            this.showErrors = true;
            //this.onlineService.setWorkAssignmentsConfirm(false);
            return;
        }
        this.showErrors = false;
        var formModel = this.requestForm.value;
        var saveRequest = {
            id: formModel.id || 0,
            skillId: formModel.skillId,
            skill: formModel.skill,
            hours: formModel.hours,
            description: formModel.description,
            requiresHeavyLifting: formModel.requiresHeavyLifting,
            wage: formModel.wage,
            transportCost: 0
        };
        this.waService.save(saveRequest);
        this.onlineService.setWorkAssignmentsConfirm(true);
        this.requestList = this.waService.getAll().slice();
        this.requestForm.reset();
        this.selectedSkill = new __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["b" /* Lookup */]();
        this.buildForm();
        this.newRequest = true;
        this.setHasRequests();
    };
    WorkAssignmentsComponent.prototype.onRowSelect = function (event) {
        this.newRequest = false;
        this.request = this.cloneRequest(event.data);
    };
    WorkAssignmentsComponent.prototype.cloneRequest = function (c) {
        var request = new __WEBPACK_IMPORTED_MODULE_3__models_work_assignment__["a" /* WorkAssignment */]();
        for (var prop in c) {
            request[prop] = c[prop];
        }
        return request;
    };
    WorkAssignmentsComponent.prototype.finalize = function () {
        this.router.navigate(['/online-orders/final-confirm']);
    };
    WorkAssignmentsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-assignments',
            template: __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_8__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_8__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__["a" /* WorkAssignmentsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__["a" /* WorkAssignmentsService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]) === "function" && _e || Object, typeof (_f = typeof __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"]) === "function" && _f || Object])
    ], WorkAssignmentsComponent);
    return WorkAssignmentsComponent;
    var _a, _b, _c, _d, _e, _f;
}());

//# sourceMappingURL=work-assignments.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_work_assignment__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/models/work-assignment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_BehaviorSubject__ = __webpack_require__("../../../../rxjs/BehaviorSubject.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_BehaviorSubject___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_BehaviorSubject__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__work_order_models_work_order__ = __webpack_require__("../../../../../src/app/online-orders/work-order/models/work-order.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};









var WorkAssignmentsService = (function () {
    function WorkAssignmentsService(onlineService, orderService, lookupsService) {
        var _this = this;
        this.onlineService = onlineService;
        this.orderService = orderService;
        this.lookupsService = lookupsService;
        this.requests = new Array();
        this.storageKey = 'machete.workassignments';
        this.transportsSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs_BehaviorSubject__["BehaviorSubject"](new Array());
        this.transportRulesSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs_BehaviorSubject__["BehaviorSubject"](new Array());
        this.workOrderSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs_BehaviorSubject__["BehaviorSubject"](new __WEBPACK_IMPORTED_MODULE_8__work_order_models_work_order__["a" /* WorkOrder */]());
        console.log('.ctor');
        var data = sessionStorage.getItem(this.storageKey);
        if (data) {
            console.log('sessionStorage:', data);
            var requests = JSON.parse(data);
            this.requests = requests;
        }
        this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_6__lookups_models_lookup__["a" /* LCategory */].TRANSPORT)
            .subscribe(function (data) {
            _this.transports = data;
            _this.transportsSource.next(data);
        }, function (error) { return console.error('initializeTranports.error: ' + JSON.stringify(error)); }, function () { return console.log('initializeTransport.OnComplete'); });
        this.onlineService.getTransportRules()
            .subscribe(function (data) {
            _this.transportRules = data;
            _this.transportRulesSource.next(data);
        });
        this.orderService.getStream()
            .subscribe(function (data) {
            _this.workOrder = data;
            _this.workOrderSource.next(data);
        });
        this.combinedSource = __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["Observable"].combineLatest(this.getTransportRulesStream(), this.getTransportsStream(), this.getWorkOrderStream());
        var subscribed = this.combinedSource.subscribe(function (values) {
            var rules = values[0], transports = values[1], order = values[2];
            console.log('combined subscription::', values);
        });
    }
    WorkAssignmentsService.prototype.getTransportsStream = function () {
        return this.transportsSource.asObservable();
    };
    WorkAssignmentsService.prototype.getTransportRulesStream = function () {
        return this.transportRulesSource.asObservable();
    };
    WorkAssignmentsService.prototype.getWorkOrderStream = function () {
        return this.workOrderSource.asObservable();
    };
    WorkAssignmentsService.prototype.getStream = function () {
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["Observable"].of(this.requests);
    };
    WorkAssignmentsService.prototype.getAll = function () {
        return this.requests;
    };
    WorkAssignmentsService.prototype.save = function (request) {
        var _this = this;
        __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["Observable"].zip(this.getTransportRulesStream(), this.getTransportsStream(), this.getWorkOrderStream(), function () { })
            .subscribe(function () {
            _this._save(request);
        });
    };
    WorkAssignmentsService.prototype._save = function (request) {
        if (request.id === 0) {
            request.id = this.getNextRequestId();
        }
        var index = this.findSelectedRequestIndex(request);
        if (index === -1) {
            this.requests.push(request); // create
        }
        else {
            this.requests[index] = request; // replace
        }
        this.compactRequests();
        console.log('saving:', this.requests);
        sessionStorage.setItem(this.storageKey, JSON.stringify(this.requests));
    };
    WorkAssignmentsService.prototype.getNextRequestId = function () {
        var sorted = this.requests.sort(__WEBPACK_IMPORTED_MODULE_1__models_work_assignment__["a" /* WorkAssignment */].sort);
        if (sorted.length === 0) {
            return 1;
        }
        else {
            return sorted[sorted.length - 1].id + 1;
        }
    };
    WorkAssignmentsService.prototype.delete = function (request) {
        var index = this.findSelectedRequestIndex(request);
        if (index < 0) {
            throw new Error('Can\'t find request (WorkAssignment) by id; failed to delete request.');
        }
        this.requests.splice(index, 1);
        this.compactRequests();
        console.log('saving after delete:', this.requests);
        sessionStorage.setItem(this.storageKey, JSON.stringify(this.requests));
    };
    WorkAssignmentsService.prototype.clear = function () { };
    WorkAssignmentsService.prototype.findSelectedRequestIndex = function (request) {
        return this.requests.findIndex(function (a) { return a.id === request.id; });
    };
    WorkAssignmentsService.prototype.compactRequests = function () {
        var rule = this.getTransportRule();
        for (var i in this.requests) {
            var newid = Number(i);
            this.requests[newid].id = newid + 1;
            this.requests[newid].transportCost =
                this.calculateTransportCost(newid + 1, rule);
        }
    };
    WorkAssignmentsService.prototype.getTransportRule = function () {
        var order = this.workOrder;
        if (order === null || order === undefined) {
            throw new Error('OrderService returned an undefined order');
        }
        if (order.transportMethodID <= 0) {
            throw new Error('Order missing valid transportMethodID');
        }
        var lookup = this.transports.find(function (f) { return f.id == order.transportMethodID; });
        if (lookup === null || lookup === undefined) {
            throw new Error('LookupService didn\'t return a valid lookup for transportMethodID: ' + order.transportMethodID);
        }
        var rules = this.transportRules.filter(function (f) { return f.lookupKey == lookup.key; });
        if (rules === null || rules === undefined) {
            throw new Error('No TransportRules match lookup key: ' + lookup.key);
        }
        var result = rules.find(function (f) { return f.zipcodes.includes(order.zipcode); });
        if (result === null || result == undefined) {
            throw new Error('Zipcode does not match any rule');
        }
        return result;
    };
    WorkAssignmentsService.prototype.calculateTransportCost = function (id, rule) {
        // can have a cost rule for a van, with an id greater that min/max worker,
        // that then leads to no rule.
        // TODO: Handle too many ids exception
        var result = rule.costRules.find(function (r) { return id > r.minWorker && id <= r.maxWorker; });
        if (result === undefined || result === null) {
            throw new Error('work assignment id outside of cost rules');
        }
        return result.cost;
    };
    WorkAssignmentsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_7__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_7__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _c || Object])
    ], WorkAssignmentsService);
    return WorkAssignmentsService;
    var _a, _b, _c;
}());

//# sourceMappingURL=work-assignments.service.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/models/work-order.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrder; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();

var WorkOrder = (function (_super) {
    __extends(WorkOrder, _super);
    function WorkOrder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return WorkOrder;
}(__WEBPACK_IMPORTED_MODULE_0__shared__["a" /* Record */]));

//# sourceMappingURL=work-order.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"orderForm\" (ngSubmit)=\"save()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"dateTimeofWork\">Time needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                <p-calendar id=\"dateTimeofWork\"\r\n                            showTime=\"true\"\r\n                            stepMinute=\"15\"\r\n                            defaultDate=\"\"\r\n                            formControlName=\"dateTimeofWork\">\r\n                </p-calendar>\r\n                <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.dateTimeofWork}}\r\n                </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"contactName\">Contact name</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                  <input class=\"ui-inputtext ng-dirty ng-invalid\" formControlName=\"contactName\" id=\"contactName\"\r\n                         type=\"text\" pInputText/>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                    {{formErrors.contactName}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"worksiteAddress1\">Address (1)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                  <input class=\"ui-inputtext\" formControlName=\"worksiteAddress1\" id=\"worksiteAddress1\" type=\"text\"\r\n                         pInputText/>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                    {{formErrors.worksiteAddress1}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"worksiteAddress2\">Address (2)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"worksiteAddress2\" id=\"worksiteAddress2\" type=\"text\"\r\n                   pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"city\">City</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"city\" id=\"city\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.city}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"state\">State</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"state\" id=\"state\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"zipcode\">Zipcode</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"zipcode\" id=\"zipcode\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.zipcode}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"phone\">Phone</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"phone\" id=\"phone\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.phone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Work Description</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\"\r\n                        formControlName=\"description\" id=\"description\" type=\"text\"></textarea>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.description}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"additionalNotes\">Additional notes to dispatcher</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n              <span class=\"md-inputfield\">\r\n                  <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\"\r\n                            formControlName=\"additionalNotes\" id=\"additionalNotes\" type=\"text\"></textarea>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                      {{formErrors.additionalNotes}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"transportMethodID\">Transport method</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <p-dropdown id=\"transportMethodID\" [options]=\"transportMethodsDropDown\" formControlName=\"transportMethodID\"\r\n                          [autoWidth]=\"false\"></p-dropdown>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.transportMethodID}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <button type=\"button\" (click)=\"showDialog()\" pButton icon=\"fa-external-link-square\" label=\"Show transport rates\"></button>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>\r\n<div>\r\n  <p-dialog header=\"Title\" [(visible)]=\"displayTransportCosts\">\r\n      Content\r\n  </p-dialog>\r\n  <p-dialog header=\"User's guide\" [(visible)]=\"displayUserGuide\" modal=\"modal\" width=\"300\" [responsive]=\"true\">\r\n    Enter the basic information about the work, like the location\r\n    of the worksite and the method of transport for the workers. \r\n    <p>\r\n    You may pick up the workers, or Casa Latina offers a transport program \r\n    (fees apply), or you can pay for workers to use public transport.    \r\n    <p-footer>\r\n        <button type=\"button\" pButton icon=\"fa-check\" (click)=\"ackUserGuide()\" label=\"Ok\"></button>\r\n    </p-footer>\r\n  </p-dialog>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_work_order__ = __webpack_require__("../../../../../src/app/online-orders/work-order/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
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
    function WorkOrderComponent(lookupsService, orderService, onlineService, configsService, router, fb) {
        this.lookupsService = lookupsService;
        this.orderService = orderService;
        this.onlineService = onlineService;
        this.configsService = configsService;
        this.router = router;
        this.fb = fb;
        this.order = new __WEBPACK_IMPORTED_MODULE_2__models_work_order__["a" /* WorkOrder */]();
        this.showErrors = false;
        this.newOrder = true;
        this.displayTransportCosts = false;
        this.displayUserGuide = true;
        this.storageKey = 'machete.work-order.component';
        this.formErrors = {
            'dateTimeofWork': '',
            'contactName': '',
            'worksiteAddress1': '',
            'worksiteAddress2': '',
            'city': '',
            'state': '',
            'zipcode': '',
            'phone': '',
            'description': '',
            'additionalNotes': '',
            'transportMethodID': ''
        };
        console.log('.ctor');
        var result = sessionStorage.getItem(this.storageKey + '.UG');
        if (result === 'false') {
            this.displayUserGuide = false;
        }
        else {
            this.displayUserGuide = true;
        }
    }
    WorkOrderComponent.prototype.showDialog = function () {
        this.displayTransportCosts = true;
    };
    WorkOrderComponent.prototype.ackUserGuide = function () {
        this.displayUserGuide = false;
        sessionStorage.setItem(this.storageKey + '.UG', 'false');
    };
    WorkOrderComponent.prototype.ngOnInit = function () {
        this.initializeScheduling();
        this.buildForm();
        this.initializeTransports();
        this.initializeProfile();
    };
    WorkOrderComponent.prototype.initializeScheduling = function () {
        this.schedulingRules = this.onlineService.scheduleRules;
    };
    WorkOrderComponent.prototype.initializeProfile = function () {
        var _this = this;
        this.orderService.getStream()
            .subscribe(function (data) {
            _this.order = data;
            _this.buildForm();
        });
    };
    WorkOrderComponent.prototype.initializeTransports = function () {
        var _this = this;
        this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT)
            .subscribe(function (listData) {
            _this.transportMethods = listData;
            var items = [new __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__["a" /* MySelectItem */]('Select transportion', null)];
            var transports = listData.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
            _this.transportMethodsDropDown = items.concat(transports);
            ;
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit: getLookups onCompleted'); });
    };
    WorkOrderComponent.prototype.buildForm = function () {
        var _this = this;
        this.orderForm = this.fb.group({
            'dateTimeofWork': [this.order.dateTimeofWork, [
                    Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('Date & time is required.'),
                    Object(__WEBPACK_IMPORTED_MODULE_7__shared__["e" /* schedulingValidator */])(this.schedulingRules)
                ]],
            'contactName': [this.order.contactName, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('Contact name is required.')],
            'worksiteAddress1': [this.order.worksiteAddress1, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('Address is required.')],
            'worksiteAddress2': [this.order.worksiteAddress2],
            'city': [this.order.city, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('City is required.')],
            'state': [this.order.state, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('State is required.')],
            'zipcode': [this.order.zipcode, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('Zip code is required.')],
            'phone': [this.order.phone, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('Phone is required.')],
            'description': [this.order.description, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('Description is required.')],
            'additionalNotes': [this.order.additionalNotes],
            'transportMethodID': [this.order.transportMethodID, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["d" /* requiredValidator */])('A transport method is required.')]
        });
        this.orderForm.valueChanges
            .subscribe(function (data) { return _this.onValueChanged(data); });
        this.onValueChanged();
    };
    WorkOrderComponent.prototype.onValueChanged = function (data) {
        var form = this.orderForm;
        for (var field in this.formErrors) {
            // clear previous error message (if any)
            this.formErrors[field] = '';
            var control = form.get(field);
            if (control && !control.valid) {
                for (var key in control.errors) {
                    console.log('onValueChanged.error:' + field + ': ' + control.errors[key]);
                    this.formErrors[field] += control.errors[key] + ' ';
                }
            }
        }
    };
    WorkOrderComponent.prototype.load = function () {
    };
    WorkOrderComponent.prototype.save = function () {
        this.onValueChanged();
        if (this.orderForm.status === 'INVALID') {
            console.log('save: INVALID', this.formErrors);
            this.onlineService.setWorkorderConfirm(false);
            this.showErrors = true;
            return;
        }
        this.showErrors = false;
        var order = this.prepareOrderForSave();
        this.orderService.save(order);
        this.onlineService.setWorkorderConfirm(true);
        this.newOrder = false;
        this.router.navigate(['/online-orders/work-assignments']);
    };
    WorkOrderComponent.prototype.prepareOrderForSave = function () {
        var formModel = this.orderForm.value;
        var order = {
            id: 0,
            dateTimeofWork: formModel.dateTimeofWork,
            contactName: formModel.contactName,
            worksiteAddress1: formModel.worksiteAddress1,
            worksiteAddress2: formModel.worksiteAddress2,
            city: formModel.city,
            state: formModel.state,
            zipcode: formModel.zipcode,
            phone: formModel.phone,
            description: formModel.description,
            additionalNotes: formModel.additionalNotes,
            transportMethodID: formModel.transportMethodID
        };
        return order;
    };
    WorkOrderComponent.prototype.clearOrder = function () {
    };
    WorkOrderComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-order',
            template: __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_6__work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_4__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__["a" /* ConfigsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__["a" /* ConfigsService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_10__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_10__angular_router__["Router"]) === "function" && _e || Object, typeof (_f = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"]) === "function" && _f || Object])
    ], WorkOrderComponent);
    return WorkOrderComponent;
    var _a, _b, _c, _d, _e, _f;
}());

//# sourceMappingURL=work-order.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_work_order__ = __webpack_require__("../../../../../src/app/online-orders/work-order/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var WorkOrderService = (function () {
    function WorkOrderService(employerService) {
        var _this = this;
        this.employerService = employerService;
        this.orderSource = new __WEBPACK_IMPORTED_MODULE_3_rxjs__["ReplaySubject"](1);
        //order$ = this.orderSource.asObservable();
        this.storageKey = 'machete.workorder';
        console.log('.ctor');
        var data = sessionStorage.getItem(this.storageKey);
        if (data) {
            var order = JSON.parse(data);
            order.dateTimeofWork = new Date(order.dateTimeofWork);
            this.order = order;
            this.orderSource.next(this.order);
        }
        else {
            this.employerService.getEmployerBySubject()
                .subscribe(function (data) {
                _this.order = _this.mapOrderFrom(data);
                _this.orderSource.next(_this.order);
            });
        }
    }
    WorkOrderService.prototype.getStream = function () {
        return this.orderSource.asObservable();
    };
    WorkOrderService.prototype.get = function () {
        console.log('get called');
        var data = sessionStorage.getItem(this.storageKey);
        if (data) {
            var order = JSON.parse(data);
            //console.log('get: returning stored order', order); 
            order.dateTimeofWork = new Date(order.dateTimeofWork);
            return order;
        }
        else {
            return this.order;
        }
    };
    WorkOrderService.prototype.mapOrderFrom = function (employer) {
        var order = new __WEBPACK_IMPORTED_MODULE_1__models_work_order__["a" /* WorkOrder */]();
        order.contactName = employer.name;
        order.worksiteAddress1 = employer.address1;
        order.worksiteAddress2 = employer.address2;
        order.city = employer.city;
        order.state = employer.state;
        order.zipcode = employer.zipcode;
        order.phone = employer.phone || employer.cellphone;
        return order;
    };
    WorkOrderService.prototype.save = function (order) {
        console.log('save', order);
        sessionStorage.setItem(this.storageKey, JSON.stringify(order));
        this.order = order;
        this.orderSource.next(this.order);
    };
    // TODO: Call clear when order expires, is completed, removed.
    WorkOrderService.prototype.clear = function () {
        this.order = null;
        sessionStorage.removeItem(this.storageKey);
    };
    WorkOrderService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__employers_employers_service__["a" /* EmployersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__employers_employers_service__["a" /* EmployersService */]) === "function" && _a || Object])
    ], WorkOrderService);
    return WorkOrderService;
    var _a;
}());

//# sourceMappingURL=work-order.service.js.map

/***/ }),

/***/ "../../../../../src/app/reports/models/report.ts":
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

/***/ "../../../../../src/app/reports/models/search-inputs.ts":
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

/***/ "../../../../../src/app/reports/models/search-options.ts":
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

/***/ "../../../../../src/app/reports/reports-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_component__ = __webpack_require__("../../../../../src/app/reports/reports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var reportsRoutes = [
    {
        path: 'reports',
        component: __WEBPACK_IMPORTED_MODULE_2__reports_component__["a" /* ReportsComponent */],
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
        canActivate: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
    }
];
var ReportsRoutingModule = (function () {
    function ReportsRoutingModule() {
    }
    ReportsRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(reportsRoutes)
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]
            ]
        })
    ], ReportsRoutingModule);
    return ReportsRoutingModule;
}());

//# sourceMappingURL=reports-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/reports/reports.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/reports/reports.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-g\">\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <p-dropdown [options]=\"reportsDropDown\" (onChange)=\"getView()\" [(ngModel)]=\"selectedReportID\" [filter]=\"true\" [style]=\"{'width':'20em'}\"></p-dropdown>\r\n    <button pButton type=\"button\" icon=\"ui-icon-sync\" (click)=\"getView()\" iconPos=\"left\"></button>\r\n    <button pButton type=\"button\" icon=\"ui-icon-edit\" (click)=\"displayDialog=true\" iconPos=\"left\"></button>\r\n  </div>\r\n  <div *ngIf=\"inputs.memberNumber === true\" class=\"ui-g-12 ui-md-6\">\r\n    <label for=\"memberNumber\">Membr number</label>\r\n    <input id=\"memberNumber\" type=\"text\" pInputText [(ngModel)]=\"o.memberNumber\" (onBlur)=\"getView()\" dataType=\"number\"/>\r\n  </div>\r\n  <div *ngIf=\"inputs.beginDate === true\" class=\"ui-g-12 ui-md-6 ui-lg-3\">\r\n    <p-calendar  placeholder=\"Start date\" (onSelect)=\"getView()\" (onBlur)=\"getView()\" [(ngModel)]=\"o.beginDate\" [showIcon]=\"true\" dataType=\"string\"></p-calendar>\r\n  </div>\r\n  <div *ngIf=\"inputs.endDate === true\" class=\"ui-g-12 ui-md-6 ui-lg-3\">\r\n    <p-calendar placeholder=\"End date\" (onSelect)=\"getView()\" (onBlur)=\"getView()\" [(ngModel)]=\"o.endDate\" [showIcon]=\"true\" dataType=\"string\"></p-calendar>\r\n  </div>\r\n</div>\r\n<p-dialog header=\"{{title}}\" [(visible)]=\"displayDescription\">\r\n  {{description}}\r\n</p-dialog>\r\n<div>\r\n<p-dataTable\r\n  #dt\r\n  [value]=\"viewData\"\r\n  sortField=\"value\"\r\n  sortOrder=\"-1\"\r\n  sortMode=\"single\"\r\n  [globalFilter]=\"gb\"\r\n  [responsive]=\"true\"\r\n  >\r\n  <p-header>\r\n    <div class=\"ui-helper-clearfix\">\r\n      <button type=\"button\" pButton icon=\"ui-icon-file-download\" iconPos=\"left\" label=\"CSV\" (click)=\"getExport(dt)\" style=\"float:left\"></button>\r\n      <input #gb type=\"text\" placeholder=\"Global search\" width=\"200\">\r\n\r\n      <button pButton type=\"button\" icon=\"ui-icon-help-outline\" (click)=\"showDescription()\" iconPos=\"left\" style=\"float:right\"></button>\r\n    </div>\r\n  </p-header>\r\n  <p-column *ngFor=\"let col of cols\" [field]=\"col.field\" [header]=\"col.header\" [sortable]=\"true\"></p-column>\r\n</p-dataTable>\r\n  <p-dialog\r\n    header=\"Report Details\"\r\n    [(visible)]=\"displayDialog\"\r\n    [responsive]=\"true\"\r\n    showEffect=\"fade\"\r\n    [modal]=\"false\"\r\n    resizable=\"true\"\r\n    width=\"1000\"\r\n  >\r\n    <div>\r\n      <div class=\"ui-g\" style=\"display:flex\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"name\">Name</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\"><input pInputText id=\"name\" [(ngModel)]=\"name\" style=\"width: 100%;\"/></div>\r\n      </div>\r\n      <div class=\"ui-g\" style=\"display:flex\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"commonName\">Common name</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\"><input pInputText id=\"commonName\" style=\"width: 100%;\" [(ngModel)]=\"selectedReport.commonName\" /></div>\r\n      </div>\r\n      <!--<div class=\"ui-g\">-->\r\n        <!--<div class=\"ui-sm-4 ui-md-3 ui-lg-2\"><label for=\"title\">Title</label></div>-->\r\n        <!--<div class=\"ui-sm-8 ui-md-9 ui-lg-10\"><input pInputText id=\"title\" [(ngModel)]=\"selectedReport.title\" /></div>-->\r\n      <!--</div>-->\r\n      <div class=\"ui-g\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"description\">Description</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\"><input pInputText id=\"description\" style=\"width: 100%;\" [(ngModel)]=\"selectedReport.description\" /></div>\r\n      </div>\r\n      <div class=\"ui-g\" style=\"display:flex\">\r\n        <div class=\"ui-sm-4 ui-md-3 ui-lg-3\" style=\"flex: 0\"><label for=\"sqlquery\">SQL Query</label></div>\r\n        <div class=\"ui-sm-8 ui-md-9 ui-lg-9\" style=\"flex: 1\">\r\n              <textarea pInputTextarea id=\"sqlquery\" rows=\"20\" style=\"width: 100%;\" [(ngModel)]=\"selectedReport.sqlquery\" autoResize=\"true\"></textarea>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <p-footer>\r\n      <div class=\"ui-dialog-buttonpane ui-widget-content ui-helper-clearfix\">\r\n        <!--<button type=\"button\" pButton icon=\"fa-close\" (click)=\"delete()\" label=\"Delete\"></button>-->\r\n        <!--<button type=\"button\" pButton icon=\"fa-check\" (click)=\"save()\" label=\"Save\"></button>-->\r\n      </div>\r\n    </p-footer>\r\n  </p-dialog>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/reports/reports.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__reports_service__ = __webpack_require__("../../../../../src/app/reports/reports.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_search_options__ = __webpack_require__("../../../../../src/app/reports/models/search-options.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_report__ = __webpack_require__("../../../../../src/app/reports/models/report.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_search_inputs__ = __webpack_require__("../../../../../src/app/reports/models/search-inputs.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
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
        if (this.reportList.length === 0) {
            return;
        }
        this.selectedReport = this.reportList.filter(function (x) { return x.name === _this.selectedReportID; })[0];
        // TODO catch exception if not found
        this.description = this.selectedReport.description;
        this.title = this.selectedReport.title || this.selectedReport.commonName;
        this.name = this.selectedReport.name;
        this.cols = this.selectedReport.columns.filter(function (a) { return a.visible === true; });
        this.inputs = this.selectedReport.inputs;
    };
    ReportsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.reportsService.getReportList()
            .subscribe(function (listData) {
            _this.reportList = listData;
            _this.reportsDropDown = listData.map(function (r) { return new __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__["a" /* MySelectItem */](r.commonName, r.name); });
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
    ReportsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-reports',
            template: __webpack_require__("../../../../../src/app/reports/reports.component.html"),
            styles: [__webpack_require__("../../../../../src/app/reports/reports.component.css")],
            providers: [__WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */]]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */]) === "function" && _a || Object])
    ], ReportsComponent);
    return ReportsComponent;
    var _a;
}());

//# sourceMappingURL=reports.component.js.map

/***/ }),

/***/ "../../../../../src/app/reports/reports.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_component__ = __webpack_require__("../../../../../src/app/reports/reports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__reports_routing_module__ = __webpack_require__("../../../../../src/app/reports/reports-routing.module.ts");
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
        console.log('.ctor');
    }
    ReportsModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
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
                __WEBPACK_IMPORTED_MODULE_4__angular_http__["d" /* JsonpModule */],
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
    return ReportsModule;
}());

//# sourceMappingURL=reports.module.js.map

/***/ }),

/***/ "../../../../../src/app/reports/reports.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__ = __webpack_require__("../../../../rxjs/add/operator/toPromise.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map__ = __webpack_require__("../../../../rxjs/add/operator/map.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch__ = __webpack_require__("../../../../rxjs/add/operator/catch.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
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
    }
    ReportsService.prototype.getReportData = function (reportName, o) {
        // TODO throw exception if report is not populated
        var params = this.encodeData(o);
        var uri = __WEBPACK_IMPORTED_MODULE_5__environments_environment__["a" /* environment */].dataUrl + '/api/reports';
        if (reportName) {
            uri = uri + '/' + reportName;
        }
        if (reportName && params) {
            uri = uri + '?' + params;
        }
        console.log('getReportData: ' + uri);
        return this.http.get(uri)
            .map(function (res) { return res['data']; })
            .catch(this.handleError);
    };
    ReportsService.prototype.getReportList = function () {
        var uri = __WEBPACK_IMPORTED_MODULE_5__environments_environment__["a" /* environment */].dataUrl + '/api/reports';
        console.log('getReportList: ', uri);
        return this.http.get(uri)
            .map(function (o) { return o['data']; })
            .catch(this.handleError);
    };
    ReportsService.prototype.handleError = function (error) {
        console.error('handleError:', error);
        return __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["Observable"].of(error);
    };
    ReportsService.prototype.encodeData = function (data) {
        return Object.keys(data).map(function (key) {
            return [key, data[key]].map(encodeURIComponent).join('=');
        }).join('&');
    };
    ReportsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_6__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object])
    ], ReportsService);
    return ReportsService;
    var _a;
}());

//# sourceMappingURL=reports.service.js.map

/***/ }),

/***/ "../../../../../src/app/selective-preloading-strategy.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SelectivePreloadingStrategy; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of__ = __webpack_require__("../../../../rxjs/add/observable/of.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__);
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
    SelectivePreloadingStrategy = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])()
    ], SelectivePreloadingStrategy);
    return SelectivePreloadingStrategy;
}());

//# sourceMappingURL=selective-preloading-strategy.js.map

/***/ }),

/***/ "../../../../../src/app/shared/handle-error.ts":
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

/***/ "../../../../../src/app/shared/index.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_0__services_auth_guard_service__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_1__services_auth_service__["a"]; });


//# sourceMappingURL=index.js.map

/***/ }),

/***/ "../../../../../src/app/shared/models/employer.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Employer; });
var Employer = (function () {
    function Employer() {
    }
    return Employer;
}());

//# sourceMappingURL=employer.js.map

/***/ }),

/***/ "../../../../../src/app/shared/models/my-select-item.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MySelectItem; });
var MySelectItem = (function () {
    function MySelectItem(label, value) {
        this.label = label;
        this.value = value;
    }
    return MySelectItem;
}());

//# sourceMappingURL=my-select-item.js.map

/***/ }),

/***/ "../../../../../src/app/shared/services/auth-guard.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthGuardService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var AuthGuardService = (function () {
    function AuthGuardService(authService, router) {
        this.authService = authService;
        this.router = router;
        console.log('.ctor');
    }
    AuthGuardService.prototype.canActivate = function (route, state) {
        var _this = this;
        console.log(state);
        var isLoggedIn = this.authService.isLoggedInObs();
        isLoggedIn.subscribe(function (loggedin) {
            if (!loggedin) {
                console.log('canActivate NOT loggedIn: url:', state);
                _this.authService.setRedirectUrl(state.url);
                _this.router.navigate(['unauthorized']);
            }
        });
        return isLoggedIn;
    };
    AuthGuardService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__auth_service__["a" /* AuthService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]) === "function" && _b || Object])
    ], AuthGuardService);
    return AuthGuardService;
    var _a, _b;
}());

//# sourceMappingURL=auth-guard.service.js.map

/***/ }),

/***/ "../../../../../src/app/shared/services/auth.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/@angular/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var AuthService = (function () {
    // TODO:
    // need async way to call for auth password, then send intercept on its way
    function AuthService(http, router) {
        this.http = http;
        this.router = router;
        this.mgr = new __WEBPACK_IMPORTED_MODULE_3_oidc_client__["UserManager"](__WEBPACK_IMPORTED_MODULE_4__environments_environment__["a" /* environment */].oidc_client_settings);
        this.userLoadedEvent = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        this.loggedIn = false;
    }
    AuthService.prototype.getUserEmitter = function () {
        return this.userLoadedEvent;
    };
    AuthService.prototype.setRedirectUrl = function (url) {
        this.redirectUrl = url;
        console.log("auth.service.setRedirectUrl.url: " + this.redirectUrl);
    };
    AuthService.prototype.getRedirectUrl = function () {
        return this.redirectUrl;
    };
    AuthService.prototype.isLoggedInObs = function () {
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].fromPromise(this.mgr.getUser()).map(function (user) {
            if (user && !user.expired) {
                return true;
            }
            else {
                return false;
            }
        });
    };
    AuthService.prototype.clearState = function () {
        this.mgr.clearStaleState().then(function () {
            console.log('auth.service.clearStateState success');
        }).catch(function (e) {
            console.error('auth.service.clearStateState error', e.message);
        });
    };
    AuthService.prototype.getUser$ = function () {
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].fromPromise(this.mgr.getUser());
    };
    AuthService.prototype.getUserRoles$ = function () {
        return this.getUser$()
            .mergeMap(function (user) {
            console.log(user);
            if (user === null || user === undefined) {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].of(new Array());
            }
            else {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].of(user.profile.role);
            }
        });
    };
    AuthService.prototype.getUsername$ = function () {
        return this.getUser$()
            .mergeMap(function (user) {
            // TODO: if user is null, disable menu
            if (user === null || user === undefined) {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].of(null);
            }
            else {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].of(user.profile.preferred_username);
            }
        });
    };
    AuthService.prototype.getUser = function () {
        var _this = this;
        this.mgr.getUser().then(function (user) {
            _this.currentUser = user;
            //console.log('auth.service.getUser returned: ' + JSON.stringify(user));
            _this.getUserEmitter().emit(user);
        }).catch(function (err) {
            console.error('getUser: ', err);
        });
    };
    AuthService.prototype.removeUser = function () {
        var _this = this;
        this.mgr.removeUser().then(function () {
            _this.getUserEmitter().emit(null);
            console.log('auth.service.removeUser: user removed');
        }).catch(function (err) {
            console.error('auth.service.removeUser returned: ' + JSON.stringify(err));
        });
    };
    AuthService.prototype.startSigninMainWindow = function () {
        this.mgr.signinRedirect({ data: this.redirectUrl }).then(function () {
            console.log('signinRedirect done');
        }).catch(function (err) {
            console.error('auth.service.startSigninMainWindow returned: ' + JSON.stringify(err));
        });
    };
    AuthService.prototype.endSigninMainWindow = function (url) {
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].fromPromise(this.mgr.signinRedirectCallback(url));
        // .then(function (user) {
        //   console.log('auth.service.endSigninMainWindow.user: ', user.profile.sub);
        //   if (user.state) {
        //     this.router.navigate(['dashboard']);
        //   }
        // }).catch(function (err) {
        //   console.error('auth.service.endSigninMainWindow returned: ' + JSON.stringify(err));
        // });
    };
    AuthService.prototype.startSignoutMainWindow = function () {
        var _this = this;
        this.mgr.getUser().then(function (user) {
            return _this.mgr.signoutRedirect({ id_token_hint: user.id_token }).then(function (resp) {
                console.log('signed out', resp);
                setTimeout(5000, function () {
                    console.log('testing to see if fired...');
                });
            }).catch(function (err) {
                console.error('auth.service.startSignoutMainWindow returned: ' + JSON.stringify(err));
            });
        });
    };
    ;
    AuthService.prototype.endSignoutMainWindow = function () {
        this.mgr.signoutRedirectCallback().then(function (resp) {
            console.log('signed out', resp);
        }).catch(function (err) {
            console.error('auth.service.endSignoutMainWindow returned: ' + JSON.stringify(err));
        });
    };
    ;
    AuthService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["b" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["b" /* Http */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_5__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__angular_router__["Router"]) === "function" && _b || Object])
    ], AuthService);
    return AuthService;
    var _a, _b;
}());

//# sourceMappingURL=auth.service.js.map

/***/ }),

/***/ "../../../../../src/app/shared/services/token.interceptor.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TokenInterceptor; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var TokenInterceptor = (function () {
    function TokenInterceptor(auth, route) {
        this.auth = auth;
        this.route = route;
    }
    TokenInterceptor.prototype.intercept = function (request, next) {
        var _this = this;
        var url = this.route.url;
        return this.auth.getUser$()
            .mergeMap(function (user) {
            //console.log('currentUser: ', user);
            if (user === null || user === undefined) {
                _this.auth.redirectUrl = url;
                _this.route.navigate(['/welcome']);
                return next.handle(request);
            }
            if (user.expired) {
                _this.auth.redirectUrl = url;
                // TODO: should go to login start, or silently renew?
                _this.route.navigate(['/unauthorized']);
                return next.handle(request);
            }
            request = request.clone({
                setHeaders: {
                    Authorization: "Bearer " + user.access_token
                }
            });
            return next.handle(request);
        });
    };
    TokenInterceptor = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__auth_service__["a" /* AuthService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]) === "function" && _b || Object])
    ], TokenInterceptor);
    return TokenInterceptor;
    var _a, _b;
}());

//# sourceMappingURL=token.interceptor.js.map

/***/ }),

/***/ "../../../../../src/app/work-orders/work-orders-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrdersRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__work_orders_component__ = __webpack_require__("../../../../../src/app/work-orders/work-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var woRoutes = [
    {
        path: 'work-orders',
        component: __WEBPACK_IMPORTED_MODULE_2__work_orders_component__["a" /* WorkOrdersComponent */],
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
        canActivate: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]],
    }
];
var WorkOrdersRoutingModule = (function () {
    function WorkOrdersRoutingModule() {
    }
    WorkOrdersRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"].forChild(woRoutes)
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_router__["RouterModule"]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]
            ]
        })
    ], WorkOrdersRoutingModule);
    return WorkOrdersRoutingModule;
}());

//# sourceMappingURL=work-orders-routing.module.js.map

/***/ }),

/***/ "../../../../../src/app/work-orders/work-orders.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/work-orders/work-orders.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\r\n  These are your past orders:\r\n</p>\r\n<p-dataTable\r\n  #dt\r\n  [value]=\"orders\"\r\n  [responsive]=\"true\"\r\n  resizable=\"true\"\r\n  >\r\n  <p-header>\r\n    \r\n  </p-header>\r\n  <p-column field=\"paperOrderNum\" header=\"Order #\"></p-column>\r\n  <p-column field=\"dateTimeofWork\" header=\"Time needed\"></p-column>\r\n  <p-column field=\"statusEN\" header=\"Status\"></p-column>\r\n  <p-column field=\"transportMethodEN\" header=\"Trans. method\"></p-column>\r\n  <p-column field=\"\" header=\"Worker count\"></p-column>\r\n  <p-column field=\"contactName\" header=\"Contact name\"></p-column>\r\n  <p-column field=\"workSiteAddress1\" header=\"Address\"></p-column>\r\n  <p-column field=\"zipcode\" header=\"Zipcode\"></p-column>\r\n</p-dataTable>"

/***/ }),

/***/ "../../../../../src/app/work-orders/work-orders.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrdersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__work_orders_service__ = __webpack_require__("../../../../../src/app/work-orders/work-orders.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var WorkOrdersComponent = (function () {
    function WorkOrdersComponent(workOrderService) {
        this.workOrderService = workOrderService;
    }
    WorkOrdersComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.workOrderService.getOrders()
            .subscribe(function (data) {
            _this.orders = data;
        });
    };
    WorkOrdersComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-orders',
            template: __webpack_require__("../../../../../src/app/work-orders/work-orders.component.html"),
            styles: [__webpack_require__("../../../../../src/app/work-orders/work-orders.component.css")],
            providers: [__WEBPACK_IMPORTED_MODULE_1__work_orders_service__["a" /* WorkOrdersService */]]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__work_orders_service__["a" /* WorkOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__work_orders_service__["a" /* WorkOrdersService */]) === "function" && _a || Object])
    ], WorkOrdersComponent);
    return WorkOrdersComponent;
    var _a;
}());

//# sourceMappingURL=work-orders.component.js.map

/***/ }),

/***/ "../../../../../src/app/work-orders/work-orders.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrdersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/@angular/common.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__work_orders_component__ = __webpack_require__("../../../../../src/app/work-orders/work-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_orders_routing_module__ = __webpack_require__("../../../../../src/app/work-orders/work-orders-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_primeng_primeng__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};





var WorkOrdersModule = (function () {
    function WorkOrdersModule() {
    }
    WorkOrdersModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["DataTableModule"],
                __WEBPACK_IMPORTED_MODULE_3__work_orders_routing_module__["a" /* WorkOrdersRoutingModule */]
            ],
            declarations: [__WEBPACK_IMPORTED_MODULE_2__work_orders_component__["a" /* WorkOrdersComponent */]]
        })
    ], WorkOrdersModule);
    return WorkOrdersModule;
}());

//# sourceMappingURL=work-orders.module.js.map

/***/ }),

/***/ "../../../../../src/app/work-orders/work-orders.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrdersService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var WorkOrdersService = (function () {
    function WorkOrdersService(http) {
        this.http = http;
    }
    WorkOrdersService.prototype.getOrders = function () {
        var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders';
        return this.http.get(uri)
            .map(function (o) { return o['data']; })
            .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
    };
    WorkOrdersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object])
    ], WorkOrdersService);
    return WorkOrdersService;
    var _a;
}());

//# sourceMappingURL=work-orders.service.js.map

/***/ }),

/***/ "../../../../../src/environments/environment.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
var environment = {
    name: 'mvc-embedded',
    production: false,
    dataUrl: 'http://localhost:63374',
    authUrl: 'https://localhost:44379/id',
    baseRef: '/V2',
    oidc_client_settings: {
        authority: 'https://localhost:44379/id',
        client_id: 'machete-ui-local-embedded',
        redirect_uri: 'http://localhost:4213/V2/authorize',
        post_logout_redirect_uri: 'http://localhost:4213/V2',
        response_type: 'id_token token',
        scope: 'openid email roles api profile',
        silent_redirect_uri: 'http://localhost:4213/V2/silent-renew.html',
        automaticSilentRenew: true,
        accessTokenExpiringNotificationTime: 4,
        // silentRequestTimeout:10000,
        filterProtocolClaims: true,
        loadUserInfo: true
    }
};
//# sourceMappingURL=environment.js.map

/***/ }),

/***/ "../../../../../src/main.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__("../../../platform-browser-dynamic/@angular/platform-browser-dynamic.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__("../../../../../src/app/app.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__ = __webpack_require__("../../../../chart.js/dist/Chart.bundle.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__);





if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["enableProdMode"])();
}
console.log('main.ts environment.name:', __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].name);
Object(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */]);
//# sourceMappingURL=main.js.map

/***/ }),

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__("../../../../../src/main.ts");


/***/ })

},[0]);
//# sourceMappingURL=main.bundle.js.map