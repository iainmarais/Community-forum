-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               11.5.2-MariaDB - mariadb.org binary distribution
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

-- Dumping data for table communityforum.boards: ~2 rows (approximately)
REPLACE INTO `boards` (`BoardId`, `CategoryId`, `CreatedByUserId`, `BoardName`, `BoardDescription`) VALUES
	('08db5b5b-075c-4dd5-9597-8981e07a84e5', '2472b880-6168-4b9e-88b3-6bfc60debf98', '55bdba28-34c3-48d4-a9f2-4cd4a3afb829', 'General discussion', 'Discuss everything else here.'),
	('7f4e776b-6129-49e9-b5a1-3e9fd3638b8f', '96679e27-d33e-45dc-beac-8bc3b68519a8', '55bdba28-34c3-48d4-a9f2-4cd4a3afb829', 'IT support requests', 'This board is for all IT support requests.');

-- Dumping data for table communityforum.categories: ~2 rows (approximately)
REPLACE INTO `categories` (`CategoryId`, `CategoryName`, `CategoryDescription`) VALUES
	('2472b880-6168-4b9e-88b3-6bfc60debf98', 'Off-topic', 'Everything else goes here.'),
	('96679e27-d33e-45dc-beac-8bc3b68519a8', 'IT & computers - General', 'Anything to do with IT and computers goes here.');

-- Dumping data for table communityforum.chatgroups: ~0 rows (approximately)

-- Dumping data for table communityforum.chatmessages: ~0 rows (approximately)

-- Dumping data for table communityforum.chats: ~0 rows (approximately)

-- Dumping data for table communityforum.contacts: ~0 rows (approximately)

-- Dumping data for table communityforum.galleryitems: ~0 rows (approximately)

-- Dumping data for table communityforum.permissions: ~41 rows (approximately)
REPLACE INTO `permissions` (`PermissionId`, `PermissionName`, `PermissionType`, `Description`) VALUES
	('perm_chat_create', 'Chat_Create', 'General', 'Create new chat messages'),
	('perm_chat_delete', 'Chat_Delete', 'General', 'Delete chat messages'),
	('perm_chat_edit', 'Chat_Edit', 'General', 'Edit existing chat messages'),
	('perm_chat_post_image', 'Chat_PostImage', 'General', 'Post images in chat'),
	('perm_gallery_delete_image', 'Gallery_DeleteImage', 'General', 'Delete images'),
	('perm_gallery_edit_image', 'Gallery_EditImage', 'General', 'Edit images'),
	('perm_gallery_upload_image', 'Gallery_UploadImage', 'General', 'Upload images'),
	('perm_mod_ban_user', 'Mod_BanUser', 'Moderation', 'Ban users'),
	('perm_mod_lock_thread', 'Mod_LockThread', 'Moderation', 'Lock threads'),
	('perm_mod_moderate_posts', 'Mod_ModeratePosts', 'Moderation', 'Moderate posts'),
	('perm_mod_move_thread', 'Mod_MoveThread', 'Moderation', 'Move threads'),
	('perm_mod_mute_user', 'Mod_MuteUser', 'Moderation', 'Mute users'),
	('perm_mod_pin_thread', 'Mod_PinThread', 'Moderation', 'Pin threads'),
	('perm_mod_restore_post', 'Mod_RestorePost', 'Moderation', 'Restore posts'),
	('perm_mod_view_reported_content', 'Mod_ViewReportedContent', 'Moderation', 'View reported content'),
	('perm_mod_warn_user', 'Mod_WarnUser', 'Moderation', 'Warn users'),
	('perm_posts_create', 'Posts_Create', 'Content', 'Create new posts'),
	('perm_posts_delete', 'Posts_Delete', 'Content', 'Delete posts'),
	('perm_posts_edit', 'Posts_Edit', 'Content', 'Edit existing posts'),
	('perm_posts_post_image', 'Posts_PostImage', 'Content', 'Post images'),
	('perm_posts_post_reply', 'Posts_PostReply', 'Content', 'Post replies'),
	('perm_posts_update', 'Posts_Update', 'Content', 'Update posts'),
	('perm_roles_create', 'Roles_Create', 'Administrative', 'Create new roles'),
	('perm_roles_delete', 'Roles_Delete', 'Administrative', 'Delete roles'),
	('perm_roles_edit', 'Roles_Edit', 'Administrative', 'Edit existing roles'),
	('perm_threads_create', 'Threads_Create', 'Content', 'Create new threads'),
	('perm_threads_delete', 'Threads_Delete', 'Content', 'Delete threads'),
	('perm_threads_edit', 'Threads_Edit', 'Content', 'Edit existing threads'),
	('perm_threads_lock', 'Threads_Lock', 'Content', 'Lock threads'),
	('perm_threads_unlock', 'Threads_Unlock', 'Content', 'Unlock threads'),
	('perm_topics_create', 'Topics_Create', 'Content', 'Create new topics'),
	('perm_topics_delete', 'Topics_Delete', 'Content', 'Delete topics'),
	('perm_topics_edit', 'Topics_Edit', 'Content', 'Edit existing topics'),
	('perm_users_ban_user', 'Users_BanUser', 'Administrative', 'Ban user'),
	('perm_users_change_password', 'Users_ChangePassword', 'Administrative', 'Change user password'),
	('perm_users_change_roles', 'Users_ChangeRoles', 'Administrative', 'Change user roles'),
	('perm_users_create', 'Users_Create', 'Administrative', 'Create new users'),
	('perm_users_delete', 'Users_Delete', 'Administrative', 'Delete users'),
	('perm_users_edit', 'Users_Edit', 'Administrative', 'Edit existing users'),
	('perm_users_mute_user', 'Users_MuteUser', 'Administrative', 'Mute user'),
	('perm_users_warn_user', 'Users_WarnUser', 'Administrative', 'Warn user');

-- Dumping data for table communityforum.posts: ~0 rows (approximately)

-- Dumping data for table communityforum.rolepermissions: ~41 rows (approximately)
REPLACE INTO `rolepermissions` (`RolePermissionId`, `RoleId`, `PermissionId`) VALUES
	('495ac5a7-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_create'),
	('495acb2d-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_edit'),
	('495acbd7-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_delete'),
	('495acc6b-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_change_roles'),
	('495acce6-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_change_password'),
	('495ad52c-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_ban_user'),
	('495ad5ce-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_mute_user'),
	('495ad649-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_users_warn_user'),
	('495ad6c2-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_roles_create'),
	('495ad73b-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_roles_edit'),
	('495ad7b4-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_roles_delete'),
	('495ad829-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_threads_create'),
	('495ad89b-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_threads_edit'),
	('495ad911-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_threads_delete'),
	('495ad978-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_threads_lock'),
	('495ad9e9-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_threads_unlock'),
	('495ada5a-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_posts_create'),
	('495adac8-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_posts_edit'),
	('495adb37-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_posts_delete'),
	('495adba4-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_posts_update'),
	('495adc12-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_posts_post_image'),
	('495adc7c-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_posts_post_reply'),
	('495adce5-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_topics_create'),
	('495add51-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_topics_edit'),
	('495addbc-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_topics_delete'),
	('495ade26-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_chat_create'),
	('495ade8d-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_chat_edit'),
	('495adef5-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_chat_delete'),
	('495adf5e-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_chat_post_image'),
	('495adfc7-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_gallery_upload_image'),
	('495ae04e-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_gallery_delete_image'),
	('495ae0bf-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_gallery_edit_image'),
	('495af640-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_moderate_posts'),
	('495af763-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_pin_thread'),
	('495af7ea-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_lock_thread'),
	('495af865-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_move_thread'),
	('495af8db-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_ban_user'),
	('495af946-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_mute_user'),
	('495af9b4-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_view_reported_content'),
	('495afa25-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_restore_post'),
	('495afa96-824a-11ef-8ba4-507b9d1c4828', 'role_admin', 'perm_mod_warn_user');

-- Dumping data for table communityforum.roles: ~11 rows (approximately)
REPLACE INTO `roles` (`RoleId`, `RoleName`, `RoleType`, `Description`) VALUES
	('role_admin', 'Administrator', 'Admin', 'Full access to the entire forum. Responsible for managing users, settings, and major administrative tasks.'),
	('role_banned', 'Banned User', 'Banned', 'Users banned from interacting with the forum due to violations of the forum rules.'),
	('role_communitymanager', 'Community Manager', 'Manager', 'Oversees community engagement, organizes events, and creates initiatives to improve participation.'),
	('role_contentcreator', 'Content Creator', 'Creator', 'Focuses on producing high-quality, original content like tutorials, articles, and guides.'),
	('role_expert', 'Verified Expert', 'Expert', 'Recognized for expertise in a specific area, contributes by answering questions and validating discussions.'),
	('role_guest', 'Guest', 'Guest', 'Visitors who can view publicly accessible content but cannot interact until they register.'),
	('role_juniormoderator', 'Junior Moderator', 'JuniorModerator', 'Moderators-in-training who assist full moderators with limited privileges.'),
	('role_sponsor', 'Sponsor', 'Sponsor', 'Users or entities that sponsor the forum through funding or events, with promotional visibility privileges.'),
	('role_support', 'Support Staff', 'Support', 'Helps users with forum-related issues, with access to administrative tools related to user accounts.'),
	('role_user', 'Regular User', 'User', 'Regular members who can create posts, ask questions, and participate in discussions.'),
	('role-moderator', 'Moderator', 'Moderator', 'Monitors forum activity, enforces rules, and handles issues like inappropriate posts and user conflicts.');

-- Dumping data for table communityforum.systempermissions: ~34 rows (approximately)
REPLACE INTO `systempermissions` (`SystemPermissionId`, `SystemPermissionName`, `SystemPermissionType`, `Description`) VALUES
	('chat_create', 'Create Chat', 'Chat_Create', 'Allows the creation of new chat sessions.'),
	('chat_create_group', 'Create Chat Group', 'Chat_CreateGroup', 'Allows users to create new chat groups.'),
	('chat_delete', 'Delete Chat', 'Chat_Delete', 'Allows deletion of chat messages.'),
	('chat_delete_group', 'Delete Chat Group', 'Chat_DeleteGroup', 'Allows deletion of chat groups.'),
	('chat_edit', 'Edit Chat', 'Chat_Edit', 'Allows editing of existing chat messages.'),
	('chat_edit_group', 'Edit Chat Group', 'Chat_EditGroup', 'Allows users to edit existing chat groups.'),
	('chat_join_group', 'Join Chat Group', 'Chat_JoinGroup', 'Allows users to join existing chat groups.'),
	('chat_post_image', 'Chat Post Image', 'Chat_PostImage', 'Allows users to post images in chat.'),
	('gallery_delete_image', 'Delete Image from Gallery', 'Gallery_DeleteImage', 'Allows deletion of images from the gallery.'),
	('gallery_edit_image', 'Edit Image in Gallery', 'Gallery_EditImage', 'Allows editing details of images in the gallery.'),
	('gallery_upload_image', 'Upload Image to Gallery', 'Gallery_UploadImage', 'Allows users to upload images to the gallery.'),
	('posts_create', 'Create Posts', 'Posts_Create', 'Allows the creation of new posts.'),
	('posts_delete', 'Delete Posts', 'Posts_Delete', 'Allows deletion of posts.'),
	('posts_edit', 'Edit Posts', 'Posts_Edit', 'Allows editing existing posts.'),
	('posts_post_image', 'Post Image', 'Posts_PostImage', 'Allows users to upload images in posts.'),
	('posts_post_reply', 'Post Reply', 'Posts_PostReply', 'Allows users to reply to posts.'),
	('posts_update', 'Update Posts', 'Posts_Update', 'Allows updating the content of posts.'),
	('roles_create', 'Create Roles', 'Roles_Create', 'Allows the creation of new roles.'),
	('roles_delete', 'Delete Roles', 'Roles_Delete', 'Allows the deletion of roles.'),
	('roles_edit', 'Edit Roles', 'Roles_Edit', 'Allows the editing of existing roles.'),
	('threads_create', 'Create Threads', 'Threads_Create', 'Allows the creation of new threads.'),
	('threads_delete', 'Delete Threads', 'Threads_Delete', 'Allows deletion of threads.'),
	('threads_edit', 'Edit Threads', 'Threads_Edit', 'Allows editing existing threads.'),
	('threads_lock', 'Lock Threads', 'Threads_Lock', 'Allows locking threads to prevent further replies.'),
	('threads_unlock', 'Unlock Threads', 'Threads_Unlock', 'Allows unlocking threads for further replies.'),
	('topics_create', 'Create Topics', 'Topics_Create', 'Allows the creation of new topics.'),
	('topics_delete', 'Delete Topics', 'Topics_Delete', 'Allows deletion of topics.'),
	('topics_edit', 'Edit Topics', 'Topics_Edit', 'Allows editing existing topics.'),
	('users_ban_user', 'Ban User', 'Users_BanUser', 'Allows banning users from the system.'),
	('users_change_password', 'Change User Password', 'Users_ChangePassword', 'Allows users to change their own passwords.'),
	('users_change_roles', 'Change User Roles', 'Users_ChangeRoles', 'Allows changing roles assigned to users.'),
	('users_create', 'Create Users', 'Users_Create', 'Allows the creation of new users.'),
	('users_delete', 'Delete Users', 'Users_Delete', 'Allows the deletion of users.'),
	('users_edit', 'Edit Users', 'Users_Edit', 'Allows the editing of existing users.');

-- Dumping data for table communityforum.threads: ~0 rows (approximately)

-- Dumping data for table communityforum.topics: ~1 rows (approximately)
REPLACE INTO `topics` (`TopicId`, `BoardId`, `TopicName`, `Description`, `CreatedByUserId`, `CreatedDate`) VALUES
	('96a655b9-b317-4ac6-942e-66212efe08d6', '7f4e776b-6129-49e9-b5a1-3e9fd3638b8f', 'Windows IT support ', 'Post your requests for help and support pertaining to Windows here.', '55bdba28-34c3-48d4-a9f2-4cd4a3afb829', '2024-10-03 09:04:48.208038');

-- Dumping data for table communityforum.userpermissions: ~0 rows (approximately)

-- Dumping data for table communityforum.userrefreshtokens: ~1 rows (approximately)
REPLACE INTO `userrefreshtokens` (`UserRefreshTokenId`, `UserId`, `RefreshToken`, `RefreshTokenExpirationDate`, `Source`) VALUES
	('ee4b6401-f81d-4558-b132-56ab02297770', '55bdba28-34c3-48d4-a9f2-4cd4a3afb829', 'Q4S8K8c7SI6ebDym51bwqDFccgHlInKyET4jvF4Iaqk=', '2024-12-03 12:15:16.405835', 'Forum');

-- Dumping data for table communityforum.users: ~1 rows (approximately)
REPLACE INTO `users` (`UserId`, `AdminUserId`, `ForumUserId`, `UserProfileImageBase64`, `Username`, `EmailAddress`, `Address`, `CityName`, `CountryName`, `PostalCode`, `UserFirstname`, `UserLastname`, `Gender`, `HashedPassword`, `RoleId`, `TotalPosts`, `IsOnline`, `IsVisible`, `LastLoginTime`) VALUES
	('55bdba28-34c3-48d4-a9f2-4cd4a3afb829', '785e9a4f-b76d-409f-99c0-8556bb2d478c', '0a0b8e53-2db9-4b9a-9b6f-5c1d9c9f2fda', '', 'iain', 'iainmarais@gmail.com', NULL, NULL, NULL, NULL, 'Iain', 'Marais', NULL, '$2a$10$cVih7FrYPXHJsOvi8dgplOzwr9vYAcI8rH.4jIjSJaa8M/jI2abWO', 'role_admin', NULL, 0, 0, '2024-10-04 14:15:16.408087');

-- Dumping data for table communityforum.__efmigrationshistory: ~0 rows (approximately)

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
