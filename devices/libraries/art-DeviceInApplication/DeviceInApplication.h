#ifndef DeviceInApplication_h
#define DeviceInApplication_h

#include "Arduino.h"
#include "ArduinoJson.h"

class ESPDevice;

class DeviceInApplication
{

public:

	DeviceInApplication(ESPDevice* espDevice, char* applicationId, char* applicationTopic);
	~DeviceInApplication();
	
	void								insertInApplication(String json);
	void								deleteFromApplication();		
	
	char*								getApplicationId();
	void								setApplicationId(char* value);		
	
	char*								getApplicationTopic();
	void								setApplicationTopic(char* value);		
	
	static void createDeviceInApplication(DeviceInApplication* (&deviceInApplication), ESPDevice* espDevice, JsonObject& jsonObject)
    {
		deviceInApplication = new DeviceInApplication(
			espDevice,
			strdup(jsonObject["applicationId"]),
			strdup(jsonObject["applicationTopic"]));
    }
	
private:	

	ESPDevice*          				_espDevice;	
	
	char*								_applicationId;
	char*								_applicationTopic;
	
};

#endif