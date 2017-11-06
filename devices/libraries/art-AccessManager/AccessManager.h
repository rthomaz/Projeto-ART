#ifndef AccessManager_h
#define AccessManager_h

#include "Arduino.h"
#include "ArduinoJson.h"
#include "DebugManager.h"
#include "WiFiManager.h"
#include "ESP8266HTTPClient.h"

class BrokerSettings {
  public:

    BrokerSettings(String host, int port, String user, String pwd);

    String								getHost();
	int									getPort();
	String								getUser();
	String								getPwd();	
	
  private:
    
	String								_host;
	int									_port;
	String								_user;
	String								_pwd;

    friend class AccessManager;
};

class NTPSettings {
  public:

    NTPSettings(String host, int port, int updateInterval, int displayTimeOffset);

    String								getHost();
	int									getPort();
	int									getUpdateInterval();
	int									getDisplayTimeOffset();
	
  private:
    
	String								_host;
	int									_port;
	int									_updateInterval;
	int									_displayTimeOffset;

    friend class AccessManager;
};

class AccessManager
{
  public:
  
    AccessManager(DebugManager& debugManager, WiFiManager& wifiManager, String host, uint16_t port, String uri = "/");
		
	void								begin();
	
	void								autoInitialize();
	
	bool								initialized();
	
	BrokerSettings*						getBrokerSettings();
	NTPSettings*						getNTPSettings();
	
	String								getHardwareId();	
	String								getHardwareInApplicationId();	
	
	void								insertInApplication(String json);	
	void								deleteFromApplication();	
	
  private:			
			
	DebugManager*          				_debugManager;	
	WiFiManager*          				_wifiManager;

	bool 								_initialized = false;
	
	String 								_host;
	uint16_t 							_port;
	String 								_uri;
	
	int									_chipId;
	int									_flashChipId;
	String								_macAddress;

	BrokerSettings*						_brokerSettings;
	NTPSettings*						_ntpSettings;
	
	String								_hardwareId = "";
	String								_hardwareInApplicationId = "";
	
};

#endif
