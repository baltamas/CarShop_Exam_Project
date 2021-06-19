"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var cars_service_1 = require("./cars.service");
describe('CarsService', function () {
    beforeEach(function () { return testing_1.TestBed.configureTestingModule({}); });
    it('should be created', function () {
        var service = testing_1.TestBed.get(cars_service_1.CarsService);
        expect(service).toBeTruthy();
    });
});
//# sourceMappingURL=cars.service.spec.js.map