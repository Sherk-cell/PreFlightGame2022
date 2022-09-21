"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConsoleLogger = (function () {
    function ConsoleLogger() {
    }
    ConsoleLogger.prototype.logMessage = function (message) {
        console.log("LOG: " + message);
    };
    ConsoleLogger.prototype.logError = function (message) {
        console.log("ERR: " + message);
        console.error(message);
    };
    return ConsoleLogger;
}());
exports.ConsoleLogger = ConsoleLogger;
var NullLogger = (function () {
    function NullLogger() {
    }
    NullLogger.prototype.logMessage = function (message) {
    };
    NullLogger.prototype.logError = function (message) {
    };
    return NullLogger;
}());
exports.NullLogger = NullLogger;
var ErrorLogger = (function () {
    function ErrorLogger() {
    }
    ErrorLogger.prototype.logMessage = function (message) {
    };
    ErrorLogger.prototype.logError = function (message) {
        console.error("ERR: " + message);
    };
    return ErrorLogger;
}());
exports.ErrorLogger = ErrorLogger;
