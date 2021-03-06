﻿'use strict';
app.factory('deviceContext', ['$rootScope', function ($rootScope) {

    var context = $rootScope.$new();
        
    context.device = [];   
    context.deviceLoaded = false;
        
    context.deviceWiFi = [];   
    context.deviceWiFiLoaded = false;

    context.deviceNTP = [];   
    context.deviceNTPLoaded = false;

    context.deviceSerial = [];   
    context.deviceSerialLoaded = false;

    context.deviceDebug = [];   
    context.deviceDebugLoaded = false;

    context.deviceDisplay = [];   
    context.deviceDisplayLoaded = false;

    context.deviceSensor = [];   
    context.deviceSensorLoaded = false;

    context.sensorInDevice = []; 
    context.sensorInDeviceLoaded = false;
        
    return context;

}]);