START = "start"
UPDATE = "update"
FIXED_UPDATE = "fixed_update"
SEC_UPDATE = "sec_update"


def debug_log(msg):
    gcore.debug_log(msg)
def array(_type, lst):
    return gcore.array[_type](lst)