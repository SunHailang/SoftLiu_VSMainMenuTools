using Config.ExcelConfigAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.ClientAndServer
{
    [PersistentEnum, CustomDescEnum("Test Data Enum Type")]
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
        [Tips("介绍Desc"), MarkSpecificType(SpecificType.VARCHAR, 64)]
        public string dict_desc;
    }
}
