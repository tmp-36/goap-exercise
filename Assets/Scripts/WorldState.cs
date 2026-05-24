using System;

[Flags]
public enum WorldFlags : uint
{
    NONE = 0,
    LIGHT_ON = 1 << 0,
    NEAR_SWITCH = 1 << 1,
};

[Serializable]
public struct WorldState
{
    public WorldFlags values;
    public WorldFlags mask;

    public static bool IsCompatible(WorldState current, WorldState required)
    {
        return ((current.values ^ required.values) & required.mask) == 0;
    }

    /// <summary>
    /// Does this action satisfy any of the effects?
    /// </summary>
    public static bool AdvancesGoal(WorldState required, WorldState effects)
    {
        WorldFlags overlap = required.mask & effects.mask;

        if (overlap == 0)
        {
            return false;
        }

        WorldFlags difference = effects.values ^ required.values;
        WorldFlags matching = ~difference & overlap;
        return matching > 0;
    }

    public static int PopCount(uint value)
    {
        const uint c1 = 0x_55555555u;
        const uint c2 = 0x_33333333u;
        const uint c3 = 0x_0F0F0F0Fu;
        const uint c4 = 0x_01010101u;

        value -= (value >> 1) & c1;
        value = (value & c2) + ((value >> 2) & c2);
        value = (((value + (value >> 4)) & c3) * c4) >> 24;

        return (int)value;
    }

    public static int PopCount(ulong value)
    {
        const ulong c1 = 0x_55555555_55555555ul;
        const ulong c2 = 0x_33333333_33333333ul;
        const ulong c3 = 0x_0F0F0F0F_0F0F0F0Ful;
        const ulong c4 = 0x_01010101_01010101ul;

        value -= (value >> 1) & c1;
        value = (value & c2) + ((value >> 2) & c2);
        value = (((value + (value >> 4)) & c3) * c4) >> 56;

        return (int)value;
    }
}
