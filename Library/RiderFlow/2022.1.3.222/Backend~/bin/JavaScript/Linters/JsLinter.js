"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Path = require("path");
var JsLinter = (function () {
    function JsLinter(logger, linterPath, linterOptions) {
        this.logger = logger;
        var mainModule;
        var optModule;
        try {
            mainModule = require(Path.join(linterPath, "lib", "main.js"));
            optModule = require(Path.join(linterPath, "lib", "options.js"));
            this.nodeLint = require(Path.join(linterPath, "lib", "nodelint.js"));
            this.linterModule = require(Path.join(linterPath, "lib", "linter.js"));
        }
        catch (e) {
            throw new Error(e.message);
        }
        var formatOptions;
        if (!linterOptions || linterOptions.length === 0) {
            formatOptions = [null, null];
        }
        else {
            formatOptions = linterOptions.slice();
            formatOptions.unshift(null, null);
        }
        this.linterOptons = optModule
            .getOptions(global.process.env.HOME, mainModule.parseArgs(formatOptions));
        this.linter = this.nodeLint.load(this.linterOptons.edition);
    }
    JsLinter.prototype.doLint = function (filePath, fileText) {
        if (!this.linterModule)
            throw new Error("JSLint isn't initialized, wrong linter package location");
        global.process.chdir(Path.dirname(filePath));
        var report = this.linterModule.doLint(this.linter, fileText, this.linterOptons);
        return JsLinter.prepareReport(report);
    };
    JsLinter.prepareReport = function (report) {
        if (!report) {
            return [];
        }
        var fileResults = report.errors;
        fileResults.forEach(function (v) {
            if (v) {
                delete v.id;
                delete v.evidence;
                delete v.raw;
                delete v.a;
                delete v.b;
            }
        });
        return fileResults;
    };
    JsLinter.id = "jslint";
    return JsLinter;
}());
var JsLinterFactory = (function () {
    function JsLinterFactory(logger) {
        this.logger = logger;
    }
    JsLinterFactory.prototype.getLinterId = function () {
        return JsLinter.id;
    };
    JsLinterFactory.prototype.createLinter = function (initData) {
        if (!initData.linterPath) {
            throw Error("Can't create JsLint. LinterPath is empty");
        }
        return new JsLinter(this.logger, initData.linterPath, initData.linterOptions);
    };
    return JsLinterFactory;
}());
exports.default = JsLinterFactory;
