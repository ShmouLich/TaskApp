IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(max) NULL,
    [Surname] nvarchar(max) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [Companies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([Id])
);

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Tasks] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [CompanyId] int NOT NULL,
    [AssignedId] nvarchar(450) NOT NULL,
    [CreatorId] nvarchar(450) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [DueDate] datetime2 NULL,
    [CompletedDate] datetime2 NULL,
    [Status] int NOT NULL,
    [Priority] int NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tasks_AspNetUsers_AssignedId] FOREIGN KEY ([AssignedId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Tasks_AspNetUsers_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Tasks_Companies_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Companies] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [Text] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    [TaskItemId] int NOT NULL,
    [AuthorId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Comments_Tasks_TaskItemId] FOREIGN KEY ([TaskItemId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Documents] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(max) NOT NULL,
    [FilePath] nvarchar(max) NOT NULL,
    [FileSize] bigint NOT NULL,
    [UploadedAt] datetime2 NOT NULL,
    [TaskItemId] int NOT NULL,
    [UploadedById] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Documents] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Documents_AspNetUsers_UploadedById] FOREIGN KEY ([UploadedById]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Documents_Tasks_TaskItemId] FOREIGN KEY ([TaskItemId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CheckListItems] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [IsChecked] bit NOT NULL,
    [Order] int NOT NULL,
    [TaskItemId] int NOT NULL,
    CONSTRAINT [PK_CheckListItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CheckListItems_Tasks_TaskItemId] FOREIGN KEY ([TaskItemId]) REFERENCES [Tasks] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_Comments_AuthorId] ON [Comments] ([AuthorId]);

CREATE INDEX [IX_Comments_TaskItemId] ON [Comments] ([TaskItemId]);

CREATE INDEX [IX_Documents_TaskItemId] ON [Documents] ([TaskItemId]);

CREATE INDEX [IX_Documents_UploadedById] ON [Documents] ([UploadedById]);

CREATE INDEX [IX_CheckListItems_TaskItemId] ON [CheckListItems] ([TaskItemId]);

CREATE INDEX [IX_Tasks_AssignedId] ON [Tasks] ([AssignedId]);

CREATE INDEX [IX_Tasks_CompanyId] ON [Tasks] ([CompanyId]);

CREATE INDEX [IX_Tasks_CreatorId] ON [Tasks] ([CreatorId]);

CREATE INDEX [IX_Tasks_DueDate] ON [Tasks] ([DueDate]);

CREATE INDEX [IX_Tasks_Status] ON [Tasks] ([Status]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251205112347_InitialCreate', N'9.0.11');

COMMIT;
GO

