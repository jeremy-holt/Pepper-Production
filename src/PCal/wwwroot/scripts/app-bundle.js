define('app',["require", "exports"], function (require, exports) {
    "use strict";
    var App = (function () {
        function App() {
        }
        App.prototype.configureRouter = function (config, router) {
            config.title = "Pepper Calculator";
            config.map([
                { route: '', moduleId: 'components/home/home', nav: true, title: 'Home', name: 'home' },
                { route: 'calculator', moduleId: 'components/calculator/calculator-main', name: 'calculator', nav: true, title: 'Calculator' },
                { route: 'farm-inputs-main', moduleId: 'components/dictionary/farm-inputs-main', name: 'Dictionary', nav: true, title: 'Dictionary' }
            ]);
            this.router = router;
        };
        return App;
    }());
    exports.App = App;
});

define('environment',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = {
        debug: true,
        testing: true
    };
});

define('main',["require", "exports", "./environment", "aurelia-fetch-client"], function (require, exports, environment_1, aurelia_fetch_client_1) {
    "use strict";
    Promise.config({
        longStackTraces: environment_1.default.debug,
        warnings: {
            wForgottenReturn: false
        }
    });
    function configure(aurelia) {
        aurelia.use
            .standardConfiguration()
            .feature("resources");
        var container = aurelia.container;
        var http = new aurelia_fetch_client_1.HttpClient();
        http.configure(function (config) {
            config
                .useStandardConfiguration()
                .withBaseUrl("api/")
                .withDefaults({
                credentials: "same-origin",
                headers: {
                    'X-Requested-With': "Fetch"
                }
            })
                .withInterceptor({
                request: function (request) {
                    console.log("Requesting " + request.method + " " + request.url);
                    return request;
                },
                response: function (response) {
                    console.log("Received " + response.status + " " + response.url);
                    return response;
                }
            });
        });
        container.registerInstance(aurelia_fetch_client_1.HttpClient, http);
        if (environment_1.default.debug) {
            aurelia.use.developmentLogging();
        }
        if (environment_1.default.testing) {
            aurelia.use.plugin("aurelia-testing");
        }
        aurelia.start().then(function () { return aurelia.setRoot(); });
    }
    exports.configure = configure;
});

define('resources/index',["require", "exports"], function (require, exports) {
    "use strict";
    function configure(config) {
        config.globalResources(['./elements/nav-menu']);
    }
    exports.configure = configure;
});

define('components/calculator/calculator-main',["require", "exports"], function (require, exports) {
    "use strict";
    var CalculatorMain = (function () {
        function CalculatorMain() {
        }
        return CalculatorMain;
    }());
    exports.CalculatorMain = CalculatorMain;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('components/dictionary/farm-inputs-main',["require", "exports", "aurelia-framework", "../../services/FarmProductsService"], function (require, exports, aurelia_framework_1, FarmProductsService_1) {
    "use strict";
    var FarmInputsMain = (function () {
        function FarmInputsMain(service) {
            this.service = service;
        }
        FarmInputsMain.prototype.activate = function () {
            var _this = this;
            return this.service.getFarmProductsList()
                .then(function (data) {
                _this.farmProducts = data;
                console.log(_this.farmProducts);
            });
        };
        return FarmInputsMain;
    }());
    FarmInputsMain = __decorate([
        aurelia_framework_1.inject(FarmProductsService_1.FarmProductsService),
        __metadata("design:paramtypes", [FarmProductsService_1.FarmProductsService])
    ], FarmInputsMain);
    exports.FarmInputsMain = FarmInputsMain;
});

define('components/home/home',["require", "exports"], function (require, exports) {
    "use strict";
    var Home = (function () {
        function Home() {
        }
        return Home;
    }());
    exports.Home = Home;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('resources/elements/nav-menu',["require", "exports", "aurelia-framework"], function (require, exports, aurelia_framework_1) {
    "use strict";
    var NavMenu = (function () {
        function NavMenu() {
            this.router = null;
        }
        return NavMenu;
    }());
    __decorate([
        aurelia_framework_1.bindable,
        __metadata("design:type", Object)
    ], NavMenu.prototype, "router", void 0);
    exports.NavMenu = NavMenu;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/FarmProductsService',["require", "exports", "aurelia-framework", "aurelia-fetch-client"], function (require, exports, aurelia_framework_1, aurelia_fetch_client_1) {
    "use strict";
    var FarmProductsService = (function () {
        function FarmProductsService(httpClient) {
            this.httpClient = httpClient;
        }
        FarmProductsService.prototype.getFarmProductsList = function () {
            return this.httpClient.fetch("farmProducts")
                .then(function (response) { return response.json(); }, function (response) { return response.text(); })
                .then(function (data) {
                for (var _i = 0, data_1 = data; _i < data_1.length; _i++) {
                    var item = data_1[_i];
                    item.coverageText = ".   bbbb " + item.coverageText;
                }
                return data;
            });
        };
        return FarmProductsService;
    }());
    FarmProductsService = __decorate([
        aurelia_framework_1.inject(aurelia_fetch_client_1.HttpClient),
        __metadata("design:paramtypes", [aurelia_fetch_client_1.HttpClient])
    ], FarmProductsService);
    exports.FarmProductsService = FarmProductsService;
});

define('text!app.html', ['module'], function(module) { module.exports = "<template>\n  <require from='bootstrap/css/bootstrap.css'></require>\n  <require from='./styles.css'></require>\n\n  <nav-menu router.bind=\"router\"></nav-menu>\n\n  <div class=\"container\">\n    <router-view></router-view>\n  </div>\n\n</template>\n"; });
define('text!styles.css', ['module'], function(module) { module.exports = "body { padding-top: 70px; }\n\nsection {\n  margin: 0 20px;\n}\n\na:focus {\n  outline: none;\n}\n\n.navbar-nav li.loader {\n    margin: 12px 24px 0 6px;\n}\n\n.no-selection {\n  margin: 20px;\n}\n\n.panel {\n  margin: 20px;\n}\n\n.button-bar {\n  right: 0;\n  left: 0;\n  bottom: 0;\n  border-top: 1px solid #ddd;\n  background: white;\n}\n\n.button-bar > button {\n  float: right;\n  margin: 20px;\n}\n\nli.list-group-item {\n  list-style: none;\n}\n\nli.list-group-item > a {\n  text-decoration: none;\n}\n\nli.list-group-item.active > a {\n  color: white;\n}\n"; });
define('text!components/calculator/calculator-main.html', ['module'], function(module) { module.exports = "<template>\n  <h1>Calculator</h1>\n</template>\n"; });
define('text!components/dictionary/farm-inputs-main.html', ['module'], function(module) { module.exports = "<template>\n  <h1>Farm inputs</h1>\n\n  <table>\n   <tbody>\n     <tr repeat.for=\"item of farmProducts\">\n       <td>${item.id}</td>\n       <td>${item.name}</td>\n       <td>${item.coverageText}</td>\n     </tr>\n   </tbody>\n  </table>\n</template>\n"; });
define('text!components/home/home.html', ['module'], function(module) { module.exports = "<template>\n  <h1>Home ABC 123</h1>\n</template>\n"; });
define('text!resources/elements/nav-menu.html', ['module'], function(module) { module.exports = "<template>\n  <nav class=\"navbar navbar-default navbar-fixed-top\" role=\"navigation\">\n    <div class=\"navbar-header\">\n      <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\"#bs-example-navbar.collapse-1\">\n       <span class=\"sr-only\">Toggle Navigation</span>\n        <span class=\"icon-bar\"></span>\n        <span class=\"icon-bar\"></span>\n        <span class=\"icon-bar\"></span>\n      </button>\n\n      <a class=\"navbar-brand\" href=\"#\">\n        <i class=\"fa fa-home\"></i>\n        <span>${router.title}</span>\n      </a>\n    </div>\n\n    <div class=\"collapse navbar-collapse\" id=\"bs-example-navbar-collapse-1\">\n      <ul class=\"nav navbar-nav\">\n        <li repeat.for=\"row of router.navigation\" class=\"${row.isActive ? 'active' : ''}\">\n          <a href.bind=\"row.href\" data-toggle=\"collapse\" data-target=\"#bs-example-navbar-collapse-1.in\">${row.title}</a>\n        </li>\n      </ul>\n\n      <ul class=\"nav navbar-nav navbar-right\">\n        <li class=\"loader\" if.bind=\"router.isNavigating\">\n          <i class=\"fa fa-spinner fa-spin fa-2x\"></i>\n        </li>\n      </ul>\n      \n    </div>\n  </nav>\n</template>\n"; });
//# sourceMappingURL=app-bundle.js.map