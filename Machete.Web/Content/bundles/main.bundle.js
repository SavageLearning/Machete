webpackJsonp(["main"],{

/***/ "../../../../../src/$$_lazy_route_resource lazy recursive":
/***/ (function(module, exports) {

function webpackEmptyAsyncContext(req) {
	// Here Promise.resolve().then() is used instead of new Promise() to prevent
	// uncatched exception popping up in devtools
	return Promise.resolve().then(function() {
		throw new Error("Cannot find module '" + req + "'.");
	});
}
webpackEmptyAsyncContext.keys = function() { return []; };
webpackEmptyAsyncContext.resolve = webpackEmptyAsyncContext;
module.exports = webpackEmptyAsyncContext;
webpackEmptyAsyncContext.id = "../../../../../src/$$_lazy_route_resource lazy recursive";

/***/ }),

/***/ "../../../../../src/app/app-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__selective_preloading_strategy__ = __webpack_require__("../../../../../src/app/selective-preloading-strategy.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__auth___ = __webpack_require__("../../../../../src/app/auth/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__auth_register_register_component__ = __webpack_require__("../../../../../src/app/auth/register/register.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
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
var AppRoutingModule = /** @class */ (function () {
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
                // here because used in router guards
                __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */],
                __WEBPACK_IMPORTED_MODULE_7__employers_employers_service__["a" /* EmployersService */],
                __WEBPACK_IMPORTED_MODULE_4__selective_preloading_strategy__["a" /* SelectivePreloadingStrategy */]
            ]
        }),
        __metadata("design:paramtypes", [])
    ], AppRoutingModule);
    return AppRoutingModule;
}());



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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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
var AppComponent = /** @class */ (function () {
    function AppComponent(renderer, router) {
        this.renderer = renderer;
        this.router = router;
        this.layoutCompact = false;
        this.layoutMode = MenuOrientation.STATIC;
        this.darkMenu = true;
        this.profileMode = 'inline';
    }
    AppComponent.prototype.ngOnInit = function () {
        this.router.events.subscribe(function (evt) {
            if (!(evt instanceof __WEBPACK_IMPORTED_MODULE_4__angular_router__["NavigationEnd"])) {
                return;
            }
            window.scrollTo(0, 0);
        });
    };
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
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"])
    ], AppComponent.prototype, "layourContainerViewChild", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["ViewChild"])('layoutMenuScroller'),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_0__angular_core__["ElementRef"])
    ], AppComponent.prototype, "layoutMenuScrollerViewChild", void 0);
    AppComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-root',
            template: __webpack_require__("../../../../../src/app/app.component.html"),
            styles: [__webpack_require__("../../../../../src/app/app.component.scss")],
            providers: [__WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__["a" /* LookupsService */], __WEBPACK_IMPORTED_MODULE_2__configs_configs_service__["a" /* ConfigsService */]]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_0__angular_core__["Renderer"], __WEBPACK_IMPORTED_MODULE_4__angular_router__["Router"]])
    ], AppComponent);
    return AppComponent;
}());



/***/ }),

/***/ "../../../../../src/app/app.footer.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppFooter; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var AppFooter = /** @class */ (function () {
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



/***/ }),

/***/ "../../../../../src/app/app.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__ = __webpack_require__("../../../platform-browser/esm5/platform-browser.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_animations__ = __webpack_require__("../../../platform-browser/esm5/animations.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_routing_module__ = __webpack_require__("../../../../../src/app/app-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_http__ = __webpack_require__("../../../http/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__menu_app_menu_component__ = __webpack_require__("../../../../../src/app/menu/app.menu.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__app_topbar_component__ = __webpack_require__("../../../../../src/app/app.topbar.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__app_footer_component__ = __webpack_require__("../../../../../src/app/app.footer.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__menu_app_profile_component__ = __webpack_require__("../../../../../src/app/menu/app.profile.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__not_found_component__ = __webpack_require__("../../../../../src/app/not-found.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__auth_authorize_authorize_component__ = __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__shared_services_token_interceptor__ = __webpack_require__("../../../../../src/app/shared/services/token.interceptor.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__online_orders_online_orders_module__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__reports_reports_module__ = __webpack_require__("../../../../../src/app/reports/reports.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__exports_exports_module__ = __webpack_require__("../../../../../src/app/exports/exports.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__my_work_orders_my_work_orders_module__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.module.ts");
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
























var AppModule = /** @class */ (function () {
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
                __WEBPACK_IMPORTED_MODULE_21__my_work_orders_my_work_orders_module__["a" /* MyWorkOrdersModule */],
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_13__angular_router__["Router"]])
    ], AppModule);
    return AppModule;
}());



/***/ }),

/***/ "../../../../../src/app/app.topbar.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AppTopBar; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
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


var AppTopBar = /** @class */ (function () {
    function AppTopBar(app) {
        this.app = app;
    }
    AppTopBar = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-topbar',
            template: "\n        <div class=\"topbar clearfix\">\n            <div class=\"topbar-left\">\n                <div class=\"logo\"></div>\n            </div>\n\n            <div class=\"topbar-right\">\n                <a id=\"menu-button\" href=\"#\" (click)=\"app.onMenuButtonClick($event)\">\n                    <i></i>\n                </a>\n            </div>\n        </div>\n    "
        }),
        __param(0, Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */]; }))),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__app_component__["a" /* AppComponent */]])
    ], AppTopBar);
    return AppTopBar;
}());



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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var AuthorizeComponent = /** @class */ (function () {
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
            //console.log('endSigninMainWindow.user: ', user);
            _this.auth.getUserEmitter().emit(user);
            if (user.state && user.profile.role == "Hirer" && user.state == "/welcome") {
                rtr.navigate(['/online-orders/introduction']);
                return;
            }
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */], __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]])
    ], AuthorizeComponent);
    return AuthorizeComponent;
}());



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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
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


var DashboardComponent = /** @class */ (function () {
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */]])
    ], DashboardComponent);
    return DashboardComponent;
}());



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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var RegisterComponent = /** @class */ (function () {
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
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



var UnauthorizedComponent = /** @class */ (function () {
    //, private location:Location
    function UnauthorizedComponent(location, auth) {
        this.location = location;
        this.auth = auth;
    }
    UnauthorizedComponent.prototype.ngOnInit = function () {
    };
    UnauthorizedComponent.prototype.login = function () {
        this.auth.startSigninMainWindow();
    };
    UnauthorizedComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-unauthorized',
            template: __webpack_require__("../../../../../src/app/auth/unauthorized/unauthorized.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/unauthorized/unauthorized.component.scss")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common__["Location"], __WEBPACK_IMPORTED_MODULE_2__shared_services_auth_service__["a" /* AuthService */]])
    ], UnauthorizedComponent);
    return UnauthorizedComponent;
}());



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

module.exports = "<div [innerHTML]=\"welcome\">\r\n</div>\r\n<button pButton type=\"button\" (click)=\"login()\" label=\"Login\">Sign in</button>"

/***/ }),

/***/ "../../../../../src/app/auth/welcome/welcome.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WelcomeComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};




var WelcomeComponent = /** @class */ (function () {
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__configs_configs_service__["a" /* ConfigsService */],
            __WEBPACK_IMPORTED_MODULE_2__shared_index__["b" /* AuthService */],
            __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"]])
    ], WelcomeComponent);
    return WelcomeComponent;
}());



/***/ }),

/***/ "../../../../../src/app/configs/configs.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ConfigsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
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





var ConfigsService = /** @class */ (function () {
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
            return __WEBPACK_IMPORTED_MODULE_3_rxjs_Observable__["a" /* Observable */].of(this.configs);
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */]])
    ], ConfigsService);
    return ConfigsService;
}());



/***/ }),

/***/ "../../../../../src/app/employers/employers-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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
var EmployersRoutingModule = /** @class */ (function () {
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

module.exports = "<h1>\r\n  Employer profile\r\n</h1>\r\n<div class=\"card\">\r\n  This is your employer profile. If you're a new employer, you must create a profile before you can place an order.\r\n  This profile information will pre-populate new orders to save you time.\r\n</div>\r\n<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"employerForm\" (ngSubmit)=\"saveEmployer()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"name\">Name</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"name\" \r\n                      id=\"name\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.name}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"phone\">Phone number</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"phone\" \r\n                      id=\"phone\"\r\n                      type=\"text\" \r\n                      placeholder=\"123-456-7890\"\r\n                      pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.phone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"cellphone\">Cell phone</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"cellphone\" \r\n                      id=\"cellphone\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.cellphone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"email\">Email</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"email\" \r\n                      id=\"email\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.email}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"referredBy\">Referred by?</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"referredBy\" \r\n                      id=\"referredBy\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.referredBy}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"referredByOther\">Referred by notes</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"referredByOther\" \r\n                      id=\"referredByOther\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.referredByOther}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n        \r\n      </div>\r\n      <!-- -------------------------vertical divide---------------------------- -->\r\n      <div class=\"ui-g-12  ui-md-6\">\r\n        <div class=\"ui-g-12\"> \r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"business\">Is a business?</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <p-dropdown id=\"business\" [options]=\"yesNoDropDown\" formControlName=\"business\"\r\n                [autoWidth]=\"false\"></p-dropdown>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.business}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"address1\">Address (1)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"address1\" \r\n                      id=\"address1\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.address1}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"address2\">Address (2)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"address2\" \r\n                      id=\"address2\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.address2}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"city\">City</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"city\" \r\n                      id=\"city\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.city}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"state\">State</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"state\" \r\n                      id=\"state\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"zipcode\">Zipcode</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"zipcode\" \r\n                      id=\"zipcode\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.zipcode}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        \r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>        "

/***/ }),

/***/ "../../../../../src/app/employers/employers.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_models_employer__ = __webpack_require__("../../../../../src/app/shared/models/employer.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__shared_validators_phone__ = __webpack_require__("../../../../../src/app/shared/validators/phone.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







//import { phoneValidator } from '../shared/validators/phone';


var EmployersComponent = /** @class */ (function () {
    function EmployersComponent(employersService, lookupsService, fb, router) {
        this.employersService = employersService;
        this.lookupsService = lookupsService;
        this.fb = fb;
        this.router = router;
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
    }
    EmployersComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.buildForm();
        this.employersService.getEmployer()
            .subscribe(function (data) {
            _this.employer = data || new __WEBPACK_IMPORTED_MODULE_2__shared_models_employer__["a" /* Employer */]();
            _this.buildForm();
        });
    };
    EmployersComponent.prototype.buildForm = function () {
        var _this = this;
        this.employerForm = this.fb.group({
            //'id': [this.employer.id],
            'address1': [this.employer.address1, Object(__WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__["a" /* requiredValidator */])('Address is required')],
            'address2': [this.employer.address2],
            'blogparticipate': [this.employer.blogparticipate],
            'business': [this.employer.business],
            'businessname': [this.employer.businessname],
            'cellphone': [this.employer.cellphone],
            'city': [this.employer.city, Object(__WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__["a" /* requiredValidator */])('City is required')],
            'email': [this.employer.email, Object(__WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__["a" /* requiredValidator */])('Email is required')],
            'fax': [this.employer.fax],
            'name': [this.employer.name, Object(__WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__["a" /* requiredValidator */])('Name is required')],
            'phone': [this.employer.phone, Object(__WEBPACK_IMPORTED_MODULE_8__shared_validators_phone__["a" /* phoneValidator */])('Phone is required in ###-###-#### format')],
            'referredBy': [this.employer.referredBy],
            'referredByOther': [this.employer.referredByOther],
            'state': [this.employer.state, Object(__WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__["a" /* requiredValidator */])('State is required')],
            'zipcode': [this.employer.zipcode, Object(__WEBPACK_IMPORTED_MODULE_7__online_orders_shared_index__["a" /* requiredValidator */])('zipcode is required')]
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
                for (var key in control.errors) {
                    if (this.showErrors == true) {
                        console.log('onValueChanged.error:' + field + ': ' + control.errors[key]);
                    }
                    this.formErrors[field] += control.errors[key] + ' ';
                }
            }
        }
    };
    EmployersComponent.prototype.saveEmployer = function () {
        var _this = this;
        console.log('saveEmployer: called');
        this.onValueChanged();
        if (this.employerForm.status === 'INVALID') {
            this.showErrors = true;
            return;
        }
        console.log('saveEmployer: form status valid');
        this.showErrors = false;
        var formModel = this.employerForm.value;
        this.employersService.save(formModel)
            .subscribe(function (data) {
            console.log('employerService.save returned:', data);
            _this.router.navigate(['/online-orders/introduction']);
        }, 
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
            providers: []
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__employers_service__["a" /* EmployersService */],
            __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */],
            __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"],
            __WEBPACK_IMPORTED_MODULE_6__angular_router__["Router"]])
    ], EmployersComponent);
    return EmployersComponent;
}());



/***/ }),

/***/ "../../../../../src/app/employers/employers.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__employers_component__ = __webpack_require__("../../../../../src/app/employers/employers.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__employers_routing_module__ = __webpack_require__("../../../../../src/app/employers/employers-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
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






var EmployersModule = /** @class */ (function () {
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
                __WEBPACK_IMPORTED_MODULE_3__employers_routing_module__["a" /* EmployersRoutingModule */],
                __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["KeyFilterModule"]
            ],
            declarations: [__WEBPACK_IMPORTED_MODULE_2__employers_component__["a" /* EmployersComponent */]]
        }),
        __metadata("design:paramtypes", [])
    ], EmployersModule);
    return EmployersModule;
}());



/***/ }),

/***/ "../../../../../src/app/employers/employers.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return EmployersService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_rxjs__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_rxjs__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};








var EmployersService = /** @class */ (function () {
    function EmployersService(http, auth) {
        this.http = http;
        this.auth = auth;
        console.log('.ctor');
        this.employerSource = new __WEBPACK_IMPORTED_MODULE_6_rxjs__["BehaviorSubject"](null);
        this.fetchEmployer();
    }
    EmployersService.prototype.fetchEmployer = function () {
        var _this = this;
        var uri = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/employer/profile';
        return this.http.get(uri)
            .map(function (data) {
            _this.setEmployer(data['data']);
            return data['data'];
        })
            .catch(function (error) {
            console.log(error);
            _this.setEmployer(null);
            return Promise.reject(error.message || error);
        });
    };
    EmployersService.prototype.getEmployer = function () {
        //console.log('get---');
        return this.employerSource.asObservable();
    };
    EmployersService.prototype.setEmployer = function (employer) {
        //console.log('set===', employer);
        this.employerSource.next(employer);
    };
    EmployersService.prototype.save = function (employer) {
        var _this = this;
        var uri = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/employer/profile';
        var method;
        //uri = uri + '/' + employer.id;
        console.log('save:', uri, employer);
        // create or update 
        return this.http.put(uri, JSON.stringify(employer), {
            headers: new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json')
        }).map(function (data) {
            //console.log('employer from PUT:', data['data']);
            _this.setEmployer(data['data']);
            return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].of(data['data']);
        })
            .catch(__WEBPACK_IMPORTED_MODULE_4__shared_handle_error__["a" /* HandleError */].error);
    };
    EmployersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */], __WEBPACK_IMPORTED_MODULE_5__shared_index__["b" /* AuthService */]])
    ], EmployersService);
    return EmployersService;
}());



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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var ExportsOptionsComponent = /** @class */ (function () {
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
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormGroup"])
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
}());



/***/ }),

/***/ "../../../../../src/app/exports/exports-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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
var ExportsRoutingModule = /** @class */ (function () {
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__exports_service__ = __webpack_require__("../../../../../src/app/exports/exports.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_file_saver__ = __webpack_require__("../../../../file-saver/FileSaver.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_file_saver___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_file_saver__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
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






var ExportsComponent = /** @class */ (function () {
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */], __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"]])
    ], ExportsComponent);
    return ExportsComponent;
}());



/***/ }),

/***/ "../../../../../src/app/exports/exports.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__exports_component__ = __webpack_require__("../../../../../src/app/exports/exports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__("../../../http/esm5/http.js");
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








var ExportsModule = /** @class */ (function () {
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



/***/ }),

/***/ "../../../../../src/app/exports/exports.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ExportsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/toPromise.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/map.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/catch.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__("../../../http/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};








var ExportsService = /** @class */ (function () {
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
        return __WEBPACK_IMPORTED_MODULE_5_rxjs_Observable__["a" /* Observable */].of(error);
    };
    ExportsService.prototype.encodeData = function (data) {
        return Object.keys(data).map(function (key) {
            return [key, data[key]].map(encodeURIComponent).join('=');
        }).join('&');
    };
    ExportsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_7__angular_common_http__["b" /* HttpClient */]])
    ], ExportsService);
    return ExportsService;
}());



/***/ }),

/***/ "../../../../../src/app/lookups/lookups.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LookupsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
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






var LookupsService = /** @class */ (function () {
    function LookupsService(http) {
        this.http = http;
        this.uriBase = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/lookups';
        this.lookups = new Array();
        this.lookupsSource = new __WEBPACK_IMPORTED_MODULE_5_rxjs__["BehaviorSubject"](null);
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
            return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["a" /* Observable */].of(res['data']);
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_4__angular_common_http__["b" /* HttpClient */]])
    ], LookupsService);
    return LookupsService;
}());



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
var Record = /** @class */ (function () {
    function Record(init) {
        Object.assign(this, init);
    }
    return Record;
}());

var Lookup = /** @class */ (function (_super) {
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_animations__ = __webpack_require__("../../../animations/esm5/animations.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__app_component__ = __webpack_require__("../../../../../src/app/app.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__load_menu_rules__ = __webpack_require__("../../../../../src/app/menu/load-menu-rules.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
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







var AppMenuComponent = /** @class */ (function () {
    function AppMenuComponent(app, auth) {
        this.app = app;
        this.auth = auth;
        console.log('.ctor');
    }
    AppMenuComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.auth.getUserEmitter()
            .subscribe(function (user) {
            if (user == null) {
                return new Array();
            }
            var roles = user.profile['role'];
            if (typeof roles === 'string') {
                roles = [roles];
            }
            _this.model = Object(__WEBPACK_IMPORTED_MODULE_5__load_menu_rules__["a" /* loadMenuRules */])(roles);
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
        __param(0, Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_4__app_component__["a" /* AppComponent */]; }))),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_4__app_component__["a" /* AppComponent */],
            __WEBPACK_IMPORTED_MODULE_6__shared_index__["b" /* AuthService */]])
    ], AppMenuComponent);
    return AppMenuComponent;
}());

var AppSubMenu = /** @class */ (function () {
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
        __metadata("design:type", Object)
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
        __param(0, Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Inject"])(Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["forwardRef"])(function () { return __WEBPACK_IMPORTED_MODULE_4__app_component__["a" /* AppComponent */]; }))),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_4__app_component__["a" /* AppComponent */], __WEBPACK_IMPORTED_MODULE_3__angular_router__["Router"], __WEBPACK_IMPORTED_MODULE_2__angular_common__["Location"]])
    ], AppSubMenu);
    return AppSubMenu;
}());



/***/ }),

/***/ "../../../../../src/app/menu/app.profile.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"profile\" [ngClass]=\"{'profile-expanded':active}\">\r\n    <div class=\"profile-image\"></div>\r\n    <a href=\"#\" (click)=\"onClick($event)\">\r\n        <span class=\"profile-name\">{{this.user}}</span>\r\n        <i class=\"material-icons\">keyboard_arrow_down</i>\r\n    </a>\r\n</div>\r\n\r\n<ul class=\"ultima-menu profile-menu\" [@menu]=\"active ? 'visible' : 'hidden'\">\r\n    <!-- <li role=\"menuitem\">\r\n        <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\r\n            <i class=\"material-icons\">person</i>\r\n            <span>Profile</span>\r\n        </a>\r\n    </li> -->\r\n    <li role=\"menuitem\">\r\n        <a href=\"#\" (click)=\"startSignoutMainWindow()\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\r\n            <i class=\"material-icons\">power_settings_new</i>\r\n            <span>Logout</span>\r\n        </a>\r\n    </li>\r\n</ul>"

/***/ }),

/***/ "../../../../../src/app/menu/app.profile.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return InlineProfileComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
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


var InlineProfileComponent = /** @class */ (function () {
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
    InlineProfileComponent.prototype.startSignoutMainWindow = function () {
        this.auth.startSignoutMainWindow();
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__shared_index__["b" /* AuthService */]])
    ], InlineProfileComponent);
    return InlineProfileComponent;
}());



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
            routerLink: ['/my-work-orders'],
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
                //LRole.HIRER,
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


/***/ }),

/***/ "../../../../../src/app/menu/menu-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return MenuRule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return LRole; });
var MenuRule = /** @class */ (function () {
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


/***/ }),

/***/ "../../../../../src/app/my-work-orders/my-work-orders-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrdersRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__my_work_orders_component__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__order_complete_order_complete_component__ = __webpack_require__("../../../../../src/app/my-work-orders/order-complete/order-complete.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_datatable_work_order_datatable_component__ = __webpack_require__("../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__online_orders_guards_profile_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/profile.guard.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};







var woRoutes = [
    {
        path: 'my-work-orders',
        component: __WEBPACK_IMPORTED_MODULE_2__my_work_orders_component__["a" /* MyWorkOrdersComponent */],
        //canLoad: [AuthGuardService],
        canActivate: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */], __WEBPACK_IMPORTED_MODULE_6__online_orders_guards_profile_guard__["a" /* ProfileGuard */]],
        children: [
            {
                path: ':id',
                component: __WEBPACK_IMPORTED_MODULE_4__order_complete_order_complete_component__["a" /* OrderCompleteComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
            },
            {
                path: '',
                component: __WEBPACK_IMPORTED_MODULE_5__work_order_datatable_work_order_datatable_component__["a" /* WorkOrderDatatableComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
            }
        ]
    }
];
var WorkOrdersRoutingModule = /** @class */ (function () {
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



/***/ }),

/***/ "../../../../../src/app/my-work-orders/my-work-orders.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/my-work-orders/my-work-orders.component.html":
/***/ (function(module, exports) {

module.exports = "\r\n<router-outlet></router-outlet>"

/***/ }),

/***/ "../../../../../src/app/my-work-orders/my-work-orders.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyWorkOrdersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__my_work_orders_service__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var MyWorkOrdersComponent = /** @class */ (function () {
    function MyWorkOrdersComponent() {
    }
    MyWorkOrdersComponent.prototype.ngOnInit = function () {
    };
    MyWorkOrdersComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-my-work-orders',
            template: __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.component.html"),
            styles: [__webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.component.css")],
            providers: [__WEBPACK_IMPORTED_MODULE_1__my_work_orders_service__["a" /* MyWorkOrdersService */]]
        }),
        __metadata("design:paramtypes", [])
    ], MyWorkOrdersComponent);
    return MyWorkOrdersComponent;
}());



/***/ }),

/***/ "../../../../../src/app/my-work-orders/my-work-orders.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyWorkOrdersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__my_work_orders_component__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__my_work_orders_routing_module__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_angular2_moment__ = __webpack_require__("../../../../angular2-moment/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_angular2_moment___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_angular2_moment__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__order_complete_order_complete_component__ = __webpack_require__("../../../../../src/app/my-work-orders/order-complete/order-complete.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared_components_work_orders_work_orders_module__ = __webpack_require__("../../../../../src/app/shared/components/work-orders/work-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__work_order_datatable_work_order_datatable_component__ = __webpack_require__("../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};









var MyWorkOrdersModule = /** @class */ (function () {
    function MyWorkOrdersModule() {
    }
    MyWorkOrdersModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["DataTableModule"],
                __WEBPACK_IMPORTED_MODULE_4_primeng_primeng__["ButtonModule"],
                __WEBPACK_IMPORTED_MODULE_3__my_work_orders_routing_module__["a" /* WorkOrdersRoutingModule */],
                __WEBPACK_IMPORTED_MODULE_5_angular2_moment__["MomentModule"],
                __WEBPACK_IMPORTED_MODULE_7__shared_components_work_orders_work_orders_module__["a" /* WorkOrdersModule */]
            ],
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__my_work_orders_component__["a" /* MyWorkOrdersComponent */],
                __WEBPACK_IMPORTED_MODULE_6__order_complete_order_complete_component__["a" /* OrderCompleteComponent */],
                __WEBPACK_IMPORTED_MODULE_8__work_order_datatable_work_order_datatable_component__["a" /* WorkOrderDatatableComponent */],
            ]
        })
    ], MyWorkOrdersModule);
    return MyWorkOrdersModule;
}());



/***/ }),

/***/ "../../../../../src/app/my-work-orders/my-work-orders.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MyWorkOrdersService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
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





var MyWorkOrdersService = /** @class */ (function () {
    function MyWorkOrdersService(http) {
        this.http = http;
    }
    MyWorkOrdersService.prototype.getOrders = function () {
        var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders';
        return this.http.get(uri)
            .map(function (o) { return o['data']; })
            .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
    };
    MyWorkOrdersService.prototype.getOrder = function (id) {
        var url = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders/' + id;
        var postHeaders = new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json');
        return this.http.get(url, {
            headers: postHeaders
        }).map(function (data) {
            var wo = data['data'];
            console.log('getOrder received:', wo);
            return wo;
        }, function (err) {
            // TODO error
            console.error('online-orders.getOrder returned', err);
        });
    };
    MyWorkOrdersService.prototype.executePaypal = function (orderID, payerID, paymentID, token) {
        var url = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders/' + orderID + '/paypalexecute';
        var postHeaders = new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json');
        return this.http.post(url, JSON.stringify({
            payerID: payerID,
            paymentID: paymentID,
            paymentToken: token
        }), { headers: postHeaders })
            .map(function (data) {
            return data;
        });
    };
    MyWorkOrdersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */]])
    ], MyWorkOrdersService);
    return MyWorkOrdersService;
}());



/***/ }),

/***/ "../../../../../src/app/my-work-orders/order-complete/order-complete.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/my-work-orders/order-complete/order-complete.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid card ui-g\">\r\n  <full-order-view \r\n    [transportLabel]=\"transportLabel\"\r\n    [workerCount]=\"workerCount\"\r\n    [transportCost]=\"transportCost\"\r\n    [laborCost]=\"laborCost\"\r\n    [order]=\"order\"\r\n    [showPayPal]=\"true\">\r\n  </full-order-view> \r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/my-work-orders/order-complete/order-complete.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OrderCompleteComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_models_work_order__ = __webpack_require__("../../../../../src/app/shared/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_paypal_checkout__ = __webpack_require__("../../../../paypal-checkout/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_paypal_checkout___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_paypal_checkout__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__my_work_orders_service__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};









var OrderCompleteComponent = /** @class */ (function () {
    function OrderCompleteComponent(ordersService, lookups, route, router, configsService) {
        var _this = this;
        this.ordersService = ordersService;
        this.lookups = lookups;
        this.route = route;
        this.router = router;
        this.configsService = configsService;
        this.order = new __WEBPACK_IMPORTED_MODULE_1__shared_models_work_order__["a" /* WorkOrder */]();
        this.transportCost = 0;
        // paypal values
        this.didPaypalScriptLoad = false;
        this.loading = true;
        this.paypalConfig = {
            // The env (sandbox/production) tells the paypal client which URL to use. 
            env: '',
            client: {
                sandbox: '',
                production: ''
            },
            commit: true,
            payment: function (data, actions) {
                return actions.payment.create({
                    payment: {
                        transactions: [
                            { amount: { total: _this.transportCost, currency: 'USD' } }
                        ]
                    },
                    experience: {
                        input_fields: {
                            no_shipping: 1
                        }
                    },
                });
            },
            onAuthorize: function (data, actions) {
                console.log('Payment was successful!', data, actions);
                // TODO: add confirmation notice/spinner
                _this.ordersService.executePaypal(_this.order.id, data['payerID'], data['paymentID'], data['paymentToken'])
                    .subscribe(function (data) {
                    console.log('execute paypal returned:', data);
                    _this.ordersService.getOrder(_this.order.id)
                        .subscribe(function (foo) { return _this.order = foo; });
                }, function (error) { return console.error('execute paypal errored:', error); });
            },
            onCancel: function (data) {
                console.log('The payment was cancelled!', data);
            },
            onError: function (err) {
                console.log('There was an error:', err);
            }
        };
        console.log('.ctor');
    }
    OrderCompleteComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('order-complete.component:ngOnInit');
        var id = +this.route.snapshot.paramMap.get('id');
        __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["a" /* Observable */].combineLatest(this.lookups.getLookups(__WEBPACK_IMPORTED_MODULE_3__lookups_models_lookup__["a" /* LCategory */].TRANSPORT), this.ordersService.getOrder(id), this.configsService.getConfig('PayPalClientID'), this.configsService.getConfig('PayPalEnvironment')).subscribe(function (_a) {
            var l = _a[0], o = _a[1], id = _a[2], env = _a[3];
            console.log('ngOnInit:combineLatest received:', l, o, id, env);
            _this.paypalConfig['env'] = env.value;
            _this.paypalConfig.client[env.value] = id.value;
            console.log('paypalConfig', _this.paypalConfig);
            _this.order = o;
            if (o == null) {
                _this.router.navigate(['/online-orders/order-not-found']);
                return;
            }
            _this.transportLabel = l.find(function (ll) { return ll.id == o.transportMethodID; }).text_EN;
            var wa = o.workAssignments;
            if (wa != null && wa.length > 0) {
                // sums up the transport  costs
                _this.transportCost =
                    wa.map(function (wa) { return wa.transportCost; })
                        .reduce(function (a, b) { return a + b; });
                _this.workerCount = wa.length;
                // sums up the labor costs
                _this.laborCost =
                    wa.map(function (wa) { return wa.hourlyWage * wa.hours; })
                        .reduce(function (a, b) { return a + b; });
            }
            else {
                _this.workerCount = 0;
                _this.transportCost = 0;
                _this.laborCost = 0;
            }
        }, function (error) { return console.error('error', error); });
    };
    OrderCompleteComponent.prototype.ngAfterViewChecked = function () {
        if (this.transportCost > 0 && !this.didPaypalScriptLoad) {
            //this.loadPaypalScript().then(() => {
            __WEBPACK_IMPORTED_MODULE_5_paypal_checkout__["Button"].render(this.paypalConfig, '#paypal-button');
            this.loading = false;
            this.didPaypalScriptLoad = true;
            //});
        }
    };
    OrderCompleteComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-order-complete',
            template: __webpack_require__("../../../../../src/app/my-work-orders/order-complete/order-complete.component.html"),
            styles: [__webpack_require__("../../../../../src/app/my-work-orders/order-complete/order-complete.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_7__my_work_orders_service__["a" /* MyWorkOrdersService */],
            __WEBPACK_IMPORTED_MODULE_2__lookups_lookups_service__["a" /* LookupsService */],
            __WEBPACK_IMPORTED_MODULE_6__angular_router__["ActivatedRoute"],
            __WEBPACK_IMPORTED_MODULE_6__angular_router__["Router"],
            __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__["a" /* ConfigsService */]])
    ], OrderCompleteComponent);
    return OrderCompleteComponent;
}());



/***/ }),

/***/ "../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\r\n  These are your past orders:\r\n</p>\r\n<p-dataTable\r\n  #dt\r\n  sortMode=\"single\"\r\n  sortField=\"paperOrderNum\"\r\n  [sortOrder]=\"0\"\r\n  [value]=\"orders\"\r\n  [responsive]=\"true\"\r\n  resizable=\"true\"\r\n  [paginator]=\"true\"\r\n  [rows]=\"10\"\r\n  >\r\n  <p-header>\r\n    \r\n  </p-header>\r\n  <p-column styleClass=\"col-button\" field=\"paperOrderNum\" header=\"Order #\" [sortable]=\"true\">\r\n    <ng-template let-order=\"rowData\" pTemplate=\"body\">\r\n        <button type=\"button\" pButton (click)=\"viewOrder(order)\" icon=\"ui-icon-edit\"></button>\r\n        {{order.id }}\r\n    </ng-template>\r\n  </p-column>\r\n  <p-column field=\"dateTimeofWork\" header=\"Time needed\"  [sortable]=\"true\">\r\n      <ng-template let-col let-row=\"rowData\" pTemplate=\"body\">\r\n          <span >{{ row.dateTimeofWork |amDateFormat:'MMM DD YYYY, h:mm' }}</span>\r\n        </ng-template>\r\n  </p-column>\r\n  <p-column field=\"statusEN\" header=\"Status\"  [sortable]=\"true\"></p-column>\r\n  <p-column field=\"transportMethodEN\" header=\"Trans. method\" [sortable]=\"true\"></p-column>\r\n  <!-- <p-column field=\"\" header=\"Worker count\" [sortable]=\"true\"></p-column> -->\r\n  <p-column field=\"contactName\" header=\"Contact name\" [sortable]=\"true\"></p-column>\r\n  <p-column field=\"workSiteAddress1\" header=\"Address\" [sortable]=\"true\"></p-column>\r\n</p-dataTable>  \r\n"

/***/ }),

/***/ "../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderDatatableComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__my_work_orders_service__ = __webpack_require__("../../../../../src/app/my-work-orders/my-work-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var WorkOrderDatatableComponent = /** @class */ (function () {
    function WorkOrderDatatableComponent(workOrderService, router) {
        this.workOrderService = workOrderService;
        this.router = router;
    }
    WorkOrderDatatableComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.workOrderService.getOrders()
            .subscribe(function (data) {
            _this.orders = data;
        });
    };
    WorkOrderDatatableComponent.prototype.viewOrder = function (order) {
        this.router.navigate(["/my-work-orders/" + order.id]);
    };
    WorkOrderDatatableComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-order-datatable',
            template: __webpack_require__("../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.html"),
            styles: [__webpack_require__("../../../../../src/app/my-work-orders/work-order-datatable/work-order-datatable.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__my_work_orders_service__["a" /* MyWorkOrdersService */],
            __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]])
    ], WorkOrderDatatableComponent);
    return WorkOrderDatatableComponent;
}());



/***/ }),

/***/ "../../../../../src/app/not-found.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return PageNotFoundComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

var PageNotFoundComponent = /** @class */ (function () {
    function PageNotFoundComponent() {
    }
    PageNotFoundComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            template: '<h2>Page not found</h2>'
        })
    ], PageNotFoundComponent);
    return PageNotFoundComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/guards/order-confirm.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OrderConfirmGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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



var OrderConfirmGuard = /** @class */ (function () {
    function OrderConfirmGuard(onlineService, router) {
        var _this = this;
        this.onlineService = onlineService;
        this.router = router;
        this.isConfirmed = false;
        console.log('.ctor');
        onlineService.getWorkAssignmentConfirmedStream().subscribe(function (confirm) {
            console.log('.ctor->OrderConfirmed:', confirm);
            _this.isConfirmed = confirm;
        });
    }
    OrderConfirmGuard.prototype.canActivate = function () {
        return this.isConfirmed;
    };
    OrderConfirmGuard = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */], __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]])
    ], OrderConfirmGuard);
    return OrderConfirmGuard;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/guards/profile.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ProfileGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var ProfileGuard = /** @class */ (function () {
    function ProfileGuard(employersService, router) {
        this.employersService = employersService;
        this.router = router;
        console.log('.ctor');
    }
    ProfileGuard.prototype.canActivate = function () {
        var _this = this;
        return this.employersService.fetchEmployer()
            .map(function (em) {
            console.log('canActivate->getEmployer:', em);
            var exists = em ? true : false;
            if (!exists) {
                _this.router.navigate(['/employers']);
            }
            console.log('canActivate:', exists);
            return exists;
        });
    };
    ProfileGuard = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__employers_employers_service__["a" /* EmployersService */], __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]])
    ], ProfileGuard);
    return ProfileGuard;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/guards/work-assignments.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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



var WorkAssignmentsGuard = /** @class */ (function () {
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */], __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]])
    ], WorkAssignmentsGuard);
    return WorkAssignmentsGuard;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/guards/work-order.guard.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderGuard; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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



var WorkOrderGuard = /** @class */ (function () {
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */], __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]])
    ], WorkOrderGuard);
    return WorkOrderGuard;
}());



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

module.exports = "<div class=\"card\">\r\n  <strong>You must accept the following terms to continue:</strong>\r\n  <table>\r\n    <tr *ngFor=\"let item of confirmChoices\">\r\n      <td class=\"ui-g-1\">\r\n        <input type=\"checkbox\" [(ngModel)]=\"item.confirmed\" (ngModelChange)=\"checkConfirm()\">         \r\n      </td>\r\n      <td class=\"ui-g-11\">\r\n        {{item.description}}\r\n      </td>\r\n    </tr>\r\n  </table>\r\n  <button pButton [disabled]=\"!confirmStatus\" (click)=\"nextStep()\" label=\"Confirm and proceed\"></button>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroConfirmComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var IntroConfirmComponent = /** @class */ (function () {
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
        });
    };
    IntroConfirmComponent.prototype.checkConfirm = function (event) {
        var result = this.confirmChoices
            .map(function (a) { return a.confirmed; })
            .reduce(function (a, b) { return a && b; });
        this.confirmStatus = result;
        this.onlineService.setInitialConfirm(this.confirmChoices);
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */], __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]])
    ], IntroConfirmComponent);
    return IntroConfirmComponent;
}());



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

module.exports = "<div class=\"card\">\r\n    <p>\r\n        Casa Latina connects Latino immigrant workers with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! For more information about our program please read these Frequently Asked Questions\r\n      </p>\r\n      <p>\r\n        If you are ready to hire a worker, please fill out the following form.\r\n      </p>\r\n      <p>\r\n        If you still have questions about hiring a worker, please call us at 206.956.0779 x3.\r\n      </p>\r\n      \r\n      <button pButton type=\"button\" (click)=\"onClick()\" label=\"Next\"></button>\r\n      <button pButton type=\"button\" (click)=\"onClear()\" label=\"Start over\"></button>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/introduction/introduction.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroductionComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var IntroductionComponent = /** @class */ (function () {
    function IntroductionComponent(router, onlineService, orderService, assignmentService) {
        this.router = router;
        this.onlineService = onlineService;
        this.orderService = orderService;
        this.assignmentService = assignmentService;
    }
    IntroductionComponent.prototype.ngOnInit = function () {
    };
    IntroductionComponent.prototype.onClick = function () {
        this.router.navigate(['/online-orders/intro-confirm']);
    };
    IntroductionComponent.prototype.onClear = function () {
        this.onlineService.clearState();
        this.orderService.clearState();
        this.assignmentService.clearState();
    };
    IntroductionComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-introduction',
            template: __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"],
            __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */],
            __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */],
            __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */]])
    ], IntroductionComponent);
    return IntroductionComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_component__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__introduction_introduction_component__ = __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__order_confirm_order_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/order-confirm/order-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__guards_work_order_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/work-order.guard.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__guards_work_assignments_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/work-assignments.guard.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__guards_order_confirm_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/order-confirm.guard.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__order_not_found_order_not_found_component__ = __webpack_require__("../../../../../src/app/online-orders/order-not-found/order-not-found.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__guards_profile_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/profile.guard.ts");
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
        //canLoad: [AuthGuardService],
        canActivate: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */], __WEBPACK_IMPORTED_MODULE_14__guards_profile_guard__["a" /* ProfileGuard */]],
        children: [
            {
                path: 'introduction',
                component: __WEBPACK_IMPORTED_MODULE_3__introduction_introduction_component__["a" /* IntroductionComponent */],
            },
            {
                path: 'intro-confirm',
                component: __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__["a" /* IntroConfirmComponent */],
            },
            {
                path: 'work-order',
                component: __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__["a" /* WorkOrderComponent */],
                canActivate: [__WEBPACK_IMPORTED_MODULE_9__guards_work_order_guard__["a" /* WorkOrderGuard */]]
            },
            {
                path: 'work-assignments',
                component: __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */],
                canActivate: [__WEBPACK_IMPORTED_MODULE_11__guards_work_assignments_guard__["a" /* WorkAssignmentsGuard */]]
            },
            {
                path: 'order-confirm',
                component: __WEBPACK_IMPORTED_MODULE_7__order_confirm_order_confirm_component__["a" /* OrderConfirmComponent */],
                canActivate: [__WEBPACK_IMPORTED_MODULE_12__guards_order_confirm_guard__["a" /* OrderConfirmGuard */]]
            },
            {
                path: 'order-not-found',
                component: __WEBPACK_IMPORTED_MODULE_13__order_not_found_order_not_found_component__["a" /* OrderNotFoundComponent */],
            },
            {
                path: '**', redirectTo: 'order-not-found'
            }
        ]
    },
];
var OnlineOrdersRoutingModule = /** @class */ (function () {
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
                    __WEBPACK_IMPORTED_MODULE_11__guards_work_assignments_guard__["a" /* WorkAssignmentsGuard */],
                    __WEBPACK_IMPORTED_MODULE_12__guards_order_confirm_guard__["a" /* OrderConfirmGuard */],
                    __WEBPACK_IMPORTED_MODULE_2__online_orders_component__["a" /* OnlineOrdersComponent */],
                    __WEBPACK_IMPORTED_MODULE_10__work_order_work_order_service__["a" /* WorkOrderService */]
                ]
            ]
        })
    ], OnlineOrdersRoutingModule);
    return OnlineOrdersRoutingModule;
}());



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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var OnlineOrdersComponent = /** @class */ (function () {
    function OnlineOrdersComponent(onlineService, router) {
        var _this = this;
        this.onlineService = onlineService;
        this.router = router;
        this.activeIndex = 0;
        this.confirmation = false;
        router.events.subscribe(function (event) {
            // NavigationEnd event occurs after route succeeds
            if (event instanceof __WEBPACK_IMPORTED_MODULE_5__angular_router__["NavigationEnd"]) {
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
                    case '/online-orders/order-confirm': {
                        _this.activeIndex = 4;
                        break;
                    }
                }
            }
        });
    }
    OnlineOrdersComponent.prototype.ngOnInit = function () {
        this.items = [
            { label: 'Intro', routerLink: ['introduction'] },
            { label: 'Confirm', routerLink: ['intro-confirm'] },
            { label: 'site details', routerLink: ['work-order'] },
            { label: 'job details', routerLink: ['work-assignments'] },
            { label: 'confirm', routerLink: ['order-confirm'] }
        ];
    };
    OnlineOrdersComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-online-orders',
            template: __webpack_require__("../../../../../src/app/online-orders/online-orders.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/online-orders.component.css")],
            providers: [
                __WEBPACK_IMPORTED_MODULE_4__work_order_work_order_service__["a" /* WorkOrderService */],
                __WEBPACK_IMPORTED_MODULE_3__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */],
                __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"]
            ]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */],
            __WEBPACK_IMPORTED_MODULE_5__angular_router__["Router"]])
    ], OnlineOrdersComponent);
    return OnlineOrdersComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__introduction_introduction_component__ = __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__online_orders_component__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__order_confirm_order_confirm_component__ = __webpack_require__("../../../../../src/app/online-orders/order-confirm/order-confirm.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__online_orders_routing_module__ = __webpack_require__("../../../../../src/app/online-orders/online-orders-routing.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_9_primeng_primeng__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__schedule_rules_service__ = __webpack_require__("../../../../../src/app/online-orders/schedule-rules.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__transport_rules_service__ = __webpack_require__("../../../../../src/app/online-orders/transport-rules.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__order_not_found_order_not_found_component__ = __webpack_require__("../../../../../src/app/online-orders/order-not-found/order-not-found.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__shared_components_work_orders_work_orders_module__ = __webpack_require__("../../../../../src/app/shared/components/work-orders/work-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16__guards_profile_guard__ = __webpack_require__("../../../../../src/app/online-orders/guards/profile.guard.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};

















var OnlineOrdersModule = /** @class */ (function () {
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
                __WEBPACK_IMPORTED_MODULE_9_primeng_primeng__["ToggleButtonModule"],
                __WEBPACK_IMPORTED_MODULE_8__online_orders_routing_module__["a" /* OnlineOrdersRoutingModule */],
                __WEBPACK_IMPORTED_MODULE_15__shared_components_work_orders_work_orders_module__["a" /* WorkOrdersModule */]
            ],
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__introduction_introduction_component__["a" /* IntroductionComponent */],
                __WEBPACK_IMPORTED_MODULE_3__online_orders_component__["a" /* OnlineOrdersComponent */],
                __WEBPACK_IMPORTED_MODULE_4__intro_confirm_intro_confirm_component__["a" /* IntroConfirmComponent */],
                __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_component__["a" /* WorkOrderComponent */],
                __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */],
                __WEBPACK_IMPORTED_MODULE_7__order_confirm_order_confirm_component__["a" /* OrderConfirmComponent */],
                __WEBPACK_IMPORTED_MODULE_14__order_not_found_order_not_found_component__["a" /* OrderNotFoundComponent */]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_11__online_orders_service__["a" /* OnlineOrdersService */],
                __WEBPACK_IMPORTED_MODULE_12__schedule_rules_service__["a" /* ScheduleRulesService */],
                __WEBPACK_IMPORTED_MODULE_13__transport_rules_service__["a" /* TransportRulesService */],
                __WEBPACK_IMPORTED_MODULE_16__guards_profile_guard__["a" /* ProfileGuard */]
            ]
        })
    ], OnlineOrdersModule);
    return OnlineOrdersModule;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__shared_rules_load_confirms__ = __webpack_require__("../../../../../src/app/online-orders/shared/rules/load-confirms.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var OnlineOrdersService = /** @class */ (function () {
    function OnlineOrdersService(http, orderService) {
        this.http = http;
        this.orderService = orderService;
        this.workOrderConfirmSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs__["BehaviorSubject"](false);
        this.workAssignmentsConfirmSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs__["BehaviorSubject"](false);
        this.storageKey = 'machete.online-orders-service';
        this.initialConfirmKey = this.storageKey + '.initialconfirm';
        this.workOrderConfirmKey = this.storageKey + '.workorderconfirm';
        this.workAssignmentConfirmKey = this.storageKey + '.workassignmentsconfirm';
        console.log('.ctor');
        // this loads static data from a file. will replace later.
        this.loadConfirmState();
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
        // This pattern is ugly; should be able to simplify, perhaps use BehaviorSubjectSource instead
        // of companion private variable
        var loadedConfirms = JSON.parse(sessionStorage.getItem(this.initialConfirmKey));
        if (loadedConfirms != null && loadedConfirms.length > 0) {
            this.initialConfirmSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs__["BehaviorSubject"](loadedConfirms);
        }
        else {
            this.initialConfirmSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs__["BehaviorSubject"](Object(__WEBPACK_IMPORTED_MODULE_5__shared_rules_load_confirms__["a" /* loadConfirms */])());
        }
        // notify the subscribers
        this.workOrderConfirmSource.next(sessionStorage.getItem(this.workOrderConfirmKey) == 'true');
        this.workAssignmentsConfirmSource.next(sessionStorage.getItem(this.workAssignmentConfirmKey) == 'true');
    };
    OnlineOrdersService.prototype.clearState = function () {
        console.log('OnlineOrdersService.clearState-----');
        this.setInitialConfirm(Object(__WEBPACK_IMPORTED_MODULE_5__shared_rules_load_confirms__["a" /* loadConfirms */])());
        this.setWorkorderConfirm(false);
        this.setWorkAssignmentsConfirm(false);
    };
    OnlineOrdersService.prototype.setInitialConfirm = function (choice) {
        //console.log('setInitialConfirm:', choice);
        sessionStorage.setItem(this.initialConfirmKey, JSON.stringify(choice));
        this.initialConfirmSource.next(choice);
    };
    OnlineOrdersService.prototype.setWorkorderConfirm = function (choice) {
        //console.log('setWorkOrderConfirm:', choice);
        sessionStorage.setItem(this.workOrderConfirmKey, JSON.stringify(choice));
        this.workOrderConfirmSource.next(choice);
    };
    OnlineOrdersService.prototype.setWorkAssignmentsConfirm = function (choice) {
        //console.log('setWorkAssignmentsConfirm:', choice);
        sessionStorage.setItem(this.workAssignmentConfirmKey, JSON.stringify(choice));
        this.workAssignmentsConfirmSource.next(choice);
    };
    OnlineOrdersService.prototype.createOrder = function (order) {
        var url = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders';
        var postHeaders = new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json');
        return this.http.post(url, JSON.stringify(order), {
            headers: postHeaders
        }).map(function (data) {
            return data['data'];
        }, function (err) {
            if (err.error instanceof Error) {
                console.error('Client-side error occured.', err.error);
            }
            else {
                console.error('online-orders.service.POST: ' + err.message);
            }
        });
    };
    OnlineOrdersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */],
            __WEBPACK_IMPORTED_MODULE_2__work_order_work_order_service__["a" /* WorkOrderService */]])
    ], OnlineOrdersService);
    return OnlineOrdersService;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/order-confirm/order-confirm.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "div.ui-g-12.ui-md-8.ui-g-nopad { font-weight: bold; }", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/order-confirm/order-confirm.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid card ui-g\">\r\n  <full-order-view \r\n    [transportLabel]=\"transportLabel\"\r\n    [workerCount]=\"workerCount\"\r\n    [transportCost]=\"transportCost\"\r\n    [laborCost]=\"laborCost\"\r\n    [order]=\"order\"\r\n    [showPayPal]=\"false\">\r\n  </full-order-view>  \r\n  <div *ngIf=\"transportCost > 0\">\r\n  You have chosen a transport method that has fees associated with it. You will need to\r\n  pay the fees before the workers will be dispatched. You can pay using our PayPal form, \r\n  or call 206.956.0779 x3 to make arrangements. When you finalize this order below, you \r\n  will be taken to the PayPal transaction page. \r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <button pButton type=\"button\" (click)=\"submit()\" label=\"Finalize and submit\"></button>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/order-confirm/order-confirm.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OrderConfirmComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_models_work_order__ = __webpack_require__("../../../../../src/app/shared/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};









var OrderConfirmComponent = /** @class */ (function () {
    function OrderConfirmComponent(ordersService, onlineService, lookups, assignmentService, router) {
        this.ordersService = ordersService;
        this.onlineService = onlineService;
        this.lookups = lookups;
        this.assignmentService = assignmentService;
        this.router = router;
        this.order = new __WEBPACK_IMPORTED_MODULE_2__shared_models_work_order__["a" /* WorkOrder */]();
    }
    OrderConfirmComponent.prototype.ngOnInit = function () {
        var _this = this;
        __WEBPACK_IMPORTED_MODULE_7_rxjs_Observable__["a" /* Observable */].combineLatest(this.lookups.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT), this.ordersService.getStream(), this.assignmentService.getStream()).subscribe(function (_a) {
            var l = _a[0], o = _a[1], wa = _a[2];
            console.log('ngOnInit->combineLatest.subscribe', l, o, wa);
            _this.order = o;
            if (o.transportMethodID > 0) {
                _this.transportLabel = l.find(function (ll) { return ll.id == o.transportMethodID; }).text_EN;
            }
            if (wa != null && wa.length > 0) {
                // sums up the transport  costs
                _this.transportCost =
                    wa.map(function (wa) { return wa.transportCost; })
                        .reduce(function (a, b) { return a + b; });
                _this.workerCount = wa.length;
                // sums up the labor costs
                _this.laborCost =
                    wa.map(function (wa) { return wa.hourlyWage * wa.hours; })
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
    OrderConfirmComponent.prototype.submit = function () {
        var _this = this;
        this.onlineService.createOrder(this.order)
            .subscribe(function (data) {
            if (data.id == null) {
                console.error('workorder doesn\'t have an ID');
                return;
            }
            _this.onlineService.clearState();
            _this.ordersService.clearState();
            _this.assignmentService.clearState();
            _this.router.navigate(['/my-work-orders/' + data.id]);
        }, function (err) {
            console.error('POST error', err);
        });
    };
    OrderConfirmComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-order-confirm',
            template: __webpack_require__("../../../../../src/app/online-orders/order-confirm/order-confirm.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/order-confirm/order-confirm.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */],
            __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */],
            __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */],
            __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */],
            __WEBPACK_IMPORTED_MODULE_8__angular_router__["Router"]])
    ], OrderConfirmComponent);
    return OrderConfirmComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/order-not-found/order-not-found.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/order-not-found/order-not-found.component.html":
/***/ (function(module, exports) {

module.exports = "<p>\n  order-not-found works!\n</p>\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/order-not-found/order-not-found.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OrderNotFoundComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};

var OrderNotFoundComponent = /** @class */ (function () {
    function OrderNotFoundComponent() {
    }
    OrderNotFoundComponent.prototype.ngOnInit = function () {
    };
    OrderNotFoundComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-order-not-found',
            template: __webpack_require__("../../../../../src/app/online-orders/order-not-found/order-not-found.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/order-not-found/order-not-found.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], OrderNotFoundComponent);
    return OrderNotFoundComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/schedule-rules.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ScheduleRulesService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
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





var ScheduleRulesService = /** @class */ (function () {
    function ScheduleRulesService(http) {
        this.http = http;
        this.uriBase = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/schedulerules';
        this.rules = new Array();
        this.rulesAge = 0;
        console.log('.ctor');
    }
    ScheduleRulesService.prototype.isStale = function () {
        if (this.rulesAge > Date.now() - 3600 * 1000) {
            return false;
        }
        return true;
    };
    ScheduleRulesService.prototype.isNotStale = function () {
        return !this.isStale();
    };
    ScheduleRulesService.prototype.getScheduleRules = function () {
        var _this = this;
        if (this.isNotStale()) {
            return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].of(this.rules);
        }
        return this.http.get(this.uriBase)
            .map(function (res) {
            _this.rules = res['data'];
            _this.rulesAge = Date.now();
            return res['data'];
        })
            .catch(__WEBPACK_IMPORTED_MODULE_4__shared_handle_error__["a" /* HandleError */].error);
    };
    ScheduleRulesService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */]])
    ], ScheduleRulesService);
    return ScheduleRulesService;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/shared/index.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__validators_required__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/required.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_0__validators_required__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__validators_scheduling__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/scheduling.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_1__validators_scheduling__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__validators_transport__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/transport.ts");
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_schedule_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/schedule-rule.ts");
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_transport_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/transport-rule.ts");
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__models_record__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/record.ts");
/* unused harmony namespace reexport */








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

var Confirm = /** @class */ (function (_super) {
    __extends(Confirm, _super);
    function Confirm() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return Confirm;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));



/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/record.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Record; });
var Record = /** @class */ (function () {
    function Record(init) {
        Object.assign(this, init);
    }
    return Record;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/schedule-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony export ScheduleRule */
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

var ScheduleRule = /** @class */ (function (_super) {
    __extends(ScheduleRule, _super);
    function ScheduleRule() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return ScheduleRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));



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

var SkillRule = /** @class */ (function (_super) {
    __extends(SkillRule, _super);
    function SkillRule() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.maxHour = 8;
        return _this;
    }
    return SkillRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));



/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/transport-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* unused harmony export CostRule */
/* unused harmony export TransportRule */
/* unused harmony export TransportType */
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

var CostRule = /** @class */ (function (_super) {
    __extends(CostRule, _super);
    function CostRule() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return CostRule;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));

var TransportRule = /** @class */ (function (_super) {
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
            description: "This order is not complete until I receive a confirmation email from Casa Latina.  \n      If you do not hear from us or if you need a worker with 48 hours please call 206.956.0779 x3 \n      during our business hours.",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'arrival_time',
            description: " Please allow a one hour window for workers to arrive.This will account for \n      transportation routes with multiple stops and for traffic. There is no transportation fee to \n      hire a Casa Latina worker when you pick them up from our office. To have your worker(s) arrive \n      at your door, there is a small fee payable through this form.",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'employer_agency',
            description: "Casa Latina workers are not contractors. You will need to provide all tools, \n      materials, and safety equipment (link to http://casa-latina.org/information-for-employers) \n      necessary for the job(s) I wish to have done. ",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'screening',
            description: "I understand that as a condition of participation in our referral program, all \n      workers have been screened through the national and local sex offender registries.  However, \n      they have not gone through any other type of background checks.",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'in-house_training',
            description: "Casa Latina does not employ the workers that it refers.  When I make the decision \n      to hire, I am their employer and they are my employees. All workers referred through Casa Latina \n      have completed at least 10 hours of in-house training and Casa Latina does its best to only \n      refer workers who are skilled and dependable, but it cannot and does not guarantee that worker \n      performance will always meet my expectations. Casa Latina is not responsible for any failure \n      in worker performance or for any damage or injuries that could conceivably occur during the \n      performance of the work I have hired these workers to perform.",
            confirmed: false
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_confirm__["a" /* Confirm */]({
            name: 'satisfaction',
            description: "If I am dissatisfied with work performance, I may ask that the worker stop \n      working and just pay them for the hours worked.  I am not obligated to continue to work with \n      her or him.",
            confirmed: false
        }),
    ];
}


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
            return { 'hours': 'Please choose the hours of work you need.' };
        }
        if (!skillIdControl.value) {
            return null; // should be handled in work-assignment.component reactive forms
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
        var rule = rules.find(function (s) { return s.day === date.getDay(); });
        if (diffdate < 0) {
            return { 'scheduling': 'Date cannot be in the past.' };
        }
        if (diffdate < (rule.leadHours * 3600)) {
            return { 'scheduling': 'Lead time less than ' + String(rule.leadHours) + ' hours.' };
        }
        if (date.getHours() < (rule.minStartMin / 60)) {
            return { 'scheduling': 'Start time cannot be before ' + String(rule.minStartMin / 60) + ':00 hours' };
        }
        if (date.getHours() > (rule.maxEndMin / 60)) {
            return { 'scheduling': 'Start time cannot be after ' + String(rule.maxEndMin / 60) + ':00 hours' };
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


/***/ }),

/***/ "../../../../../src/app/online-orders/shared/validators/zipcode.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = zipcodeValidator;
function zipcodeValidator(rules) {
    return function (control) {
        if (control.value == null) {
            return null;
        }
        var rule = rules.find(function (s) { return s.zipcodes.includes(control.value); });
        if (rule === null || rule == undefined) {
            return { 'zipcode': 'Zipcode not found in service range.' };
        }
        return null;
    };
}


/***/ }),

/***/ "../../../../../src/app/online-orders/transport-rules.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TransportRulesService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
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





var TransportRulesService = /** @class */ (function () {
    function TransportRulesService(http) {
        this.http = http;
        this.uriBase = __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].dataUrl + '/api/transportrules';
        this.rules = new Array();
        this.rulesAge = 0;
        console.log('.ctor');
    }
    TransportRulesService.prototype.isStale = function () {
        if (this.rulesAge > Date.now() - 300 * 1000) {
            return false;
        }
        return true;
    };
    TransportRulesService.prototype.isNotStale = function () {
        return !this.isStale();
    };
    TransportRulesService.prototype.getTransportRules = function () {
        var _this = this;
        if (this.isNotStale()) {
            console.log('returning cache', this.rulesAge);
            return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].of(this.rules);
        }
        return this.http.get(this.uriBase)
            .map(function (res) {
            _this.rules = res['data'];
            _this.rulesAge = Date.now();
            return res['data'];
        })
            .catch(__WEBPACK_IMPORTED_MODULE_4__shared_handle_error__["a" /* HandleError */].error);
    };
    TransportRulesService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */]])
    ], TransportRulesService);
    return TransportRulesService;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "label { \r\n    font-weight: bold; \r\n} \r\n \r\n.ui-g-12 { \r\n    padding: .1em; \r\n} \r\n \r\n.ui-g-6 { \r\n    padding: .1em; \r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card ui-g-nopad\">\r\n    <form [formGroup]=\"requestForm\" (ngSubmit)=\"saveRequest()\" class=\"ui-g form-group ui-g-nopad\">\r\n      <div class=\"ui-g-12 ui-md-6 ui-g-nopad\">\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"skillsList\">Skill needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-dropdown id=\"skillsList\"\r\n                        [options]=\"skillsDropDown\"\r\n                        formControlName=\"skillId\"\r\n                        [(ngModel)]=\"request.skillId\"\r\n                        (onChange)=\"selectSkill(request.skillId)\"\r\n                        [autoWidth]=\"false\"\r\n                        placeholder=\"Select a skill\"></p-dropdown>\r\n\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['skillId'].valid && showErrors\">\r\n            {{formErrors.skillId}}\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\" *ngIf=\"this.selectedSkill.skillDescriptionEn != null\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n              <label for=\"description\">Skill description</label>\r\n          </div>\r\n          <div id=\"description\" class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.skillDescriptionEn}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\" *ngIf=\"this.selectedSkill.wage != null\">\r\n          <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n              <label for=\"wage\">Hourly rate</label>\r\n          </div>\r\n          <div id=\"wage\" class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.wage | currency:'USD':'true'}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\" *ngIf=\"this.selectedSkill.minHour != null\">\r\n          <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n            <label for=\"minHour\">Minimum time</label>\r\n          </div>\r\n          <div id=\"minHour\" class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.minHour}}\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n            <label for=\"hours\">Hours needed</label>\r\n          </div>\r\n          <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"hours\" id=\"hours\" type=\"text\" pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['hours'].valid && showErrors\">\r\n            {{formErrors.hours}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-8 ui-md-4 ui-g-nopad\">\r\n            <label for=\"requiresHeavyLifting\">Requires heavy lifting?</label>\r\n          </div>\r\n          <div class=\"ui-g-4 ui-md-8 ui-g-nopad\">\r\n            <p-inputSwitch id=\"requiresHeavyLifting\" formControlName=\"requiresHeavyLifting\"></p-inputSwitch>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Additional info about job</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <textarea rows=\"3\" class=\"ui-inputtext\" formControlName=\"description\" id=\"description\" type=\"text\" pInputText></textarea>\r\n          </div>\r\n        </div>\r\n    \r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save Job\"></button>\r\n      </div>\r\n    </form>\r\n  <div>\r\n    <p-dataTable [value]=\"requestList\" [(selection)]=\"selectedRequest\" (onRowSelect)=\"onRowSelect($event)\" [responsive]=\"true\">\r\n      <p-column field=\"id\" header=\"Request #\" [style]=\"{'padding':'.1em .875em'}\"></p-column>\r\n      <p-column field=\"skill\" header=\"Skill needed\" [style]=\"{'padding':'.1em .875em'}\"></p-column>\r\n      <p-column field=\"transportCost\" header=\"Transport cost\" [style]=\"{'padding':'.1em .875em'}\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field]| currency:'USD':'true'}}</span>\r\n        </ng-template>\r\n      </p-column>\r\n      <p-column field=\"hours\" header=\"hours requested\" [style]=\"{'padding':'.1em .875em'}\"></p-column>\r\n      <p-column field=\"description\" header=\"notes\" [style]=\"{'padding':'.1em .875em'}\" *ngIf=\"description != null\"></p-column>\r\n      <p-column field=\"requiresHeavyLifting\" header=\"Heavy lifting?\" [style]=\"{'padding':'.1em .875em'}\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field] ? 'Yes' : 'No'}}</span>\r\n        </ng-template>        \r\n      </p-column>\r\n      <p-column field=\"hourlyWage\" header=\"Hourly wage\" [style]=\"{'padding':'.1em .875em'}\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field]| currency:'USD':'true'}}</span>\r\n        </ng-template>\r\n      </p-column>\r\n\r\n      <p-column styleClass=\"col-button\">\r\n        <ng-template pTemplate=\"header\">\r\n          Actions\r\n        </ng-template>\r\n        <ng-template let-request=\"rowData\" pTemplate=\"body\">\r\n          <button type=\"button\" pButton (click)=\"editRequest(request)\" icon=\"ui-icon-edit\"></button>\r\n          <button type=\"button\" pButton (click)=\"deleteRequest(request)\" icon=\"ui-icon-delete\"></button>\r\n        </ng-template>\r\n      </p-column>\r\n    </p-dataTable>\r\n      <div class=\"ui-g\">\r\n        <button pButton type=\"button\"  (click)=\"finalize()\" [disabled]=\"!hasRequests\" label=\"Done with Jobs\"></button>\r\n      </div>\r\n  </div>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_models_work_assignment__ = __webpack_require__("../../../../../src/app/shared/models/work-assignment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__shared_validators_hours__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/hours.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__transport_rules_service__ = __webpack_require__("../../../../../src/app/online-orders/transport-rules.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__shared_models_skill_rule__ = __webpack_require__("../../../../../src/app/online-orders/shared/models/skill-rule.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};













var WorkAssignmentsComponent = /** @class */ (function () {
    function WorkAssignmentsComponent(lookupsService, waService, onlineService, transportRulesService, router, fb) {
        this.lookupsService = lookupsService;
        this.waService = waService;
        this.onlineService = onlineService;
        this.transportRulesService = transportRulesService;
        this.router = router;
        this.fb = fb;
        this.selectedSkill = new __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["b" /* Lookup */]();
        this.requestList = new Array(); // list built by user in UI
        this.request = new __WEBPACK_IMPORTED_MODULE_3__shared_models_work_assignment__["a" /* WorkAssignment */](); // composed by UI to make/edit a request
        this.newRequest = true;
        this.showErrors = false;
        this.hasRequests = false;
        this.formErrors = {
            'skillId': '',
            'skill': '',
            'hours': '',
            'description': '',
            'requiresHeavyLifting': '',
            'hourlyWage': ''
        };
        console.log('.ctor');
    }
    WorkAssignmentsComponent.prototype.ngOnInit = function () {
        var _this = this;
        console.log('ngOnInit');
        // waService.transportRules could fail under race conditions
        this.transportRulesService.getTransportRules()
            .subscribe(function (data) { return _this.transportRules = data; }, 
        // When this leads to a REST call, compactRequests will depend on it
        function (error) { return console.error('ngOnInit.getTransportRules.error' + error); }, function () { return console.log('ngOnInit:getTransportRules onCompleted'); });
        this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].SKILL)
            .subscribe(function (listData) {
            _this.skills = listData;
            _this.skillsDropDown = listData.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
            _this.skillsRules = listData.map(function (l) { return new __WEBPACK_IMPORTED_MODULE_12__shared_models_skill_rule__["a" /* SkillRule */](l); });
            console.log(_this.skillsRules);
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit:skills onCompleted'); });
        this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT)
            .subscribe(function (data) { return _this.transports = data; }, function (error) { return _this.errorMessage = error; }, function () { return console.log('ngOnInit:transports onCompleted'); });
        this.requestList = this.waService.getAll();
        this.setHasRequests();
        this.buildForm();
    };
    WorkAssignmentsComponent.prototype.buildForm = function () {
        var _this = this;
        this.requestForm = this.fb.group({
            'id': '',
            'skillId': ['', Object(__WEBPACK_IMPORTED_MODULE_8__shared__["a" /* requiredValidator */])('Please select the type of work to be performed.')],
            'skill': [''],
            'hours': ['', Object(__WEBPACK_IMPORTED_MODULE_10__shared_validators_hours__["a" /* hoursValidator */])(this.skillsRules, this.skills, 'skillId', 'hours')],
            'description': [''],
            'requiresHeavyLifting': [false],
            'hourlyWage': ['']
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
        this.requestForm.controls['hourlyWage'].setValue(skill.wage);
    };
    // loads an existing item into the form fields
    WorkAssignmentsComponent.prototype.editRequest = function (request) {
        this.requestForm.controls['id'].setValue(request.id);
        this.requestForm.controls['skillId'].setValue(request.skillId);
        this.requestForm.controls['skill'].setValue(request.skill);
        this.requestForm.controls['hours'].setValue(request.hours);
        this.requestForm.controls['description'].setValue(request.description);
        this.requestForm.controls['requiresHeavyLifting'].setValue(request.requiresHeavyLifting);
        this.requestForm.controls['hourlyWage'].setValue(request.hourlyWage);
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
            hourlyWage: formModel.hourlyWage,
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
        var request = new __WEBPACK_IMPORTED_MODULE_3__shared_models_work_assignment__["a" /* WorkAssignment */]();
        for (var prop in c) {
            request[prop] = c[prop];
        }
        return request;
    };
    WorkAssignmentsComponent.prototype.finalize = function () {
        this.router.navigate(['/online-orders/order-confirm']);
    };
    WorkAssignmentsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-assignments',
            template: __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */],
            __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__["a" /* WorkAssignmentsService */],
            __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */],
            __WEBPACK_IMPORTED_MODULE_11__transport_rules_service__["a" /* TransportRulesService */],
            __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"],
            __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"]])
    ], WorkAssignmentsComponent);
    return WorkAssignmentsComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_models_work_assignment__ = __webpack_require__("../../../../../src/app/shared/models/work-assignment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__transport_rules_service__ = __webpack_require__("../../../../../src/app/online-orders/transport-rules.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__shared_handle_error__ = __webpack_require__("../../../../../src/app/shared/handle-error.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};









var WorkAssignmentsService = /** @class */ (function () {
    function WorkAssignmentsService(onlineService, orderService, lookupsService, transportRulesService) {
        var _this = this;
        this.onlineService = onlineService;
        this.orderService = orderService;
        this.lookupsService = lookupsService;
        this.transportRulesService = transportRulesService;
        this.requests = new Array();
        this.storageKey = 'machete.workassignments';
        console.log('.ctor');
        var data = sessionStorage.getItem(this.storageKey);
        if (data) {
            console.log('sessionStorage:', data);
            var requests = JSON.parse(data);
            this.requests = requests;
        }
        this.combinedSource = __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].combineLatest(this.transportRulesService.getTransportRules(), this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT), this.orderService.getStream());
        var subscribed = this.combinedSource.subscribe(function (values) {
            console.log('combined subscription::', values);
            var rules = values[0], transports = values[1], order = values[2];
            _this.workOrder = order;
            _this.transportRules = rules;
            _this.transports = transports;
            _this.compactRequests();
        }, function (error) { return __WEBPACK_IMPORTED_MODULE_8__shared_handle_error__["a" /* HandleError */].error(error); }, function () { return console.log('combined subscription closed'); });
    }
    WorkAssignmentsService.prototype.getStream = function () {
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].of(this.requests);
    };
    WorkAssignmentsService.prototype.getAll = function () {
        return this.requests;
    };
    WorkAssignmentsService.prototype.save = function (request) {
        var _this = this;
        __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].zip(this.transportRulesService.getTransportRules(), this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT), this.orderService.getStream(), function () { })
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
        var sorted = this.requests.sort(__WEBPACK_IMPORTED_MODULE_1__shared_models_work_assignment__["a" /* WorkAssignment */].sort);
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
    WorkAssignmentsService.prototype.clearState = function () {
        this.requests = new Array();
        this.workOrder = null;
        console.log('WorkAssignmentsService.clearState-----');
        sessionStorage.removeItem(this.storageKey);
    };
    WorkAssignmentsService.prototype.findSelectedRequestIndex = function (request) {
        return this.requests.findIndex(function (a) { return a.id === request.id; });
    };
    WorkAssignmentsService.prototype.compactRequests = function () {
        var rule = this.getTransportRule();
        if (rule == null) {
            console.log('compactRequests: rule null, skipping...');
            return;
        }
        for (var i in this.requests) {
            var newid = Number(i);
            this.requests[newid].id = newid + 1;
            this.requests[newid].transportCost =
                this.calculateTransportCost(newid + 1, rule);
            console.log('completed compactRequest');
        }
    };
    WorkAssignmentsService.prototype.getTransportRule = function () {
        var order = this.workOrder;
        if (order === null || order === undefined) {
            console.log('OrderService returned an undefined order');
            return null;
        }
        if (order.transportMethodID <= 0) {
            console.log('Order missing valid transportMethodID');
            return null;
        }
        var lookup = this.transports.find(function (f) { return f.id == order.transportMethodID; });
        if (lookup === null || lookup === undefined) {
            console.log('LookupService didn\'t return a valid lookup for transportMethodID: ', order);
            return null;
        }
        var rules = this.transportRules.filter(function (f) { return f.lookupKey == lookup.key; });
        if (rules === null || rules === undefined) {
            throw new Error('No TransportRules match lookup key: ' + lookup.key);
        }
        var result = rules.find(function (f) { return f.zipcodes.includes(order.zipcode) ||
            f.zipcodes.includes("*"); });
        if (result === null || result == undefined) {
            throw new Error("Zipcode " + order.zipcode + " does not match any rule");
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3__online_orders_service__["a" /* OnlineOrdersService */],
            __WEBPACK_IMPORTED_MODULE_4__work_order_work_order_service__["a" /* WorkOrderService */],
            __WEBPACK_IMPORTED_MODULE_6__lookups_lookups_service__["a" /* LookupsService */],
            __WEBPACK_IMPORTED_MODULE_7__transport_rules_service__["a" /* TransportRulesService */]])
    ], WorkAssignmentsService);
    return WorkAssignmentsService;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "label { \r\n    font-weight: bold; \r\n} ", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card ui-g-nopad\">\r\n    <form [formGroup]=\"orderForm\" (ngSubmit)=\"save()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6 ui-g-nopad\">\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"dateTimeofWork\">Time needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                <p-calendar id=\"dateTimeofWork\"\r\n                            showTime=\"true\"\r\n                            stepMinute=\"15\"\r\n                            defaultDate=\"\"\r\n                            formControlName=\"dateTimeofWork\">\r\n                </p-calendar>\r\n                <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.dateTimeofWork}}\r\n                </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"contactName\">Contact name</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                  <input class=\"ui-inputtext ng-dirty ng-invalid\" formControlName=\"contactName\" id=\"contactName\"\r\n                         type=\"text\" pInputText/>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                    {{formErrors.contactName}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"worksiteAddress1\">Address (1)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                  <input class=\"ui-inputtext\" formControlName=\"worksiteAddress1\" id=\"worksiteAddress1\" type=\"text\"\r\n                         pInputText/>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                    {{formErrors.worksiteAddress1}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"worksiteAddress2\">Address (2)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"worksiteAddress2\" id=\"worksiteAddress2\" type=\"text\"\r\n                   pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"city\">City</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"city\" id=\"city\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.city}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"state\">State</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"state\" id=\"state\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"zipcode\">Zipcode</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"zipcode\" id=\"zipcode\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.zipcode}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"phone\">Phone</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" \r\n                formControlName=\"phone\" \r\n                id=\"phone\" \r\n                type=\"text\" \r\n                placeholder=\"123-456-7890\"\r\n                pInputText />\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.phone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Work Description</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\"\r\n                        formControlName=\"description\" id=\"description\" type=\"text\"></textarea>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.description}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n            <div class=\"ui-g-8 ui-md-10 ui-g-nopad\">\r\n              <label for=\"englishRequired\">Do you need an English-speaking worker?</label>\r\n            </div>\r\n            <div class=\"ui-g-4 ui-md-2 ui-g-nopad\">\r\n              <p-toggleButton [(ngModel)]=\"engReqToggle\" id=\"englishRequired\" formControlName=\"englishRequired\"></p-toggleButton>\r\n            </div>\r\n          </div>\r\n\r\n\r\n        <div class=\"ui-g-12\" *ngIf=\"engReqToggle\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"englishRequiredNote\">Please describe English requirement</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n              <span class=\"md-inputfield\">\r\n                  <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\"\r\n                            formControlName=\"englishRequiredNote\" id=\"englishRequiredNote\" type=\"text\"></textarea>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                      {{formErrors.englishRequiredNote}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"transportMethodID\">Transport method</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <p-dropdown id=\"transportMethodID\" [(ngModel)]=\"selectedTransport\" [options]=\"transportMethodsDropDown\" formControlName=\"transportMethodID\"\r\n                          [autoWidth]=\"false\"></p-dropdown>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.transportMethodID}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <!-- <button type=\"button\" (click)=\"showDialog()\" pButton icon=\"fa-external-link-square\" label=\"Show transport rates\" *ngIf=\"false\"></button> -->\r\n          <a href=\"http://casa-latina.org/get-involved/hire-worker#Do I have to pick up the worker?\" target=\"_blank\">\r\n            For transportation pricing, click here.\r\n          </a>\r\n        </div>\r\n        <div class=\"ui-g-12\" *ngIf=\"selectedTransport==32\">\r\n          Before picking a Casa Latina Van drop off time, please check for available times using the following calendar<br/>\r\n          <a href=\"https://calendar.google.com/calendar/embed?src=voluntarios.casa.latina@gmail.com&ctz=America/Los_Angeles\" target=\"_blank\">\r\n            For Van transport, select an available time slot.\r\n          </a> <br/>\r\n          Arrival times are estimates and can be adjusted by CL dispatchers due to traffic, or high demand, etc.\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>\r\n\r\n<div>\r\n  <p-dialog header=\"Title\" [(visible)]=\"displayTransportCosts\">\r\n      Content\r\n  </p-dialog>\r\n  <p-dialog header=\"User's guide\" [(visible)]=\"displayUserGuide\" modal=\"modal\" width=\"300\" [responsive]=\"true\">\r\n    Enter the basic information about the work, like the location\r\n    of the worksite and the method of transport for the workers. \r\n    <p>\r\n    You may pick up the workers, or Casa Latina offers a transport program \r\n    (fees apply), or you can pay for workers to use public transport.    \r\n    <p-footer>\r\n        <button type=\"button\" pButton icon=\"fa-check\" (click)=\"ackUserGuide()\" label=\"Ok\"></button>\r\n    </p-footer>\r\n  </p-dialog>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__shared_models_work_order__ = __webpack_require__("../../../../../src/app/shared/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__ = __webpack_require__("../../../../../src/app/shared/models/my-select-item.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__schedule_rules_service__ = __webpack_require__("../../../../../src/app/online-orders/schedule-rules.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__shared_validators_zipcode__ = __webpack_require__("../../../../../src/app/online-orders/shared/validators/zipcode.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__transport_rules_service__ = __webpack_require__("../../../../../src/app/online-orders/transport-rules.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__shared_validators_phone__ = __webpack_require__("../../../../../src/app/shared/validators/phone.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
















var WorkOrderComponent = /** @class */ (function () {
    function WorkOrderComponent(lookupsService, orderService, onlineService, configsService, schedulingRulesService, transportRulesService, router, fb) {
        this.lookupsService = lookupsService;
        this.orderService = orderService;
        this.onlineService = onlineService;
        this.configsService = configsService;
        this.schedulingRulesService = schedulingRulesService;
        this.transportRulesService = transportRulesService;
        this.router = router;
        this.fb = fb;
        this.order = new __WEBPACK_IMPORTED_MODULE_2__shared_models_work_order__["a" /* WorkOrder */]();
        this.showErrors = false;
        this.newOrder = true;
        this.displayTransportCosts = false;
        this.displayUserGuide = true;
        this.engReqToggle = false;
        this.selectedTransport = 0;
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
            'englishRequired': '',
            'englishRequiredNote': '',
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
        var _this = this;
        this.buildForm();
        __WEBPACK_IMPORTED_MODULE_11_rxjs_Observable__["a" /* Observable */].combineLatest(this.lookupsService.getLookups(__WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* LCategory */].TRANSPORT), this.orderService.getStream(), this.schedulingRulesService.getScheduleRules(), this.transportRulesService.getTransportRules()).subscribe(function (_a) {
            var l = _a[0], o = _a[1], s = _a[2], t = _a[3];
            _this.order = o;
            _this.engReqToggle = o.englishRequired;
            _this.transportMethods = l;
            _this.schedulingRules = s;
            _this.transportRules = t;
            // map transport entries to dropdown
            var items = [new __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__["a" /* MySelectItem */]('Select transportion', null)];
            var transports = l.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_9__shared_models_my_select_item__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
            _this.transportMethodsDropDown = items.concat(transports);
            _this.buildForm();
        });
    };
    WorkOrderComponent.prototype.buildForm = function () {
        var _this = this;
        this.selectedTransport = this.order.transportMethodID;
        this.orderForm = this.fb.group({
            'dateTimeofWork': [this.order.dateTimeofWork, [
                    Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('Date & time is required.'),
                    Object(__WEBPACK_IMPORTED_MODULE_7__shared__["b" /* schedulingValidator */])(this.schedulingRules)
                ]],
            'contactName': [this.order.contactName, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('Contact name is required')],
            'worksiteAddress1': [this.order.worksiteAddress1, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('Address is required')],
            'worksiteAddress2': [this.order.worksiteAddress2],
            'city': [this.order.city, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('City is required.')],
            'state': [this.order.state, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('State is required.')],
            'zipcode': [this.order.zipcode, [
                    Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('Zipcode is required.'),
                    Object(__WEBPACK_IMPORTED_MODULE_13__shared_validators_zipcode__["a" /* zipcodeValidator */])(this.transportRules)
                ]],
            'phone': [this.order.phone, Object(__WEBPACK_IMPORTED_MODULE_15__shared_validators_phone__["a" /* phoneValidator */])('Phone is required in ###-###-#### format')],
            'description': [this.order.description, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('Description is required')],
            'englishRequired': [this.order.englishRequired],
            'englishRequiredNote': [this.order.englishRequiredNote],
            'transportMethodID': [this.order.transportMethodID, Object(__WEBPACK_IMPORTED_MODULE_7__shared__["a" /* requiredValidator */])('A transport method is required')]
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
                    if (this.showErrors == true) {
                        console.log('onValueChanged.error:' + field + ': ' + control.errors[key]);
                    }
                    this.formErrors[field] += control.errors[key] + ' ';
                }
            }
        }
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
        var order = new __WEBPACK_IMPORTED_MODULE_2__shared_models_work_order__["a" /* WorkOrder */]({
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
            englishRequired: formModel.englishRequired,
            englishRequiredNote: formModel.englishRequiredNote,
            transportMethodID: formModel.transportMethodID
        });
        return order;
    };
    WorkOrderComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-order',
            template: __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/work-order/work-order.component.css")]
        }),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_3__lookups_lookups_service__["a" /* LookupsService */],
            __WEBPACK_IMPORTED_MODULE_6__work_order_service__["a" /* WorkOrderService */],
            __WEBPACK_IMPORTED_MODULE_4__online_orders_service__["a" /* OnlineOrdersService */],
            __WEBPACK_IMPORTED_MODULE_8__configs_configs_service__["a" /* ConfigsService */],
            __WEBPACK_IMPORTED_MODULE_12__schedule_rules_service__["a" /* ScheduleRulesService */],
            __WEBPACK_IMPORTED_MODULE_14__transport_rules_service__["a" /* TransportRulesService */],
            __WEBPACK_IMPORTED_MODULE_10__angular_router__["Router"],
            __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"]])
    ], WorkOrderComponent);
    return WorkOrderComponent;
}());



/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__shared_models_work_order__ = __webpack_require__("../../../../../src/app/shared/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__shared_models_employer__ = __webpack_require__("../../../../../src/app/shared/models/employer.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs__ = __webpack_require__("../../../../rxjs/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_rxjs__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};





var WorkOrderService = /** @class */ (function () {
    function WorkOrderService(employerService) {
        var _this = this;
        this.employerService = employerService;
        this.orderSource = new __WEBPACK_IMPORTED_MODULE_4_rxjs__["BehaviorSubject"](null);
        //order$ = this.orderSource.asObservable();
        this.storageKey = 'machete.workorder';
        var data = sessionStorage.getItem(this.storageKey);
        var order = new __WEBPACK_IMPORTED_MODULE_1__shared_models_work_order__["a" /* WorkOrder */](JSON.parse(data));
        // check that data's not null first
        if (data && order && order.isNotEmpty()) {
            console.log('.ctor->Loading existing order', order);
            order.dateTimeofWork = new Date(order.dateTimeofWork); // deserializing date
            this.order = order;
            this.orderSource.next(this.order);
        }
        else {
            console.log('.ctor->Create work order from employer');
            this.employerService.getEmployer()
                .subscribe(function (data) {
                // loading employer data as the defaults for
                // the new workorder
                _this.order = _this.mapOrderFrom(data || new __WEBPACK_IMPORTED_MODULE_3__shared_models_employer__["a" /* Employer */]());
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
        var order = new __WEBPACK_IMPORTED_MODULE_1__shared_models_work_order__["a" /* WorkOrder */]();
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
    WorkOrderService.prototype.clearState = function () {
        this.order = new __WEBPACK_IMPORTED_MODULE_1__shared_models_work_order__["a" /* WorkOrder */]();
        console.log('WorkOrdersService.clearState-----');
        sessionStorage.removeItem(this.storageKey);
        this.orderSource.next(this.order);
    };
    WorkOrderService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__employers_employers_service__["a" /* EmployersService */]])
    ], WorkOrderService);
    return WorkOrderService;
}());



/***/ }),

/***/ "../../../../../src/app/reports/models/report.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Report; });
/**
 * Created by jcarter on 3/9/17.
 */
var Report = /** @class */ (function () {
    function Report() {
    }
    return Report;
}());



/***/ }),

/***/ "../../../../../src/app/reports/models/search-inputs.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SearchInputs; });
/**
 * Created by jcii on 5/22/17.
 */
var SearchInputs = /** @class */ (function () {
    function SearchInputs() {
    }
    return SearchInputs;
}());



/***/ }),

/***/ "../../../../../src/app/reports/models/search-options.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SearchOptions; });
var SearchOptions = /** @class */ (function () {
    function SearchOptions() {
    }
    return SearchOptions;
}());



/***/ }),

/***/ "../../../../../src/app/reports/reports-routing.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsRoutingModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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
var ReportsRoutingModule = /** @class */ (function () {
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
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






var ReportsComponent = /** @class */ (function () {
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__reports_service__["a" /* ReportsService */]])
    ], ReportsComponent);
    return ReportsComponent;
}());



/***/ }),

/***/ "../../../../../src/app/reports/reports.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_component__ = __webpack_require__("../../../../../src/app/reports/reports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/esm5/forms.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_http__ = __webpack_require__("../../../http/esm5/http.js");
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







var ReportsModule = /** @class */ (function () {
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



/***/ }),

/***/ "../../../../../src/app/reports/reports.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ReportsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/toPromise.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_1_rxjs_add_operator_toPromise__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_add_operator_map__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/map.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_rxjs_add_operator_catch__ = __webpack_require__("../../../../rxjs/_esm5/add/operator/catch.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__angular_common_http__ = __webpack_require__("../../../common/esm5/http.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};







var ReportsService = /** @class */ (function () {
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
        return __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["a" /* Observable */].of(error);
    };
    ReportsService.prototype.encodeData = function (data) {
        return Object.keys(data).map(function (key) {
            return [key, data[key]].map(encodeURIComponent).join('=');
        }).join('&');
    };
    ReportsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_6__angular_common_http__["b" /* HttpClient */]])
    ], ReportsService);
    return ReportsService;
}());



/***/ }),

/***/ "../../../../../src/app/selective-preloading-strategy.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SelectivePreloadingStrategy; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0_rxjs_add_observable_of__ = __webpack_require__("../../../../rxjs/_esm5/add/observable/of.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__ = __webpack_require__("../../../../rxjs/_esm5/Observable.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var SelectivePreloadingStrategy = /** @class */ (function () {
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
            return __WEBPACK_IMPORTED_MODULE_2_rxjs_Observable__["a" /* Observable */].of(null);
        }
    };
    SelectivePreloadingStrategy = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_1__angular_core__["Injectable"])()
    ], SelectivePreloadingStrategy);
    return SelectivePreloadingStrategy;
}());



/***/ }),

/***/ "../../../../../src/app/shared/components/work-orders/full-order-view/full-order-view.component.css":
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__("../../../../css-loader/lib/css-base.js")(false);
// imports


// module
exports.push([module.i, "label {\r\n    font-weight: bold;\r\n}\r\n\r\n.ui-g-12 {\r\n    padding: .1em;\r\n}\r\n\r\n.thin-border {\r\n    border-style: solid;\r\n    border-color: black;\r\n    border-width: 1px;\r\n}", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/shared/components/work-orders/full-order-view/full-order-view.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-g-12 ui-md-6 ui-g-nopad\">\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n      <label for=\"dateTimeofWork\">Time requested</label>\r\n    </div>\r\n    <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n      {{order.dateTimeofWork |date:'short'}}\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n      <label for=\"contactName\">Contact & site information</label>\r\n    </div>\r\n    <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n      <div>{{order.contactName}}</div>\r\n      <div>{{order.worksiteAddress1}}</div>\r\n      <div>{{order.worksiteAddress2}}</div>\r\n      <div>{{order.city}}, \r\n      {{order.state}}\r\n      {{order.zipcode}}</div>\r\n      <div>{{order.phone}}</div>\r\n    </div>\r\n  </div>\r\n</div>\r\n<div class=\"ui-g-12 ui-md-6 ui-g-nopad\">\r\n  <div class=\"ui-g-12\" *ngIf=\"order.id\">\r\n      <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n        <label for=\"description\">Order #</label>\r\n      </div>\r\n      <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n        {{order.id }}\r\n      </div>\r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n      <label for=\"description\">Work Description</label>\r\n    </div>\r\n    <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n      {{order.description}}\r\n    </div>\r\n  </div>\r\n\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-8 ui-md-4 ui-g-nopad\">\r\n      <label for=\"englishRequired\">English required?</label>\r\n    </div>\r\n    <div class=\"ui-g-4 ui-md-8 ui-g-nopad\">\r\n      {{order.englishRequired ? 'Yes' : 'No'}}\r\n    </div>\r\n  </div>\r\n\r\n  <div class=\"ui-g-12\" *ngIf=\"order.englishRequiredNote\">\r\n    <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n      <label for=\"englishRequiredNote\">English requirements</label>\r\n    </div>\r\n    <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n      {{order.englishRequiredNote}}\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n      <label for=\"transportLabel\">Transport method</label>\r\n    </div>\r\n    <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n      {{transportLabel}}\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n      <label for=\"workerCount\">Worker count</label>\r\n    </div>\r\n    <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n      {{workerCount}}\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n      <label for=\"transportCost\">transport fees</label>\r\n    </div>\r\n    <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n      {{transportCost | currency:'USD':'true'}}\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      Due now and payable with Paypal\r\n    </div>  \r\n    <div class=\"ui-g-12\" *ngIf=\"showPayPal && transportCost > 0 && order.ppState != 'approved'\">        \r\n        <div id=\"paypal-button\" ></div>\r\n    </div>    \r\n  </div>\r\n  <div class=\"ui-g-12\" *ngIf=\"order.ppPaymentID != null\">\r\n    <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n      <label for=\"ppPaymentID\">Paypal Payment #</label>\r\n    </div>\r\n    <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n      {{order.ppPaymentID}}\r\n    </div>\r\n  </div>   \r\n  <div class=\"ui-g-12\" *ngIf=\"order.ppState != null\">\r\n    <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n      <label for=\"ppState\">Payment State</label>\r\n    </div>\r\n    <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n      {{order.ppState}}\r\n    </div>\r\n  </div>   \r\n  <div class=\"ui-g-12 thin-border\">\r\n    <div class=\"ui-g-6 ui-md-4 ui-g-nopad\">\r\n      <label for=\"laborCost\">labor cost</label>\r\n    </div>\r\n    <div class=\"ui-g-6 ui-md-8 ui-g-nopad\">\r\n      {{laborCost | currency:'USD':'true'}}\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n        To be paid directly to workers in cash at the end of the work day\r\n    </div>     \r\n  </div>\r\n</div>  \r\n\r\n<div>\r\n    <p-dataTable [value]=\"order.workAssignments\" \r\n      [responsive]=\"true\">\r\n      <p-column field=\"skill\" header=\"Skill needed\" [style]=\"{'padding':'.1em .875em'}\"></p-column>\r\n      <p-column field=\"requiresHeavyLifting\" header=\"Heavy lifting?\" [style]=\"{'padding':'.1em .875em'}\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field] ? 'Yes' : 'No'}}</span>\r\n        </ng-template>  \r\n      </p-column>\r\n      <p-column field=\"transportCost\" header=\"Transport cost\" [style]=\"{'padding':'.1em .875em'}\">\r\n          <ng-template pTemplate=\"header\">\r\n              <span>{{ header }}</span>\r\n         </ng-template>\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field]| currency:'USD':'true'}}</span>\r\n        </ng-template>\r\n      </p-column>\r\n      <p-column field=\"hours\" header=\"hours requested\"[style]=\"{'padding':'.1em .875em'}\"></p-column>\r\n      <p-column field=\"hourlyWage\" header=\"Hourly wage\" [style]=\"{'padding':'.1em .875em'}\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa[col.field]| currency:'USD':'true'}}</span>\r\n      </ng-template>\r\n      </p-column>\r\n      <p-column header=\"Wage subtotal\" [style]=\"{'padding':'.1em .875em'}\">\r\n        <ng-template let-col let-wa=\"rowData\" let-ri=\"rowIndex\" pTemplate=\"body\">\r\n          <span>{{wa['hours'] * wa['hourlyWage'] | currency:'USD':'true'}}</span>\r\n        </ng-template>\r\n      </p-column>\r\n    </p-dataTable>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/shared/components/work-orders/full-order-view/full-order-view.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FullOrderViewComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_work_order__ = __webpack_require__("../../../../../src/app/shared/models/work-order.ts");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


var FullOrderViewComponent = /** @class */ (function () {
    function FullOrderViewComponent() {
        console.log('.ctor');
    }
    FullOrderViewComponent.prototype.ngOnInit = function () {
    };
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", __WEBPACK_IMPORTED_MODULE_1__models_work_order__["a" /* WorkOrder */])
    ], FullOrderViewComponent.prototype, "order", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", String)
    ], FullOrderViewComponent.prototype, "transportLabel", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Number)
    ], FullOrderViewComponent.prototype, "workerCount", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Number)
    ], FullOrderViewComponent.prototype, "transportCost", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Number)
    ], FullOrderViewComponent.prototype, "laborCost", void 0);
    __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Input"])(),
        __metadata("design:type", Boolean)
    ], FullOrderViewComponent.prototype, "showPayPal", void 0);
    FullOrderViewComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'full-order-view',
            template: __webpack_require__("../../../../../src/app/shared/components/work-orders/full-order-view/full-order-view.component.html"),
            styles: [__webpack_require__("../../../../../src/app/shared/components/work-orders/full-order-view/full-order-view.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], FullOrderViewComponent);
    return FullOrderViewComponent;
}());



/***/ }),

/***/ "../../../../../src/app/shared/components/work-orders/work-orders.module.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrdersModule; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_common__ = __webpack_require__("../../../common/esm5/common.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__full_order_view_full_order_view_component__ = __webpack_require__("../../../../../src/app/shared/components/work-orders/full-order-view/full-order-view.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_primeng_primeng__ = __webpack_require__("../../../../primeng/primeng.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_primeng_primeng___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_primeng_primeng__);
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};




var WorkOrdersModule = /** @class */ (function () {
    function WorkOrdersModule() {
    }
    WorkOrdersModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_3_primeng_primeng__["DataTableModule"],
                __WEBPACK_IMPORTED_MODULE_3_primeng_primeng__["InputSwitchModule"]
            ],
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__full_order_view_full_order_view_component__["a" /* FullOrderViewComponent */]
            ],
            exports: [
                __WEBPACK_IMPORTED_MODULE_2__full_order_view_full_order_view_component__["a" /* FullOrderViewComponent */]
            ]
        })
    ], WorkOrdersModule);
    return WorkOrdersModule;
}());



/***/ }),

/***/ "../../../../../src/app/shared/handle-error.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return HandleError; });
/**
 * Created by jcii on 6/2/17.
 */
var HandleError = /** @class */ (function () {
    function HandleError() {
    }
    HandleError.error = function (error) {
        console.error(error);
        return Promise.reject(error.message || error);
    };
    return HandleError;
}());



/***/ }),

/***/ "../../../../../src/app/shared/index.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__services_auth_guard_service__ = __webpack_require__("../../../../../src/app/shared/services/auth-guard.service.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_0__services_auth_guard_service__["a"]; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "b", function() { return __WEBPACK_IMPORTED_MODULE_1__services_auth_service__["a"]; });




/***/ }),

/***/ "../../../../../src/app/shared/models/employer.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Employer; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/shared/models/record.ts");
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

var Employer = /** @class */ (function (_super) {
    __extends(Employer, _super);
    function Employer() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return Employer;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));



/***/ }),

/***/ "../../../../../src/app/shared/models/my-select-item.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MySelectItem; });
var MySelectItem = /** @class */ (function () {
    function MySelectItem(label, value) {
        this.label = label;
        this.value = value;
    }
    return MySelectItem;
}());



/***/ }),

/***/ "../../../../../src/app/shared/models/record.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Record; });
var Record = /** @class */ (function () {
    function Record(init) {
        Object.assign(this, init);
    }
    return Record;
}());



/***/ }),

/***/ "../../../../../src/app/shared/models/work-assignment.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignment; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/shared/models/record.ts");
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

var WorkAssignment = /** @class */ (function (_super) {
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
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));



/***/ }),

/***/ "../../../../../src/app/shared/models/work-order.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrder; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__record__ = __webpack_require__("../../../../../src/app/shared/models/record.ts");
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

var WorkOrder = /** @class */ (function (_super) {
    __extends(WorkOrder, _super);
    function WorkOrder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    WorkOrder.prototype.isNotEmpty = function () {
        return !this.isEmpty();
    };
    WorkOrder.prototype.isEmpty = function () {
        for (var key in this) {
            if (this[key] !== null && this[key] != "")
                console.log('Not empty: ', this[key]);
            return false;
        }
        return true;
    };
    return WorkOrder;
}(__WEBPACK_IMPORTED_MODULE_0__record__["a" /* Record */]));



/***/ }),

/***/ "../../../../../src/app/shared/services/auth-guard.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthGuardService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
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



var AuthGuardService = /** @class */ (function () {
    function AuthGuardService(authService, router) {
        this.authService = authService;
        this.router = router;
        console.log('.ctor');
    }
    AuthGuardService.prototype.canActivate = function (route, state) {
        var _this = this;
        //console.log(state);
        var isLoggedIn = this.authService.isLoggedInObs();
        isLoggedIn.subscribe(function (loggedin) {
            if (!loggedin) {
                //console.log('canActivate NOT loggedIn: url:', state)
                // state.url: where they were going before they got here
                _this.authService.setRedirectUrl(state.url);
                _this.router.navigate(['unauthorized']);
            }
        });
        return isLoggedIn;
    };
    AuthGuardService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_2__auth_service__["a" /* AuthService */], __WEBPACK_IMPORTED_MODULE_1__angular_router__["Router"]])
    ], AuthGuardService);
    return AuthGuardService;
}());



/***/ }),

/***/ "../../../../../src/app/shared/services/auth.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_http__ = __webpack_require__("../../../http/esm5/http.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__ = __webpack_require__("../../../../rxjs/_esm5/Rx.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};






var AuthService = /** @class */ (function () {
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
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].fromPromise(this.mgr.getUser()).map(function (user) {
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
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].fromPromise(this.mgr.getUser());
    };
    AuthService.prototype.getUserRoles$ = function () {
        return this.getUser$()
            .mergeMap(function (user) {
            console.log(user);
            if (user === null || user === undefined) {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].of(new Array());
            }
            else {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].of(user.profile.role);
            }
        });
    };
    AuthService.prototype.getUsername$ = function () {
        return this.getUser$()
            .mergeMap(function (user) {
            // TODO: if user is null, disable menu
            if (user === null || user === undefined) {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].of(null);
            }
            else {
                return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].of(user.profile.preferred_username);
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
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["a" /* Observable */].fromPromise(this.mgr.signinRedirectCallback(url));
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__angular_http__["b" /* Http */], __WEBPACK_IMPORTED_MODULE_5__angular_router__["Router"]])
    ], AuthService);
    return AuthService;
}());



/***/ }),

/***/ "../../../../../src/app/shared/services/token.interceptor.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TokenInterceptor; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_router__ = __webpack_require__("../../../router/esm5/router.js");
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};



var TokenInterceptor = /** @class */ (function () {
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
                _this.auth.startSigninMainWindow();
                //this.route.navigate(['/unauthorized']);
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
        __metadata("design:paramtypes", [__WEBPACK_IMPORTED_MODULE_1__auth_service__["a" /* AuthService */],
            __WEBPACK_IMPORTED_MODULE_2__angular_router__["Router"]])
    ], TokenInterceptor);
    return TokenInterceptor;
}());



/***/ }),

/***/ "../../../../../src/app/shared/validators/phone.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (immutable) */ __webpack_exports__["a"] = phoneValidator;
function phoneValidator(message) {
    return function (control) {
        var phoneNumRx = new RegExp(/^[\d]{3,3}-[\d]{3,3}-[\d]{4,4}$/);
        if (phoneNumRx.test(control.value)) {
            return null;
        }
        return { 'phone': message };
    };
}


/***/ }),

/***/ "../../../../../src/environments/environment.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return environment; });
var environment = {
    name: 'prod',
    production: true,
    dataUrl: 'https://api.machetessl.org',
    authUrl: 'https://identity.machetessl.org/id',
    baseRef: '/V2',
    oidc_client_settings: {
        authority: 'https://identity.machetessl.org/id',
        client_id: 'machete-casa-prod',
        redirect_uri: 'https://casa.machetessl.org/V2/authorize',
        post_logout_redirect_uri: 'https://casa.machetessl.org/V2',
        response_type: 'id_token token',
        scope: 'openid email roles api profile',
        silent_redirect_uri: 'https://casa.machetessl.org/V2/silent-renew.html',
        automaticSilentRenew: true,
        accessTokenExpiringNotificationTime: 4,
        // silentRequestTimeout:10000,
        filterProtocolClaims: true,
        loadUserInfo: true
    }
};


/***/ }),

/***/ "../../../../../src/main.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
Object.defineProperty(__webpack_exports__, "__esModule", { value: true });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/esm5/core.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__ = __webpack_require__("../../../platform-browser-dynamic/esm5/platform-browser-dynamic.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__app_app_module__ = __webpack_require__("../../../../../src/app/app.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__ = __webpack_require__("../../../../chart.js/dist/Chart.bundle.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_4_chart_js_dist_Chart_bundle_min_js__);





if (__WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].production) {
    Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["enableProdMode"])();
}
console.log('main.ts environment.name:', __WEBPACK_IMPORTED_MODULE_3__environments_environment__["a" /* environment */].name);
Object(__WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_dynamic__["a" /* platformBrowserDynamic */])().bootstrapModule(__WEBPACK_IMPORTED_MODULE_2__app_app_module__["a" /* AppModule */]);


/***/ }),

/***/ "../../../../moment/locale recursive ^\\.\\/.*$":
/***/ (function(module, exports, __webpack_require__) {

var map = {
	"./af": "../../../../moment/locale/af.js",
	"./af.js": "../../../../moment/locale/af.js",
	"./ar": "../../../../moment/locale/ar.js",
	"./ar-dz": "../../../../moment/locale/ar-dz.js",
	"./ar-dz.js": "../../../../moment/locale/ar-dz.js",
	"./ar-kw": "../../../../moment/locale/ar-kw.js",
	"./ar-kw.js": "../../../../moment/locale/ar-kw.js",
	"./ar-ly": "../../../../moment/locale/ar-ly.js",
	"./ar-ly.js": "../../../../moment/locale/ar-ly.js",
	"./ar-ma": "../../../../moment/locale/ar-ma.js",
	"./ar-ma.js": "../../../../moment/locale/ar-ma.js",
	"./ar-sa": "../../../../moment/locale/ar-sa.js",
	"./ar-sa.js": "../../../../moment/locale/ar-sa.js",
	"./ar-tn": "../../../../moment/locale/ar-tn.js",
	"./ar-tn.js": "../../../../moment/locale/ar-tn.js",
	"./ar.js": "../../../../moment/locale/ar.js",
	"./az": "../../../../moment/locale/az.js",
	"./az.js": "../../../../moment/locale/az.js",
	"./be": "../../../../moment/locale/be.js",
	"./be.js": "../../../../moment/locale/be.js",
	"./bg": "../../../../moment/locale/bg.js",
	"./bg.js": "../../../../moment/locale/bg.js",
	"./bm": "../../../../moment/locale/bm.js",
	"./bm.js": "../../../../moment/locale/bm.js",
	"./bn": "../../../../moment/locale/bn.js",
	"./bn.js": "../../../../moment/locale/bn.js",
	"./bo": "../../../../moment/locale/bo.js",
	"./bo.js": "../../../../moment/locale/bo.js",
	"./br": "../../../../moment/locale/br.js",
	"./br.js": "../../../../moment/locale/br.js",
	"./bs": "../../../../moment/locale/bs.js",
	"./bs.js": "../../../../moment/locale/bs.js",
	"./ca": "../../../../moment/locale/ca.js",
	"./ca.js": "../../../../moment/locale/ca.js",
	"./cs": "../../../../moment/locale/cs.js",
	"./cs.js": "../../../../moment/locale/cs.js",
	"./cv": "../../../../moment/locale/cv.js",
	"./cv.js": "../../../../moment/locale/cv.js",
	"./cy": "../../../../moment/locale/cy.js",
	"./cy.js": "../../../../moment/locale/cy.js",
	"./da": "../../../../moment/locale/da.js",
	"./da.js": "../../../../moment/locale/da.js",
	"./de": "../../../../moment/locale/de.js",
	"./de-at": "../../../../moment/locale/de-at.js",
	"./de-at.js": "../../../../moment/locale/de-at.js",
	"./de-ch": "../../../../moment/locale/de-ch.js",
	"./de-ch.js": "../../../../moment/locale/de-ch.js",
	"./de.js": "../../../../moment/locale/de.js",
	"./dv": "../../../../moment/locale/dv.js",
	"./dv.js": "../../../../moment/locale/dv.js",
	"./el": "../../../../moment/locale/el.js",
	"./el.js": "../../../../moment/locale/el.js",
	"./en-au": "../../../../moment/locale/en-au.js",
	"./en-au.js": "../../../../moment/locale/en-au.js",
	"./en-ca": "../../../../moment/locale/en-ca.js",
	"./en-ca.js": "../../../../moment/locale/en-ca.js",
	"./en-gb": "../../../../moment/locale/en-gb.js",
	"./en-gb.js": "../../../../moment/locale/en-gb.js",
	"./en-ie": "../../../../moment/locale/en-ie.js",
	"./en-ie.js": "../../../../moment/locale/en-ie.js",
	"./en-nz": "../../../../moment/locale/en-nz.js",
	"./en-nz.js": "../../../../moment/locale/en-nz.js",
	"./eo": "../../../../moment/locale/eo.js",
	"./eo.js": "../../../../moment/locale/eo.js",
	"./es": "../../../../moment/locale/es.js",
	"./es-do": "../../../../moment/locale/es-do.js",
	"./es-do.js": "../../../../moment/locale/es-do.js",
	"./es-us": "../../../../moment/locale/es-us.js",
	"./es-us.js": "../../../../moment/locale/es-us.js",
	"./es.js": "../../../../moment/locale/es.js",
	"./et": "../../../../moment/locale/et.js",
	"./et.js": "../../../../moment/locale/et.js",
	"./eu": "../../../../moment/locale/eu.js",
	"./eu.js": "../../../../moment/locale/eu.js",
	"./fa": "../../../../moment/locale/fa.js",
	"./fa.js": "../../../../moment/locale/fa.js",
	"./fi": "../../../../moment/locale/fi.js",
	"./fi.js": "../../../../moment/locale/fi.js",
	"./fo": "../../../../moment/locale/fo.js",
	"./fo.js": "../../../../moment/locale/fo.js",
	"./fr": "../../../../moment/locale/fr.js",
	"./fr-ca": "../../../../moment/locale/fr-ca.js",
	"./fr-ca.js": "../../../../moment/locale/fr-ca.js",
	"./fr-ch": "../../../../moment/locale/fr-ch.js",
	"./fr-ch.js": "../../../../moment/locale/fr-ch.js",
	"./fr.js": "../../../../moment/locale/fr.js",
	"./fy": "../../../../moment/locale/fy.js",
	"./fy.js": "../../../../moment/locale/fy.js",
	"./gd": "../../../../moment/locale/gd.js",
	"./gd.js": "../../../../moment/locale/gd.js",
	"./gl": "../../../../moment/locale/gl.js",
	"./gl.js": "../../../../moment/locale/gl.js",
	"./gom-latn": "../../../../moment/locale/gom-latn.js",
	"./gom-latn.js": "../../../../moment/locale/gom-latn.js",
	"./gu": "../../../../moment/locale/gu.js",
	"./gu.js": "../../../../moment/locale/gu.js",
	"./he": "../../../../moment/locale/he.js",
	"./he.js": "../../../../moment/locale/he.js",
	"./hi": "../../../../moment/locale/hi.js",
	"./hi.js": "../../../../moment/locale/hi.js",
	"./hr": "../../../../moment/locale/hr.js",
	"./hr.js": "../../../../moment/locale/hr.js",
	"./hu": "../../../../moment/locale/hu.js",
	"./hu.js": "../../../../moment/locale/hu.js",
	"./hy-am": "../../../../moment/locale/hy-am.js",
	"./hy-am.js": "../../../../moment/locale/hy-am.js",
	"./id": "../../../../moment/locale/id.js",
	"./id.js": "../../../../moment/locale/id.js",
	"./is": "../../../../moment/locale/is.js",
	"./is.js": "../../../../moment/locale/is.js",
	"./it": "../../../../moment/locale/it.js",
	"./it.js": "../../../../moment/locale/it.js",
	"./ja": "../../../../moment/locale/ja.js",
	"./ja.js": "../../../../moment/locale/ja.js",
	"./jv": "../../../../moment/locale/jv.js",
	"./jv.js": "../../../../moment/locale/jv.js",
	"./ka": "../../../../moment/locale/ka.js",
	"./ka.js": "../../../../moment/locale/ka.js",
	"./kk": "../../../../moment/locale/kk.js",
	"./kk.js": "../../../../moment/locale/kk.js",
	"./km": "../../../../moment/locale/km.js",
	"./km.js": "../../../../moment/locale/km.js",
	"./kn": "../../../../moment/locale/kn.js",
	"./kn.js": "../../../../moment/locale/kn.js",
	"./ko": "../../../../moment/locale/ko.js",
	"./ko.js": "../../../../moment/locale/ko.js",
	"./ky": "../../../../moment/locale/ky.js",
	"./ky.js": "../../../../moment/locale/ky.js",
	"./lb": "../../../../moment/locale/lb.js",
	"./lb.js": "../../../../moment/locale/lb.js",
	"./lo": "../../../../moment/locale/lo.js",
	"./lo.js": "../../../../moment/locale/lo.js",
	"./lt": "../../../../moment/locale/lt.js",
	"./lt.js": "../../../../moment/locale/lt.js",
	"./lv": "../../../../moment/locale/lv.js",
	"./lv.js": "../../../../moment/locale/lv.js",
	"./me": "../../../../moment/locale/me.js",
	"./me.js": "../../../../moment/locale/me.js",
	"./mi": "../../../../moment/locale/mi.js",
	"./mi.js": "../../../../moment/locale/mi.js",
	"./mk": "../../../../moment/locale/mk.js",
	"./mk.js": "../../../../moment/locale/mk.js",
	"./ml": "../../../../moment/locale/ml.js",
	"./ml.js": "../../../../moment/locale/ml.js",
	"./mr": "../../../../moment/locale/mr.js",
	"./mr.js": "../../../../moment/locale/mr.js",
	"./ms": "../../../../moment/locale/ms.js",
	"./ms-my": "../../../../moment/locale/ms-my.js",
	"./ms-my.js": "../../../../moment/locale/ms-my.js",
	"./ms.js": "../../../../moment/locale/ms.js",
	"./mt": "../../../../moment/locale/mt.js",
	"./mt.js": "../../../../moment/locale/mt.js",
	"./my": "../../../../moment/locale/my.js",
	"./my.js": "../../../../moment/locale/my.js",
	"./nb": "../../../../moment/locale/nb.js",
	"./nb.js": "../../../../moment/locale/nb.js",
	"./ne": "../../../../moment/locale/ne.js",
	"./ne.js": "../../../../moment/locale/ne.js",
	"./nl": "../../../../moment/locale/nl.js",
	"./nl-be": "../../../../moment/locale/nl-be.js",
	"./nl-be.js": "../../../../moment/locale/nl-be.js",
	"./nl.js": "../../../../moment/locale/nl.js",
	"./nn": "../../../../moment/locale/nn.js",
	"./nn.js": "../../../../moment/locale/nn.js",
	"./pa-in": "../../../../moment/locale/pa-in.js",
	"./pa-in.js": "../../../../moment/locale/pa-in.js",
	"./pl": "../../../../moment/locale/pl.js",
	"./pl.js": "../../../../moment/locale/pl.js",
	"./pt": "../../../../moment/locale/pt.js",
	"./pt-br": "../../../../moment/locale/pt-br.js",
	"./pt-br.js": "../../../../moment/locale/pt-br.js",
	"./pt.js": "../../../../moment/locale/pt.js",
	"./ro": "../../../../moment/locale/ro.js",
	"./ro.js": "../../../../moment/locale/ro.js",
	"./ru": "../../../../moment/locale/ru.js",
	"./ru.js": "../../../../moment/locale/ru.js",
	"./sd": "../../../../moment/locale/sd.js",
	"./sd.js": "../../../../moment/locale/sd.js",
	"./se": "../../../../moment/locale/se.js",
	"./se.js": "../../../../moment/locale/se.js",
	"./si": "../../../../moment/locale/si.js",
	"./si.js": "../../../../moment/locale/si.js",
	"./sk": "../../../../moment/locale/sk.js",
	"./sk.js": "../../../../moment/locale/sk.js",
	"./sl": "../../../../moment/locale/sl.js",
	"./sl.js": "../../../../moment/locale/sl.js",
	"./sq": "../../../../moment/locale/sq.js",
	"./sq.js": "../../../../moment/locale/sq.js",
	"./sr": "../../../../moment/locale/sr.js",
	"./sr-cyrl": "../../../../moment/locale/sr-cyrl.js",
	"./sr-cyrl.js": "../../../../moment/locale/sr-cyrl.js",
	"./sr.js": "../../../../moment/locale/sr.js",
	"./ss": "../../../../moment/locale/ss.js",
	"./ss.js": "../../../../moment/locale/ss.js",
	"./sv": "../../../../moment/locale/sv.js",
	"./sv.js": "../../../../moment/locale/sv.js",
	"./sw": "../../../../moment/locale/sw.js",
	"./sw.js": "../../../../moment/locale/sw.js",
	"./ta": "../../../../moment/locale/ta.js",
	"./ta.js": "../../../../moment/locale/ta.js",
	"./te": "../../../../moment/locale/te.js",
	"./te.js": "../../../../moment/locale/te.js",
	"./tet": "../../../../moment/locale/tet.js",
	"./tet.js": "../../../../moment/locale/tet.js",
	"./th": "../../../../moment/locale/th.js",
	"./th.js": "../../../../moment/locale/th.js",
	"./tl-ph": "../../../../moment/locale/tl-ph.js",
	"./tl-ph.js": "../../../../moment/locale/tl-ph.js",
	"./tlh": "../../../../moment/locale/tlh.js",
	"./tlh.js": "../../../../moment/locale/tlh.js",
	"./tr": "../../../../moment/locale/tr.js",
	"./tr.js": "../../../../moment/locale/tr.js",
	"./tzl": "../../../../moment/locale/tzl.js",
	"./tzl.js": "../../../../moment/locale/tzl.js",
	"./tzm": "../../../../moment/locale/tzm.js",
	"./tzm-latn": "../../../../moment/locale/tzm-latn.js",
	"./tzm-latn.js": "../../../../moment/locale/tzm-latn.js",
	"./tzm.js": "../../../../moment/locale/tzm.js",
	"./uk": "../../../../moment/locale/uk.js",
	"./uk.js": "../../../../moment/locale/uk.js",
	"./ur": "../../../../moment/locale/ur.js",
	"./ur.js": "../../../../moment/locale/ur.js",
	"./uz": "../../../../moment/locale/uz.js",
	"./uz-latn": "../../../../moment/locale/uz-latn.js",
	"./uz-latn.js": "../../../../moment/locale/uz-latn.js",
	"./uz.js": "../../../../moment/locale/uz.js",
	"./vi": "../../../../moment/locale/vi.js",
	"./vi.js": "../../../../moment/locale/vi.js",
	"./x-pseudo": "../../../../moment/locale/x-pseudo.js",
	"./x-pseudo.js": "../../../../moment/locale/x-pseudo.js",
	"./yo": "../../../../moment/locale/yo.js",
	"./yo.js": "../../../../moment/locale/yo.js",
	"./zh-cn": "../../../../moment/locale/zh-cn.js",
	"./zh-cn.js": "../../../../moment/locale/zh-cn.js",
	"./zh-hk": "../../../../moment/locale/zh-hk.js",
	"./zh-hk.js": "../../../../moment/locale/zh-hk.js",
	"./zh-tw": "../../../../moment/locale/zh-tw.js",
	"./zh-tw.js": "../../../../moment/locale/zh-tw.js"
};
function webpackContext(req) {
	return __webpack_require__(webpackContextResolve(req));
};
function webpackContextResolve(req) {
	var id = map[req];
	if(!(id + 1)) // check for number or string
		throw new Error("Cannot find module '" + req + "'.");
	return id;
};
webpackContext.keys = function webpackContextKeys() {
	return Object.keys(map);
};
webpackContext.resolve = webpackContextResolve;
module.exports = webpackContext;
webpackContext.id = "../../../../moment/locale recursive ^\\.\\/.*$";

/***/ }),

/***/ 0:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__("../../../../../src/main.ts");


/***/ })

},[0]);
//# sourceMappingURL=main.bundle.js.map