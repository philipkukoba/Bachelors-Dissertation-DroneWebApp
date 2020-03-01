USE [master]
GO
/****** Object:  Database [DroneDB]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[AbsoluteGeoreferencingVariance]    Script Date: 29/02/2020 23:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbsoluteGeoreferencingVariance](
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
	[QualityReportId] [varchar](100) NULL,
 CONSTRAINT [PK_AbsoluteGeoreferencingVariance] PRIMARY KEY CLUSTERED 
(
	[AbsoluteGeolocationVariance] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AbsoluteUncertainty]    Script Date: 29/02/2020 23:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AbsoluteUncertainty](
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
	[QualityReportId] [varchar](100) NULL,
 CONSTRAINT [PK_AbsoluteUncertainty] PRIMARY KEY CLUSTERED 
(
	[UncertaintyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepartureInfo]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[DestinationInfo]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[Drone]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[DroneFlight]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[GCPError]    Script Date: 29/02/2020 23:56:47 ******/
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
	[QualityReportId] [varchar](100) NULL,
 CONSTRAINT [PK_GCPError] PRIMARY KEY CLUSTERED 
(
	[GCPErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroundControlPoints]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[Pilot]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[PointCloudXYZ]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[QualityReport]    Script Date: 29/02/2020 23:56:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QualityReport](
	[QualityReportId] [varchar](100) NOT NULL,
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
/****** Object:  Table [dbo].[RawImages]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[TFW]    Script Date: 29/02/2020 23:56:47 ******/
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
/****** Object:  Table [dbo].[Uncertainty]    Script Date: 29/02/2020 23:56:47 ******/
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
	[QualityReportId] [varchar](100) NULL,
 CONSTRAINT [PK_Uncertainty] PRIMARY KEY CLUSTERED 
(
	[UncertaintyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AbsoluteGeoreferencingVariance]  WITH CHECK ADD  CONSTRAINT [FK_AbsoluteGeoreferencingVariance_QualityReport] FOREIGN KEY([QualityReportId])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[AbsoluteGeoreferencingVariance] CHECK CONSTRAINT [FK_AbsoluteGeoreferencingVariance_QualityReport]
GO
ALTER TABLE [dbo].[AbsoluteUncertainty]  WITH CHECK ADD  CONSTRAINT [FK_AbsoluteUncertainty_QualityReport] FOREIGN KEY([QualityReportId])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[AbsoluteUncertainty] CHECK CONSTRAINT [FK_AbsoluteUncertainty_QualityReport]
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
ALTER TABLE [dbo].[Uncertainty]  WITH CHECK ADD  CONSTRAINT [FK_Uncertainty_QualityReport] FOREIGN KEY([QualityReportId])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[Uncertainty] CHECK CONSTRAINT [FK_Uncertainty_QualityReport]
GO
USE [master]
GO
ALTER DATABASE [DroneDB] SET  READ_WRITE 
GO
