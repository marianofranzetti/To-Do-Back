# To-Do-Back

# Pasos a seguir para hacerlo funcionar en local

Crear base de datos en sql server => prueba

luego abrir el proyecto y abrir el package manager y ejecutar el comando update-database, verificar las tablas creadas luego

luego ejecutar los siguientes scripts por separado de base de datos para poder utilizar la app como admin

# Primero
GO
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'59e23871-1de2-4fa3-bbd4-d34e7a9b5105', N'esadmin', N'true')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'2264d36d-8f62-42e0-930d-d7e10b2290e5', N'esadmin', N'true')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (3, N'59e23871-1de2-4fa3-bbd4-d34e7a9b5105', N'esadmin', N'true')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF

# Segundo
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2264d36d-8f62-42e0-930d-d7e10b2290e5', N'user@example.com', N'USER@EXAMPLE.COM', N'user@example.com', N'USER@EXAMPLE.COM', 0, N'AQAAAAIAAYagAAAAEDyHQgFhi4teM7u8ZSJ1IqGROcmFV7FCuAQscEvZwfi24OeOUtuVPpGd+Ii1FWNaig==', N'HLFPJR762QDQW67NRJMARG5GZRHRPXH6', N'80fad866-d7bb-473a-890c-21ce2f1de4d9', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'59e23871-1de2-4fa3-bbd4-d34e7a9b5105', N'nuevo@nuevo.com', N'NUEVO@NUEVO.COM', N'nuevo@nuevo.com', N'NUEVO@NUEVO.COM', 0, N'AQAAAAIAAYagAAAAEHYM3DepnciFyJz6Tynpv1qg7lz9ep8nZL0wbQWLGJZ39hAb0PZ7ZT7HWPZ8gb88+Q==', N'46STVH6TSPWMYIWYF2I3KHSBJZQ3SB3G', N'0b665edc-52bc-437b-9dde-b419fd9bcad2', NULL, 0, 0, NULL, 1, 0)

# Tercero
GO
SET IDENTITY_INSERT [dbo].[Tareas] ON 

INSERT [dbo].[Tareas] ([Id], [Nombre], [Descripcion], [FechaLimite]) VALUES (2021, N'Nueva', N'Prueba', CAST(N'2025-02-14T03:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Tareas] OFF

# Luego:
Iniciar la app desde el boton de ejecucion

# Nota: 
hay un script anexado en el caso de querer crear la base de datos de manera local, se crea la base de datos y luego se ejecuta el script de creacion, esto es para el caso en que alguien de infraestructura, no corra la migracion y lo ejecute en un ambiente productivo PATH => ToDoAPI\Scripts\script.sql en estos casos no deben ejecutarse la migracion ya que ya hay una base de datos creada

# La app corre en el puerto: 7256, en localhost

![image](https://github.com/user-attachments/assets/74f11886-120b-425d-aadc-2d94f1fb1e9c)
