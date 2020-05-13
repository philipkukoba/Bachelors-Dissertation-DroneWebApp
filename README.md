# Repository van drone1

## Inhoud van deze repository

#### [Installatie](https://github.ugent.be/bp-2020/drone1/blob/master/README.md#installatie-1)

#### [Opbouw van deze repository](https://github.ugent.be/bp-2020/drone1/blob/master/README.md#opbouw-van-deze-repository-1)

#### [Gebruikershandleiding](https://github.ugent.be/bp-2020/drone1/blob/master/README.md#gebruikershandleiding-1)

#### [Meer informatie](https://github.ugent.be/bp-2020/drone1/blob/master/README.md#meer-informatie-1)

## Installatie
BELANGRIJKE OPMERKING: de executable DatCon.exe kan niet worden uitgevoerd op de virtuele server, omdat dit niet is toegelaten. Het converteren van een DAT-bestand naar een CSV-bestand (tijdens het uploaden van een DAT-bestand) is dus niet mogelijk op de server. Dit is wel gelukt op alle vier onze computers.

### Inhoud van de distributie

In de distributie van dit project bevinden zich volgende bestanden:
*	de DroneWebApp-map met alle code voor de webapplicatie;
*	het SQL-script DroneDB.sql om de databank aan te maken;
*	ChangeDataSourceName_Script.exe, een Perl-executable om Web.config aan te passen;
*	het verslag met handleidingen en documentatie.

### Installatiehandleiding voor ontwikkelaar

De installatiehandleiding is opgedeeld in vier delen:
*	Deel 1: Vereiste software
*	Deel 2: Aanmaken van de databank
*	Deel 3: Opstarten van de webapplicatie
* Deel 4: Publishen van de webapplicatie

#### Vereiste software

In dit deel installeert u alle benodigde software om de webapplicatie te laten werken op Windows 7 en hoger.
1.	Installeer **SQL Server 2019** op uw machine. U kan deze software [hier](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) downloaden op de website van Microsoft. 
*	Scrol naar beneden en kies de versie die u verkiest (Developer of Express). 
* Klik ‘Download now’. Het programma downloadt.
*	Volg na het uitvoeren van het gedownloade bestand de instructies op het scherm.
2.	Installeer **SQL Server Management Studio** (18.4) (SSMS) op uw machine. U kan deze software [hier](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?redirectedfrom=MSDN&view=sql-server-ver15) downloaden. 
*	Klik op ‘Download SQL Server Management Studio (SSMS)’. Het programma downloadt.
*	Volg na het uitvoeren van het gedownloade bestand de instructies op het scherm.
3.	Installeer **Visual Studio 2019** op uw machine. U kan deze software [hier](https://visualstudio.microsoft.com/vs/) downloaden. 
*	Klik op ‘Download Visual Studio’ en kies de Community 2019 of de Professional 2019 versie naargelang uw eigen voorkeur. Het programma downloadt.
*	Volg na het uitvoeren van het bestand de instructies op het scherm. Kies tijdens de installatie om de volgende workloads te installeren: ‘ASP.NET and web development’ en ‘Data storage and processing’.
4.	Surf naar de website van **IvyTools**. Dit kan via [deze link](http://www.ivytools.net/index.html).
*	Op deze webpagina kunt u de gratis personal license key verkrijgen die u zult nodig hebben om de IvyTools software te activeren. Klik hiervoor op ‘Click here to get your free personal license key’.
*	Kopieer deze sleutel.
*	Navigeer in de distributie naar de map ‘drone1\IvyPdf_1.62’. 
*	Voer het bestand IvyTemplateEditor.exe uit.
*	Navigeer via de balk bovenaan het programma naar ‘Help > About > Apply License Code’.
*	Plak de eerder gekopieerde sleutel in het veld en druk op OK.
*	Sluit IvyTemplateEditor af.
*	U heeft nu toegang tot de IvyParser dll-bestanden in de webapplicatie. Deze worden gebruikt bij het inlezen van een pdf.
5.	Surf naar de website van **Java**. Dit kan via [deze link](https://www.java.com/nl/).
*	Klik op deze webpagina op ‘Gratis Java-download’.
*	Scrol op de webpagina die verschijnt naar beneden tot bij de knop ‘Ga akkoord met de licentiebepalingen en start de download’, en klik op deze knop om een executable van Java te downloaden.
*	Start de executable.
*	Er verschijnt een scherm. Onderaan staat de knop ‘Install’. Deze zal de installatie automatisch starten.
*	Indien u al een of meerdere oudere versies van Java geïnstalleerd had op uw machine, komt nu de mogelijkheid om de verouderde versie te verwijderen. Indien u dit wil, kan u op ‘Uninstall’ klikken. Indien u de oude versies wil behouden kan u gewoon op ‘Next’ klikken.
*	Indien u koos voor het verwijderen van de oudere versies, komt er een scherm die samenvat welke versies verwijderd zijn. Hier kunt u gewoon op ‘Next’ klikken.
*	Na al deze stappen komt er een scherm dat bevestigt dat Java geïnstalleerd is. Klik op ‘Close’. 
*	U kan nu aan de slag met Java op uw toestel.

#### Aanmaken van de databank

In dit deel maakt u de SQL-Serverdatabank aan.
1.	Start **SQL Server Management Studio** op en verbind met uw machine. 
*	Het veld ‘Server name’ wordt automatisch ingevuld. Dit is tevens de naam die in het bestand Web.config gebruikt wordt om de brug te leggen naar de databank. Deze naam wordt automatisch ingevuld door het Perlscript in punt 9 (zie later). 
*	Klik op ‘Connect’.
2.	Ga naar het menu ‘File’ bovenaan links. 
3.	Kies ‘Open’ en ga naar ‘File’. 
4.	Navigeer in de distributie naar het script **DroneDB.sql** en open dit.
5.	Klik op ‘Execute’ om het script uit te voeren. 
6.	In het ‘Messages’-venster verschijnt  “Commands completed successfully”. U kan verifiëren dat de databank is aangemaakt met volgende stappen:
*	Klik in het ‘Object Explorer’-venster op ‘refresh’.
*	Vouw de Machinenaammap en Databasesmap open. Hierin bevindt zich nu de nieuwe database **DroneDB**. Merk op dat de Machinenaammap dezelfde naam heeft als de eerder genoteerde ‘Server name’.
7.	Een lege databank is nu aangemaakt en klaar voor gebruik.
8.	Sluit SQL Server Management Studio.
9.	In de distributie bevindt zich op het pad ‘DroneWebApp\DroneWebApp\Programs\Perl’ een bestand genaamd **ChangeDataSourceName_Script.exe**. Voer dit bestand uit om de juiste connection strings in te vullen in **Web.config**. Deze leggen de verbinding tussen de databank en de webapplicatie.


#### Opstarten van de webapplicatie met Visual Studio

1.	Voer de webapplicatie vanuit **Visual Studio** uit met F5.
2.	De allereerste keer kan een venster verschijnen dat u vraagt om het ‘IIS Express SSL certificate’ te vertrouwen. Klik ‘yes’.
3.	Er verschijnt een ‘security warning’. Klik ‘yes’.
4.	U kunt nu aan de slag met de dronewebapplicatie.

#### Publishen van de webapplicatie

1.	Installeer IIS op uw machine.
*	Druk op de Windows + R toets om het ‘Run’ venster te openen.
*	Typ ‘appwiz.cpl’ in het tekstveld en druk op enter.
*	Nu opent het ‘Programma’s en Onderdelen’-venster. Klik hier op Windows-onderdelen in- of uitschakelen.
*	Vink de ‘Internet Information Services’ check box aan en klik op OK.
2.	Installeer .NET versie 4, dit kan u hier downloaden.
*	Scrol naar beneden.
*	Klik op ‘Downloaden’ en volg de instructies op het scherm.
3.	Start SQL Server Management Studio op en verbind met uw machine.
4.	Start Visual Studio op als administrator en open het ‘DroneWebApp’-project.
5.	Klik rechts bij Solution Explorer op het project en kies ‘Publish’.
6.	Klik in het ‘Publish’-venster op Start.
7.	Kies bij ‘Pick a publish target’ voor ‘IIS, FTP, etc’ en klik op ‘Create Profile’.
*	Kies bij ‘Publish method’ voor File System
*	Navigeer bij ‘Target location’ naar de ‘wwwroot’ folder van IIS (bv C:\inetpub\wwwroot).
*	Klik op ‘Next’.
*	Selecteer bij ‘Configuration’ ‘Release’ en bij ‘File Publish Options’ ‘Delete all existing files prior to publish’.
*	Klik op ‘Save’.
8.	Klik nu op ‘Publish’.
9.	Open nu IIS Manager.
*	Druk op de Windows + R toets om het ‘Run’ venster te openen.
*	Typ ‘inetmgr’ in het tekstveld en klik ok OK.
10.	Vouw de serverknoop open bij ‘Verbindingen’ en ga bij ‘Sites’ naar ‘Default Web Site’.
11.	Klik op ‘Default Web Site’ en ga bij ‘Acties’ naar ‘Geavanceerde instellingen’.
*	Selecteer bij ‘Groep van toepassingen’ ‘DefaultAppPool’.
12.	Ga naar ‘Toepassingsgroepen’
*	Rechtsklik op ‘DefaultAppPool’ en selecteer ‘Basisinstellingen’.
*	Selecteer bij ‘.NET CLR-versie’ ‘.NET CLR-versie v4’
*	Selecteer bij ‘Beheerde pipeline-modus’ ‘Geïntegreerd’.
*	Klik op ‘OK’.
13.	Ga naar SQL Server Management Studio.
*	Open het ‘Security’-tabblad.
*	Klik rechts op ‘Logins’ en selecteer ‘New login’.
*	Geef als ‘Login name’ ‘IIS APPPOOL\DefaultAppPool’ in.
*	Ga daarna naar ‘User Mapping’ en selecteer ‘DroneDB’.
*	Selecteer als rol ‘db datareader’, ‘db datawriter’ en ‘db owner’.
*	Klik op ‘OK’.
14.	Ga terug naar ‘IIS Manager’.
*	Klik rechts op ‘DroneWebApp’.
*	Klik bij ‘toepassing beheren’ op ‘bladeren’.
15.	De website wordt nu opgestart.


## Opbouw van deze repository:

Deze repository bevat volgende mappen:
* De **DroneWebApp** map met de solution en het project;
* De map **documenten** bevat het _analyseverslag_, _de eindpresentatie_, de _groepsbesprekingen_, _informatie van de klant_, het _tussentijds verslag_, _de tussentijdse presentatie_ en informatie over de _virtuele machine_;
* De map **demo_bestanden** bevat bestanden die gebruikt kunnen worden om de webapp te testen (uploaden van files);
* De map met de **logboeken**;
* De map **meetings** met de Scrum Stand Up meetings;
* De map **verslag** met het verslag van sprint 1;
* Het **SQL-script** `DroneDB.sql` dat gebruikt wordt om de databank aan te maken.


## Gebruikershandleiding

U kan de gebruikershandleiding [hier](https://github.ugent.be/bp-2020/drone1/blob/master/verslag/Gebruikershandleiding.pdf) vinden.

## Meer informatie

Voor meer informatie kunt u terecht op de **About**-pagina van de website.
