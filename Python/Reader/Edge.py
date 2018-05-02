class Edge:
    def __init__(self, vid, vtype, label, value, sourceID, targetID):
        self.vid = vid.text
        self.vtype = vtype.text
        self.vlabel = label.text
        self.vvalue = value.text
        if self.value() == None:
            self.vvalue = 'None'
        self.vsourceID = sourceID.text
        self.vtargetID = targetID.text

    def id(self):
        return self.vid

    def type(self):
        return self.vtype

    def label(self):
        return self.vlabel

    def value(self):
        return self.vvalue

    def sourceID(self):
        return self.vsourceID

    def targetID(self):
        return self.vtargetID