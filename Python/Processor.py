from kanren import Relation, facts, var, run
from Data.PlayerDamageData import *

key_enemy = ["KEYENEMY1", "KEYENEMY2", "KEYENEMY3", "KEYENEMY4"]
key_dif_multi = ["DIFMULTI1", "DIFMULTI2", "DIFMULTI3", "DIFMULTI4"]

balanceFactor = Relation()

damageBalanceFactor = 1

#infos retiradas do xml

#numero de hits em ordem de tipo de inimigo
hits = [0,0,0,0]
#numero de usos de ataque em ordem de tipo de inimigo
happenings = [0, 0, 0, 0]

difficultyMultipliers = [1, 1, 1, 1]
itemDistances = [0, 0, 0, 0]

#BASEAR EM TEMPO

#infos estabelecidas aqui
facts(balanceFactor, ("enemy1", "0.5"),
    ("enemy2", "0.4"),
    ("enemy3", "0.3"),
    ("enemy4", "0.3"))

def adjustDifficulty (factor):
    x = var()
    hapFactor = float(happenings[factor])
    hitFactor = float(hits[factor])
    balFactor = run(1, x, balanceFactor("enemy{0}".format(factor + 1), x))

    if hapFactor > 0:
        result = hitFactor/hapFactor * float(balFactor[0])
        print("{0}:{1};".format(key_enemy[factor], result))
        

def adjustPlayerDamage (hit_time):
    result = hit_time * float(damageBalanceFactor)
    print("{0}:{1};".format("PlayerHitRate", result))


def ReturnPoolValues(w1, w2, w3, w4, invert) :
    value = 1;
    if not invert:
        value = (difficultyMultipliers[0] * w1) * (difficultyMultipliers[1] * w2) * (difficultyMultipliers[2] * w3) * (difficultyMultipliers[3] * w4);
    else:
        value = max(min(3.1 - (difficultyMultipliers[0] * w1), 3.1), 0.1) * max(min(3.1 - (difficultyMultipliers[1] * w2), 3.1), 0.1) * max(min(3.1 - (difficultyMultipliers[2] * w3), 3.1), 0.1) * max(min(3.1 - (difficultyMultipliers[3] * w4), 3.1), 0.1);
    return value;

def GetItemDistances(m1, m2, m3, m4):
    difficultyMultipliers[0] = m1;
    difficultyMultipliers[1] = m2;
    difficultyMultipliers[2] = m3;
    difficultyMultipliers[3] = m4;
    itemDistances[0] = ReturnPoolValues(1,1,3,2,True);
    itemDistances[1] = ReturnPoolValues(1,1,3,2,False);
    itemDistances[2] = ReturnPoolValues(2,3,1,1,True);
    itemDistances[3] = ReturnPoolValues(2,3,1,1,False);
    print("{0}:{1};".format("ItemDistance0", itemDistances[0]))
    print("{0}:{1};".format("ItemDistance1", itemDistances[1]))
    print("{0}:{1};".format("ItemDistance2", itemDistances[2]))
    print("{0}:{1};".format("ItemDistance3", itemDistances[3]))

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

    dif_multi = [0, 0, 0, 0]
    for i in range(len(args)):
        for j in range(len(key_enemy)):
            #print('Debug: {0} : {1};'.format(KEYENEMY[j], args[i]))
            if key_enemy[j] in args[i]:
                adjustDifficulty(j)
        for j in range(len(key_dif_multi)):
            if key_dif_multi[j] in args[i]:
                splited = args[i].split('=')
                last = splited[len(splited) - 1]
                if last.isdigit():
                    dif_multi[j] = float(last)


    damageData = DamageData(xml)
    adjustPlayerDamage(damageData.result())

    GetItemDistances(dif_multi[0], dif_multi[1], dif_multi[2], dif_multi[3])
