"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Linters_1 = require("./Linters");
var JsLinter_1 = require("./JsLinter");
var EsLinter_1 = require("./EsLinter");
var TsLinter_1 = require("./TsLinter");
function getLinterFactories(logger) {
    logger.logMessage('Loading extensions...');
    var linterFactories = new Linters_1.LocalMap();
    function registerFactory(factory) {
        logger.logMessage("-- Loaded " + factory.getLinterId());
        linterFactories.set(factory.getLinterId(), factory);
    }
    registerFactory(new JsLinter_1.default(logger));
    registerFactory(new EsLinter_1.default(logger));
    registerFactory(new TsLinter_1.default(logger));
    logger.logMessage("Loaded " + linterFactories.size + " extensions.");
    return linterFactories;
}
exports.getLinterFactories = getLinterFactories;
