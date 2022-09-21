"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LinterResponse = (function () {
    function LinterResponse() {
        this.messages = [];
    }
    return LinterResponse;
}());
exports.LinterResponse = LinterResponse;
var ErrorMessage = (function () {
    function ErrorMessage(message, linterId) {
        this.linterId = linterId ? linterId : null;
        this.message = message;
    }
    return ErrorMessage;
}());
exports.ErrorMessage = ErrorMessage;
var InitData = (function () {
    function InitData() {
    }
    return InitData;
}());
exports.InitData = InitData;
var LocalMap = (function () {
    function LocalMap() {
        this.map = {};
    }
    LocalMap.prototype.has = function (key) {
        return this.map.hasOwnProperty(key);
    };
    LocalMap.prototype.get = function (key) {
        return this.map[key];
    };
    LocalMap.prototype.set = function (key, value) {
        this.map[key] = value;
        return this;
    };
    LocalMap.prototype.clear = function () {
        this.map = {};
    };
    LocalMap.prototype.size = function () {
        return Object.keys(this.map).length;
    };
    return LocalMap;
}());
exports.LocalMap = LocalMap;
