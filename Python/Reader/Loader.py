import os
from xml.etree import ElementTree


def fullpath(xmlname, xmlpath):
    file_name = xmlname
    return os.path.abspath(os.path.join(xmlpath, file_name))


def loadxml(xmlname, xmllocation):
    file_name = xmlname
    full_file_name = os.path.abspath(os.path.join(xmllocation, file_name))
    print('OPEN {0}'.format(full_file_name))
    return ElementTree.parse(full_file_name)


def loadxml(path):
    print('OPEN {0}'.format(path))
    return ElementTree.parse(path)


def showall(xml, targets):
    info = xml.findall(targets)
    for i in info:
        print(i)




