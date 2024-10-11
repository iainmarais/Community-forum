-- --------------------------------------------------------
-- Host:                         localhost
-- Server version:               11.4.2-MariaDB-log - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for communityforum
CREATE DATABASE IF NOT EXISTS `communityforum` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `communityforum`;

-- Dumping structure for table communityforum.boards
CREATE TABLE IF NOT EXISTS `boards` (
  `BoardId` varchar(255) NOT NULL,
  `CategoryId` varchar(255) NOT NULL,
  `CreatedByUserId` longtext NOT NULL,
  `BoardName` longtext NOT NULL,
  `BoardDescription` longtext NOT NULL,
  PRIMARY KEY (`BoardId`),
  KEY `IX_Boards_CategoryId` (`CategoryId`),
  CONSTRAINT `FK_Boards_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.categories
CREATE TABLE IF NOT EXISTS `categories` (
  `CategoryId` varchar(255) NOT NULL,
  `CategoryName` longtext NOT NULL,
  `CategoryDescription` longtext NOT NULL,
  PRIMARY KEY (`CategoryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.chatgroups
CREATE TABLE IF NOT EXISTS `chatgroups` (
  `ChatGroupId` varchar(255) NOT NULL,
  `ChatGroupName` longtext NOT NULL,
  `ChatGroupDescription` longtext NOT NULL,
  PRIMARY KEY (`ChatGroupId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.chatmessages
CREATE TABLE IF NOT EXISTS `chatmessages` (
  `ChatMessageId` varchar(255) NOT NULL,
  `ChatId` longtext NOT NULL,
  `ChatGroupId` longtext NOT NULL,
  `CreatedByUserId` longtext NOT NULL,
  `RecipientUserId` longtext NOT NULL,
  `ChatMessageContent` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `IsEdited` tinyint(1) NOT NULL,
  `EditedTime` datetime(6) NOT NULL,
  PRIMARY KEY (`ChatMessageId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.chats
CREATE TABLE IF NOT EXISTS `chats` (
  `ChatId` varchar(255) NOT NULL,
  `ChatGroupId` longtext NOT NULL,
  `ChatName` longtext NOT NULL,
  `CreatedByUserId` longtext NOT NULL,
  `SecondParticipantUserId` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`ChatId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.contacts
CREATE TABLE IF NOT EXISTS `contacts` (
  `ContactId` varchar(255) NOT NULL,
  `UserId` longtext NOT NULL,
  `CreatedByUserId` longtext NOT NULL,
  `ContactName` longtext NOT NULL,
  `ContactEmailAddress` longtext NOT NULL,
  `AboutMessage` longtext NOT NULL,
  `ContactProfileImageBase64` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`ContactId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.galleryitems
CREATE TABLE IF NOT EXISTS `galleryitems` (
  `GalleryItemId` varchar(255) NOT NULL,
  `CreatedByUserId` varchar(255) NOT NULL,
  `GalleryItemName` longtext NOT NULL,
  `GalleryItemDescription` longtext NOT NULL,
  `GalleryItemLink` longtext NOT NULL,
  `NumLikes` int(11) NOT NULL,
  `NumDislikes` int(11) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`GalleryItemId`),
  KEY `IX_GalleryItems_CreatedByUserId` (`CreatedByUserId`),
  CONSTRAINT `FK_GalleryItems_Users_CreatedByUserId` FOREIGN KEY (`CreatedByUserId`) REFERENCES `users` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.permissions
CREATE TABLE IF NOT EXISTS `permissions` (
  `PermissionId` varchar(255) NOT NULL,
  `PermissionName` longtext NOT NULL,
  `PermissionType` longtext NOT NULL,
  `Description` longtext NOT NULL,
  PRIMARY KEY (`PermissionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.posts
CREATE TABLE IF NOT EXISTS `posts` (
  `PostId` varchar(255) NOT NULL,
  `ThreadId` varchar(255) NOT NULL,
  `IsFirstPost` tinyint(1) NOT NULL,
  `CreatedByUserId` longtext NOT NULL,
  `PostContent` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `ReplyToPostId` longtext NOT NULL,
  `PostReported` tinyint(1) NOT NULL,
  `ReportReason` longtext NOT NULL,
  `ReportedByUserId` longtext NOT NULL,
  `DateMarkedForDeletion` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `IsMarkedForDelete` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`PostId`),
  KEY `IX_Posts_ThreadId` (`ThreadId`),
  CONSTRAINT `FK_Posts_Threads_ThreadId` FOREIGN KEY (`ThreadId`) REFERENCES `threads` (`ThreadId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.rolepermissions
CREATE TABLE IF NOT EXISTS `rolepermissions` (
  `RolePermissionId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `PermissionId` varchar(255) NOT NULL,
  `IsAllowed` tinyint(1) NOT NULL,
  PRIMARY KEY (`RolePermissionId`),
  KEY `IX_RolePermissions_PermissionId` (`PermissionId`),
  KEY `IX_RolePermissions_RoleId` (`RoleId`),
  CONSTRAINT `FK_RolePermissions_Permissions_PermissionId` FOREIGN KEY (`PermissionId`) REFERENCES `permissions` (`PermissionId`),
  CONSTRAINT `FK_RolePermissions_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.roles
CREATE TABLE IF NOT EXISTS `roles` (
  `RoleId` varchar(255) NOT NULL,
  `RoleName` longtext NOT NULL,
  `RoleType` longtext NOT NULL,
  `Description` longtext DEFAULT NULL,
  PRIMARY KEY (`RoleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.systempermissions
CREATE TABLE IF NOT EXISTS `systempermissions` (
  `SystemPermissionId` varchar(255) NOT NULL,
  `SystemPermissionType` longtext NOT NULL,
  `SystemPermissionName` longtext NOT NULL,
  `Description` longtext DEFAULT NULL,
  PRIMARY KEY (`SystemPermissionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.threads
CREATE TABLE IF NOT EXISTS `threads` (
  `ThreadId` varchar(255) NOT NULL,
  `ThreadName` longtext NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  `TopicId` varchar(255) NOT NULL,
  `CreatedByUserId` varchar(255) NOT NULL,
  `NumberOfPosts` int(11) NOT NULL,
  `HasNewPosts` tinyint(1) NOT NULL,
  PRIMARY KEY (`ThreadId`),
  KEY `IX_Threads_CreatedByUserId` (`CreatedByUserId`),
  KEY `IX_Threads_TopicId` (`TopicId`),
  CONSTRAINT `FK_Threads_Topics_TopicId` FOREIGN KEY (`TopicId`) REFERENCES `topics` (`TopicId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Threads_Users_CreatedByUserId` FOREIGN KEY (`CreatedByUserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.topics
CREATE TABLE IF NOT EXISTS `topics` (
  `TopicId` varchar(255) NOT NULL,
  `BoardId` varchar(255) NOT NULL,
  `TopicName` longtext NOT NULL,
  `Description` longtext NOT NULL,
  `CreatedByUserId` varchar(255) NOT NULL,
  `CreatedDate` datetime(6) NOT NULL,
  PRIMARY KEY (`TopicId`),
  KEY `IX_Topics_BoardId` (`BoardId`),
  KEY `IX_Topics_CreatedByUserId` (`CreatedByUserId`),
  CONSTRAINT `FK_Topics_Boards_BoardId` FOREIGN KEY (`BoardId`) REFERENCES `boards` (`BoardId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Topics_Users_CreatedByUserId` FOREIGN KEY (`CreatedByUserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.userpermissions
CREATE TABLE IF NOT EXISTS `userpermissions` (
  `UserPermissionId` varchar(255) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `SystemPermissionId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserPermissionId`),
  KEY `IX_UserPermissions_SystemPermissionId` (`SystemPermissionId`),
  KEY `IX_UserPermissions_UserId` (`UserId`),
  CONSTRAINT `FK_UserPermissions_SystemPermissions_SystemPermissionId` FOREIGN KEY (`SystemPermissionId`) REFERENCES `systempermissions` (`SystemPermissionId`),
  CONSTRAINT `FK_UserPermissions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.userrefreshtokens
CREATE TABLE IF NOT EXISTS `userrefreshtokens` (
  `UserRefreshTokenId` varchar(255) NOT NULL,
  `AssignedToUserId` varchar(255) NOT NULL,
  `RefreshToken` varchar(8000) NOT NULL,
  `RefreshTokenExpirationDate` datetime(6) NOT NULL,
  `Source` longtext NOT NULL,
  `IsRevoked` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`UserRefreshTokenId`),
  KEY `IX_UserRefreshTokens_AssignedToUserId` (`AssignedToUserId`),
  CONSTRAINT `FK_UserRefreshTokens_Users_AssignedToUserId` FOREIGN KEY (`AssignedToUserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.users
CREATE TABLE IF NOT EXISTS `users` (
  `UserId` varchar(255) NOT NULL,
  `AdminUserId` longtext NOT NULL,
  `ForumUserId` longtext NOT NULL,
  `UserProfileImageBase64` longtext NOT NULL,
  `Username` longtext NOT NULL,
  `EmailAddress` longtext NOT NULL,
  `Address` longtext DEFAULT NULL,
  `CityName` longtext DEFAULT NULL,
  `CountryName` longtext DEFAULT NULL,
  `PostalCode` longtext DEFAULT NULL,
  `UserFirstname` longtext DEFAULT NULL,
  `UserLastname` longtext DEFAULT NULL,
  `Gender` longtext DEFAULT NULL,
  `HashedPassword` longtext NOT NULL,
  `RoleId` longtext NOT NULL,
  `TotalPosts` int(11) DEFAULT NULL,
  `IsOnline` tinyint(1) NOT NULL,
  `IsVisible` tinyint(1) NOT NULL,
  `LastLoginTime` datetime(6) NOT NULL,
  `UserId1` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`UserId`),
  KEY `IX_Users_UserId1` (`UserId1`),
  CONSTRAINT `FK_Users_Users_UserId1` FOREIGN KEY (`UserId1`) REFERENCES `users` (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.usersessiontokens
CREATE TABLE IF NOT EXISTS `usersessiontokens` (
  `UserSessionTokenId` varchar(255) NOT NULL,
  `DateCreated` datetime(6) NOT NULL,
  `AssignedToUserId` varchar(255) NOT NULL,
  `IsRevoked` tinyint(1) NOT NULL,
  `DateRevoked` datetime(6) DEFAULT NULL,
  `SessionToken` varchar(8000) NOT NULL,
  `DateExpired` datetime(6) NOT NULL,
  `Source` longtext NOT NULL,
  PRIMARY KEY (`UserSessionTokenId`),
  UNIQUE KEY `IX_UserSessionTokens_SessionToken` (`SessionToken`) USING HASH,
  KEY `IX_UserSessionTokens_AssignedToUserId` (`AssignedToUserId`),
  CONSTRAINT `FK_UserSessionTokens_Users_AssignedToUserId` FOREIGN KEY (`AssignedToUserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

-- Dumping structure for table communityforum.__efmigrationshistory
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Data exporting was unselected.

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
