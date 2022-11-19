local type = type
local setmetatable = setmetatable
local rawget = rawget
local rawset = rawset
local require = require
local package = package
local ipairs = ipairs
local pairs = pairs

local readonly = {
    __index = error,
    __newindex = error,
}

local Config = setmetatable({}, {
    __index = function(t, k)
        local v = require("Test.EditThis.ProjectLuaPath." .. k)
        if type(v) ~= "table" then error("config load [%s] fail, table expected":format(k)) end

        rawset(t, k, setmetatable(v, readonly))

        local fillData = v.fillData
        if type(fillData) == "function" then fillData() end

        return v
    end,
    __newindex = error,
    __call = error,
})

local function alloc(size)
    local t = {}
    for it = 1, size do t[it] = {} end
    return t
end

ConfigManager = setmetatable({
    allocList = function(size) 
        local t = {}
        for it = 1, size do t[it] = {} end
        return t
    end,

    allocIDMap = function(idList)
        local t = {}
        for _, id in ipairs(idList) do t[id] = {} end
        return t
    end,

    allocNameMap = function(nameList)
        local t = {}
        for _, name in ipairs(nameList) do t[name] = {} end
        return t
    end,

    load = function(...)
        local t
        for _, name in ipairs{...} do t = Config[name] end
    end,

    unload = function(...)
        for _, name in ipairs{...} do
            rawset(Config, name, nil)
            package.loaded[name] = nil
        end
    end,

    listByID = function(t, id)
        for _, v in ipairs(t.configs) do
            if v.id == id then return v end
        end
        return nil
    end,

    listByName = function(t, name)
        for _, v in ipairs(t.configs) do
            if v.name == name then return v end
        end
        return nil
    end,

    idMapByID = function(t, id) return t.configs[id] end,
    
    idMapByName = function(t, name)
        for k, v in pairs(t.configs) do
            if v.name == name then return v end
        end
        return nil
    end,

    nameMapByID = function(t, id)
        for k, v in pairs(t.configs) do
            if v.id == id then return v end
        end
        return nil
    end,

    nameMapByName = function(t, name) return t.configs[name] end,

    listFill = function(table, keyList, valueTable)
        local keyListSize = #keyList
        for it = 1, #table do
            local config = table[it]
            local valueList = valueTable[it]
            for index = 1, keyListSize do
                config[keyList[index]] = valueList[index]
            end            
        end
    end,

    idMapFill = function(table, idList, keyList, valueTable)
        local keyListSize = #keyList
        for it = 1, #idList do
            local config = table[idList[it]]
            local valueList = valueTable[it]
            for index = 1, keyListSize do
                config[keyList[index]] = valueList[index]
            end      
        end
    end,

    nameMapFill = function(table, nameList, keyList, valueTable)
        local keyListSize = #keyList
        for it = 1, #nameList do
            local config = table[nameList[it]]
            local valueList = valueTable[it]
            for index = 1, keyListSize do
                config[keyList[index]] = valueList[index]
            end      
        end
    end,
    
}, readonly)

_G.Config = Config
return Config