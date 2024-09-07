using FluentMigrator;

namespace DailyBalances.Core.Migrations;

[Migration(202409061946)]
public sealed class CreateBalancesTable : Migration
{
    public override void Down()
    {
        Delete.Table("balances");
    }

    public override void Up()
    {
        Create
            .Table("balances")
            .WithColumn("merchant_id").AsString(40).NotNullable()
            .WithColumn("day").AsDate().NotNullable()
            .WithColumn("total").AsDecimal().NotNullable();

        Create
            .PrimaryKey("pk_balances")
            .OnTable("balances")
            .Columns("merchant_id", "day");
    }
}
