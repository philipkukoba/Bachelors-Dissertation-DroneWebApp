USE [DroneDB]
GO
/****** Object:  Table [dbo].[DroneFlight]    Script Date: 25/02/2020 12:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DroneFlight](
	[FlightId] [int] NOT NULL,
	[DroneId] [int] NULL,
	[GroundControlPoints] [int] NULL,
	[QualityReport] [int] NULL,
	[TFW] [int] NULL,
	[RawImages] [varchar](50) NULL,
	[PointCloudXYZId] [int] NULL,
	[PilotName] [varchar](50) NULL,
	[DroneLogbook] [int] NULL,
 CONSTRAINT [PK_DroneFlight] PRIMARY KEY CLUSTERED 
(
	[FlightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PointCloudXYZ]    Script Date: 25/02/2020 12:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PointCloudXYZ](
	[PointCloudXYZId] [int] NULL,
	[X] [float] NULL,
	[Y] [float] NULL,
	[Z] [float] NULL,
	[Red] [int] NULL,
	[Green] [int] NULL,
	[Blue] [int] NULL,
	[Intensity] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TFW]    Script Date: 25/02/2020 12:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TFW](
	[TFWId] [int] NOT NULL,
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
