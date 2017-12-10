﻿'use strict';
app.factory('deviceSensorsFinder', ['$rootScope', 'deviceContext', function ($rootScope, deviceContext) {

    var context = deviceContext;

    var serviceFactory = {}; 

    var getByKey = function (deviceSensorsId) {
        for (var i = 0; i < context.deviceSensors.length; i++) {
            var item = context.deviceSensors[i];
            if (item.deviceSensorsId === deviceSensorsId) {
                return item;
            }
        }
    };

    // *** Public Methods ***

    serviceFactory.getByKey = getByKey;

    return serviceFactory;

}]);