using Config.ExcelConfigAttribute;

namespace Config.ClientAndServer
{
    [PersistentEnum]
    public enum EnumType
    {
        [Tips("公开")] PUBLIC = 0,
        [Tips("私有")] PRIVATE = 1,
    }


    [TargetExcel("dict_test")]
    public class TestData
    {
        [Tips("测试ID"), MarkAsID, MarkSpecificType(SpecificType.INT, 10)]
        public int dict_id;
        [Tips("隐私"), MarkSpecificType(SpecificType.SMALLINT, 5)]
        public EnumType dict_type;
        [Tips("介绍Desc"), MarkI18N, MarkSpecificType(SpecificType.VARCHAR, 64)]
        public string dict_desc;
        [Tips("Int类型数组"), Capacity(3), MarkSpecificType(SpecificType.INT, 10)]
        public int[] arrayInt;
        [Tips("Int类型分割"), MarkSplitString(SplitSingleType.Int), MarkSpecificType(SpecificType.INT, 10)]
        public string splitInt;
        [Tips("String类型分割"), MarkSplitString(SplitSingleType.String), MarkSpecificType(SpecificType.VARCHAR, 64)]
        public string splitString;
    }
}
