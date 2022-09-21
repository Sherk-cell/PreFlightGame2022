"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Logging_1 = require("./Logging");
var loader = require("./LintersLoader");
var LintSession_1 = require("./LintSession");
var logger = new Logging_1.ErrorLogger();
var linterFactories = loader.getLinterFactories(logger);
var session = new LintSession_1.default(logger, linterFactories);
session.start();
