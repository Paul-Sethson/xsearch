# -*- coding: utf-8 -*-
################################################################################
#
# searchoption.py
#
# class SearchOption: encapsulates a (command-line) search option
#
################################################################################

class SearchOption(object):
    """a class to encapsulate a specific command line option"""
    def __init__(self, shortarg, longarg, desc, func):
        self.shortarg = shortarg
        self.longarg = longarg
        self.desc = desc
        self.func = func

    @property
    def sortarg(self):
        if self.shortarg:
            return self.shortarg.lower() + 'a' + self.longarg.lower()
        return self.longarg.lower()
