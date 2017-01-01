var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define(["require", "exports", "aurelia-dependency-injection", "aurelia-cli"], function (require, exports, aurelia_dependency_injection_1, aurelia_cli_1) {
    "use strict";
    var ValueConverterGenerator = (function () {
        function ValueConverterGenerator(project, options, ui) {
            this.project = project;
            this.options = options;
            this.ui = ui;
        }
        ValueConverterGenerator.prototype.execute = function () {
            var _this = this;
            return this.ui
                .ensureAnswer(this.options.args[0], 'What would you like to call the value converter?')
                .then(function (name) {
                var fileName = _this.project.makeFileName(name);
                var className = _this.project.makeClassName(name);
                _this.project.valueConverters.add(aurelia_cli_1.ProjectItem.text(fileName + ".ts", _this.generateSource(className)));
                return _this.project.commitChanges()
                    .then(function () { return _this.ui.log("Created " + fileName + "."); });
            });
        };
        ValueConverterGenerator.prototype.generateSource = function (className) {
            return "export class " + className + "ValueConverter {\n  toView(value) {\n\n  }\n\n  fromView(value) {\n\n  }\n}\n\n";
        };
        return ValueConverterGenerator;
    }());
    ValueConverterGenerator = __decorate([
        aurelia_dependency_injection_1.inject(aurelia_cli_1.Project, aurelia_cli_1.CLIOptions, aurelia_cli_1.UI),
        __metadata("design:paramtypes", [typeof (_a = typeof aurelia_cli_1.Project !== "undefined" && aurelia_cli_1.Project) === "function" && _a || Object, typeof (_b = typeof aurelia_cli_1.CLIOptions !== "undefined" && aurelia_cli_1.CLIOptions) === "function" && _b || Object, typeof (_c = typeof aurelia_cli_1.UI !== "undefined" && aurelia_cli_1.UI) === "function" && _c || Object])
    ], ValueConverterGenerator);
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = ValueConverterGenerator;
    var _a, _b, _c;
});
//# sourceMappingURL=value-converter.js.map