﻿'use strict';
app.factory('sensorTypeService', ['$http', 'ngAuthSettings', '$rootScope', '$localStorage', 'stompService', 'sensorDatasheetContext', 'sensorTypeConstant',
    function ($http, ngAuthSettings, $rootScope, $localStorage, stompService, sensorDatasheetContext, sensorTypeConstant) {

        var serviceFactory = {};

        // Local cache        

        if ($localStorage.sensorTypeData) {
            var data = JSON.parse(Base64.decode($localStorage.sensorTypeData));
            for (var i = 0; i < data.length; i++) {
                sensorDatasheetContext.sensorType.push(data[i]);
            }
            $rootScope.$emit(sensorTypeConstant.getAllCompletedEventName);
            return serviceFactory;
        }

        // Get from Server
        
        var _initializing = false;
        var _initialized = false;

        var serviceBase = ngAuthSettings.distributedServicesUri;

        var getAllCompletedSubscription = null;

        var onConnected = function () {

            getAllCompletedSubscription = stompService.subscribe(sensorTypeConstant.getAllCompletedTopic, onGetAllCompleted);

            if (!_initializing && !_initialized) {
                _initializing = true;
                getAll();
            }
        }

        var getAll = function () {
            return $http.post(serviceBase + sensorTypeConstant.getAllApiUri).then(function (results) {
                //alert('envio bem sucedido');
            });
        };

        var onGetAllCompleted = function (payload) {

            var dataUTF8 = decodeURIComponent(escape(payload.body));

            $localStorage.sensorTypeData = Base64.encode(dataUTF8);
            $localStorage.$save();

            var data = JSON.parse(dataUTF8);

            for (var i = 0; i < data.length; i++) {
                sensorDatasheetContext.sensorType.push(data[i]);
            }

            sensorDatasheetContext.$digest();

            _initializing = false;
            _initialized = true;

            clearOnConnected();

            getAllCompletedSubscription.unsubscribe();

            $rootScope.$emit(sensorTypeConstant.getAllCompletedEventName);
        }

        $rootScope.$on('$destroy', function () {
            clearOnConnected();
        });

        // stompService

        var clearOnConnected = $rootScope.$on(stompService.connectedEventName, onConnected);

        if (stompService.connected()) onConnected();

        return serviceFactory;

    }]);