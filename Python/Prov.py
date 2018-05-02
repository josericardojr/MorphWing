import sys
from Reader.LoadedXML import *

file = '';
args = sys.argv;
for i in range(len(args)):
	if args[i] == 'do':
		if i + 1< len(sys.argv):
			file = args[i + 1]
		else:
			print('forgot the xml file name')


if file != '':
	try:
		xml = LoadedXML(file)
		
	except ValueError:
		print("Unexpected error:", sys.exc_info()[0])
else:
	print('erro')
	