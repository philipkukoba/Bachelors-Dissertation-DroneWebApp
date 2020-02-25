USE [DroneDB]
GO
/****** Object:  Table [dbo].[DepartureInfo]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepartureInfo](
	[DepartureInfoId] [int] NOT NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[UTCTime] [time](7) NULL,
 CONSTRAINT [PK_DepartureInfo] PRIMARY KEY CLUSTERED 
(
	[DepartureInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestinationInfo]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestinationInfo](
	[DestinationInfoId] [int] NOT NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[UTCTime] [time](7) NULL,
 CONSTRAINT [PK_DestinationInfo] PRIMARY KEY CLUSTERED 
(
	[DestinationInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Drone]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drone](
	[DroneId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Drone] PRIMARY KEY CLUSTERED 
(
	[DroneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DroneFlight]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneFlight](
	[FlightId] [varchar](50) NOT NULL,
	[DroneId] [int] NULL,
	[GroundControlPoints] [varchar](50) NULL,
	[QualityReport] [int] NULL,
	[TFW] [int] NULL,
	[RawImages] [varchar](50) NULL,
	[PointCloudXYZId] [int] NULL,
	[Date] [date] NULL,
	[PilotName] [varchar](50) NULL,
	[Registration] [varchar](50) NULL,
	[DepartureInfo] [int] NULL,
	[DestinationInfo] [int] NULL,
 CONSTRAINT [PK_DroneFlight] PRIMARY KEY CLUSTERED 
(
	[FlightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroundControlPoints]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroundControlPoints](
	[GCPId] [varchar](50) NOT NULL,
	[X] [float] NULL,
	[Y] [float] NULL,
	[Z] [float] NULL,
 CONSTRAINT [PK_GroundControlPoints] PRIMARY KEY CLUSTERED 
(
	[GCPId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pilot]    Script Date: 25/02/2020 15:23:07 ******/
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
/****** Object:  Table [dbo].[PointCloudXYZ]    Script Date: 25/02/2020 15:23:07 ******/
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
	[Intensity] [int] NULL,
 CONSTRAINT [PK_PointCloudXYZ] PRIMARY KEY CLUSTERED 
(
	[PointCloudXYZId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QualityReport]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QualityReport](
	[QualityReportId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_QualityReport] PRIMARY KEY CLUSTERED 
(
	[QualityReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RawImages]    Script Date: 25/02/2020 15:23:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RawImages](
	[ImageName] [varchar](50) NOT NULL,
	[Image] [image] NULL,
 CONSTRAINT [PK_RawImages] PRIMARY KEY CLUSTERED 
(
	[ImageName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TFW]    Script Date: 25/02/2020 15:23:07 ******/
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
 CONSTRAINT [PK_TFW] PRIMARY KEY CLUSTERED 
(
	[TFWId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_DepartureInfo] FOREIGN KEY([DepartureInfo])
REFERENCES [dbo].[DepartureInfo] ([DepartureInfoId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_DepartureInfo]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_DestinationInfo] FOREIGN KEY([DestinationInfo])
REFERENCES [dbo].[DestinationInfo] ([DestinationInfoId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_DestinationInfo]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_Drone] FOREIGN KEY([DroneId])
REFERENCES [dbo].[Drone] ([DroneId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_Drone]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_GroundControlPoints] FOREIGN KEY([GroundControlPoints])
REFERENCES [dbo].[GroundControlPoints] ([GCPId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_GroundControlPoints]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_Pilot] FOREIGN KEY([PilotName])
REFERENCES [dbo].[Pilot] ([PilotName])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_Pilot]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_PointCloudXYZ] FOREIGN KEY([PointCloudXYZId])
REFERENCES [dbo].[PointCloudXYZ] ([PointCloudXYZId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_PointCloudXYZ]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_QualityReport] FOREIGN KEY([QualityReport])
REFERENCES [dbo].[QualityReport] ([QualityReportId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_QualityReport]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_RawImages] FOREIGN KEY([RawImages])
REFERENCES [dbo].[RawImages] ([ImageName])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_RawImages]
GO
ALTER TABLE [dbo].[DroneFlight]  WITH CHECK ADD  CONSTRAINT [FK_DroneFlight_TFW] FOREIGN KEY([TFW])
REFERENCES [dbo].[TFW] ([TFWId])
GO
ALTER TABLE [dbo].[DroneFlight] CHECK CONSTRAINT [FK_DroneFlight_TFW]
GO
