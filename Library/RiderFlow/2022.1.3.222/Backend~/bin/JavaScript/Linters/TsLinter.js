"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var path = require("path");
var TsLinter = (function () {
    function TsLinter(logger, initData) {
        this.logger = logger;
        try {
            this.tslint = require(initData.linterPath);
        }
        catch (e) {
            throw new Error(e.message);
        }
        if (initData.configPath && initData.configPath.length === 0)
            this.configPath = null;
        else
            this.configPath = initData.configPath;
        this.linter = new this.tslint.Linter(TsLinter.getOptions(initData.linterOptions));
    }
    TsLinter.prototype.doLint = function (filePath, fileText, tsconfigPath) {
        if (!this.tslint)
            throw new Error("TSLint isn't initialized, wrong linter package location");
        var configuration = this.tslint.Configuration.findConfiguration(this.configPath, filePath).results;
        var ext = path.extname(filePath);
        if (ext !== ".ts" && ext !== ".js" && ext !== ".tsx" && ext !== ".jsx") {
            var pos = filePath.lastIndexOf(".");
            filePath = filePath.substr(0, pos < 0 ? filePath.length : pos) + ".js";
        }
        this.linter.lint(filePath, fileText, configuration);
        return this.prepareReport(this.linter.getResult());
    };
    TsLinter.getOptions = function (args) {
        var opts = {
            fix: false,
            formatter: "json",
            formattersDirectory: undefined,
            rulesDirectory: undefined,
            typeCheck: undefined
        };
        args.forEach((function (value, index, array) {
            var upperVal = value.toUpperCase();
            if (upperVal === "--RULES-DIR" || upperVal === "-R")
                opts.rulesDirectory = array[index + 1];
            if (upperVal === "--FORMATTERS-DIR" || upperVal === "-S")
                opts.formattersDirectory = array[index + 1];
        }));
        return opts;
    };
    TsLinter.prototype.prepareReport = function (report) {
        if (!report) {
            return [];
        }
        var failures = report.failures.slice();
        report.failures.length = 0;
        failures.forEach(function (v) {
            delete v.sourceFile;
            delete v.rawLines;
            delete v.fileName;
            v.endPosition = v.endPosition.position;
            v.startPosition = v.startPosition.position;
            if (v.fix && !(v.fix instanceof Array)) {
                v.fix = [v.fix];
            }
        });
        return failures;
    };
    TsLinter.id = "tslint";
    return TsLinter;
}());
var TslintFactory = (function () {
    function TslintFactory(logger) {
        this.logger = logger;
    }
    TslintFactory.prototype.getLinterId = function () {
        return TsLinter.id;
    };
    TslintFactory.prototype.createLinter = function (initData) {
        if (!initData.linterPath) {
            throw Error("Can't create TsLint. LinterPath is empty");
        }
        return new TsLinter(this.logger, initData);
    };
    return TslintFactory;
}());
exports.default = TslintFactory;
