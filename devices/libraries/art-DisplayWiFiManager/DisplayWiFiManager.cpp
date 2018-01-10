#include "DisplayWiFiManager.h"

using namespace ART;

DisplayWiFiManager::DisplayWiFiManager(DisplayManager& displayManager, ESPDevice& espDevice)
{
	this->_displayManager = &displayManager;
	this->_espDevice = &espDevice;
	
	this->_startConfigPortalCallback = std::bind(&DisplayWiFiManager::startConfigPortalCallback, this);	
	this->_captivePortalCallback = [=](String ip) { this->captivePortalCallback(ip); };		
	this->_successConfigPortalCallback = std::bind(&DisplayWiFiManager::successConfigPortalCallback, this);
	this->_failedConfigPortalCallback = [=](int connectionResult) { this->failedConfigPortalCallback(connectionResult); };		
	this->_connectingConfigPortalCallback = std::bind(&DisplayWiFiManager::connectingConfigPortalCallback, this);
		
	this->_espDevice->getDeviceWiFi()->setStartConfigPortalCallback(this->_startConfigPortalCallback);
	this->_espDevice->getDeviceWiFi()->setCaptivePortalCallback(this->_captivePortalCallback);
	this->_espDevice->getDeviceWiFi()->setSuccessConfigPortalCallback(this->_successConfigPortalCallback);    
	this->_espDevice->getDeviceWiFi()->setFailedConfigPortalCallback(this->_failedConfigPortalCallback);    
	this->_espDevice->getDeviceWiFi()->setConnectingConfigPortalCallback(this->_connectingConfigPortalCallback); 
}

DisplayWiFiManager::~DisplayWiFiManager()
{
}

void DisplayWiFiManager::printSignal(){
	
	int quality = this->_espDevice->getDeviceWiFi()->getQuality();
    int barSignal = this->_espDevice->getDeviceWiFi()->convertQualitytToBarsSignal(quality);
	
    if(this->_espDevice->getDeviceWiFi()->isConnected())
      this->printConnectedSignal(106, 0, 4, 2, barSignal);
    else
      this->printNoConnectedSignal(106, 0, 4, 2);  
}

void DisplayWiFiManager::printConnectedSignal(int x, int y, int barWidth, int margin, int barSignal){
	
	int barsCount = 4;
	int barHeight = 4;

	for (int i = 0; i < 4; i++)
	{
		int currentX = x + (barWidth * i) + (margin * i);
		int currentY = (y + (barsCount - i - 1) * barHeight);
		int currentHeight = 0;

		currentHeight = barHeight * (i + 1);
		
		if (barSignal <= i)
		{
			this->_displayManager->display.drawRect(currentX, currentY, barWidth, currentHeight, WHITE);
		}
		else
		{
			this->_displayManager->display.fillRect(currentX, currentY, barWidth, currentHeight, WHITE);
		}
	}
}

void DisplayWiFiManager::printNoConnectedSignal(int x, int y, int barWidth, int margin){
	
	int barsCount = 4;
	int barHeight = 4;

	for (int i = 0; i < 4; i++)
	{
		int currentX = x + (barWidth * i) + (margin * i);
		int currentY = (y + (barsCount - i - 1) * barHeight);
		int currentHeight = 0;

		currentHeight = barHeight * (i + 1);

		this->_displayManager->display.drawRect(currentX, currentY, barWidth, currentHeight, WHITE);
	}
	
	this->_displayManager->display.setFont();
    this->_displayManager->display.setTextSize(1);
	this->_displayManager->display.setTextColor(BLACK, WHITE);
    this->_displayManager->display.setCursor(x + 15, y + 8);     
	this->_displayManager->display.setTextWrap(false);  	
	this->_displayManager->display.println("X");
}

void DisplayWiFiManager::printPortalHeaderInDisplay(String title)
{  
	this->_displayManager->display.setFont();
	this->_displayManager->display.setTextSize(2);  
	this->_displayManager->display.setCursor(0, 0);       
	this->_displayManager->display.setTextWrap(false);  
	this->_displayManager->display.setTextColor(BLACK, WHITE);
	this->_displayManager->display.println(title);
	this->_displayManager->display.display();
	this->_displayManager->display.setTextColor(WHITE);
	this->_displayManager->display.setTextSize(1);  
}

void DisplayWiFiManager::showEnteringSetup()
{
	this->_displayManager->display.stopscroll();

	this->_displayManager->display.clearDisplay();
	this->_displayManager->display.setTextSize(2);
	this->_displayManager->display.setTextColor(WHITE);
	this->_displayManager->display.setCursor(0, 0);       

	this->_displayManager->display.setFont();

	this->_displayManager->display.println(" entrando");
	this->_displayManager->display.println(" no setup");
	this->_displayManager->display.println(" do  wifi");

	this->_displayManager->display.display();

	delay(400);

	this->_displayManager->display.print(" ");  
	for (int i=0; i <= 6; i++) {
		this->_displayManager->display.print(".");  
		this->_displayManager->display.display();
		delay(400);
	} 
}

void DisplayWiFiManager::showWiFiConect()
{
	String configPortalSSID = this->_espDevice->getDeviceWiFi()->getConfigPortalSSID();
	String configPortalPwd = this->_espDevice->getDeviceWiFi()->getConfigPortalPwd();

	this->_displayManager->display.clearDisplay();

	printPortalHeaderInDisplay("  Conecte  ");

	this->_displayManager->display.println();
	this->_displayManager->display.println();
	this->_displayManager->display.setFont(&FreeSansBold9pt7b);
	this->_displayManager->display.setTextSize(1);  
	this->_displayManager->display.print("ssid:  ");
	this->_displayManager->display.println(configPortalSSID);  
	this->_displayManager->display.print("pwd: ");  
	this->_displayManager->display.setTextWrap(false);
	this->_displayManager->display.print(configPortalPwd);    

	this->_displayManager->display.display();
}

void DisplayWiFiManager::startConfigPortalCallback () {
	this->_firstTimecaptivePortalCallback = true;  
	this->showEnteringSetup();  
	this->showWiFiConect();  
}

void DisplayWiFiManager::captivePortalCallback (String ip) {

	this->_displayManager->display.stopscroll();

	if(!this->_firstTimecaptivePortalCallback){
		return;
	}

	this->_firstTimecaptivePortalCallback = false;

	this->_displayManager->display.clearDisplay();

	this->printPortalHeaderInDisplay("  Acesse    ");

	this->_displayManager->display.println();
	this->_displayManager->display.println();
	this->_displayManager->display.println();
	this->_displayManager->display.setFont(&FreeSansBold9pt7b);
	this->_displayManager->display.setTextSize(1);  
	this->_displayManager->display.setTextWrap(false);
	this->_displayManager->display.print("  http://");
	this->_displayManager->display.println(ip);    

	this->_displayManager->display.display();  
}

void DisplayWiFiManager::successConfigPortalCallback () {  

	this->_displayManager->display.stopscroll();

	String ssid = this->_espDevice->getDeviceWiFi()->getSSID();

	this->_displayManager->display.clearDisplay();

	this->printPortalHeaderInDisplay("  Acesso    ");

	this->_displayManager->display.println();
	this->_displayManager->display.println();
	this->_displayManager->display.setFont(&FreeSansBold9pt7b);
	this->_displayManager->display.setTextSize(1);  
	this->_displayManager->display.setTextWrap(false);
	this->_displayManager->display.println("Conectado a");
	this->_displayManager->display.print(ssid);
	this->_displayManager->display.print("!");
	this->_displayManager->display.display();  

	delay(4000);
}

void DisplayWiFiManager::failedConfigPortalCallback (int connectionResult) {  

	this->_displayManager->display.stopscroll();

	this->_displayManager->display.clearDisplay();

	this->printPortalHeaderInDisplay("  Acesso    ");

	if(connectionResult == WL_CONNECT_FAILED){
		this->_displayManager->display.println();
		this->_displayManager->display.println();
		this->_displayManager->display.setFont(&FreeSansBold9pt7b);
		this->_displayManager->display.setTextSize(1);  
		this->_displayManager->display.setTextWrap(false);
		this->_displayManager->display.println("   Ops! falha");
		this->_displayManager->display.println("  na tentativa");
	}

	this->_displayManager->display.display();    

	bool invertDisplay = false;
	for (int i=0; i <= 10; i++) {
		this->_displayManager->display.invertDisplay(invertDisplay);
		invertDisplay = !invertDisplay;
		delay(500);
	}

	this->_firstTimecaptivePortalCallback = true;

	this->showWiFiConect();  
  
}

void DisplayWiFiManager::connectingConfigPortalCallback () {  

	this->_displayManager->display.stopscroll();

	String ssid = this->_espDevice->getDeviceWiFi()->getSSID();

	this->_displayManager->display.clearDisplay();

	this->printPortalHeaderInDisplay("  Acesso    ");

	this->_displayManager->display.setCursor(0, 27);       

	this->_displayManager->display.setFont(&FreeSansBold9pt7b);
	this->_displayManager->display.setTextSize(1);  
	this->_displayManager->display.setTextWrap(false);  
	this->_displayManager->display.println(" Conectando a");

	this->_displayManager->display.print(" ");
	this->_displayManager->display.println(ssid);

	this->_displayManager->display.display();    

	// progress  
	this->_displayManager->display.setCursor(0, 63);       
	this->_displayManager->display.setTextWrap(false);  
	this->_displayManager->display.println(".... .... .... .... .... .... .... .... .... .... .... ....");  
	this->_displayManager->display.display();
	this->_displayManager->display.startscrollleft(0x07, 0x0F);  
}
