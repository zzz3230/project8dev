using System.Collections.Generic;

public struct Weight
{
    public enum WeightFormat { Kg, Mass, UniWeight }

    public float mass
    {
        get
        {
            return ConvertUniWeight(_uniWeight, WeightFormat.Mass);
        }
    }

    public float kg
    {
        get
        {
            return ConvertUniWeight(_uniWeight, WeightFormat.Kg);
        }
        set
        {
            _uniWeight = ConvertToUniWeight(value, WeightFormat.Kg);
        }
    }

    public long uniWeight
    {
        get
        {
            return _uniWeight;
        }
        set
        {
            _uniWeight = value;
        }
    }

    long _uniWeight;

    Dictionary<WeightFormat, double> _odds;

    public float ConvertUniWeight(long uw, WeightFormat format)
    {
        return (float)(uw / _odds[format]);
    }

    public long ConvertToUniWeight(float any, WeightFormat format)
    {
        return (long)(any * _odds[format]);
    }

    public Weight(long uniWeight)
    {
        _uniWeight = uniWeight;
        _odds = new Dictionary<WeightFormat, double>
        {
            {WeightFormat.Kg, 1000 },
            {WeightFormat.Mass, 1000 }
        };
    }

    public Weight(float kg)
    {
        _uniWeight = 0;
        _odds = new Dictionary<WeightFormat, double>
        {
            {WeightFormat.Kg, 1000 },
            {WeightFormat.Mass, 1000 }
        };

        _uniWeight = ConvertToUniWeight(kg, WeightFormat.Kg);
    }
}


public class GroupPhysicalSystem
{
    public Dictionary<BaseBuildingPrefabClass, Weight> CalcObjectsWeight(
        List<BaseBuildingPrefabClass> objects,
        Dictionary<BaseBuildingPrefabClass, List<BaseBuildingPrefabClass>> links,
        Dictionary<BaseBuildingPrefabClass, BuildingConnection> connections)
    {

        return null;
    }
}
