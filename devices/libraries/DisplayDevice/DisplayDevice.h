#ifndef DisplayDevice_h
#define DisplayDevice_h

#include "DisplayDeviceBinary.h"
#include "DisplayDeviceMQ.h"
#include "DisplayDeviceWiFiAccess.h"

#include "Arduino.h"
#include "Adafruit_SSD1306.h"

namespace ART
{
	class ESPDevice;

	class DisplayDevice
	{

	public:

		DisplayDevice(ESPDevice* espDevice);
		~DisplayDevice();

		static void								create(DisplayDevice* (&displayDevice), ESPDevice* espDevice);

		void									begin();

		ESPDevice *								getESPDevice();

		DisplayDeviceBinary*					getDisplayDeviceBinary();
		DisplayDeviceMQ*						getDisplayDeviceMQ();
		DisplayDeviceWiFiAccess*				getDisplayDeviceWiFiAccess();

		Adafruit_SSD1306						display;

	private:

		ESPDevice *								_espDevice;

		DisplayDeviceBinary *					_displayDeviceBinary;
		DisplayDeviceMQ *						_displayDeviceMQ;
		DisplayDeviceWiFiAccess *				_displayDeviceWiFiAccess;

	};
}

#endif