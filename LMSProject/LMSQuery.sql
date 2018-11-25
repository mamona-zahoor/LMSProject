USE [Pactice]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[Email] [nchar](27) NOT NULL,
	[Password] [nchar](11) NOT NULL,
	CONSTRAINT [PK_Admin] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applied](
    [Username] [nchar] (30) NOT NULL,
	[Email] [nchar](27) NOT NULL,
	[Registeration_Num] [nchar](11) NULL,
	[Designation] [nchar](30) NULL,
	[Password] [nchar](30) NOT NULL,
	CONSTRAINT [PK_Applied] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 
Insert into Admin(Email, Password) values ('librarian.csedept@gmail.com', 'AdminLib123')


