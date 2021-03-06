#ifndef DeviceSerialItem_h
#define DeviceSerialItem_h

#include "Arduino.h"
#include "../ArduinoJson/ArduinoJson.h"

namespace ART
{
	class DeviceSerialItem
	{

	public:

		DeviceSerialItem(JsonObject& jsonObject);
		~DeviceSerialItem();		

	private:

		HardwareSerial*						_hardwareSerial;

		char* 								_deviceSerialId;

		bool								_enabled;

		short								_index;

		SerialMode							_serialMode;

		bool								_allowPinSwapRX;
		bool								_allowPinSwapTX;

		short								_pinRX;
		short								_pinTX;

		void								setDeviceSerialId(const char* value);
		char*								getDeviceSerialId() const;

		void								setIndex(short value);

		void								setEnabled(bool value);

		void								setSerialMode(SerialMode value);

		void								setAllowPinSwapRX(bool value);
		void								setAllowPinSwapTX(bool value);

		void								setPinRX(short value);
		void								setPinTX(short value);

		friend class						DeviceSerial;
	};
}

#endif