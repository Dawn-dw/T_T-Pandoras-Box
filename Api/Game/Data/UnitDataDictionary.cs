using Newtonsoft.Json;

namespace Api.Game.Data;

public class UnitDataDictionary
{
    private readonly IDictionary<int, UnitData> _unitsData = new Dictionary<int, UnitData>();
    public UnitDataDictionary(JsonSerializerSettings jsonSerializerSettings)
    {
        var path = Path.Combine("Resources", "Data", "Units.json");
        if (!File.Exists(path)) return;

        _unitsData = new Dictionary<int, UnitData>();
        var fileData = File.ReadAllText(path);
        var data = JsonConvert.DeserializeObject<IDictionary<string, UnitData>>(fileData, jsonSerializerSettings);
        Init(data);
    }

    public void Init(IDictionary<string, UnitData>? data)
    {
        if (data is null) return;
        foreach (var dataValue in data.Values)
        {
            dataValue.RecalculateHashes();
            _unitsData.Add(dataValue.NameHash, dataValue);
        }
    }
    
    public UnitData? this[int key] => _unitsData.TryGetValue(key, out var unitData) ? unitData : null;
}