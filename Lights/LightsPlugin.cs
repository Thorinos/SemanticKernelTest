using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.SemanticKernel;

public class LightsPlugin
{
  // Mock data for the lights
  private readonly List<LightModel> lights = new()
   {
      new LightModel { Id = 1, Name = "Table Lamp", IsOn = false },
      new LightModel { Id = 2, Name = "Porch light", IsOn = false },
      new LightModel { Id = 3, Name = "Chandelier", IsOn = true }
   };

  [KernelFunction]
  [Description("Gets a list of lights and their current state")]
  public async Task<List<LightModel>> GetLightsAsync()
  {
    foreach (var light in lights)
    {
      Console.WriteLine($"===== Light {light.Name} is: {(light.IsOn.HasValue && light.IsOn.Value ? "On" : "Off")} =====");
    }

    return await Task.FromResult(lights);
  }

  [KernelFunction]
  [Description("Changes the state of the light")]
  public async Task<LightModel?> ChangeStateAsync(int id, bool isOn)
  {
    var light = lights.FirstOrDefault(light => light.Id == id);

    if (light == null)
    {
      return null;
    }

    // Update the light with the new state
    light.IsOn = isOn;

    Console.WriteLine($"===== New state ➡️ Light {light.Name} is: {(light.IsOn.HasValue && light.IsOn.Value ? "On" : "Off")} =====");

    return await Task.FromResult(light);
  }
}

public class LightModel
{
  [JsonPropertyName("id")]
  public int Id { get; set; }

  [JsonPropertyName("name")]
  public string Name { get; set; }

  [JsonPropertyName("is_on")]
  public bool? IsOn { get; set; }
}
