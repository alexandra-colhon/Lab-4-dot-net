"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var expenses_service_1 = require("./expenses.service");
describe('ExpensesService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(expenses_service_1.ExpensesService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=expenses.service.spec.js.map