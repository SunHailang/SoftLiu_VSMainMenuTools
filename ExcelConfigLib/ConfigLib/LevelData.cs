using Config.ExcelConfigAttribute;

namespace Config.ClientAndServer
{
    [PersistentEnum]
    public enum BattleType
    {
        [Tips("非战斗")]
        WITHOUT_BATTLE = 0,
        [Tips("战斗")]
        WITH_BATTLE = 1,
    }

    [PersistentEnum]
    public enum CHECK_TYPE
    {
        [Tips("无需校验")]
        TYPE_1 = 1,
        [Tips("仅结果校验")]
        TYPE_2 = 2,
        [Tips("结果+过程校验")]
        TYPE_3 = 3,
    }
    [PersistentEnum]
    public enum HUNTING_MAP_TYPE
    {
        [Tips("森林")]
        FOREST = 1,
        [Tips("峡谷")]
        GORGE = 2,
        [Tips("雪山")]
        SNOWMOUNTAIN = 3,
        [Tips("苔原")]
        TUNDRA = 4,

        [Tips("主据点")]
        CITY = 100,
        [Tips("小据点1")]
        SMALL_CITY = 101,
        [Tips("狩猎团据点")]
        GUILD_CITY = 200,

    }

    [PersistentEnum]
    public enum YesOrNo
    {
        [Tips("否")]
        No = 0,
        [Tips("是")]
        Yes = 1,
    }


    [TargetExcel("dict_stage")]
    public class LevelData
    {
        [Tips("ID"), MarkAsID]
        public int stage_id;
        [Tips("类型")]
        public int type;
        [Tips("备注"), CommentOnly]
        public string whisper;
        [Tips("名字"), MarkI18N]
        public string name;
        [Tips("场景")]
        public string scene_id;
		
	    [Tips("替换方式")]
        public int replace_mode;
        [Tips("替换怪物")]
        public int monster_battle;
	    [Tips("怪物换区")]
        public int group_id;
        [Tips("关卡等级")]
        public int stage_level;
        [Tips("单人修正")]
        public int single_revise;
        [Tips("双人修正")]
        public int double_revise;
        [Tips("三人修正")]
        public int three_revise;
        [Tips("四人及以上修正")]
        public int four_revise;
        [Tips("时间限制")]
        public int time_limit;
        [Tips("队伍复活次数限制")]
        public int resurgence_limit;
        [Tips("单人使用复活道具次数限制")]
        public int resurgence_with_item_limit;
        [Tips("单个玩家成败条件")]
        public int stage_win_type;
        [Tips("临时道具模式")]
        public int temp_item_type;
        [Tips("临时道具组")]
        public int temp_item_group_id;

        [Tips("关卡结算目标"), Capacity(3)]
        public int[] stage_target;
        [Tips("关卡配置"), Capacity(5)]
        public int[] level_config_serial;
        
        [Tips("怪物评分")]
        public int boss_score_group_id;
        [Tips("初始时间分")]
        public int time_initial_score;
        [Tips("时间扣分")]
        public int time_score_deduction;
        [Tips("时间加分")]
        public int time_score_addition;

        [Tips("初始受伤分")]
        public int hurt_initial_score;
        [Tips("受伤扣分")]
        public int hurt_score_deduction;
        [Tips("死亡初始分")]
        public int death_initial_score;
        [Tips("死亡扣分")]
        public int death_score_deduction;
        [Tips("关卡总分评级标准"), Capacity(5)]
        public int[] grade;

        [Tips("自我治疗分")]
        public int personal_score_cure_self;
        [Tips("治疗他人分")]
        public int personal_score_cure_partner;
        [Tips("抵挡伤害分")]
        public int personal_score_withstand;
        [Tips("初始受伤分")]
        public int personal_score_hurt_initial;
        [Tips("受伤扣分")]
        public int personal_score_hurt;
        [Tips("死亡初始分")]
        public int personal_score_death_initial;
        [Tips("死亡扣分")]
        public int personal_score_death;
        [Tips("个人分评级标准"), Capacity(5)]
        public int[] personal_grade;

        [Tips("结算类型")]
        public int is_by_server;
        [Tips("出生点"), MarkSplitString]
        public string born_point;
        [Tips("出生点朝向")]
        public int born_point_faceto;
        [Tips("返回出生点"), MarkSplitString]
        public string back_born_point;
        [Tips("返回出生点朝向")]
        public int back_born_point_faceto;
        [Tips("背景音乐")]
        public string background_music;
        [Tips("shut_trager_param")]
        public int shut_trager_param;
                
        [Tips("小贴士")]
        public string tips_group;
        [Tips("是否需要战斗")]
        public BattleType is_need_battle;
        
        [Tips("关卡引导")]
        public int guide_group_id = 0;
        
        [Tips("大地图最小缩放")]
        public int bigmap_size_min;
        [Tips("大地图最大缩放")]
        public int bigmap_size_max;
        
        [Tips("失败后是否禁止弹出变强界面")]
        public YesOrNo ban_be_strong = YesOrNo.No;
        
        [Tips("所在区域类型")]
        public HUNTING_MAP_TYPE area_id = 0;
        
        [Tips("校验类型")]
        public CHECK_TYPE check_type = 0;
        [Tips("结果校验战力预设")]
        public int check_gs_max = 0;
        [Tips("结果校验战斗时长下限")]
        public int check_time_min = 0;
        [Tips("结果校验boss击杀时长下限")]
        public int check_hunt_time_min = 0;
        [Tips("结果校验采集时长下限")]
        public int check_gather_time_min = 0;
        
        [Tips("返回据点ID")]
        public int item_get_id;
        [Tips("返回据点ID")]
        public int return_scene;
    }
}