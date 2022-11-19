package config;

public enum __EnumName__
{
    /*values*/;

    private int value;
    public int value() { return value; }
    private __EnumName__(int value) { this.value = value; }

    public static __EnumName__ valueOf(int value)
    {
        switch(value)
        {
/*switchContent*/
            default: return null;
        }
    }

    public static __EnumName__ read(byte[] bytes, int index)
    {
        return valueOf(Config.readInt32(bytes, index));
    }
}