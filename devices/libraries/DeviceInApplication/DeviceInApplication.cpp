#include "DeviceInApplication.h"
#include "ESPDevice.h"

namespace ART
{
	DeviceInApplication::DeviceInApplication(ESPDevice* espDevice)
	{
		_espDevice = espDevice;		
	}

	DeviceInApplication::~DeviceInApplication()
	{
		delete (_espDevice);
		delete (_applicationId);
		delete (_applicationTopic);
	}

	void DeviceInApplication::load(JsonObject & jsonObject)
	{
		DeviceDebug* deviceDebug = _espDevice->getDeviceDebug();

		deviceDebug->print("DeviceInApplication", "load", "begin\n");

		char* applicationId = strdup(jsonObject["applicationId"]);
		_applicationId = new char(sizeof(strlen(applicationId)));
		_applicationId = applicationId;

		char* applicationTopic = strdup(jsonObject["applicationTopic"]);
		_applicationTopic = new char(sizeof(strlen(applicationTopic)));
		_applicationTopic = applicationTopic;
	}

	char* DeviceInApplication::getApplicationId() const
	{
		return (_applicationId);
	}

	void DeviceInApplication::setApplicationId(char* value)
	{
		_applicationId = new char(sizeof(strlen(value)));
		_applicationId = value;
	}

	char* DeviceInApplication::getApplicationTopic() const
	{
		return (_applicationTopic);
	}

	void DeviceInApplication::setApplicationTopic(char* value)
	{
		_applicationTopic = new char(sizeof(strlen(value)));
		_applicationTopic = value;
	}

	void DeviceInApplication::insertInApplication(String json)
	{
		StaticJsonBuffer<300> jsonBuffer;

		JsonObject& root = jsonBuffer.parseObject(json);

		if (!root.success()) {
			Serial.print("[DeviceInApplication::insertInApplication] parse failed: ");
			Serial.println(json);
			return;
		}

		char* applicationId = strdup(root["applicationId"]);
		char* applicationTopic = strdup(root["applicationTopic"]);

		setApplicationId(applicationId);
		setApplicationTopic(applicationTopic);

		Serial.println("[DeviceInApplication::insertInApplication] ");
		Serial.print("applicationId: ");
		Serial.println(applicationId);
		Serial.print("applicationTopic: ");
		Serial.println(applicationTopic);
	}

	void DeviceInApplication::deleteFromApplication()
	{
		setApplicationId("");
		setApplicationTopic("");

		Serial.println("[DeviceInApplication::deleteFromApplication] delete from Application with success !");
	}
}