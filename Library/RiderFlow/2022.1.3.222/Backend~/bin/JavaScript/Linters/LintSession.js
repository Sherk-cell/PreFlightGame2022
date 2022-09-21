"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Linters_1 = require("./Linters");
var readline_1 = require("readline");
var LinterRequest = (function () {
    function LinterRequest() {
    }
    return LinterRequest;
}());
var LintData = (function () {
    function LintData() {
    }
    return LintData;
}());
var InitResponse = (function () {
    function InitResponse() {
        this.messages = [];
    }
    return InitResponse;
}());
var LintSession = (function () {
    function LintSession(logger, linterFactories) {
        this.knownRequestType = {
            init: "init",
            lint: "lint"
        };
        global.process.stdin.setEncoding("utf8");
        global.process.stdout.setEncoding("utf8");
        this.logger = logger;
        this.linterFactories = linterFactories;
        this.linters = new Linters_1.LocalMap();
        this.readliner = readline_1.createInterface({
            input: global.process.stdin,
            terminal: false
        });
    }
    LintSession.prototype.start = function () {
        var _this = this;
        this.readliner.on("line", function (line) {
            var request = JSON.parse(line);
            var response;
            switch (request.requestType) {
                case _this.knownRequestType.init:
                    response = _this.init(request.projectId, request.data);
                    break;
                case _this.knownRequestType.lint:
                    response = _this.lint(request.projectId, request.data);
                    break;
                default:
                    response = new Linters_1.LinterResponse();
                    response.messages.push(new Linters_1.ErrorMessage("Unexpected request type: " + request.requestType));
            }
            global.process.stdout.write(LintSession.stringifyToLine(response));
        }).on("close", function () {
            process.exit(0);
        });
    };
    LintSession.prototype.init = function (projectId, data) {
        var response = new Linters_1.LinterResponse();
        try {
            if (!this.linters.has(projectId)) {
                this.linters.set(projectId, new Linters_1.LocalMap());
            }
            for (var _i = 0, data_1 = data; _i < data_1.length; _i++) {
                var d = data_1[_i];
                try {
                    var linterFactory = this.linterFactories.get(d.linterId);
                    if (!linterFactory) {
                        throw new Error("Can't find linter factory by id: " + d.linterId);
                    }
                    var linter = linterFactory.createLinter(d);
                    this.linters.get(projectId).set(d.linterId, linter);
                }
                catch (e) {
                    this.logger.logError(e.stack);
                    response.messages.push(new Linters_1.ErrorMessage(e.message, d.linterId));
                }
            }
        }
        catch (e) {
            this.logger.logError(e.stack);
            response.messages.push(new Linters_1.ErrorMessage(e.message));
        }
        return response;
    };
    LintSession.prototype.lint = function (projectId, data) {
        var response = new Linters_1.LinterResponse();
        try {
            if (!this.linters.has(projectId)) {
                throw new Error("Can't find any initialized linters for the project: " + projectId);
            }
            for (var _i = 0, _a = data.linterIds; _i < _a.length; _i++) {
                var id = _a[_i];
                try {
                    var linter = this.linters.get(projectId).get(id);
                    if (!linter) {
                        throw new Error("Can't find initialized linter: " + id + ", try to change linter settings");
                    }
                    response[id] = linter.doLint(data.filePath, data.text);
                }
                catch (e) {
                    this.logger.logError(e.stack);
                    response.messages.push(new Linters_1.ErrorMessage(e.message, id));
                }
            }
            return response;
        }
        catch (e) {
            this.logger.logError(e.stack);
            response.messages.push(new Linters_1.ErrorMessage("An error occurred while linting. " + e.message));
            return response;
        }
    };
    LintSession.stringifyToLine = function (response) {
        return JSON.stringify(response) + "\n";
    };
    return LintSession;
}());
exports.default = LintSession;
