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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__auth_dashboard_dashboard_component__ = __webpack_require__("../../../../../src/app/auth/dashboard/dashboard.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__auth_unauthorized_unauthorized_component__ = __webpack_require__("../../../../../src/app/auth/unauthorized/unauthorized.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_7_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__auth_authorize_authorize_component__ = __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.ts");
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
        redirectTo: '/dashboard',
        pathMatch: 'full'
    },
    {
        path: 'dashboard',
        component: __WEBPACK_IMPORTED_MODULE_5__auth_dashboard_dashboard_component__["a" /* DashboardComponent */]
    },
    {
        path: 'unauthorized',
        component: __WEBPACK_IMPORTED_MODULE_6__auth_unauthorized_unauthorized_component__["a" /* UnauthorizedComponent */]
    },
    // Used to receive redirect from Identity server
    {
        path: 'authorize',
        component: __WEBPACK_IMPORTED_MODULE_8__auth_authorize_authorize_component__["a" /* AuthorizeComponent */]
    },
    //{ path: '**', component: PageNotFoundComponent }
    { path: '**', redirectTo: '/dashboard' }
];
var AppRoutingModule = (function () {
    function AppRoutingModule() {
        __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info('app-routing.module.ctor called');
    }
    AppRoutingModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                //AppComponent,
                __WEBPACK_IMPORTED_MODULE_6__auth_unauthorized_unauthorized_component__["a" /* UnauthorizedComponent */],
                __WEBPACK_IMPORTED_MODULE_5__auth_dashboard_dashboard_component__["a" /* DashboardComponent */]
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
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};


console.log('app.component.ts environment.name:', __WEBPACK_IMPORTED_MODULE_1__environments_environment__["a" /* environment */].name);
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
            styles: [__webpack_require__("../../../../../src/app/app.component.scss")]
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

/***/ "../../../../../src/app/app.menu.component.html":
/***/ (function(module, exports) {

module.exports = "<ng-template ngFor let-child let-i=\"index\" [ngForOf]=\"(root ? item : item.items)\">\r\n    <li [ngClass]=\"{'active-menuitem': isActive(i)}\" *ngIf=\"child.visible === false ? false : true\">\r\n        <a [href]=\"child.url||'#'\" \r\n            (click)=\"itemClick($event,child,i)\" \r\n            class=\"ripplelink\" \r\n            *ngIf=\"!child.routerLink\" \r\n            [attr.tabindex]=\"!visible ? '-1' : null\" \r\n            [attr.target]=\"child.target\">\r\n            <i class=\"material-icons\">{{child.icon}}</i>\r\n            <span>{{child.label}}</span>\r\n            <i class=\"material-icons\" *ngIf=\"child.items\">keyboard_arrow_down</i>\r\n        </a>\r\n\r\n        <a (click)=\"itemClick($event,child,i)\" \r\n            class=\"ripplelink\" \r\n            *ngIf=\"child.routerLink\"\r\n            [routerLink]=\"child.routerLink\" \r\n            routerLinkActive=\"active-menuitem-routerlink\" \r\n            [routerLinkActiveOptions]=\"{exact: true}\" \r\n            [attr.tabindex]=\"!visible ? '-1' : null\" \r\n            [attr.target]=\"child.target\">\r\n            <i class=\"material-icons\">{{child.icon}}</i>\r\n            <span>{{child.label}}</span>\r\n            <i class=\"material-icons\" *ngIf=\"child.items\">keyboard_arrow_down</i>\r\n        </a>\r\n        <ul app-submenu [item]=\"child\" *ngIf=\"child.items\" [@children]=\"isActive(i) ? 'visible' : 'hidden'\" \r\n            [visible]=\"isActive(i)\" [reset]=\"reset\"></ul>\r\n    </li>\r\n</ng-template>"

/***/ }),

/***/ "../../../../../src/app/app.menu.component.ts":
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
            { label: 'Place an order', icon: 'business', routerLink: ['/online-orders/introduction'] },
            { label: 'Employers', icon: 'business', routerLink: ['/employers'] },
            { label: 'Work Orders', icon: 'work', routerLink: ['/work-orders'] },
            { label: 'Dispatch', icon: 'today', url: ['/workassignment'] },
            { label: 'People', icon: 'people', url: ['/person'] },
            { label: 'Activities', icon: 'local_activity', url: ['/Activity'] },
            { label: 'Sign-ins', icon: 'track_changes', url: ['/workersignin'] },
            { label: 'Emails', icon: 'email', url: ['/email'] },
            { label: 'Reports', icon: 'subtitles', routerLink: ['/reports'] },
            { label: 'Exports', icon: 'file_download', routerLink: ['/exports'] },
            { label: 'Dashboard', icon: 'file_download', routerLink: ['/dashboard'] }
        ];
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
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__app_component__["a" /* AppComponent */]) === "function" && _a || Object])
    ], AppMenuComponent);
    return AppMenuComponent;
    var _a;
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
            template: __webpack_require__("../../../../../src/app/app.menu.component.html"),
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__app_menu_component__ = __webpack_require__("../../../../../src/app/app.menu.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__app_topbar_component__ = __webpack_require__("../../../../../src/app/app.topbar.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_10__app_footer_component__ = __webpack_require__("../../../../../src/app/app.footer.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_11__app_profile_component__ = __webpack_require__("../../../../../src/app/app.profile.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_12__not_found_component__ = __webpack_require__("../../../../../src/app/not-found.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_13__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_14__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_15__shared_services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_16_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_16_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_17__auth_authorize_authorize_component__ = __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_18__shared_services_token_interceptor__ = __webpack_require__("../../../../../src/app/shared/services/token.interceptor.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_19__online_orders_online_orders_module__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_20__reports_reports_module__ = __webpack_require__("../../../../../src/app/reports/reports.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_21__exports_exports_module__ = __webpack_require__("../../../../../src/app/exports/exports.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_22__work_orders_work_orders_module__ = __webpack_require__("../../../../../src/app/work-orders/work-orders.module.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_23__employers_employers_module__ = __webpack_require__("../../../../../src/app/employers/employers.module.ts");
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
            __WEBPACK_IMPORTED_MODULE_16_oidc_client__["Log"].level = __WEBPACK_IMPORTED_MODULE_16_oidc_client__["Log"].INFO;
            __WEBPACK_IMPORTED_MODULE_16_oidc_client__["Log"].logger = console;
        }
        __WEBPACK_IMPORTED_MODULE_16_oidc_client__["Log"].info('app.module.ctor()');
        //console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
    }
    AppModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_3__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_7__app_component__["a" /* AppComponent */],
                __WEBPACK_IMPORTED_MODULE_8__app_menu_component__["a" /* AppMenuComponent */],
                __WEBPACK_IMPORTED_MODULE_8__app_menu_component__["b" /* AppSubMenu */],
                __WEBPACK_IMPORTED_MODULE_9__app_topbar_component__["a" /* AppTopBar */],
                __WEBPACK_IMPORTED_MODULE_10__app_footer_component__["a" /* AppFooter */],
                __WEBPACK_IMPORTED_MODULE_11__app_profile_component__["a" /* InlineProfileComponent */],
                __WEBPACK_IMPORTED_MODULE_12__not_found_component__["a" /* PageNotFoundComponent */],
                __WEBPACK_IMPORTED_MODULE_17__auth_authorize_authorize_component__["a" /* AuthorizeComponent */]
            ],
            imports: [
                __WEBPACK_IMPORTED_MODULE_0__angular_platform_browser__["BrowserModule"],
                __WEBPACK_IMPORTED_MODULE_1__angular_platform_browser_animations__["a" /* BrowserAnimationsModule */],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormsModule"],
                __WEBPACK_IMPORTED_MODULE_6__angular_http__["c" /* HttpModule */],
                __WEBPACK_IMPORTED_MODULE_5__angular_common_http__["c" /* HttpClientModule */],
                __WEBPACK_IMPORTED_MODULE_20__reports_reports_module__["a" /* ReportsModule */],
                __WEBPACK_IMPORTED_MODULE_19__online_orders_online_orders_module__["a" /* OnlineOrdersModule */],
                __WEBPACK_IMPORTED_MODULE_21__exports_exports_module__["a" /* ExportsModule */],
                __WEBPACK_IMPORTED_MODULE_22__work_orders_work_orders_module__["a" /* WorkOrdersModule */],
                __WEBPACK_IMPORTED_MODULE_23__employers_employers_module__["a" /* EmployersModule */],
                __WEBPACK_IMPORTED_MODULE_2__app_routing_module__["a" /* AppRoutingModule */]
            ],
            providers: [
                __WEBPACK_IMPORTED_MODULE_15__shared_services_auth_service__["a" /* AuthService */],
                {
                    provide: __WEBPACK_IMPORTED_MODULE_5__angular_common_http__["a" /* HTTP_INTERCEPTORS */],
                    useClass: __WEBPACK_IMPORTED_MODULE_18__shared_services_token_interceptor__["a" /* TokenInterceptor */],
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

/***/ "../../../../../src/app/app.profile.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"profile\" [ngClass]=\"{'profile-expanded':active}\">\r\n    <div class=\"profile-image\"></div>\r\n    <a href=\"#\" (click)=\"onClick($event)\">\r\n        <span class=\"profile-name\">Jimmy Carter</span>\r\n        <i class=\"material-icons\">keyboard_arrow_down</i>\r\n    </a>\r\n</div>\r\n\r\n<ul class=\"ultima-menu profile-menu\" [@menu]=\"active ? 'visible' : 'hidden'\">\r\n    <li role=\"menuitem\">\r\n        <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\r\n            <i class=\"material-icons\">person</i>\r\n            <span>Profile</span>\r\n        </a>\r\n    </li>\r\n    <li role=\"menuitem\">\r\n        <a href=\"#\" class=\"ripplelink\" [attr.tabindex]=\"!active ? '-1' : null\">\r\n            <i class=\"material-icons\">power_settings_new</i>\r\n            <span>Logout</span>\r\n        </a>\r\n    </li>\r\n</ul>"

/***/ }),

/***/ "../../../../../src/app/app.profile.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return InlineProfileComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
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
    InlineProfileComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'inline-profile',
            template: __webpack_require__("../../../../../src/app/app.profile.component.html"),
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
        })
    ], InlineProfileComponent);
    return InlineProfileComponent;
}());

//# sourceMappingURL=app.profile.component.js.map

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
    function AuthorizeComponent(auth) {
        this.auth = auth;
    }
    AuthorizeComponent.prototype.ngOnInit = function () {
        this.auth.endSigninMainWindow(this.auth.redirectUrl);
    };
    AuthorizeComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-authorize',
            template: __webpack_require__("../../../../../src/app/auth/authorize/authorize.component.html"),
            styles: [__webpack_require__("../../../../../src/app/auth/authorize/authorize.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__shared_services_auth_service__["a" /* AuthService */]) === "function" && _a || Object])
    ], AuthorizeComponent);
    return AuthorizeComponent;
    var _a;
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
        this.loadedUserSub = this.authService.userLoadededEvent
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

/***/ "../../../../../src/app/auth/unauthorized/unauthorized.component.html":
/***/ (function(module, exports) {

module.exports = "<div>\r\n  Unauthorized Request<p> \r\n  to login click <a (click)=\"login()\">here</a>.\r\n</div>\r\n<br>\r\n<div>\r\n  To go back click <a (click)=\"goback()\">here</a>.\r\n</div>"

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
    UnauthorizedComponent.prototype.goback = function () {
        // this.location.back();
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

/***/ "../../../../../src/app/configs/configs.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return ConfigsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_common_http__ = __webpack_require__("../../../common/@angular/common/http.es5.js");
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




var ConfigsService = (function () {
    function ConfigsService(http) {
        this.http = http;
        this.uriBase = __WEBPACK_IMPORTED_MODULE_1__environments_environment__["a" /* environment */].dataUrl + '/api/lookups';
    }
    ConfigsService.prototype.getConfigs = function (category) {
        var uri = this.uriBase;
        if (category) {
            uri = uri + '?category=' + category;
        }
        return this.http.get(uri)
            .map(function (res) { return res['data']; })
            .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
    };
    ConfigsService.prototype.getConfig = function (key) {
        var uri = this.uriBase;
        uri = uri + '?key=' + key;
        return this.http.get(uri)
            .map(function (res) { return res['data']; })
            .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
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
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};



var employerRoutes = [
    {
        path: 'employers',
        component: __WEBPACK_IMPORTED_MODULE_2__employers_component__["a" /* EmployersComponent */]
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

module.exports = "<p>\r\n  employers works!\r\n</p>\r\n<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"employerForm\" (ngSubmit)=\"saveEmployer()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"name\">Name</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"name\" \r\n                      id=\"name\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.name}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"phone\">Phone number</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"phone\" \r\n                      id=\"phone\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.phone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"cellphone\">Cell phone</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"cellphone\" \r\n                      id=\"cellphone\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.cellphone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"email\">Email</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"email\" \r\n                      id=\"email\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.email}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"referredBy\">Referred by?</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"referredBy\" \r\n                      id=\"referredBy\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.referredBy}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"referredByOther\">Referred by notes</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"referredByOther\" \r\n                      id=\"referredByOther\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.referredByOther}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        \r\n        \r\n      </div>\r\n      <!-- -------------------------vertical divide---------------------------- -->\r\n      <div class=\"ui-g-12  ui-md-6\">\r\n        \r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"business\">Is a business?</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"business\" \r\n                      id=\"business\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.business}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"address1\">Address (1)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"address1\" \r\n                      id=\"address1\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.address1}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"address2\">Address (2)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"address2\" \r\n                      id=\"address2\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.address1}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"city\">City</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"city\" \r\n                      id=\"city\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"state\">State</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"state\" \r\n                      id=\"state\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"zipcode\">Zipcode</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n                <input class=\"ui-inputtext ng-dirty ng-invalid\" \r\n                      formControlName=\"zipcode\" \r\n                      id=\"zipcode\"\r\n                      type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!employerForm.valid && showErrors\">\r\n                {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n\r\n        \r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>        "

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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_oidc_client__);
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
            'address2': { '': '' },
            'blogparticipate': { 'required': '' },
            'business': { 'required': '' },
            'businessname': { 'required': '' },
            'cellphone': { 'required': '' },
            'city': { 'required': 'City is required' },
            'email': { 'required': 'Email is required' },
            'fax': { 'required': '' },
            'name': { 'required': 'Name is required' },
            'phone': { 'required': '' },
            'referredBy': { 'required': '' },
            'referredByOther': { 'required': '' },
            'state': { 'required': '' },
            'zipcode': { 'required': 'zipcode is required' }
        };
    }
    EmployersComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.employersService.getEmployerBySubject()
            .subscribe(function (data) {
            _this.employer = data;
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
            'cellphone': [this.employer.cellphone, __WEBPACK_IMPORTED_MODULE_3__angular_forms__["Validators"].required],
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
        __WEBPACK_IMPORTED_MODULE_5_oidc_client__["Log"].info('employers.component.saveEmployer: called');
        this.onValueChanged();
        if (this.employerForm.status === 'INVALID') {
            this.showErrors = true;
            return;
        }
        __WEBPACK_IMPORTED_MODULE_5_oidc_client__["Log"].info('employers.component.saveEmployer: valid');
        this.showErrors = false;
        var formModel = this.employerForm.value;
        this.employersService.save(formModel)
            .subscribe(function (data) { }, 
        //   this.employer = data;
        //   this.buildForm();
        // },
        function (error) {
            __WEBPACK_IMPORTED_MODULE_5_oidc_client__["Log"].info(JSON.stringify(error));
        }, function () { return __WEBPACK_IMPORTED_MODULE_5_oidc_client__["Log"].info(); });
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
        console.log('employers');
    }
    EmployersModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            imports: [
                __WEBPACK_IMPORTED_MODULE_1__angular_common__["CommonModule"],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormsModule"],
                __WEBPACK_IMPORTED_MODULE_4__angular_forms__["ReactiveFormsModule"],
                __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["InputTextModule"],
                __WEBPACK_IMPORTED_MODULE_5_primeng_primeng__["ButtonModule"],
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__shared_index__ = __webpack_require__("../../../../../src/app/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_oidc_client__);
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
    }
    EmployersService.prototype.getEmployerBySubject = function () {
        var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/employers';
        // TODO handle null sub in employerService.getOrders
        uri = uri + '?sub=' + this.auth.currentUser.profile['sub'];
        return this.http.get(uri)
            .map(function (o) { return o['data']; })
            .catch(__WEBPACK_IMPORTED_MODULE_3__shared_handle_error__["a" /* HandleError */].error);
    };
    EmployersService.prototype.save = function (employer) {
        var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/employers';
        uri = uri + '/' + employer.id;
        __WEBPACK_IMPORTED_MODULE_5_oidc_client__["Log"].info('employers.service.save: called');
        return this.http.put(uri, JSON.stringify(employer), {
            headers: new __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json'),
        });
        //.catch(HandleError.error);
    };
    EmployersService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__shared_index__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__shared_index__["a" /* AuthService */]) === "function" && _b || Object])
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
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__ = __webpack_require__("../../../../../src/app/reports/reports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_file_saver__ = __webpack_require__("../../../../file-saver/FileSaver.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_file_saver___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_file_saver__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_content_disposition__ = __webpack_require__("../../../../content-disposition/index.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5_content_disposition___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_5_content_disposition__);
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
        this.form = new __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormGroup"]({});
    }
    ExportsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.exportsService.getExportsList()
            .subscribe(function (listData) {
            _this.exports = listData;
            _this.exportsDropDown = listData.map(function (r) {
                return new __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__["a" /* MySelectItem */](r.name, r.name);
            });
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('exports.component: ngOnInit onCompleted'); });
    };
    ExportsComponent.prototype.getColumns = function () {
        var _this = this;
        this.exportsService.getColumns(this.selectedExportName)
            .subscribe(function (data) {
            _this.selectedColumns = data;
            _this.dateFilterDropDown = data.filter(function (f) { return f.system_type_name === 'datetime'; })
                .map(function (r) {
                return new __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__["a" /* MySelectItem */](r.name, r.name);
            });
            var group = {};
            data.forEach(function (col) {
                group[col.name] = new __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormControl"](true);
            });
            _this.form = new __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormGroup"](group);
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('exportsService.getColumns completed'); });
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
        }, function () { return console.log('exportsService.getColumns completed'); });
    };
    ExportsComponent.prototype.downloadFile = function (data, fileName, ttype) {
        var blob = new Blob([data], { type: ttype });
        Object(__WEBPACK_IMPORTED_MODULE_3_file_saver__["saveAs"])(blob, fileName);
    };
    ExportsComponent.prototype.getFilename = function (content) {
        return __WEBPACK_IMPORTED_MODULE_5_content_disposition__["parse"](content).parameters['filename'];
    };
    ExportsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-exports',
            template: __webpack_require__("../../../../../src/app/exports/exports.component.html"),
            styles: [__webpack_require__("../../../../../src/app/exports/exports.component.css")],
            providers: [__WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */]]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__exports_service__["a" /* ExportsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__angular_forms__["FormBuilder"]) === "function" && _b || Object])
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
        console.log('ExportsModule-ctor');
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_8_oidc_client__);
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
        __WEBPACK_IMPORTED_MODULE_8_oidc_client__["Log"].info('exportsService.getExportList: ' + this.uriBase);
        return this.http.get(this.uriBase)
            .map(function (res) { return res['data']; })
            .catch(this.handleError);
    };
    ExportsService.prototype.getColumns = function (tableName) {
        var uri = this.uriBase + '/' + tableName.toLowerCase();
        __WEBPACK_IMPORTED_MODULE_8_oidc_client__["Log"].info('exportsService.getColumns ' + uri);
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
        __WEBPACK_IMPORTED_MODULE_8_oidc_client__["Log"].info('exportsService.getExport: ' + JSON.stringify(params));
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
        this.lookupsAge = 0;
    }
    LookupsService.prototype.isStale = function () {
        if (this.lookupsAge > Date.now() - 1800) {
            return false;
        }
        return true;
    };
    LookupsService.prototype.isNotStale = function () {
        return !this.isStale();
    };
    LookupsService.prototype.getAllLookups = function () {
        var _this = this;
        if (this.isNotStale()) {
            return __WEBPACK_IMPORTED_MODULE_1_rxjs_Observable__["Observable"].of(this.lookups);
        }
        console.log('lookupsService.getLookups: ' + this.uriBase);
        return this.http.get(this.uriBase)
            .map(function (res) {
            _this.lookups = res['data'];
            _this.lookupsAge = Date.now();
            return res['data'];
        })
            .catch(__WEBPACK_IMPORTED_MODULE_2__shared_handle_error__["a" /* HandleError */].error);
    };
    LookupsService.prototype.getLookups = function (category) {
        return this.getAllLookups()
            .map(function (res) { return res.filter(function (l) { return l.category == category; }); })
            .catch(__WEBPACK_IMPORTED_MODULE_2__shared_handle_error__["a" /* HandleError */].error);
    };
    LookupsService.prototype.getLookup = function (id) {
        return this.getAllLookups()
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
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return Lookup; });
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

//# sourceMappingURL=lookup.js.map

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
exports.push([module.i, "", ""]);

// exports


/*** EXPORTS FROM exports-loader ***/
module.exports = module.exports.toString();

/***/ }),

/***/ "../../../../../src/app/online-orders/final-confirm/final-confirm.component.html":
/***/ (function(module, exports) {

module.exports = "<div class=\"ui-fluid card ui-g\">\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n        <label for=\"dateTimeofWork\">Time needed</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n        {{order.dateTimeofWork}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"contactName\">Contact name</label>\r\n      </div>\r\n      <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n        {{order.contactName}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"worksiteAddress1\">Address (1)</label>\r\n      </div>\r\n      <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n        {{order.worksiteAddress1}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n        <label for=\"worksiteAddress2\">Address (2)</label>\r\n      </div>\r\n      <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n        {{order.worksiteAddress2}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"city\">City</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.city}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"state\">State</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.state}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"zipcode\">Zipcode</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.zipcode}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"phone\">Phone</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.phone}}\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12 ui-md-6\">\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"description\">Work Description</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.description}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"additionalNotes\">Additional notes to dispatcher</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.additionalNotes}}\r\n      </div>\r\n    </div>\r\n    <div class=\"ui-g-12\">\r\n      <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n        <label for=\"transportMethodID\">Transport method</label>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n        {{order.transportMethodID}}\r\n      </div>\r\n    </div>\r\n  </div>\r\n  <div class=\"ui-g-12\">\r\n    <button pButton type=\"button\" (click)=\"submit()\" label=\"Finalize and submit\"></button>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/final-confirm/final-confirm.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return FinalConfirmComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__work_order_models_work_order__ = __webpack_require__("../../../../../src/app/online-orders/work-order/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
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
    function FinalConfirmComponent(ordersService, onlineService) {
        this.ordersService = ordersService;
        this.onlineService = onlineService;
        this.order = new __WEBPACK_IMPORTED_MODULE_2__work_order_models_work_order__["a" /* WorkOrder */]();
    }
    FinalConfirmComponent.prototype.ngOnInit = function () {
        var savedOrder = this.ordersService.get();
        if (savedOrder) {
            this.order = savedOrder;
        }
    };
    FinalConfirmComponent.prototype.submit = function () {
        this.onlineService.postToApi();
    };
    FinalConfirmComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-final-confirm',
            template: __webpack_require__("../../../../../src/app/online-orders/final-confirm/final-confirm.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/final-confirm/final-confirm.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _b || Object])
    ], FinalConfirmComponent);
    return FinalConfirmComponent;
    var _a, _b;
}());

//# sourceMappingURL=final-confirm.component.js.map

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

module.exports = "<strong>Please note:</strong>\r\n<ol>\r\n  <li>This order is not complete until you receive a confirmation email from Casa Latina.\r\n    If you do not hear from us or if you need a worker with 48 hours please call 206.956.0779 x3 during our\r\n    <a href=\"#\" id=\"businessHoursModal\">Business Hours</a>.\r\n  </li>\r\n  <li>Please allow a one hour window for worker(s) to arrive. This will account for transportation\r\n    routes with multiple stops and for traffic. There is no transportation fee to hire a Casa Latina\r\n    worker when you pick them up from our office. To have your worker(s) arrive at your door,\r\n    there is a <a href=\"#\" id=\"transportationMethodModal\">small fee</a> payable through this form.\r\n  </li>\r\n  <li>Casa Latina workers are not contractors. You will need to provide all tools, materials, and\r\n    safety equipment necessary for the job you wish to have done.\r\n  </li>\r\n</ol>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroConfirmComponent; });
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

var IntroConfirmComponent = (function () {
    function IntroConfirmComponent() {
    }
    IntroConfirmComponent.prototype.ngOnInit = function () {
    };
    IntroConfirmComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-intro-confirm',
            template: __webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/intro-confirm/intro-confirm.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], IntroConfirmComponent);
    return IntroConfirmComponent;
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

module.exports = "\r\n<p>\r\n  Casa Latina connects Latino immigrant workers with individuals and businesses looking for temporary labor. Our workers are skilled and dependable. From landscaping to dry walling to catering and housecleaning, if you can dream the project our workers can do it! For more information about our program please read these Frequently Asked Questions\r\n</p>\r\n<p>\r\n  If you are ready to hire a worker, please fill out the following form.\r\n</p>\r\n<p>\r\n  If you still have questions about hiring a worker, please call us at 206.956.0779 x3.\r\n</p>\r\n\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/introduction/introduction.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return IntroductionComponent; });
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

var IntroductionComponent = (function () {
    function IntroductionComponent() {
    }
    IntroductionComponent.prototype.ngOnInit = function () {
    };
    IntroductionComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-introduction',
            template: __webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/introduction/introduction.component.css")]
        }),
        __metadata("design:paramtypes", [])
    ], IntroductionComponent);
    return IntroductionComponent;
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
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
            },
            {
                path: 'work-assignments',
                component: __WEBPACK_IMPORTED_MODULE_6__work_assignments_work_assignments_component__["a" /* WorkAssignmentsComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
            },
            {
                path: 'final-confirm',
                component: __WEBPACK_IMPORTED_MODULE_7__final_confirm_final_confirm_component__["a" /* FinalConfirmComponent */],
                canLoad: [__WEBPACK_IMPORTED_MODULE_8__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
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
            providers: []
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

module.exports = "<h1>\r\n  Hire a Worker Online Order Form\r\n</h1>\r\n<p-steps [model]=\"items\" [readonly]=\"false\" [(activeIndex)]=\"activeIndex\" ></p-steps>\r\n<router-outlet></router-outlet>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/online-orders.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return OnlineOrdersComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
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
                __WEBPACK_IMPORTED_MODULE_1__lookups_lookups_service__["a" /* LookupsService */],
                __WEBPACK_IMPORTED_MODULE_6__employers_employers_service__["a" /* EmployersService */],
                __WEBPACK_IMPORTED_MODULE_2__online_orders_service__["a" /* OnlineOrdersService */],
                __WEBPACK_IMPORTED_MODULE_5__work_order_work_order_service__["a" /* WorkOrderService */],
                __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */],
                __WEBPACK_IMPORTED_MODULE_7__configs_configs_service__["a" /* ConfigsService */],
                __WEBPACK_IMPORTED_MODULE_3__angular_forms__["FormBuilder"]
            ]
        }),
        __metadata("design:paramtypes", [])
    ], OnlineOrdersComponent);
    return OnlineOrdersComponent;
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__environments_environment__ = __webpack_require__("../../../../../src/environments/environment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
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
    function OnlineOrdersService(http, orderService, assignmentService) {
        this.http = http;
        this.orderService = orderService;
        this.assignmentService = assignmentService;
        this.scheduleRules = new Array();
        this.transportRules = new Array();
        // this loads static data from a file. will replace later.
        this.scheduleRules = Object(__WEBPACK_IMPORTED_MODULE_7__shared__["b" /* loadScheduleRules */])();
        this.transportRules = Object(__WEBPACK_IMPORTED_MODULE_7__shared__["c" /* loadTransportRules */])();
    }
    OnlineOrdersService.prototype.validate = function () { };
    OnlineOrdersService.prototype.postToApi = function () {
        var url = __WEBPACK_IMPORTED_MODULE_5__environments_environment__["a" /* environment */].dataUrl + '/api/onlineorders';
        var postHeaders = new __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["d" /* HttpHeaders */]().set('Content-Type', 'application/json');
        this.order = this.orderService.get();
        this.order.workAssignments = this.assignmentService.getAll();
        this.http.post(url, JSON.stringify(this.order), {
            headers: postHeaders
        }).subscribe(function (data) { }, function (err) {
            if (err.error instanceof Error) {
                console.log('Client-side error occured.');
            }
            else {
                __WEBPACK_IMPORTED_MODULE_6_oidc_client__["Log"].error('online-orders.service.POST: ' + err.message);
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
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_3__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__work_assignments_work_assignments_service__["a" /* WorkAssignmentsService */]) === "function" && _c || Object])
    ], OnlineOrdersService);
    return OnlineOrdersService;
    var _a, _b, _c;
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
var ScheduleRule = (function () {
    function ScheduleRule(init) {
        Object.assign(this, init);
    }
    return ScheduleRule;
}());

//# sourceMappingURL=schedule-rule.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/shared/models/transport-rule.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return CostRule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return TransportRule; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "c", function() { return TransportType; });
var CostRule = (function () {
    function CostRule(init) {
        Object.assign(this, init);
    }
    return CostRule;
}());

var TransportRule = (function () {
    function TransportRule(init) {
        Object.assign(this, init);
    }
    return TransportRule;
}());

var TransportType;
(function (TransportType) {
    TransportType[TransportType["transport_van"] = 0] = "transport_van";
    TransportType[TransportType["transport_bus"] = 1] = "transport_bus";
    TransportType[TransportType["transport_pickup"] = 2] = "transport_pickup";
})(TransportType || (TransportType = {}));
//# sourceMappingURL=transport-rule.js.map

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
            zipcodes: [98101, 98102, 98103, 98104, 98105, 98106,
                98107, 98109, 98112, 98115, 98116, 98117, 98118,
                98119, 98121, 98122, 98125, 98126, 98133, 98134,
                98136, 98144, 98154, 98164, 98174, 98177, 98195,
                98199],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ minWorker: 0, maxWorker: 10000, cost: 5 })
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 2,
            key: 'bus_outside_zone',
            lookupKey: 'transport_bus',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_bus,
            zoneLabel: 'outside',
            zipcodes: [98005, 98006, 98007, 98008, 98033, 98039,
                98052, 98040, 98004, 98074, 98075, 98029, 98027,
                98028, 98155, 98166, 98146, 98168, 98057, 98056,
                98059, 98037, 98020, 98026, 98043, 98021, 98011],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ minWorker: 0, maxWorker: 10000, cost: 10 })
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 3,
            key: 'van_inside_zone',
            lookupKey: 'transport_van',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_van,
            zoneLabel: 'inside',
            zipcodes: [98101, 98102, 98103, 98104, 98105, 98106,
                98107, 98109, 98112, 98115, 98116, 98117, 98118,
                98119, 98121, 98122, 98125, 98126, 98133, 98134,
                98136, 98144, 98154, 98164, 98174, 98177, 98195,
                98199],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ minWorker: 0, maxWorker: 1, cost: 15 }),
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ minWorker: 1, maxWorker: 2, cost: 5 }),
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ minWorker: 2, maxWorker: 10, cost: 0 }),
            ]
        }),
        new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["b" /* TransportRule */]({
            id: 4,
            key: 'van_outside_zone',
            lookupKey: 'transport_van',
            transportType: __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["c" /* TransportType */].transport_van,
            zoneLabel: 'outside',
            zipcodes: [98005, 98006, 98007, 98008, 98033, 98039,
                98052, 98040, 98004, 98074, 98075, 98029, 98027,
                98028, 98155, 98166, 98146, 98168, 98057, 98056,
                98059, 98037, 98020, 98026, 98043, 98021, 98011],
            costRules: [
                new __WEBPACK_IMPORTED_MODULE_0__models_transport_rule__["a" /* CostRule */]({ minWorker: 0, maxWorker: 10, cost: 25 }),
            ]
        }),
    ];
}
//# sourceMappingURL=load-transport-rules.js.map

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
/** A hero's name can't match the given regular expression */
function schedulingValidator(rules) {
    return function (control) {
        //const forbidden = nameRe.test(control.value);
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

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"requestForm\" (ngSubmit)=\"saveRequest()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"skillsList\">Skill needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-dropdown id=\"skillsList\"\r\n                        [options]=\"skillsDropDown\"\r\n                        formControlName=\"skillId\"\r\n                        [(ngModel)]=\"request.skillId\"\r\n                        (onChange)=\"selectSkill(request.skillId)\"\r\n                        [autoWidth]=\"false\"\r\n                        placeholder=\"Select a skill\"></p-dropdown>\r\n\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['skillId'].valid && showErrors\">\r\n            {{formErrors.skillId}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"hours\">Hours needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"hours\" id=\"hours\" type=\"text\" pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12 ui-g-nopad\">\r\n          <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!requestForm.controls['hours'].valid && showErrors\">\r\n            {{formErrors.hours}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"requiresHeavyLifting\">Requires heavy lifting?</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <p-inputSwitch id=\"requiresHeavyLifting\" formControlName=\"requiresHeavyLifting\"></p-inputSwitch>\r\n          </div>\r\n        </div>\r\n\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Additional info about job</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <textarea rows=\"3\" class=\"ui-inputtext\" formControlName=\"description\" id=\"description\" type=\"text\" pInputText></textarea>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Skill description\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.skillDescriptionEn}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Hourly rate\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.wage}}\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            Minimum time\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            {{this.selectedSkill.minHour}}\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n\r\n    <p-dataTable [value]=\"requestList\" [(selection)]=\"selectedRequest\" (onRowSelect)=\"onRowSelect($event)\" [responsive]=\"true\">\r\n      <p-column field=\"id\" header=\"#\"></p-column>\r\n      <p-column field=\"skill\" header=\"Skill needed\"></p-column>\r\n      <p-column field=\"transportCost\" header=\"Transport cost\"></p-column>\r\n      <p-column field=\"hours\" header=\"hours requested\"></p-column>\r\n      <p-column field=\"description\" header=\"notes\"></p-column>\r\n      <p-column field=\"requiresHeavyLifting\" header=\"Heavy lifting?\"></p-column>\r\n      <p-column field=\"wage\" header=\"Hourly wage\"></p-column>\r\n\r\n      <p-column styleClass=\"col-button\">\r\n        <ng-template pTemplate=\"header\">\r\n          Actions\r\n        </ng-template>\r\n        <ng-template let-request=\"rowData\" pTemplate=\"body\">\r\n          <button type=\"button\" pButton (click)=\"editRequest(request)\" icon=\"ui-icon-edit\"></button>\r\n          <button type=\"button\" pButton (click)=\"deleteRequest(request)\" icon=\"ui-icon-delete\"></button>\r\n        </ng-template>\r\n      </p-column>\r\n    </p-dataTable>\r\n  </div>\r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__ = __webpack_require__("../../../../../src/app/reports/reports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_work_assignment__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/models/work-assignment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__ = __webpack_require__("../../../../../src/app/lookups/models/lookup.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_8_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__work_order_work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
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
    function WorkAssignmentsComponent(lookupsService, orderService, waService, onlineService, fb) {
        this.lookupsService = lookupsService;
        this.orderService = orderService;
        this.waService = waService;
        this.onlineService = onlineService;
        this.fb = fb;
        this.selectedSkill = new __WEBPACK_IMPORTED_MODULE_5__lookups_models_lookup__["a" /* Lookup */]();
        this.requestList = new Array(); // list built by user in UI
        this.request = new __WEBPACK_IMPORTED_MODULE_3__models_work_assignment__["a" /* WorkAssignment */](); // composed by UI to make/edit a request
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
            'wage': { 'required': 'wage is required.' }
        };
    }
    WorkAssignmentsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.onlineService.getTransportRules()
            .subscribe(function (data) {
            _this.transportRules = data;
        });
        this.lookupsService.getLookups('skill')
            .subscribe(function (listData) {
            _this.skills = listData;
            _this.skillsDropDown = listData.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_2__reports_reports_component__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('work-assignments.component: ngOnInit:skills onCompleted'); });
        this.lookupsService.getLookups('transportmethod')
            .subscribe(function (listData) {
            _this.transports = listData;
        }, function (error) { return _this.errorMessage = error; }, function () { return console.log('work-assignments.component: ngOnInit:transports onCompleted'); });
        this.requestList = this.waService.getAll();
        this.buildForm();
    };
    WorkAssignmentsComponent.prototype.buildForm = function () {
        var _this = this;
        this.requestForm = this.fb.group({
            'id': '',
            'skillId': ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["Validators"].required],
            'skill': [''],
            'hours': ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["Validators"].required],
            'description': [''],
            'requiresHeavyLifting': [false, __WEBPACK_IMPORTED_MODULE_1__angular_forms__["Validators"].required],
            'wage': ['', __WEBPACK_IMPORTED_MODULE_1__angular_forms__["Validators"].required]
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
        __WEBPACK_IMPORTED_MODULE_8_oidc_client__["Log"].info('work-assignment.component.selectSkill.skillId:' + String(skillId));
        var skill = this.skills.filter(function (f) { return f.id === Number(skillId); }).shift();
        if (skill === null || skill === undefined) {
            throw new Error('Can\'t find selected skill in component\'s list');
        }
        this.selectedSkill = skill;
        this.requestForm.controls['skill'].setValue(skill.text_EN);
        this.requestForm.controls['wage'].setValue(skill.wage);
    };
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
    };
    WorkAssignmentsComponent.prototype.calculateTransportCost = function (id) {
        var order = this.orderService.get();
        var lookup = this.transports.find(function (f) { return f.id == order.transportMethodID; });
        var rule = this.transportRules.filter(function (f) { return f.lookupKey == lookup.key; })
            .find(function (f) { return f.zipcodes.includes(Number(order.zipcode)); });
        return rule.costRules.find(function (r) { return id > r.minWorker && id <= r.maxWorker; }).cost;
    };
    WorkAssignmentsComponent.prototype.refreshRequests = function () {
        for (var _i = 0, _a = this.waService.getAll(); _i < _a.length; _i++) {
            var r = _a[_i];
            r.transportCost = this.calculateTransportCost(r.id);
        }
    };
    WorkAssignmentsComponent.prototype.compactRequestIds = function () {
        var i = 0;
        for (var _i = 0, _a = this.waService.getAll().sort(__WEBPACK_IMPORTED_MODULE_3__models_work_assignment__["a" /* WorkAssignment */].sort); _i < _a.length; _i++) {
            var r = _a[_i];
            i++;
            r.id = i;
        }
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
            id: formModel.id || this.waService.getNextRequestId(),
            skillId: formModel.skillId,
            skill: formModel.skill,
            hours: formModel.hours,
            description: formModel.description,
            requiresHeavyLifting: formModel.requiresHeavyLifting,
            wage: formModel.wage,
            transportCost: this.calculateTransportCost(formModel.id || this.waService.getNextRequestId())
        };
        if (this.newRequest) {
            this.waService.create(saveRequest);
        }
        else {
            this.waService.save(saveRequest);
        }
        this.requestList = this.waService.getAll().slice();
        this.requestForm.reset();
        this.buildForm();
        this.newRequest = true;
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
    WorkAssignmentsComponent = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Component"])({
            selector: 'app-work-assignments',
            template: __webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.html"),
            styles: [__webpack_require__("../../../../../src/app/online-orders/work-assignments/work-assignments.component.css")]
        }),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_9__work_order_work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_9__work_order_work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__["a" /* WorkAssignmentsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_7__work_assignments_service__["a" /* WorkAssignmentsService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_forms__["FormBuilder"]) === "function" && _e || Object])
    ], WorkAssignmentsComponent);
    return WorkAssignmentsComponent;
    var _a, _b, _c, _d, _e;
}());

//# sourceMappingURL=work-assignments.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-assignments/work-assignments.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkAssignmentsService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__models_work_assignment__ = __webpack_require__("../../../../../src/app/online-orders/work-assignments/models/work-assignment.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_oidc_client__);
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
    function WorkAssignmentsService() {
        this.requests = new Array();
        this.storageKey = 'machete.workassignments';
        __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-assignment.service: ' + JSON.stringify(this.getAll()));
    }
    WorkAssignmentsService.prototype.getAll = function () {
        __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-assignments.service.getAll: called');
        var data = sessionStorage.getItem(this.storageKey);
        if (data) {
            var requests = JSON.parse(data);
            return requests;
        }
        else {
            return this.requests;
        }
    };
    WorkAssignmentsService.prototype.create = function (request) {
        this.requests.push(request);
        sessionStorage.setItem(this.storageKey, JSON.stringify(this.requests));
    };
    WorkAssignmentsService.prototype.save = function (request) {
        var index = this.findSelectedRequestIndex(request);
        this.requests[index] = request;
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
        var index = this.requests.indexOf(request);
        if (index > -1) {
            this.requests.splice(index, 1);
        }
    };
    WorkAssignmentsService.prototype.clear = function () { };
    WorkAssignmentsService.prototype.findSelectedRequestIndex = function (request) {
        return this.requests.findIndex(function (a) { return a.id === request.id; });
    };
    WorkAssignmentsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [])
    ], WorkAssignmentsService);
    return WorkAssignmentsService;
}());

//# sourceMappingURL=work-assignments.service.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/models/work-order.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrder; });
var WorkOrder = (function () {
    function WorkOrder() {
    }
    return WorkOrder;
}());

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

module.exports = "<div class=\"ui-fluid\">\r\n  <div class=\"card\">\r\n    <form [formGroup]=\"orderForm\" (ngSubmit)=\"save()\" class=\"ui-g form-group\">\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"dateTimeofWork\">Time needed</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8  ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                <p-calendar id=\"dateTimeofWork\"\r\n                            showTime=\"true\"\r\n                            stepMinute=\"15\"\r\n                            defaultDate=\"\"\r\n                            formControlName=\"dateTimeofWork\">\r\n                </p-calendar>\r\n                <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.dateTimeofWork}}\r\n                </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"contactName\">Contact name</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                  <input class=\"ui-inputtext ng-dirty ng-invalid\" formControlName=\"contactName\" id=\"contactName\"\r\n                         type=\"text\" pInputText/>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                    {{formErrors.contactName}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"worksiteAddress1\">Address (1)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n                <span class=\"md-inputfield\">\r\n                  <input class=\"ui-inputtext\" formControlName=\"worksiteAddress1\" id=\"worksiteAddress1\" type=\"text\"\r\n                         pInputText/>\r\n                  <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                    {{formErrors.worksiteAddress1}}\r\n                  </div>\r\n                </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4  ui-g-nopad\">\r\n            <label for=\"worksiteAddress2\">Address (2)</label>\r\n          </div>\r\n          <div class=\"ui-g-12  ui-md-8 ui-g-nopad\">\r\n            <input class=\"ui-inputtext\" formControlName=\"worksiteAddress2\" id=\"worksiteAddress2\" type=\"text\"\r\n                   pInputText/>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"city\">City</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"city\" id=\"city\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.city}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"state\">State</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"state\" id=\"state\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.state}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"zipcode\">Zipcode</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"zipcode\" id=\"zipcode\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.zipcode}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"phone\">Phone</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <input class=\"ui-inputtext\" formControlName=\"phone\" id=\"phone\" type=\"text\" pInputText/>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.phone}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12 ui-md-6\">\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"description\">Work Description</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\"\r\n                        formControlName=\"description\" id=\"description\" type=\"text\"></textarea>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.description}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"additionalNotes\">Additional notes to dispatcher</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <textarea rows=\"5\" pInputTextarea autoResize=\"autoResize\" class=\"ui-inputtextarea\"\r\n                      formControlname=\"additionalNotes\" id=\"additionalNotes\" type=\"text\"></textarea>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <div class=\"ui-g-12 ui-md-4 ui-g-nopad\">\r\n            <label for=\"transportMethodID\">Transport method</label>\r\n          </div>\r\n          <div class=\"ui-g-12 ui-md-8 ui-g-nopad\">\r\n            <span class=\"md-inputfield\">\r\n              <p-dropdown id=\"transportMethodID\" [options]=\"transportMethodsDropDown\" formControlName=\"transportMethodID\"\r\n                          [autoWidth]=\"false\"></p-dropdown>\r\n              <div class=\"ui-message ui-messages-error ui-corner-all\" *ngIf=\"!orderForm.valid && showErrors\">\r\n                  {{formErrors.transportMethodID}}\r\n              </div>\r\n            </span>\r\n          </div>\r\n        </div>\r\n        <div class=\"ui-g-12\">\r\n          <button type=\"button\" (click)=\"showDialog()\" pButton icon=\"fa-external-link-square\" label=\"Show transport rates\"></button>\r\n        </div>\r\n      </div>\r\n      <div class=\"ui-g-12\">\r\n        <button pButton type=\"submit\" label=\"Save\"></button>\r\n      </div>\r\n    </form>\r\n  </div>\r\n</div>\r\n<div>\r\n  <p-dialog header=\"Title\" [(visible)]=\"display\">\r\n      Content\r\n  </p-dialog>    \r\n</div>\r\n"

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.component.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderComponent; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__reports_reports_component__ = __webpack_require__("../../../../../src/app/reports/reports.component.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__angular_forms__ = __webpack_require__("../../../forms/@angular/forms.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_work_order__ = __webpack_require__("../../../../../src/app/online-orders/work-order/models/work-order.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__ = __webpack_require__("../../../../../src/app/lookups/lookups.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_5__online_orders_service__ = __webpack_require__("../../../../../src/app/online-orders/online-orders.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6__work_order_service__ = __webpack_require__("../../../../../src/app/online-orders/work-order/work-order.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_7_oidc_client__);
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_8__shared__ = __webpack_require__("../../../../../src/app/online-orders/shared/index.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_9__configs_configs_service__ = __webpack_require__("../../../../../src/app/configs/configs.service.ts");
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
    function WorkOrderComponent(lookupsService, orderService, onlineService, configsService, fb) {
        this.lookupsService = lookupsService;
        this.orderService = orderService;
        this.onlineService = onlineService;
        this.configsService = configsService;
        this.fb = fb;
        this.logPrefix = 'work-order.component.';
        this.order = new __WEBPACK_IMPORTED_MODULE_3__models_work_order__["a" /* WorkOrder */]();
        this.showErrors = false;
        this.newOrder = true;
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
        this.display = false;
        __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info(this.logPrefix + 'ctor: called');
    }
    WorkOrderComponent.prototype.showDialog = function () {
        this.display = true;
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
        if (this.orderService.get() == null) {
            this.orderService.loadFromProfile()
                .subscribe(function (data) {
                __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info(_this.logPrefix + 'ngOnInit: loadFromProfile ' + JSON.stringify(_this.order));
                _this.order = _this.mapOrderFrom(data);
                _this.buildForm();
            });
        }
        else {
            this.order = this.orderService.get();
            this.buildForm();
        }
    };
    WorkOrderComponent.prototype.initializeTransports = function () {
        var _this = this;
        this.lookupsService.getLookups('transportmethod')
            .subscribe(function (listData) {
            _this.transportMethods = listData;
            _this.transportMethodsDropDown = listData.map(function (l) {
                return new __WEBPACK_IMPORTED_MODULE_1__reports_reports_component__["a" /* MySelectItem */](l.text_EN, String(l.id));
            });
        }, function (error) { return _this.errorMessage = error; }, function () { return __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info(_this.logPrefix + 'ngOnInit: getLookups onCompleted'); });
    };
    WorkOrderComponent.prototype.mapOrderFrom = function (employer) {
        var order = new __WEBPACK_IMPORTED_MODULE_3__models_work_order__["a" /* WorkOrder */]();
        order.contactName = employer.name;
        order.worksiteAddress1 = employer.address1;
        order.worksiteAddress2 = employer.address2;
        order.city = employer.city;
        order.state = employer.state;
        order.zipcode = employer.zipcode;
        order.phone = employer.phone || employer.cellphone;
        return order;
    };
    WorkOrderComponent.prototype.buildForm = function () {
        var _this = this;
        this.orderForm = this.fb.group({
            'dateTimeofWork': [this.order.dateTimeofWork, [
                    Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('Date & time is required.'),
                    Object(__WEBPACK_IMPORTED_MODULE_8__shared__["e" /* schedulingValidator */])(this.schedulingRules)
                ]],
            'contactName': [this.order.contactName, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('Contact name is required.')],
            'worksiteAddress1': [this.order.worksiteAddress1, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('Address is required.')],
            'worksiteAddress2': [this.order.worksiteAddress2],
            'city': [this.order.city, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('City is required.')],
            'state': [this.order.state, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('State is required.')],
            'zipcode': [this.order.zipcode, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('Zip code is required.')],
            'phone': [this.order.phone, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('Phone is required.')],
            'description': [this.order.description, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('Description is required.')],
            'additionalNotes': '',
            'transportMethodID': [this.order.transportMethodID, Object(__WEBPACK_IMPORTED_MODULE_8__shared__["d" /* requiredValidator */])('A transport method is required.')]
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
                    __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info('online-orders.work-order.component.onValueChanged.error:' + field + ': ' + control.errors[key]);
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
            __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info(this.logPrefix + 'save: INVALID: ' + JSON.stringify(this.formErrors));
            this.showErrors = true;
            return;
        }
        this.showErrors = false;
        var order = this.prepareOrderForSave();
        this.orderService.save(order);
        this.newOrder = false;
    };
    WorkOrderComponent.prototype.prepareOrderForSave = function () {
        var formModel = this.orderForm.value;
        var order = {
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
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_4__lookups_lookups_service__["a" /* LookupsService */]) === "function" && _a || Object, typeof (_b = typeof __WEBPACK_IMPORTED_MODULE_6__work_order_service__["a" /* WorkOrderService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_6__work_order_service__["a" /* WorkOrderService */]) === "function" && _b || Object, typeof (_c = typeof __WEBPACK_IMPORTED_MODULE_5__online_orders_service__["a" /* OnlineOrdersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_5__online_orders_service__["a" /* OnlineOrdersService */]) === "function" && _c || Object, typeof (_d = typeof __WEBPACK_IMPORTED_MODULE_9__configs_configs_service__["a" /* ConfigsService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_9__configs_configs_service__["a" /* ConfigsService */]) === "function" && _d || Object, typeof (_e = typeof __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"] !== "undefined" && __WEBPACK_IMPORTED_MODULE_2__angular_forms__["FormBuilder"]) === "function" && _e || Object])
    ], WorkOrderComponent);
    return WorkOrderComponent;
    var _a, _b, _c, _d, _e;
}());

//# sourceMappingURL=work-order.component.js.map

/***/ }),

/***/ "../../../../../src/app/online-orders/work-order/work-order.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return WorkOrderService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__employers_employers_service__ = __webpack_require__("../../../../../src/app/employers/employers.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_2_oidc_client__);
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
        this.employerService = employerService;
        this.storageKey = 'machete.workorder';
        __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-order.service: ' + JSON.stringify(this.get()));
    }
    WorkOrderService.prototype.loadFromProfile = function () {
        __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-order.service.loadFromProfile: called');
        return this.employerService.getEmployerBySubject();
    };
    WorkOrderService.prototype.save = function (order) {
        __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-order.service.save: called');
        sessionStorage.setItem(this.storageKey, JSON.stringify(order));
        this.order = order;
    };
    WorkOrderService.prototype.get = function () {
        __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-order.service.get: called');
        var data = sessionStorage.getItem(this.storageKey);
        if (data) {
            __WEBPACK_IMPORTED_MODULE_2_oidc_client__["Log"].info('work-order.service.get: returning stored order');
            var order = JSON.parse(data);
            order.dateTimeofWork = new Date(order.dateTimeofWork);
            return order;
        }
        else {
            return this.order;
        }
    };
    WorkOrderService.prototype.clear = function () {
        this.order = null;
        sessionStorage.removeItem(this.storageKey);
    };
    WorkOrderService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__employers_employers_service__["a" /* EmployersService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__employers_employers_service__["a" /* EmployersService */]) === "function" && _a || Object])
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
        component: __WEBPACK_IMPORTED_MODULE_2__reports_component__["b" /* ReportsComponent */],
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
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
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return ReportsComponent; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return MySelectItem; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__reports_service__ = __webpack_require__("../../../../../src/app/reports/reports.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__models_search_options__ = __webpack_require__("../../../../../src/app/reports/models/search-options.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3__models_report__ = __webpack_require__("../../../../../src/app/reports/models/report.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_4__models_search_inputs__ = __webpack_require__("../../../../../src/app/reports/models/search-inputs.ts");
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

var MySelectItem = (function () {
    function MySelectItem(label, value) {
        this.label = label;
        this.value = value;
    }
    return MySelectItem;
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_7_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_7_oidc_client__);
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
        __WEBPACK_IMPORTED_MODULE_7_oidc_client__["Log"].info('reports.module.ctor called');
    }
    ReportsModule = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["NgModule"])({
            declarations: [
                __WEBPACK_IMPORTED_MODULE_2__reports_component__["b" /* ReportsComponent */]
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
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_6_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_6_oidc_client__);
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
        __WEBPACK_IMPORTED_MODULE_6_oidc_client__["Log"].info('reportsService.getReportData: ' + uri);
        return this.http.get(uri)
            .map(function (res) { return res['data']; })
            .catch(this.handleError);
    };
    ReportsService.prototype.getReportList = function () {
        var uri = __WEBPACK_IMPORTED_MODULE_5__environments_environment__["a" /* environment */].dataUrl + '/api/reports';
        __WEBPACK_IMPORTED_MODULE_6_oidc_client__["Log"].info('reportsService.getReportList: ' + uri);
        return this.http.get(uri)
            .map(function (o) { return o['data']; })
            .catch(this.handleError);
    };
    ReportsService.prototype.handleError = function (error) {
        console.error('reports.service.handleError:', JSON.stringify(error));
        return __WEBPACK_IMPORTED_MODULE_4_rxjs_Observable__["Observable"].of(error);
    };
    ReportsService.prototype.encodeData = function (data) {
        return Object.keys(data).map(function (key) {
            return [key, data[key]].map(encodeURIComponent).join('=');
        }).join('&');
    };
    ReportsService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_7__angular_common_http__["b" /* HttpClient */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_7__angular_common_http__["b" /* HttpClient */]) === "function" && _a || Object])
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
/* unused harmony namespace reexport */
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__services_auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony namespace reexport (by used) */ __webpack_require__.d(__webpack_exports__, "a", function() { return __WEBPACK_IMPORTED_MODULE_1__services_auth_service__["a"]; });


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

/***/ "../../../../../src/app/shared/services/auth-guard.service.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return AuthGuardService; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__angular_router__ = __webpack_require__("../../../router/@angular/router.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_2__auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_oidc_client__ = __webpack_require__("../../../../oidc-client/lib/oidc-client.min.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_3_oidc_client___default = __webpack_require__.n(__WEBPACK_IMPORTED_MODULE_3_oidc_client__);
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
        __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth-guard.service.ctor called');
    }
    AuthGuardService.prototype.canActivate = function () {
        var _this = this;
        __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth-guard.service.canActivate: called');
        var isLoggedIn = this.authService.isLoggedInObs();
        isLoggedIn.subscribe(function (loggedin) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth-guard.service.canActivate isLoggedInObs:' + loggedin);
            if (!loggedin) {
                __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth-guard.service.canActivate NOT loggedIn: url:' + _this.router.url);
                _this.authService.redirectUrl = _this.router.url;
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
    function AuthService(http) {
        var _this = this;
        this.http = http;
        this.mgr = new __WEBPACK_IMPORTED_MODULE_3_oidc_client__["UserManager"](__WEBPACK_IMPORTED_MODULE_4__environments_environment__["a" /* environment */].oidc_client_settings);
        this.userLoadededEvent = new __WEBPACK_IMPORTED_MODULE_0__angular_core__["EventEmitter"]();
        this.loggedIn = false;
        __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth.serive.ctor: called');
        this.mgr.getUser()
            .then(function (user) {
            if (user) {
                __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].debug('auth.service.getUser.callback user:' + JSON.stringify(user));
                _this.loggedIn = true;
                _this.currentUser = user;
                _this.userLoadededEvent.emit(user);
            }
            else {
                _this.loggedIn = false;
            }
        })
            .catch(function (err) {
            _this.loggedIn = false;
        });
        this.mgr.events.addUserLoaded(function (user) {
            _this.currentUser = user;
            _this.loggedIn = !(user === undefined);
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].debug('auth.service.ctor.event: addUserLoaded: ', user);
        });
        this.mgr.events.addUserUnloaded(function (e) {
            if (!__WEBPACK_IMPORTED_MODULE_4__environments_environment__["a" /* environment */].production) {
                __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth.service.ctor.event: addUserUnloaded');
            }
            _this.loggedIn = false;
        });
    }
    AuthService.prototype.isLoggedInObs = function () {
        return __WEBPACK_IMPORTED_MODULE_2_rxjs_Rx__["Observable"].fromPromise(this.mgr.getUser()).map(function (user) {
            if (user) {
                return true;
            }
            else {
                return false;
            }
        });
    };
    AuthService.prototype.clearState = function () {
        this.mgr.clearStaleState().then(function () {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth.service.clearStateState success');
        }).catch(function (e) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.clearStateState error', e.message);
        });
    };
    AuthService.prototype.getUser = function () {
        var _this = this;
        this.mgr.getUser().then(function (user) {
            _this.currentUser = user;
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth.service.getUser returned: ' + JSON.stringify(user));
            _this.userLoadededEvent.emit(user);
        }).catch(function (err) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.getUser returned: ' + JSON.stringify(err));
        });
    };
    AuthService.prototype.removeUser = function () {
        var _this = this;
        this.mgr.removeUser().then(function () {
            _this.userLoadededEvent.emit(null);
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].info('auth.service.removeUser: user removed');
        }).catch(function (err) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.removeUser returned: ' + JSON.stringify(err));
        });
    };
    AuthService.prototype.startSigninMainWindow = function () {
        this.mgr.signinRedirect({ data: 'some data' }).then(function () {
            console.log('signinRedirect done');
        }).catch(function (err) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.startSigninMainWindow returned: ' + JSON.stringify(err));
        });
    };
    AuthService.prototype.endSigninMainWindow = function (url) {
        this.mgr.signinRedirectCallback(url).then(function (user) {
            console.log('signed in', user);
        }).catch(function (err) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.endSigninMainWindow returned: ' + JSON.stringify(err));
        });
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
                __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.startSignoutMainWindow returned: ' + JSON.stringify(err));
            });
        });
    };
    ;
    AuthService.prototype.endSignoutMainWindow = function () {
        this.mgr.signoutRedirectCallback().then(function (resp) {
            console.log('signed out', resp);
        }).catch(function (err) {
            __WEBPACK_IMPORTED_MODULE_3_oidc_client__["Log"].error('auth.service.endSignoutMainWindow returned: ' + JSON.stringify(err));
        });
    };
    ;
    AuthService.prototype._setAuthHeaders = function (user) {
        this.authHeaders = new __WEBPACK_IMPORTED_MODULE_1__angular_http__["a" /* Headers */]();
        this.authHeaders.append('Authorization', user.token_type + ' ' + user.access_token);
        if (this.authHeaders.get('Content-Type')) {
        }
        else {
            this.authHeaders.append('Content-Type', 'application/json');
        }
    };
    AuthService.prototype._setRequestOptions = function (options) {
        if (this.loggedIn) {
            this._setAuthHeaders(this.currentUser);
        }
        if (options) {
            options.headers.append(this.authHeaders.keys[0], this.authHeaders.values[0]);
        }
        else {
            options = new __WEBPACK_IMPORTED_MODULE_1__angular_http__["e" /* RequestOptions */]({ headers: this.authHeaders });
        }
        return options;
    };
    AuthService = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__angular_http__["b" /* Http */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__angular_http__["b" /* Http */]) === "function" && _a || Object])
    ], AuthService);
    return AuthService;
    var _a;
}());

//# sourceMappingURL=auth.service.js.map

/***/ }),

/***/ "../../../../../src/app/shared/services/token.interceptor.ts":
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return TokenInterceptor; });
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_0__angular_core__ = __webpack_require__("../../../core/@angular/core.es5.js");
/* harmony import */ var __WEBPACK_IMPORTED_MODULE_1__auth_service__ = __webpack_require__("../../../../../src/app/shared/services/auth.service.ts");
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
    function TokenInterceptor(auth) {
        this.auth = auth;
    }
    TokenInterceptor.prototype.intercept = function (request, next) {
        request = request.clone({
            setHeaders: {
                Authorization: "Bearer " + this.auth.currentUser.access_token
            }
        });
        return next.handle(request);
    };
    TokenInterceptor = __decorate([
        Object(__WEBPACK_IMPORTED_MODULE_0__angular_core__["Injectable"])(),
        __metadata("design:paramtypes", [typeof (_a = typeof __WEBPACK_IMPORTED_MODULE_1__auth_service__["a" /* AuthService */] !== "undefined" && __WEBPACK_IMPORTED_MODULE_1__auth_service__["a" /* AuthService */]) === "function" && _a || Object])
    ], TokenInterceptor);
    return TokenInterceptor;
    var _a;
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
        canLoad: [__WEBPACK_IMPORTED_MODULE_3__shared_services_auth_guard_service__["a" /* AuthGuardService */]]
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

module.exports = "<p>\r\n  work-orders works!\r\n</p>\r\n<p-dataTable\r\n  #dt\r\n  [value]=\"orders\"\r\n  >\r\n  <p-header>\r\n    \r\n  </p-header>\r\n  <p-column field=\"paperOrderNum\" header=\"Order #\"></p-column>\r\n  <p-column field=\"dateTimeOfWork\" header=\"Time needed\"></p-column>\r\n  <p-column field=\"statusEN\" header=\"Status\"></p-column>\r\n  <p-column field=\"transportMethodEN\" header=\"Trans. method\"></p-column>\r\n  <p-column field=\"\" header=\"Worker count\"></p-column>\r\n  <p-column field=\"contactName\" header=\"Contact name\"></p-column>\r\n  <p-column field=\"address\" header=\"Address\"></p-column>\r\n  <p-column field=\"zipcode\" header=\"Zipcode\"></p-column>\r\n</p-dataTable>"

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
        var uri = __WEBPACK_IMPORTED_MODULE_2__environments_environment__["a" /* environment */].dataUrl + '/api/workorders';
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
    name: 'prod',
    production: true,
    dataUrl: 'http://api.machetessl.org',
    authUrl: 'https://identity.machetessl.org/id',
    baseRef: '/V2',
    oidc_client_settings: {
        authority: 'https://identity.machetessl.org/id',
        client_id: 'machete-ui-test',
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