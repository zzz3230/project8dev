
#GVector3 = gcore.GVector3
from lib2to3.pytree import Base


class Int32:
    pass
class Int64:
    pass
class String:
    pass
class Vector3:
    x = 0;
    y = 0;
    z = 0;
    def __init__(self, x, y, z) -> None:
        pass
class Quaternion:
    x : float
    y : float
    z : float
    w : float
    eulerAngles : Vector3
    normalized : Quaternion
    
class Transform:
    position : Vector3
    localPosition : Vector3
    rotation : Quaternion
    localRotation : Quaternion
    eulerAngles : Vector3

class Object():
    name : str
    @staticmethod
    def Instantiate(original, pos, rot):
        pass
    @staticmethod
    def Instantiate(original, transofrm):
        pass

    @staticmethod
    def Destroy(obj):
        pass
class GameObject(Object):
    activeSelf : bool
    layer : int
    transofrm : Transform
    def GetComponent(name : str):
        pass
class Component(Object):
    transform : Transform
    gameObject : GameObject
    tag : str
    def GetComponent():
        pass
    def CompareTag(tag : str):
        pass
class Behaviour(Component):
    enabled : bool
    isActiveAndEnabled : bool
class MonoBehaviour(Behaviour):
    pass
class UnitMetadata:
    durability : float
    maxDurability : float
    uuid : str
class ItemInfo:
    stackable = False
    stack = 0
    def CompareType(other):
        pass
    id = 0
    strId = "~SSID"
    name = ""
    ico = None
    builderId = None
    discardedObject = None
    handItemInfo = None
    buildingInfo = None
class ItemInstance:
    empty : bool
    metadata : UnitMetadata
    info : ItemInfo
    count : int
class Character(MonoBehaviour):
    pass
class NewBasePlayerScript(Character):
    pass
class RuntimeUnitInfoScript:
    root = None
    itemInstance : ItemInstance
    playerScript : NewBasePlayerScript
    def CastMetadata():
        pass
class RuntimeUnitScript:
    info : RuntimeUnitInfoScript
class RuntimeHandItemScript(RuntimeUnitScript):
    def DurabilityUpdated():
        pass
class Time:
    timeSinceLevelLoad : float
    time : float
    deltaTime : float
class RefHub:
    playerScript : NewBasePlayerScript
'''def core():
    gcore.writeln("Hello world!")
   core.write_ln("msg")
    return Vector3'''

class PythonBehaviour():
    gameObject : GameObject
class PythonRuntimeHandItemScript(PythonBehaviour):
    handItemScript : RuntimeHandItemScript
class ItemsManagerPointer():
    pass
class BaseUnitComponentScript(MonoBehaviour):
    uuid : str
class SavingDataComponentScript(BaseUnitComponentScript):
    def LoadObjects(key):
        pass
    def SaveItemPtrs(key, ptrs):
        pass
    def SaveObjects(key, objs):
        pass
class ReferenceComponentScript(BaseUnitComponentScript):
    infoScript : RuntimeUnitInfoScript
class OnUseComponentScript(BaseUnitComponentScript):
    def BindOnUse(a):
        pass
class ItemStorageComponentScript(BaseUnitComponentScript):
    count : int
    def GetPointer(index) -> ItemsManagerPointer:
        pass
class UiComponentScript(BaseUnitComponentScript):
    def GetWidget():
        pass
    def SwitchShowing():
        pass
    def ShowWidget():
        pass
    def HideWidget():
        pass
class SaveableComponentScript(BaseUnitComponentScript):
    pass

class UnitDataComponentScript(SaveableComponentScript):
    def Get(key):
        pass
    def Set(key, value):
        pass