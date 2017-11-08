#include "TemperatureSensorManager.h"

// Data wire is plugged into port 2 on the Arduino
#define ONE_WIRE_BUS 0

// Setup a oneWire instance to communicate with any OneWire devices (not just Maxim/Dallas temperature ICs)
OneWire oneWire(ONE_WIRE_BUS);

// Pass our oneWire reference to Dallas Temperature. 
DallasTemperature _dallas(&oneWire);


// TemperatureSensor

TemperatureSensor::TemperatureSensor(String dsFamilyTempSensorId, String deviceAddressStr, bool validFamily, String family)
{
	this->_dsFamilyTempSensorId = dsFamilyTempSensorId;
	//this->_deviceAddress = deviceAddress;
	this->_deviceAddressStr = deviceAddressStr;	
	this->_validFamily = validFamily;
	this->_family = family;
}

String TemperatureSensor::getDSFamilyTempSensorId()
{
	return this->_dsFamilyTempSensorId;
}

const uint8_t* TemperatureSensor::getDeviceAddress()
{
	return this->_deviceAddress;
}

String TemperatureSensor::getDeviceAddressStr()
{
	return this->_deviceAddressStr;
}

bool TemperatureSensor::getValidFamily()
{
	return this->_validFamily;
}

String TemperatureSensor::getFamily()
{
	return this->_family;
}

bool TemperatureSensor::getConnected()
{
	return this->_connected;
}

void TemperatureSensor::setConnected(bool value)
{
	this->_connected = value;
}

int TemperatureSensor::getResolution()
{
	return this->_resolution;
}

void TemperatureSensor::setResolution(int value)
{
	this->_resolution = value;
}

float TemperatureSensor::getTempCelsius()
{
	return this->_tempCelsius;
}

void TemperatureSensor::setTempCelsius(float value)
{
	this->_tempCelsius = value;
}

float TemperatureSensor::getTempFahrenheit()
{
	return this->_tempFahrenheit;
}

void TemperatureSensor::setTempFahrenheit(float value)
{
	this->_tempFahrenheit = value;
}

bool TemperatureSensor::getHasAlarm()
{
	return this->_hasAlarm;
}

void TemperatureSensor::setHasAlarm(bool value)
{
	this->_hasAlarm = value;
}

char TemperatureSensor::getLowAlarm()
{
	return this->_lowAlarm;
}

void TemperatureSensor::setLowAlarm(char value)
{
	this->_lowAlarm = value;
}

char TemperatureSensor::getHighAlarm()
{
	return this->_highAlarm;
}

void TemperatureSensor::setHighAlarm(char value)
{
	this->_highAlarm = value;
}

long TemperatureSensor::getEpochTime()
{
	return this->_epochTime;
}

void TemperatureSensor::setEpochTime(long value)
{
	this->_epochTime = value;
}

// TemperatureSensorManager

TemperatureSensorManager::TemperatureSensorManager(DebugManager& debugManager, NTPManager& ntpManager, ConfigurationManager& configurationManager, MQQTManager& mqqtManager)
{ 
	this->_debugManager = &debugManager;
	this->_ntpManager = &ntpManager;
	this->_configurationManager = &configurationManager;
	this->_mqqtManager = &mqqtManager;
}

bool TemperatureSensorManager::begin()
{	
	if(this->_begin) return true;	

	if(!this->_configurationManager->initialized()) return false;	
	
	if(this->_beginning) return false;	
	
	PubSubClient* mqqt = this->_mqqtManager->getMQQT();
 
	if(!mqqt->connected()) return false;	
	
	// Begin
	
	this->_beginning = true;
	
	String hardwareId = this->_configurationManager->getHardwareSettings()->getHardwareId();      
	String hardwareInApplicationId = this->_configurationManager->getHardwareSettings()->getHardwareInApplicationId();      

	StaticJsonBuffer<TEMPERATURE_SENSOR_GET_ALL_BY_HARDWARE_IN_APPLICATION_ID_REQUEST_JSON_SIZE> JSONbuffer;
	JsonObject& root = JSONbuffer.createObject();
	root["hardwareId"] = hardwareId;
	root["hardwareInApplicationId"] = hardwareInApplicationId;

	int len = root.measureLength();
	char result[len + 1]; 
	root.printTo(result, sizeof(result));
	
	Serial.println("[TemperatureSensorManager::begin] beginning...]");
	
	mqqt->publish(TEMPERATURE_SENSOR_GET_ALL_BY_HARDWARE_IN_APPLICATION_ID_MQQT_TOPIC_PUB, result); 
	
	
	
	
	
	
	// Start up the library
	_dallas.begin();  

	
	
	return true;	
}

void TemperatureSensorManager::setSensorsByMQQTCallback(String json)
{
	Serial.println("[TemperatureSensorManager::setSensorsByMQQTCallback]");
	
	this->_begin = true;
	this->_beginning = false;

	StaticJsonBuffer<TEMPERATURE_SENSOR_GET_ALL_BY_HARDWARE_IN_APPLICATION_ID_RESPONSE_JSON_SIZE> jsonBuffer;

	JsonArray& jsonArray = jsonBuffer.parseArray(json);

	if (!jsonArray.success()) {
		Serial.print("[TemperatureSensorManager::setSensorsByMQQTCallback] parse failed: ");
		Serial.println(json);
		return;
	}			

	for(JsonArray::iterator it=jsonArray.begin(); it!=jsonArray.end(); ++it) 
	{
		Serial.println("EntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrouEntrou");
		
		JsonObject& root = it->as<JsonObject>();
		
		String dsFamilyTempSensorId = root["dsFamilyTempSensorId"];
		String deviceAddressStr = root["deviceAddress"];
		const uint8_t* deviceAddressChar = reinterpret_cast<const uint8_t*>(deviceAddressStr.c_str());
		int dsFamilyTempSensorResolutionBits = int(root["dsFamilyTempSensorResolutionBits"]);
		String family = root["family"];
		double highAlarm = double(root["highAlarm"]);
		double lowAlarm = double(root["lowAlarm"]);
		String temperatureScaleId = root["temperatureScaleId"];			
								
		bool validFamily = true;		
			
		this->_sensors.push_back(TemperatureSensor(
				dsFamilyTempSensorId, 
				//deviceAddressChar, 
				deviceAddressStr, 
				validFamily, 
				family));
	}
				
	Serial.println("[TemperatureSensorManager:: begin]");
	
	return;
	
	// Localizando devices
	uint8_t deviceCount = _dallas.getDeviceCount();
		
	if (this->_debugManager->isDebug()) {
		Serial.print("Localizando devices...");
		Serial.print("Encontrado(s) ");
		Serial.print(deviceCount);
		Serial.println(" device(s).");

		// report parasite power requirements
		Serial.print("Parasite power is: ");
		if (_dallas.isParasitePowerMode()) Serial.println("ON");
		else Serial.println("OFF");
	}	
	
	for(int i = 0; i < deviceCount; ++i){
		
		DeviceAddress deviceAddress;
		
		if (_dallas.getAddress(deviceAddress, i))
		{ 
			//dsFamilyTempSensorId			
			//String dsFamilyTempSensorId = "4fe0c742-b8a4-e711-9bee-707781d470bc";						
						
			// deviceAddress
			//byte deviceAddressChar[8];
			//String deviceAddressStr;
			//for (uint8_t i = 0; i < 8; i++)
			//{
			//	deviceAddressChar[i] = deviceAddress[i];			
			//	deviceAddressStr += String(deviceAddressChar[i], HEX);	
			//}
	
			//validFamily
			//bool validFamily = _dallas.validFamily(deviceAddressChar);

			// family
			//String family = getFamily(deviceAddressChar);     		  		  

			//this->_sensors.push_back(TemperatureSensor(
			//	dsFamilyTempSensorId, 
			//	deviceAddressChar, 
			//	deviceAddressStr, 
			//	validFamily, 
			//	family));
			
			//if (this->_debugManager->isDebug()) Serial.println(deviceAddressStr);
		  
		}
		else{
		  if (this->_debugManager->isDebug()) {
			  Serial.print("Não foi possível encontrar um endereço para o Device ");
			  Serial.println(i);
		  }		  
		}
	}
}

void TemperatureSensorManager::refresh()
{	
	_dallas.requestTemperatures();

	long epochTime = this->_ntpManager->getEpochTimeUTC();	
	
	for(int i = 0; i < this->_sensors.size(); ++i){	
		this->_sensors[i].setConnected(_dallas.isConnected(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setResolution(_dallas.getResolution(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setTempCelsius(_dallas.getTempC(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setTempFahrenheit(_dallas.getTempF(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setHasAlarm(_dallas.hasAlarm(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setLowAlarm(_dallas.getLowAlarmTemp(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setHighAlarm(_dallas.getHighAlarmTemp(this->_sensors[i].getDeviceAddress()));
		this->_sensors[i].setEpochTime(epochTime);
	}
}

TemperatureSensor *TemperatureSensorManager::getSensors()
{
	TemperatureSensor* array = this->_sensors.data();
	return array;
}

char *TemperatureSensorManager::getSensorsJson()
{	
	StaticJsonBuffer<900> JSONbuffer;

	JsonArray& device = JSONbuffer.createArray();

	for(int i = 0; i < this->_sensors.size(); ++i){	
		generateNestedSensor(this->_sensors[i], device);
	}
	
	int len = device.measureLength();

	char result[len + 1];

	device.printTo(result, sizeof(result));

	if (this->_debugManager->isDebug()) {
		Serial.print("tamanho device ==>");
		Serial.println(len);
		Serial.print("result device ==>");
		//device.printTo(Serial);  // está estourando erro aqui
		Serial.println();
	}

    return result;
}

void TemperatureSensorManager::setResolution(String json)
{
	StaticJsonBuffer<200> jsonBuffer;

	JsonObject& root = jsonBuffer.parseObject(json);
	
	if (!root.success()) {
		Serial.print("parse setResolution failed: ");
		Serial.println(json);
		return;
	}	

	String dsFamilyTempSensorId = root["dsFamilyTempSensorId"];
	int value = root["dsFamilyTempSensorResolutionId"];

	_dallas.setResolution(getDeviceAddress(dsFamilyTempSensorId), value);

	Serial.print("setResolution=");
	Serial.println(json);
}

void TemperatureSensorManager::setLowAlarm(String json)
{
	StaticJsonBuffer<300> jsonBuffer;

	JsonObject& root = jsonBuffer.parseObject(json);

	if (!root.success()) {
		Serial.println("parse setLowAlarm failed");
		return;
	}

	String dsFamilyTempSensorId = root["dsFamilyTempSensorId"];
	int value = root["dsFamilyTempSensorResolutionId"];

	_dallas.setLowAlarmTemp(getDeviceAddress(dsFamilyTempSensorId), value);

	Serial.print("setLowAlarm=");
	Serial.println(json);
}

void TemperatureSensorManager::setHighAlarm(String json)
{
	StaticJsonBuffer<200> jsonBuffer;

	JsonObject& root = jsonBuffer.parseObject(json);

	if (!root.success()) {
		Serial.println("parse sethighAlarm failed");
		return;
	}

	String dsFamilyTempSensorId = root["dsFamilyTempSensorId"];
	int value = root["dsFamilyTempSensorResolutionId"];

	_dallas.setHighAlarmTemp(getDeviceAddress(dsFamilyTempSensorId), value);

	Serial.print("sethighAlarm=");
	Serial.println(json);
}

const uint8_t *TemperatureSensorManager::getDeviceAddress(String dsFamilyTempSensorId) {
	for (int i = 0; i < this->_sensors.size(); ++i) {
		if (this->_sensors[i].getDSFamilyTempSensorId() == dsFamilyTempSensorId) {
			return this->_sensors[i].getDeviceAddress();
		}
	}
}

String TemperatureSensorManager::getFamily(byte deviceAddress[8]){
  switch (deviceAddress[0]) {
    case DS18S20MODEL:
      return "DS18S20";
    case DS18B20MODEL:
      return "DS18B20";
    case DS1822MODEL:
      return "DS1822";
    case DS1825MODEL:
      return "DS1825";
    case DS28EA00MODEL:
      return "DS28EA00";
    default:
      return "";
  } 
}

void TemperatureSensorManager::generateNestedSensor(TemperatureSensor temperatureSensor, JsonArray& root)
{	
	JsonObject& JSONencoder = root.createNestedObject();

	JSONencoder["deviceAddress"] = temperatureSensor.getDeviceAddressStr();
	JSONencoder["epochTime"] = temperatureSensor.getEpochTime();
	JSONencoder["validFamily"] = temperatureSensor.getValidFamily();
	JSONencoder["family"] = temperatureSensor.getFamily();
	JSONencoder["isConnected"] = temperatureSensor.getConnected();
	JSONencoder["resolution"] = temperatureSensor.getResolution();
	JSONencoder["tempCelsius"] = temperatureSensor.getTempCelsius();
	JSONencoder["tempFahrenheit"] = temperatureSensor.getTempFahrenheit();
	JSONencoder["hasAlarm"] = temperatureSensor.getHasAlarm();
	JSONencoder["lowAlarm"] = temperatureSensor.getLowAlarm();
	JSONencoder["highAlarm"] = temperatureSensor.getHighAlarm();
}