using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Breakfast
{
  internal class CookingSteps
  {
    [KernelFunction]
    [Description("Pour coffee into a mug")]
    public Task PourCoffee()
    {
      Console.WriteLine("===== Pouring coffee 🍵... =====");
      return Task.CompletedTask;
    }

    [KernelFunction]
    [Description("Fry eggs")]
    public Task FryEggs(int howMany = 1)
    {
      Console.WriteLine($"===== Frying {howMany} eggs 🍳... =====");
      return Task.CompletedTask;
    }

    [KernelFunction]
    [Description("Fry slice of bacon")]
    public Task FryBacon(int slices = 1)
    {
      Console.WriteLine($"===== Cooking {slices} slice(s) of bacons 🥓... =====");
      return Task.CompletedTask;
    }

    [KernelFunction]
    [Description("Toast slice of bread")]
    public Task ToastBread(int slices = 1)
    {
      Console.WriteLine($"===== Cooking {slices} slice(s) of breads 🍞... =====");
      return Task.CompletedTask;
    }

    [KernelFunction]
    [Description("Apply jam on a toast of bread")]
    public Task ApplyJam()
    {
      Console.WriteLine($"===== Putting jam on the toast. 🍓 =====");
      return Task.CompletedTask;
    }
  }
}
