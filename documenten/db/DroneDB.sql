USE [master]
GO
/****** Object:  Database [DroneDB]    Script Date: 04/03/2020 14:23:22 ******/
CREATE DATABASE [DroneDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DroneDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DroneDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DroneDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\DroneDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [DroneDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DroneDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DroneDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DroneDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DroneDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DroneDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DroneDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [DroneDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DroneDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DroneDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DroneDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DroneDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DroneDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DroneDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DroneDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DroneDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DroneDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DroneDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DroneDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DroneDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DroneDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DroneDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DroneDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DroneDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DroneDB] SET RECOVERY FULL 
GO
ALTER DATABASE [DroneDB] SET  MULTI_USER 
GO
ALTER DATABASE [DroneDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DroneDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DroneDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DroneDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DroneDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DroneDB', N'ON'
GO
ALTER DATABASE [DroneDB] SET QUERY_STORE = OFF
GO
USE [DroneDB]
GO
/****** Object:  Table [dbo].[AbsoluteGeolocationVariance]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbsoluteGeolocationVariance](
	[AbsoluteGeolocationVariance] [int] NOT NULL,
	[AGVMeanError_x] [float] NULL,
	[AGVMeanError_y] [float] NULL,
	[AGVMeanError_z] [float] NULL,
	[AGVSigma_x] [float] NULL,
	[AGVSigma_y] [float] NULL,
	[AGVSigma_z] [float] NULL,
	[AGVRMS_x] [float] NULL,
	[AGVRMS_y] [float] NULL,
	[AGVRMS_z] [float] NULL,
	[QualityReportId] [int] NULL,
 CONSTRAINT [PK_AbsoluteGeoreferencingVariance] PRIMARY KEY CLUSTERED 
(
	[AbsoluteGeolocationVariance] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CTRLPoints]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CTRLPoints](
	[CTRLId] [int] IDENTITY(1,1) NOT NULL,
	[CTRLName] [varchar](50) NULL,
	[X] [float] NULL,
	[Y] [float] NULL,
	[Z] [float] NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_CTRLPoints] PRIMARY KEY CLUSTERED 
(
	[CTRLId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepartureInfo]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepartureInfo](
	[DepartureInfoId] [int] NOT NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[UTCTime] [time](7) NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_DepartureInfo] PRIMARY KEY CLUSTERED 
(
	[DepartureInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestinationInfo]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestinationInfo](
	[DestinationInfoId] [int] NOT NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[UTCTime] [time](7) NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_DestinationInfo] PRIMARY KEY CLUSTERED 
(
	[DestinationInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Drone]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drone](
	[DroneId] [int] IDENTITY(1,1) NOT NULL,
	[Registration] [varchar](50) NULL,
	[DroneType] [varchar](50) NULL,
	[DroneName] [varchar](50) NULL,
 CONSTRAINT [PK_Drone] PRIMARY KEY CLUSTERED 
(
	[DroneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneAttributeValues]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneAttributeValues](
	[AttributeValueId] [int] IDENTITY(1,1) NOT NULL,
	[ACType] [varchar](50) NULL,
	[FirmwareDate] [date] NULL,
	[DateTime] [datetime] NULL,
	[BatterySN] [int] NULL,
	[GeoDeclination] [float] NULL,
	[GeoInclination] [float] NULL,
	[GeoIntensity] [float] NULL,
	[DroneId] [int] NOT NULL,
 CONSTRAINT [PK_DroneAttributeValues] PRIMARY KEY CLUSTERED 
(
	[AttributeValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneFlight]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneFlight](
	[FlightId] [int] IDENTITY(1,1) NOT NULL,
	[DroneId] [int] NOT NULL,
	[Location] [varchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[PilotName] [varchar](50) NULL,
	[hasTFW] [bit] NOT NULL,
	[hasGCPs] [bit] NOT NULL,
	[hasDepInfo] [bit] NOT NULL,
	[hasDestInfo] [bit] NOT NULL,
	[hasQR] [bit] NOT NULL,
	[hasXYZ] [bit] NOT NULL,
 CONSTRAINT [PK_DroneFlight] PRIMARY KEY CLUSTERED 
(
	[FlightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneGPS]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneGPS](
	[GPSId] [int] IDENTITY(1,1) NOT NULL,
	[Long] [float] NULL,
	[Lat] [float] NULL,
	[Date] [int] NULL,
	[Time] [int] NULL,
	[DateTimeStamp] [datetime] NULL,
	[HeightMSL] [float] NULL,
	[HDOP] [float] NULL,
	[PDOP] [float] NULL,
	[SAcc] [float] NULL,
	[NumGPS] [int] NULL,
	[NumGLNAS] [int] NULL,
	[NumSV] [int] NULL,
	[VelN] [float] NULL,
	[VelE] [float] NULL,
	[VelD] [float] NULL,
	[DroneLogId] [int] NOT NULL,
 CONSTRAINT [PK_DroneGPS] PRIMARY KEY CLUSTERED 
(
	[GPSId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneIMU_ATTI]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneIMU_ATTI](
	[IMU_ATTI_Id] [int] IDENTITY(1,1) NOT NULL,
	[GPS_H] [float] NULL,
	[Roll] [float] NULL,
	[Pitch] [float] NULL,
	[Yaw] [float] NULL,
	[DistanceTravelled] [float] NULL,
	[MagDirectionOfTravel] [float] NULL,
	[TrueDirectionOfTravel] [float] NULL,
	[Temperature] [float] NULL,
	[DroneLogId] [int] NOT NULL,
 CONSTRAINT [PK_DroneIMU_ATTI] PRIMARY KEY CLUSTERED 
(
	[IMU_ATTI_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneLog]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneLog](
	[DroneLogId] [int] IDENTITY(1,1) NOT NULL,
	[Tick_no] [bigint] NULL,
	[OffsetTime] [float] NULL,
	[FlightTime] [int] NULL,
	[NavHealth] [int] NULL,
	[GeneralRelHeight] [float] NULL,
	[FlyCState] [varchar](50) NULL,
	[ControllerCTRLMode] [varchar](50) NULL,
	[BatteryStatus] [varchar](50) NULL,
	[SmartBattGoHome] [int] NULL,
	[SmartBattLand] [int] NULL,
	[NonGPSCause] [varchar](50) NULL,
	[CompassError] [varchar](50) NULL,
	[ConnectedToRC] [varchar](50) NULL,
	[BatteryLowVoltage] [varchar](50) NULL,
	[GPSUsed] [varchar](50) NULL,
	[DroneId] [int] NOT NULL,
 CONSTRAINT [PK_DroneLog] PRIMARY KEY CLUSTERED 
(
	[DroneLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneMotor]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneMotor](
	[MotorId] [int] IDENTITY(1,1) NOT NULL,
	[CurrentRFront] [float] NULL,
	[CurrentLFront] [float] NULL,
	[CurrentLBack] [float] NULL,
	[CurrentRBack] [float] NULL,
	[DroneLogId] [int] NOT NULL,
 CONSTRAINT [PK_DroneMotor] PRIMARY KEY CLUSTERED 
(
	[MotorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneOA]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneOA](
	[OAId] [int] IDENTITY(1,1) NOT NULL,
	[AvoidObst] [varchar](50) NULL,
	[AirportLimit] [varchar](50) NULL,
	[GroundForceLanding] [varchar](50) NULL,
	[VertAirportLimit] [varchar](50) NULL,
	[DroneLogId] [int] NOT NULL,
 CONSTRAINT [PK_DroneOA] PRIMARY KEY CLUSTERED 
(
	[OAId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneRC]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneRC](
	[RCId] [int] IDENTITY(1,1) NOT NULL,
	[FailSafe] [varchar](50) NULL,
	[DataLost] [varchar](50) NULL,
	[AppLost] [varchar](50) NULL,
	[ModeSwitch] [char](1) NULL,
	[DroneLogId] [int] NOT NULL,
 CONSTRAINT [PK_DroneRC] PRIMARY KEY CLUSTERED 
(
	[RCId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneRTKData]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneRTKData](
	[RTKDataId] [int] IDENTITY(1,1) NOT NULL,
	[Date] [int] NULL,
	[Time] [int] NULL,
	[LonP] [float] NULL,
	[LatP] [float] NULL,
	[HmslP] [float] NULL,
	[LonS] [float] NULL,
	[LatS] [float] NULL,
	[HmslS] [float] NULL,
	[VelN] [float] NULL,
	[VelE] [float] NULL,
	[VelD] [float] NULL,
	[HDOP] [float] NULL,
	[DroneLogId] [int] NOT NULL,
 CONSTRAINT [PK_DroneRTKData] PRIMARY KEY CLUSTERED 
(
	[RTKDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GCPError]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GCPError](
	[GCPErrorId] [int] NOT NULL,
	[GCPMeanError_x] [float] NULL,
	[GCPMeanError_y] [float] NULL,
	[GCPMeanError_z] [float] NULL,
	[GCPSigma_x] [float] NULL,
	[GCPSigma_y] [float] NULL,
	[GCPSigma_z] [float] NULL,
	[GCPRMS_x] [float] NULL,
	[GCPRMS_y] [float] NULL,
	[GCPRMS_z] [float] NULL,
	[QualityReportId] [int] NULL,
 CONSTRAINT [PK_GCPError] PRIMARY KEY CLUSTERED 
(
	[GCPErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroundControlPoints]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroundControlPoints](
	[GCPId] [varchar](50) NOT NULL,
	[X] [float] NULL,
	[Y] [float] NULL,
	[Z] [float] NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_GroundControlPoints] PRIMARY KEY CLUSTERED 
(
	[GCPId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pilot]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pilot](
	[PilotName] [varchar](50) NOT NULL,
	[Street] [varchar](50) NULL,
	[ZIP] [int] NULL,
	[City] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[Phone] [varchar](50) NULL,
	[LicenseNo] [int] NULL,
	[Email] [varchar](50) NULL,
	[EmergencyPhone] [varchar](50) NULL,
 CONSTRAINT [PK_Pilot] PRIMARY KEY CLUSTERED 
(
	[PilotName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointCloudXYZ]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointCloudXYZ](
	[PointCloudXYZId] [int] IDENTITY(1,1) NOT NULL,
	[X] [float] NULL,
	[Y] [float] NULL,
	[Z] [float] NULL,
	[Red] [int] NULL,
	[Green] [int] NULL,
	[Blue] [int] NULL,
	[Intensity] [float] NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_PointCloudXYZ] PRIMARY KEY CLUSTERED 
(
	[PointCloudXYZId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QualityReport]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QualityReport](
	[QualityReportId] [int] IDENTITY(1,1) NOT NULL,
	[FlightId] [int] NULL,
	[Processed] [datetime] NULL,
	[CameraModelName] [varchar](50) NULL,
	[AverageGSD] [float] NULL,
	[AreaCovered] [float] NULL,
	[InitialProcessingTime] [time](7) NULL,
	[DatasetAmountCalibrated] [int] NULL,
	[DatasetAmountTotal] [int] NULL,
	[DatasetStatus] [char](1) NULL,
	[CameraOptimizationAmount] [float] NULL,
	[CameraOptimizationStatus] [char](1) NULL,
	[GeoreferencingGCPs] [int] NULL,
	[GeoreferencingRMS] [float] NULL,
	[GeoreferencingStatus] [char](1) NULL,
	[CPU] [varchar](50) NULL,
	[RAM] [int] NULL,
	[GPU] [varchar](50) NULL,
	[OS] [varchar](100) NULL,
	[ImageCoordinateSystem] [varchar](100) NULL,
	[GCPCoordinateSystem] [varchar](100) NULL,
	[OutputCoordinateSystem] [varchar](100) NULL,
	[GeneratedTiles] [int] NULL,
	[DensifiedPoints3D] [int] NULL,
	[AverageDensity] [float] NULL,
 CONSTRAINT [PK_QualityReport] PRIMARY KEY CLUSTERED 
(
	[QualityReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RawImages]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RawImages](
	[ImageName] [varchar](50) NOT NULL,
	[Image] [image] NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_RawImages] PRIMARY KEY CLUSTERED 
(
	[ImageName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TFW]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TFW](
	[TFWId] [int] IDENTITY(1,1) NOT NULL,
	[xScale_X] [float] NULL,
	[yRotationTerm_X] [float] NULL,
	[TranslationTerm_X] [float] NULL,
	[xRotationTerm_Y] [float] NULL,
	[yNegativeScale_Y] [float] NULL,
	[TranslationTerm_Y] [float] NULL,
	[FlightId] [int] NULL,
 CONSTRAINT [PK_TFW] PRIMARY KEY CLUSTERED 
(
	[TFWId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Uncertainty]    Script Date: 04/03/2020 14:23:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Uncertainty](
	[UncertaintyId] [int] NOT NULL,
	[AMU_x] [float] NULL,
	[AMU_y] [float] NULL,
	[AMU_z] [float] NULL,
	[ASU_x] [float] NULL,
	[ASU_y] [float] NULL,
	[ASU_z] [float] NULL,
	[RMU_x] [float] NULL,
	[RMU_y] [float] NULL,
	[RMU_z] [float] NULL,
	[RSU_x] [float] NULL,
	[RSU_y] [float] NULL,
	[RSU_z] [float] NULL,
	[QualityReportId] [int] NULL,
 CONSTRAINT [PK_AbsoluteUncertainty] PRIMARY KEY CLUSTERED 
(
	[UncertaintyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbsoluteGeolocationVariance]  WITH CHECK ADD  CONSTRAINT [FK_AbsoluteGeoreferencingVariance_QualityReport] FOREIGN KEY([QualityReportId])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[AbsoluteGeolocationVariance] CHECK CONSTRAINT [FK_AbsoluteGeoreferencingVariance_QualityReport]
GO
ALTER TABLE [dbo].[CTRLPoints]  WITH CHECK ADD  CONSTRAINT [FK_CTRLPoints_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[CTRLPoints] CHECK CONSTRAINT [FK_CTRLPoints_DroneFlight]
GO
ALTER TABLE [dbo].[DepartureInfo]  WITH CHECK ADD  CONSTRAINT [FK_DepartureInfo_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[DepartureInfo] CHECK CONSTRAINT [FK_DepartureInfo_DroneFlight]
GO
ALTER TABLE [dbo].[DestinationInfo]  WITH CHECK ADD  CONSTRAINT [FK_DestinationInfo_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[DestinationInfo] CHECK CONSTRAINT [FK_DestinationInfo_DroneFlight]
GO
ALTER TABLE [dbo].[DroneAttributeValues]  WITH CHECK ADD  CONSTRAINT [FK_DroneAttributeValues_Drone] FOREIGN KEY([DroneId])
REFERENCES [dbo].[Drone] ([DroneId])
GO
ALTER TABLE [dbo].[DroneAttributeValues] CHECK CONSTRAINT [FK_DroneAttributeValues_Drone]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_Drone] FOREIGN KEY([DroneId])
REFERENCES [dbo].[Drone] ([DroneId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_Drone]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_Pilot] FOREIGN KEY([PilotName])
REFERENCES [dbo].[Pilot] ([PilotName])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_Pilot]
GO
ALTER TABLE [dbo].[DroneGPS]  WITH CHECK ADD  CONSTRAINT [FK_DroneGPS_DroneLog] FOREIGN KEY([DroneLogId])
REFERENCES [dbo].[DroneLog] ([DroneLogId])
GO
ALTER TABLE [dbo].[DroneGPS] CHECK CONSTRAINT [FK_DroneGPS_DroneLog]
GO
ALTER TABLE [dbo].[DroneIMU_ATTI]  WITH CHECK ADD  CONSTRAINT [FK_DroneIMU_ATTI_DroneLog] FOREIGN KEY([DroneLogId])
REFERENCES [dbo].[DroneLog] ([DroneLogId])
GO
ALTER TABLE [dbo].[DroneIMU_ATTI] CHECK CONSTRAINT [FK_DroneIMU_ATTI_DroneLog]
GO
ALTER TABLE [dbo].[DroneLog]  WITH CHECK ADD  CONSTRAINT [FK_DroneLog_Drone] FOREIGN KEY([DroneId])
REFERENCES [dbo].[Drone] ([DroneId])
GO
ALTER TABLE [dbo].[DroneLog] CHECK CONSTRAINT [FK_DroneLog_Drone]
GO
ALTER TABLE [dbo].[DroneMotor]  WITH CHECK ADD  CONSTRAINT [FK_DroneMotor_DroneLog] FOREIGN KEY([DroneLogId])
REFERENCES [dbo].[DroneLog] ([DroneLogId])
GO
ALTER TABLE [dbo].[DroneMotor] CHECK CONSTRAINT [FK_DroneMotor_DroneLog]
GO
ALTER TABLE [dbo].[DroneOA]  WITH CHECK ADD  CONSTRAINT [FK_DroneOA_DroneLog] FOREIGN KEY([DroneLogId])
REFERENCES [dbo].[DroneLog] ([DroneLogId])
GO
ALTER TABLE [dbo].[DroneOA] CHECK CONSTRAINT [FK_DroneOA_DroneLog]
GO
ALTER TABLE [dbo].[DroneRC]  WITH CHECK ADD  CONSTRAINT [FK_DroneRC_DroneLog] FOREIGN KEY([DroneLogId])
REFERENCES [dbo].[DroneLog] ([DroneLogId])
GO
ALTER TABLE [dbo].[DroneRC] CHECK CONSTRAINT [FK_DroneRC_DroneLog]
GO
ALTER TABLE [dbo].[DroneRTKData]  WITH CHECK ADD  CONSTRAINT [FK_DroneRTKData_DroneLog] FOREIGN KEY([DroneLogId])
REFERENCES [dbo].[DroneLog] ([DroneLogId])
GO
ALTER TABLE [dbo].[DroneRTKData] CHECK CONSTRAINT [FK_DroneRTKData_DroneLog]
GO
ALTER TABLE [dbo].[GCPError]  WITH CHECK ADD  CONSTRAINT [FK_GCPError_QualityReport] FOREIGN KEY([QualityReportId])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[GCPError] CHECK CONSTRAINT [FK_GCPError_QualityReport]
GO
ALTER TABLE [dbo].[GroundControlPoints]  WITH CHECK ADD  CONSTRAINT [FK_GroundControlPoints_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[GroundControlPoints] CHECK CONSTRAINT [FK_GroundControlPoints_DroneFlight]
GO
ALTER TABLE [dbo].[PointCloudXYZ]  WITH CHECK ADD  CONSTRAINT [FK_PointCloudXYZ_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[PointCloudXYZ] CHECK CONSTRAINT [FK_PointCloudXYZ_DroneFlight]
GO
ALTER TABLE [dbo].[QualityReport]  WITH CHECK ADD  CONSTRAINT [FK_QualityReport_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[QualityReport] CHECK CONSTRAINT [FK_QualityReport_DroneFlight]
GO
ALTER TABLE [dbo].[RawImages]  WITH CHECK ADD  CONSTRAINT [FK_RawImages_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[RawImages] CHECK CONSTRAINT [FK_RawImages_DroneFlight]
GO
ALTER TABLE [dbo].[TFW]  WITH CHECK ADD  CONSTRAINT [FK_TFW_DroneFlight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[DroneFlight] ([FlightId])
GO
ALTER TABLE [dbo].[TFW] CHECK CONSTRAINT [FK_TFW_DroneFlight]
GO
ALTER TABLE [dbo].[Uncertainty]  WITH CHECK ADD  CONSTRAINT [FK_AbsoluteUncertainty_QualityReport] FOREIGN KEY([QualityReportId])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[Uncertainty] CHECK CONSTRAINT [FK_AbsoluteUncertainty_QualityReport]
GO
USE [master]
GO
ALTER DATABASE [DroneDB] SET  READ_WRITE 
GO
