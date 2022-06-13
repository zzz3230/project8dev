
from game import *
from game_funcs import *

class TestHandItem(PythonRuntimeHandItemScript):
    def start(self):
        self.def_y_rot = self.arrow.transform.localRotation.eulerAngles.y
    def update(self):
        rot = self.arrow.transform.localRotation.eulerAngles
        self.arrow.transform.localRotation = Quaternion.Euler(rot.x, 360 - RefHub.playerScript.transform.eulerAngles.y, rot.z);

def export():
    return TestHandItem
