"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Path = require("path");
var EsLinter = (function () {
    function EsLinter(logger, linterPath, configPath, linterOptions) {
        this.logger = logger;
        try {
            this.cliEngine = require(Path.join(linterPath, "lib", "cli-engine"));
            this.optModule = require(Path.join(linterPath, "lib", "options"));
        }
        catch (e) {
            throw new Error(e.message);
        }
        var formatOptions = [null, null];
        if (linterOptions && linterOptions.length !== 0) {
            formatOptions = linterOptions.slice();
            formatOptions.unshift(null, null);
        }
        var eslintOptions = this.optModule.parse(formatOptions);
        if (configPath) {
            eslintOptions.config = configPath;
        }
        this.options = EsLinter.translateOptions(eslintOptions);
    }
    EsLinter.prototype.doLint = function (filePath, fileText) {
        if (!this.cliEngine)
            throw new Error("ESLint isn't initialized, wrong linter package location");
        var linter = new this.cliEngine(this.options);
        var report = linter.executeOnText(fileText, filePath, true);
        return EsLinter.prepareReport(report);
    };
    EsLinter.prepareReport = function (report) {
        if (!report) {
            return [];
        }
        var fileResults = report.results[0].messages;
        fileResults.forEach(function (v) {
            if (v) {
                delete v.nodeType;
                delete v.source;
            }
        });
        return fileResults;
    };
    EsLinter.translateOptions = function (cliOptions) {
        return {
            envs: cliOptions.env,
            extensions: cliOptions.ext,
            rules: cliOptions.rule,
            plugins: cliOptions.plugin,
            globals: cliOptions.global,
            ignore: cliOptions.ignore,
            ignorePath: cliOptions.ignorePath,
            ignorePattern: cliOptions.ignorePattern,
            configFile: cliOptions.config,
            rulePaths: cliOptions.rulesdir,
            useEslintrc: cliOptions.eslintrc,
            parser: cliOptions.parser,
            parserOptions: cliOptions.parserOptions,
            cache: false,
            fix: false,
            allowInlineConfig: cliOptions.inlineConfig
        };
    };
    EsLinter.id = "eslint";
    return EsLinter;
}());
var EsLinterFactory = (function () {
    function EsLinterFactory(logger) {
        this.logger = logger;
    }
    EsLinterFactory.prototype.getLinterId = function () {
        return EsLinter.id;
    };
    EsLinterFactory.prototype.createLinter = function (initData) {
        if (!initData.linterPath) {
            throw Error("Can't create EsLint. LinterPath is empty");
        }
        return new EsLinter(this.logger, initData.linterPath, initData.configPath, initData.linterOptions);
    };
    return EsLinterFactory;
}());
exports.default = EsLinterFactory;
