using Shouldly;
using Xunit;

namespace DacpacDataMigrations.Tests;



public partial class SortItemGroupTests
{
    [Fact]
    public void Task_should_sort_items_ascending_with_default_comparer()
    {
        var task = new SortItemGroup()
        {
            In = new[] {
                new TestTaskItem() { ItemSpec = "2"},
                new TestTaskItem() { ItemSpec = "10"},
                new TestTaskItem() { ItemSpec = "1"},
            },
        };

        var result = task.Execute();

        result.ShouldBeTrue();
        task.In.ShouldNotBeNull();
        task.Out.ShouldNotBeNull();

        task.Out.Length.ShouldBe(3);

        task.Out.ShouldSatisfyAllConditions(
            () => task.Out[0].ItemSpec.ShouldBe("1"),
            () => task.Out[1].ItemSpec.ShouldBe("2"),
            () => task.Out[2].ItemSpec.ShouldBe("10")
        );
    }

    [Fact]
    public void Task_should_sort_using_ordinal_ignore_case_comparer()
    {
        var task = new SortItemGroup()
        {
            CompareNumbersInItemsAsNumbers = false,
            In = new[] {
                new TestTaskItem() { ItemSpec = "2"},
                new TestTaskItem() { ItemSpec = "10"},
                new TestTaskItem() { ItemSpec = "1"},
            },
        };

        var result = task.Execute();

        result.ShouldBeTrue();
        task.In.ShouldNotBeNull();
        task.Out.ShouldNotBeNull();

        task.Out.Length.ShouldBe(3);

        task.Out.ShouldSatisfyAllConditions(
            () => task.Out[0].ItemSpec.ShouldBe("1"),
            () => task.Out[1].ItemSpec.ShouldBe("10"),
            () => task.Out[2].ItemSpec.ShouldBe("2")
        );
    }
}
