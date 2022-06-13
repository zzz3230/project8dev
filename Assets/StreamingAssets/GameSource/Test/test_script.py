
from game import *
from game_funcs import *

class TestClass(PythonBehaviour):
    def start(self):
        #RefHub.playerScript.transform.
        #RefHub.playerScript.transform.eulerAngles.x
        self.gameObject.GetComponent[Transform]().position = Vector3(20, 10, 10)
    def sec_update(self):
        pass
    #def update(self):
    #    self.gameObject.transform.Translate(1, 0, 0)

def export():
    return TestClass

#import os, sys
#sys.path.append(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
#def main():
#    return Vector3(1, 2, 3)