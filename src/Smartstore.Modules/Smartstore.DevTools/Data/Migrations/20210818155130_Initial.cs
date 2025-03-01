﻿using FluentMigrator;
using Smartstore.Core.Data.Migrations;

namespace Smartstore.DevTools.Data.Migrations
{
    [MigrationVersion("2021-08-18 15:51:30", "DevTools: add test entity")]
    internal class Initial : Migration
    {
        private const string TABLE_NAME = "DevToolsTestEntity";

        public override void Up()
        {
            if (!Schema.Table(TABLE_NAME).Exists())
            {
                Create.Table(TABLE_NAME)
                    .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                    .WithColumn("Name").AsString(400).NotNullable()
                    .WithColumn("Description").AsString(int.MaxValue).Nullable()
                    .WithColumn("PageSize").AsInt32().Nullable()
                    .WithColumn("LimitedToStores").AsBoolean().NotNullable().Indexed("IX_LimitedToStores")
                    .WithColumn("SubjectToAcl").AsBoolean().NotNullable().Indexed("IX_SubjectToAcl")
                    .WithColumn("Published").AsBoolean().NotNullable()
                    .WithColumn("Deleted").AsBoolean().NotNullable().Indexed("IX_Deleted")
                    .WithColumn("DisplayOrder").AsInt32().NotNullable().Indexed("IX_DisplayOrder")
                    .WithColumn("CreatedOnUtc").AsDateTime2().NotNullable()
                    .WithColumn("UpdatedOnUtc").AsDateTime2().NotNullable();
            }
        }

        public override void Down()
        {
            if (Schema.Table(TABLE_NAME).Exists())
            {
                Delete.Table(TABLE_NAME);
            }
        }
    }
}
