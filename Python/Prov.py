import sys
from Reader.LoadedXML import *
import subprocess

def install(name):
    subprocess.call(['pip', 'install', name])



try:
    from Processor import *
except ImportError:
    install('kanren')
    from Processor import *

file = ''
args = sys.argv
for i in range(len(args)):
    if args[i] == 'do':
        if i + 1 < len(sys.argv):
            file = args[i + 1]
        else:
            print('forgot the xml file name')
    if args[i] == 'test':
        file = 'C:\\Users\\Lucas\\Documents\\JogoProveniência\\Assets\\info_26.5.2018;2.0.xml'


if file != '':
    try:
        xml = LoadedXML(file)
        getXMLInfo(xml,args)
    except ValueError:
        print("Unexpected error:", sys.exc_info()[0])
else:
    print('without xml path')
