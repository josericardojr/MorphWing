from kanren import Relation, facts, var, run
#from Data.PlayerDamageData import *

key_enemy = ["KEYENEMY1", "KEYENEMY2", "KEYENEMY3", "KEYENEMY4"]
key_dif_multi = ["DIFMULTI1", "DIFMULTI2", "DIFMULTI3", "DIFMULTI4"]
player_hit_rate = "PLAYERHITRATE"

balanceFactor = Relation()
difficultyAdjustMax = Relation()
difficultyAdjustMin = Relation()

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
facts(balanceFactor, ("enemy1", "2"),
    ("enemy2", "1"),
    ("enemy3", "2"),
    ("enemy4", "8"))

facts(difficultyAdjustMin, ("enemy1", "-0.3"),
    ("enemy2", "-0.3"),
    ("enemy3", "-0.3"),
    ("enemy4", "-0.3"))

facts(difficultyAdjustMax, ("enemy1", "0.5"),
    ("enemy2", "0.5"),
    ("enemy3", "0.5"),
    ("enemy4", "0.5"))

def format_number(number):
    return round(float(number),3)

def adjustDifficulty (factor):
    x = var()
    y = var();
    z = var();
    hapFactor = float(happenings[factor])
    hitFactor = float(hits[factor])
    balFactor = run(1, x, balanceFactor("enemy{0}".format(factor + 1), x))
    minAdjust = run(1, y, difficultyAdjustMin("enemy{0}".format(factor + 1), y))
    maxAdjust = run(1, z, difficultyAdjustMax("enemy{0}".format(factor + 1), z))

    if hapFactor > 0:
        result = min(max(0.05 * hapFactor - 0.05 * hitFactor * float(balFactor[0]), float(minAdjust[0])), float(maxAdjust[0]))
        print("{0}:{1};".format(key_enemy[factor], format_number(result)))

adjustDifficulty(0);

def adjustPlayerDamage (hit_time):
    result = hit_time * float(damageBalanceFactor)
    print("{0}:{1};".format(player_hit_rate, format_number(result)))

def ReturnPoolValues(w1, w2, w3, w4, invert) :

    value = (difficultyMultipliers[0] * w1) + (difficultyMultipliers[1] * w2) + (difficultyMultipliers[2] * w3) + (difficultyMultipliers[3] * w4);
    if invert:
        value = 5.1 - value;

    return min(max(value, 0.7), 5.1);

def GetItemDistances(m1, m2, m3, m4):
    difficultyMultipliers[0] = m1;
    difficultyMultipliers[1] = m2;
    difficultyMultipliers[2] = m3;
    difficultyMultipliers[3] = m4;
    itemDistances[0] = ReturnPoolValues(0.4, 1.1, 0.5, 0.4,False);
    itemDistances[1] = ReturnPoolValues(0.4, 1.1, 0.5, 0.4,True);
    itemDistances[2] = ReturnPoolValues(0.8,1.2,0.2,0.3,False);
    itemDistances[3] = ReturnPoolValues(0.8,1.2,0.2,0.3,True);

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

    for j in range(len(key_enemy)):
        for i in range(len(args)):
            if key_enemy[j] in args[i]:
                adjustDifficulty(j)

    for j in range(len(key_dif_multi)):
        for i in range(len(args)):
            if key_dif_multi[j] in args[i]:
                splited = args[i].split('=')
                last = splited[len(splited) - 1]
                if last.isdigit():
                    dif_multi[j] = float(last)

    damage_data = DamageData(xml)
    adjustPlayerDamage(damage_data.result())

    GetItemDistances(dif_multi[0], dif_multi[1], dif_multi[2], dif_multi[3])
