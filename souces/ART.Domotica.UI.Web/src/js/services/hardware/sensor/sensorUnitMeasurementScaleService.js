﻿'use strict';
app.factory('sensorUnitMeasurementScaleService', ['$http', '$log', '$rootScope', 'ngAuthSettings', 'stompService', 'sensorContext', 'unitMeasurementConverter', 'sensorUnitMeasurementScaleConstant', 'sensorUnitMeasurementScaleFinder', 'sensorTempDSFamilyFinder',
    function ($http, $log, $rootScope, ngAuthSettings, stompService, sensorContext, unitMeasurementConverter, sensorUnitMeasurementScaleConstant, sensorUnitMeasurementScaleFinder, sensorTempDSFamilyFinder) {

        var serviceFactory = {};

        var serviceBase = ngAuthSettings.distributedServicesUri;

        var setUnitMeasurementNumericalScaleTypeCountrySubscription = null;
        var setRangeSubscription = null;
        var setChartLimiterSubscription = null;

        var onConnected = function () {
            setUnitMeasurementNumericalScaleTypeCountrySubscription = stompService.subscribeAllViews(sensorUnitMeasurementScaleConstant.setUnitMeasurementNumericalScaleTypeCountryCompletedTopic, onSetUnitMeasurementNumericalScaleTypeCountryCompleted);
            setRangeSubscription = stompService.subscribeAllViews(sensorUnitMeasurementScaleConstant.setRangeCompletedTopic, onSetRangeCompleted);
            setChartLimiterSubscription = stompService.subscribeAllViews(sensorUnitMeasurementScaleConstant.setChartLimiterCompletedTopic, onSetChartLimiterCompleted);
        }

        var setUnitMeasurementNumericalScaleTypeCountry = function (sensorUnitMeasurementScaleId, sensorDatasheetId, sensorTypeId, unitMeasurementId, unitMeasurementTypeId, numericalScalePrefixId, numericalScaleTypeId, countryId) {
            var data = {
                sensorUnitMeasurementScaleId: sensorUnitMeasurementScaleId,
                sensorDatasheetId: sensorDatasheetId,
                sensorTypeId: sensorTypeId,
                unitMeasurementId: unitMeasurementId,
                unitMeasurementTypeId: unitMeasurementTypeId,
                numericalScalePrefixId: numericalScalePrefixId,
                numericalScaleTypeId: numericalScaleTypeId,
                countryId: countryId,
            }
            return $http.post(serviceBase + sensorUnitMeasurementScaleConstant.setUnitMeasurementNumericalScaleTypeCountryApiUri, data).then(function (results) {
                return results;
            });
        };    

        var setRange = function (sensorUnitMeasurementScaleId, sensorDatasheetId, sensorTypeId, value, position) {
            var data = {
                sensorUnitMeasurementScaleId: sensorUnitMeasurementScaleId,
                sensorDatasheetId: sensorDatasheetId,
                sensorTypeId: sensorTypeId,
                value: value,
                position: position,
            }
            return $http.post(serviceBase + sensorUnitMeasurementScaleConstant.setRangeApiUri, data).then(function (results) {
                return results;
            });
        };        

        var setChartLimiter = function (sensorUnitMeasurementScaleId, sensorDatasheetId, sensorTypeId, value, position) {
            var data = {
                sensorUnitMeasurementScaleId: sensorUnitMeasurementScaleId,
                sensorDatasheetId: sensorDatasheetId,
                sensorTypeId: sensorTypeId,
                value: value,
                position: position,
            }
            return $http.post(serviceBase + sensorUnitMeasurementScaleConstant.setChartLimiterApiUri, data).then(function (results) {
                return results;
            });
        };     
        
        var onSetUnitMeasurementNumericalScaleTypeCountryCompleted = function (payload) {
            var result = JSON.parse(payload.body);
            var sensor = sensorUnitMeasurementScaleFinder.getByKey(result.sensorUnitMeasurementScaleId, result.sensorDatasheetId, result.sensorTypeId);

            sensor.unitMeasurementId = result.unitMeasurementId;
            sensor.unitMeasurementTypeId = result.unitMeasurementTypeId;
            sensor.numericalScalePrefixId = result.numericalScalePrefixId;
            sensor.numericalScaleTypeId = result.numericalScaleTypeId;
            sensor.countryId = result.countryId;

            sensorContext.$digest();
            $rootScope.$emit(sensorUnitMeasurementScaleConstant.setUnitMeasurementNumericalScaleTypeCountryCompletedEventName + result.sensorUnitMeasurementScaleId, result);
        }

        var onSetRangeCompleted = function (payload) {
            var result = JSON.parse(payload.body);
            var sensor = sensorUnitMeasurementScaleFinder.getByKey(result.sensorUnitMeasurementScaleId, result.sensorDatasheetId, result.sensorTypeId);
            if (result.position === 'Max') {
                sensor.sensorUnitMeasurementScale.rangeMax = result.value;
                //sensor.sensorUnitMeasurementScale.maxConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.sensorUnitMeasurementScale.max);
            }
            else if (result.position === 'Min') {
                sensor.sensorUnitMeasurementScale.rangeMin = result.value;
                //sensor.sensorUnitMeasurementScale.minConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.sensorUnitMeasurementScale.min);
            }
            sensorContext.$digest();
            $rootScope.$emit(sensorUnitMeasurementScaleConstant.setRangeCompletedEventName + result.sensorUnitMeasurementScaleId, result);
        }

        var onSetChartLimiterCompleted = function (payload) {
            var result = JSON.parse(payload.body);
            var sensor = sensorUnitMeasurementScaleFinder.getByKey(result.sensorUnitMeasurementScaleId, result.sensorDatasheetId, result.sensorTypeId);
            if (result.position === 'Max') {
                sensor.sensorUnitMeasurementScale.chartLimiterMax = result.value;
                //sensor.sensorUnitMeasurementScale.maxConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.sensorUnitMeasurementScale.max);
            }
            else if (result.position === 'Min') {
                sensor.sensorUnitMeasurementScale.chartLimiterMin = result.value;
                //sensor.sensorUnitMeasurementScale.minConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.sensorUnitMeasurementScale.min);
            }
            sensorContext.$digest();
            $rootScope.$emit(sensorUnitMeasurementScaleConstant.setChartLimiterCompletedEventName + result.sensorUnitMeasurementScaleId, result);
        }

        //var onSetSensorUnitMeasurementScaleCompleted = function (payload) {

        //    var result = JSON.parse(payload.body);
        //    var sensor = getByKey(result.deviceId, result.sensorTempDSFamilyId);

        //    //unitMeasurement
        //    sensor.unitMeasurementId = result.unitMeasurementId;
        //    sensor.unitMeasurement = unitMeasurementService.getByKey(sensor.unitMeasurementId);

        //    //temp
        //    sensor.tempConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.tempCelsius);

        //    //sensorUnitMeasurementScale
        //    sensor.sensorUnitMeasurementScale.maxConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.sensorUnitMeasurementScale.max);
        //    sensor.sensorUnitMeasurementScale.minConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.sensorUnitMeasurementScale.min);

        //    //alarms
        //    sensor.highAlarm.alarmConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.highAlarm.alarmCelsius);
        //    sensor.lowAlarm.alarmConverted = unitMeasurementConverter.convertFromCelsius(sensor.unitMeasurementId, sensor.lowAlarm.alarmCelsius);

        //    $rootScope.$emit(sensorConstant.setSensorUnitMeasurementScaleCompletedEventName + result.sensorTempDSFamilyId, result);
        //}    

        $rootScope.$on('$destroy', function () {
            clearOnConnected();
            setUnitMeasurementNumericalScaleTypeCountrySubscription.unsubscribe();
            setRangeSubscription.unsubscribe();
            setChartLimiterSubscription.unsubscribe();
        });

        var clearOnConnected = $rootScope.$on(stompService.connectedEventName, onConnected);       

        // stompService
        if (stompService.connected()) onConnected();

        // serviceFactory

        serviceFactory.setUnitMeasurementNumericalScaleTypeCountry = setUnitMeasurementNumericalScaleTypeCountry;
        serviceFactory.setRange = setRange;
        serviceFactory.setChartLimiter = setChartLimiter;

        return serviceFactory;

    }]);