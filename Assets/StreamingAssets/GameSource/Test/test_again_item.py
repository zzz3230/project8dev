
import numbers
from game import *
from game_funcs import *

class AgainTestItem(PythonRuntimeHandItemScript):
    def used(self, data):
        if(str(data.mouseButton) == "Right"):
            self.ui.SwitchShowing()

    def btn_click(self):
        self.ui.GetWidget().text.text = "Number: " + str(self.number)
        self.number += 1
        self.unitData.Set("_number", self.number)
        

    def start(self):
        self.unitData = self.gameObject.GetComponent[UnitDataComponentScript]()
        self.unitData.Init()

        if self.unitData.HasKey("_number"): 
            self.number = self.unitData.Get[int]("_number")
            debug_log("found")
        else:
            self.number = 0
            debug_log("not found")

        debug_log(self.number)

        self.ui = self.gameObject.GetComponent[UiComponentScript]()
        on_use_scr = self.gameObject.GetComponent[OnUseComponentScript]()
        on_use_scr.BindOnUse(self.used)

        self.ui.GetWidget().btn.BindOnClick(self.btn_click)

        self.btn_click()
        
def export():
    return AgainTestItem
