package config;

import java.util.ArrayList;
import java.util.HashMap;

public class __KlassName__ {
	
/*properties*/
	
	private __KlassName__() {}
	
	private static __KlassName__[] configs;
	public static int count(){ return configs.length; }
	public static __KlassName__ byIndex(int index) { return index >= 0 && index < configs.length ? configs[index] : null; }
/*map*/

	public static void load()
	{
		if(configs != null) return;

		byte[] bytes = Config.readFile("__KlassName__");
		Header header = Config.readHeader(bytes);
		configs = new __KlassName__[header.count];

		for(int it = 0; it < configs.length; it++) configs[it] = loadSingle(bytes, header.dataOffset + header.width * it);
/*readMap*/
	}

	private static __KlassName__ loadSingle(byte[] bytes, int offset)
	{
		__KlassName__ __instance__ = new __KlassName__();
/*readProperties*/

		return __instance__;
	}

	public static __KlassName__ read(byte[] bytes, int offset)
	{
		load();
		return byIndex(Config.readInt32(bytes, offset));
	}
}
