from Reader.Attributes import *


class Vertex:
    def __init__(self, vid, vtype, label, date, attributes):
        self.vid = vid.text
        self.vtype = vtype.text
        self.vlabel = label.text
        self.vdate = date.text
        atr = []
        for a in attributes:
            atr.append(Attributes(a.find('name').text, a.find('value').text))
        self.vattributes = atr

    def id(self):
        return self.vid

    def type(self):
        return self.vtype

    def label(self):
        return self.vlabel

    def date(self):
        return self.vdate

    def attributes(self):
        return self.vattributes
