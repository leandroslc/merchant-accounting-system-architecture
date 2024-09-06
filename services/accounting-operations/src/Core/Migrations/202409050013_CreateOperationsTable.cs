using FluentMigrator;

namespace AccountingOperations.Core.Migrations;

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
            .WithColumn("type").AsByte().NotNullable();

        Create
            .PrimaryKey("pk_operations")
            .OnTable("operations")
            .Columns("merchant_id", "registered_at");
    }
}
