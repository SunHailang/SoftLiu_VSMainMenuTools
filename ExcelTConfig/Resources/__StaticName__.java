package config;

import java.util.ArrayList;
import java.util.HashMap;

public class __StaticName__
{
/*properties*/
    public static void load()
    {
        byte[] bytes = Config.readFile("__StaticName__");
        Header header = Config.readHeader(bytes);
        int offset = header.dataOffset;
        
/*readProperties*/
    }
}