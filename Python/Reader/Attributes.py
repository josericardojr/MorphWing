class Attributes:
    def __init__(self, name, value):
        self.vname = name
        self.vvalue = value

    def name(self):
        return self.vname

    def value(self):
        return self.vvalue

    def myprint(self):
        print('{0}: {1}'.format(self.name, self.value))
