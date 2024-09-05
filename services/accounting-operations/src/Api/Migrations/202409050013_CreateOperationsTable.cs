using FluentMigrator;

namespace AccoutingOperations.Database.Migrations;

[Migration(202409050013)]
public sealed class CreateOperationsTable : Migration
{
    public override void Down()
    {
        Delete.Table("operations");
    }

    public override void Up()
    {
        Create
            .Table("operations")
            .WithColumn("merchant_id").AsString(40).NotNullable()
            .WithColumn("registered_at").AsDateTime().NotNullable()
            .WithColumn("value").AsDecimal().NotNullable()
            .WithColumn("type").AsFixedLengthAnsiString(1).NotNullable();

        Create
            .PrimaryKey("pk_operations")
            .OnTable("operations")
            .Columns("merchant_id", "registered_at");
    }
}
