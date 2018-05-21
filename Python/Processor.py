from kanren import Relation, facts, var, run

KEYENEMY = ["KEYENEMY1", "KEYENEMY2", "KEYENEMY3", "KEYENEMY4"]

balanceFactor = Relation()

#infos retiradas do xml

#numero de hits em ordem de tipo de inimigo
hits = [0,0,0,0]
#numero de usos de ataque em ordem de tipo de inimigo
happenings = [0,0,0,0]

#BASEAR EM TEMPO

#infos estabelecidas aqui
facts(balanceFactor, ("enemy1", "0.5"),
    ("enemy2", "0.4"),
    ("enemy3", "0.3"),
    ("enemy4", "0.3"))


def adjustDifficulty (factor):
    x = var()
    ene = "enemy{0}".format(factor)
    hapFactor = float(happenings[factor])
    hitFactor = float(hits[factor])
    balFactor = run(1, x, balanceFactor("enemy{0}".format(factor), x))

    if hapFactor > 0:
        result = hitFactor/hapFactor * balFactor
        print("{0}:{1};".format(KEYENEMY[factor], result))


def getXMLInfo(xml, args):
    #xml = LoadedXML(fullpath('Lucas_5.xml', 'XML'))
    vertexs = xml.vertexs()

    for v in vertexs:
        #if(v.label() == "Attacking (Enemy)"):
        #    print('id: {0}\ntype: {1}\nlabel: {2}\ndate: {3}'.format(v.id(), v.type(), v.label(), v.date()))
        
        aux = 0
        if(xml.vertexs()[v].label() == "Being Hit(Player)"):
            if(xml.vertexs()[v].attributes()[3].value() == "Straight"):
                hits[0] += 1
            if(xml.vertexs()[v].attributes()[3].value() == "Chaser"):
                hits[1] += 1
            if(xml.vertexs()[v].attributes()[3].value() == "Round"):
                hits[2] += 1
            if(xml.vertexs()[v].attributes()[3].value() == "Irregular"):
                hits[3] += 1

        if(xml.vertexs()[v].label() == "Attacking (Enemy)"):
            if(xml.vertexs()[v].attributes()[6].value() == "Enemy_Straight"):
                happenings[0] += 1
            if(xml.vertexs()[v].attributes()[6].value() == "Enemy_Chaser"):
                happenings[1] += 1
            if(xml.vertexs()[v].attributes()[6].value() == "Enemy_Round"):
                happenings[2] += 1
            if(xml.vertexs()[v].attributes()[6].value() == "Enemy_Irregular"):
                happenings[3] += 1

        #atr = v.attributes()
        #for a in atr:
            #aux = aux + 1
               # print('attributes{0} name: {1} value: {2}'.format(aux, a.name(), a.value()))
        
    #print('_' * 10)

    for i in range(len(args)):
        for j in range(len(KEYENEMY)):
            #print('Debug: {0} : {1};'.format(KEYENEMY[j], args[i]))
            if KEYENEMY[j] in args[i]:
                increaseDifficulty(j)
                decreaseDifficulty(j)
