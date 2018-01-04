#ifndef ESPDevice_h
#define ESPDevice_h

#include "DeviceInApplication.h"
#include "DeviceMQ.h"
#include "DeviceNTP.h"
#include "DeviceSensors.h"

#include "Arduino.h"
#include "ArduinoJson.h"
#include <ESP8266WiFi.h>
#include "RemoteDebug.h"        //https://github.com/JoaoLopesF/RemoteDebug

class DeviceMQ;
class DeviceNTP;
class DeviceSensors;

class ESPDevice
{
	public:
		
		ESPDevice();
		~ESPDevice();
		
		void						load(String json);
		
		char *						getDeviceId();
		short						getDeviceDatasheetId();
		
		int							getChipId();
		int							getFlashChipId();
		long						getChipSize();
		
		char *						getStationMacAddress();
		char *						getSoftAPMacAddress();		

		char *						getLabel();
		void						setLabel(char * value);
		
		DeviceInApplication*		getDeviceInApplication();
		DeviceMQ*					getDeviceMQ();
		DeviceNTP*					getDeviceNTP();
		DeviceSensors*				getDeviceSensors();
		
		RemoteDebug*				getDebug();			
	
	private:	

		char *						_deviceId;
		short						_deviceDatasheetId;
		
		int							_chipId;
		int							_flashChipId;
		long						_chipSize;
		
		char *						_stationMacAddress;
		char *						_softAPMacAddress;		
		
		char *						_label;
		
		DeviceInApplication*		_deviceInApplication;
		DeviceMQ*					_deviceMQ;
		DeviceNTP*					_deviceNTP;
		DeviceSensors*				_deviceSensors;
		
		RemoteDebug* 				_debug;
		
};

#endif