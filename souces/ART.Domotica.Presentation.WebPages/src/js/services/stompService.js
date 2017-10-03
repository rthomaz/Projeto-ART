﻿'use strict';
app.factory('stompService', ['$http', '$log', 'ngAuthSettings', function ($http, $log, ngAuthSettings) {
    
    var serviceFactory = {};    

    serviceFactory.session = null;

    var onConnected = function (frame) {

        serviceFactory.session = frame.headers.session;

        if (serviceFactory.onConnected != null) {
            serviceFactory.onConnected(frame);
        }        

        debug('connected in broker');
        debug('session: ' + frame.headers.session);        
    }

    var onError = function () {

        serviceFactory.session = null;

        if (serviceFactory.onError != null) {
            serviceFactory.onError();
        }
        debug('Error connecting in broker');
    }

    var debug = function (str) {
        console.log(str);
    };

    // stomp

    var wsBrokerHostName = ngAuthSettings.wsBrokerHostName;
    var wsBrokerPort = ngAuthSettings.wsBrokerPort;

    var url = 'ws://' + wsBrokerHostName + ':' + wsBrokerPort + '/ws';

    var client = Stomp.client(url);

    var headers = {
        login: 'test',
        passcode: 'test',        
    };

    client.connect(headers, onConnected, onError);    

    // serviceFactory

    serviceFactory.onConnected = null;
    serviceFactory.onError = null;
    serviceFactory.client = client;

    return serviceFactory;   

}]);