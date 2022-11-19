#!/usr/bin/env python3
#coding=utf-8

from .ConfigTool import ConfigTool
from typing import TypeVar, Dict, Tuple

class __KlassName__:

    __slots__ = (__slotsContent__)
    count = 0 #type: int
    configs = None #type: Tuple[__KlassName__]
    idConfigsMap = None #type: Dict[int, __KlassName__]
    #{codeName}nameConfigsMap = None #type: Dict[str, __KlassName__]

    def __init__(self, data):
        __initContent__

    @classmethod
    def load(cls):
        if cls.configs: return
        data = ConfigTool.readData("__KlassName__")
        cls.count = len(data)
        cls.configs = ConfigTool.setupConfigs(data, cls)
        cls.idConfigsMap = ConfigTool.setupIDMap(cls.configs)
        #{codeName}cls.nameConfigsMap = ConfigTool.setupCodeNameMap(cls.configs)
        #{resolve}__importContent__
        #{resolve}for config in cls.configs:
        #{resolve}    __resolveContent__


    @classmethod
    def byIndex(cls, index: int) -> TypeVar("__KlassName__"):
        return cls.configs[index]

    @classmethod
    def byID(cls, id: int) -> TypeVar("__KlassName__"):
        return cls.idConfigsMap[id] if id in cls.idConfigsMap else None
    #{codeName}
    #{codeName}@classmethod
    #{codeName}def byCodeName(cls, codeName: str) -> TypeVar("__KlassName__"):
    #{codeName}    return cls.nameConfigsMap[codeName] if codeName in cls.nameConfigsMap else None