# Repository van drone1

## Opbouw van deze repository:

Deze repository bevat volgende mappen:
* De map **documenten** bevat het _analyseverslag_, de _groepsbesprekingen_, _informatie van de klant_, het _tussentijds verslag_ en informatie over de _virtuele machine_;
* De **DroneApp** map met de solution en het project;
* De map met de **logboeken**;
* Het SQL-script `DroneDB.sql` dat gebruikt wordt om de databank aan te maken.

## Prerequisites:
* Installeer SQL Server 2019 (Developer editie) op uw machine. 
  
  [Klik hier om SQL Server te downloaden en scroll naar beneden tot u de Developer versie download ziet.](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* Installeer SQL Server Management Studio (18.4) (SSMS) op uw machine. 
  
  [Klik hier om SSMS te downloaden.](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?redirectedfrom=MSDN&view=sql-server-ver15)
 
* Installeer Visual Studio 2019 op uw machine. Bij installatie kiest u om de volgende Workloads te installeren: _ASP.NET and web development_ en _Data storage and processing_. [Klik hier om Visual Studio te downloaden.](https://visualstudio.microsoft.com/downloads/)

## Maak de databank aan:
1. Start SQL Server Management Studio op en verbind met uw machine. Het veld _Server name_ wordt automatisch ingevuld. Hou deze naam bij, want u heeft deze zometeen nodig. Klik op **Connect**.
2. Klik op het **File** menu bovenaan links. Kies **Open>File**. Navigeer naar het script _DroneDB.sql_ en open dit.
3. Klik op **Execute** om het script uit te voeren. In het **Messages** venster verschijnt _Commands completed successfully_. 
4. Klik in het **Object Explorer** venster op refresh en vouw de **Machinenaam-map** en **Databases-map** open. Hierin bevindt zich nu de nieuwe database **DroneDB**. Merk op dat de Machinenaam-map dezelfde naam heeft als uw eerder genoteerde _Server name_.
5. Sluit SQL Server Management Studio.

## Start de applicatie op:
1. Start Visual Studio op en open de **DroneWebApp solution**.
2. Navigeer in **Solution Explorer** naar **Web.config**, helemaal onderaan de mappenstructuur. Dubbelklik om dit te openen.
3. In de `<connectionStrings>` tag verandert u in de tag `<add>` het attribuut _data source_ naar:  
`data source=UW_SERVER_NAME`.
4. Sla dit bestand op (**Save**).
5. Verander van **Debug** naar **Release** en voer de webapplicatie uit met **F5**.
6. De allereerste keer kan een venster verschijnen dat u vraagt om het **IIS Express SSL certificate** te vertrouwen. Klik _yes_.
7. Er verschijnt een **Security warning**. Klik _yes_.
8. U kunt nu aan de slag met de dronewebapplicatie.

## Werking

De webapplicatie heeft verscheidene functionaliteiten:
* U kunt dronevluchten, drones en piloten aanmaken, bekijken, aanpassen en verwijderen.
* U kunt documenten toevoegen aan dronevluchten met de uploadknop.
* Een documentenknop is rood wanneer dit document nog niet geüpload is voor deze vlucht en wordt groen indien dit wel het geval is.
* De documentknoppen zijn aanklikbaar en laten u toe om de bijhorende informatie in detail te bekijken.
* De XYZ en Drone Log bestanden zijn zeer groot. Het heeft geen toegevoegde waarde om deze te kunnen bekijken; daarom zijn ze niet aanklikbaar.

## Meer informatie

Voor meer informatie kunt u terecht op de **About** pagina van de website.
