using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Responses.Quotes;
using Flowdesks.Domain.Entities.Chat;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Domain.Entities.Sites;
using Flowdesks.Domain.Entities.SystemPreferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Flowdesks.Persistence.Extensions
{
    public static class DbContextModelCreatingExtension
    {
        public static void ConfigureConnexus(this ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()

            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.Name is "LastModifiedBy" or "CreatedBy" or "DeletedBy"))
            {
                property.SetColumnType("nvarchar(128)");
            }

            #region UserAndRoles

            builder.Entity<RolePermission>(b =>
            {
                b.ToTable("RolePermissions");

                b.HasOne(x => x.Role).WithMany(x => x.RolePermissions).HasForeignKey(x => x.RoleId);
            });

            builder.Entity<UserPermission>().ToTable("UserPermissions");

            builder.Entity<UserRole>(b =>
            {
                b.ToTable("UserRoles");

                b.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId);
                b.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId);
            });

            builder.Entity<Permission>(b =>
            {
                b.ToTable("Permissions");

                b.HasIndex(c => c.Group);
            });

            builder.Entity<UserLoginDeviceHistory>(b =>
            {
                b.ToTable("UserLoginDeviceHistories");

                b.HasIndex(c => c.UserId);

                b.HasOne(x => x.User).WithMany(x => x.LoginDeviceHistories).HasForeignKey(x => x.UserId);
            });

            #endregion

            

            #region Document

            builder.Entity<Document>(b =>
            {
                b.ToTable("Document");

                b.HasIndex(c => new { c.EntityId, c.EntityType });

                b.HasMany(x => x.Files).WithOne(x => x.Document).HasForeignKey(x => x.DocumentId);
            });

            builder.Entity<DocumentFile>(b =>
            {
                b.ToTable("DocumentFiles");
            });

            #endregion

            #region Note

            builder.Entity<Note>(b =>
            {
                b.ToTable("Notes");

                b.HasIndex(c => new { c.EntityId, c.EntityType });

                b.HasOne(x => x.User).WithMany(x => x.Notes).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            

            

           

           
            

            #region Chat

            builder.Entity<DirectMessage>(b =>
            {
                b.ToTable("DirectMessages");

                b.HasOne(x => x.Message).WithOne().HasForeignKey<DirectMessage>(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Receiver).WithMany(x => x.DirectMessages).IsRequired().HasForeignKey(x => x.ReceiverId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Group>(b =>
            {
                b.ToTable("Groups");

                b.HasMany(x => x.GroupMessages).WithOne(x => x.Group).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<GroupUser>(b =>
            {
                b.ToTable("GroupUsers");

                b.HasOne(x => x.User).WithMany(x => x.GroupUsers).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.Group).WithMany(x => x.GroupUsers).HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<GroupMessage>(b =>
            {
                b.ToTable("GroupMessages");

                b.HasOne(x => x.Message).WithOne().HasForeignKey<GroupMessage>(x => x.MessageId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                b.HasOne(m => m.Group).WithMany(x => x.GroupMessages).HasForeignKey(m => m.GroupId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MessageAttachment>(b =>
            {
                b.ToTable("MessageAttachments");

                b.HasOne(x => x.DirectMessage).WithOne(x => x.Attachment).HasForeignKey<DirectMessage>(x => x.AttachmentId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne(x => x.GroupMessage).WithOne(x => x.Attachment).HasForeignKey<GroupMessage>(x => x.AttachmentId).OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            

            #region Notification Setting
            builder.Entity<NotificationSetting>(b =>
            {
                b.ToTable("NotificationSettings");
                b.HasOne(x => x.User).WithMany(x => x.NotificationSettings).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            });
            #endregion

          

            

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        var converter = new ValueConverter<DateTime, DateTime>(
                            v => v,
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        );
                        property.SetValueConverter(converter);
                    }
                }
            }
        }
    }
}