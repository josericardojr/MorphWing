from Reader.Loader import *
from Reader.Edge import *
from Reader.Vertex import *

class LoadedXML:
    def __init__(self, path):
        xml = loadxml(path)
        vertices = xml.findall('vertices/vertex')
        vertex = {}

        for v in vertices:
            vid = v.find('ID')
            vertex[vid.text] = Vertex(vid, v.find('type'), v.find('label'), v.find('date'), v.find('attributes'))

        self.vvertex = vertex

        edges = xml.findall('edges/edge')

        edge = []
        for e in edges:
            edge.append(Edge(e.find('ID'), e.find('type'), e.find('label'), e.find('value'), e.find('sourceID'), e.find('targetID')))

        self.vedge = edge

    def vertexs(self):
        return self.vvertex

    def edge(self):
        return self.vedge		
		
    def example(self):
        for k, v in self.vertexs().items():
			#Felipe aqui eu acesso todos os dados por funções
           print('id: {0}\ntype: {1}\nlabel: {2}\ndate: {3}'.format(v.id(), v.type(), v.label(), v.date()))
			# o attributes() retorna uma lista
           atr = v.attributes()
           aux = 0
           for a in atr:
              aux = aux + 1
              print('attributes{0} name: {1} value: {2}'.format(aux, a.name(), a.value()))
              print('_' * 10)


        for e in self.edge():
           print('id: {0}\ntype: {1}\nlabel: {2}\nvalue: {3}\nsourceID: {4}\ntargetID: {5}'.format(e.id(), e.type(), e.label(), e.value(), e.sourceID(), e.targetID()))
           print('_' * 10)


